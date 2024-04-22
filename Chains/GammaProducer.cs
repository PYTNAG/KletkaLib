namespace Chains;

public static class GammaProducer
{
    /// <summary>
    /// Translate beta matrix to gamma.
    /// </summary>
    /// <param name="betaMatrix">Beta chain</param>
    /// <param name="componentPreimageLength">Length of preimages of components</param>
    /// <param name="translator">
    ///     Function that translates [componentPreimageLength] bytes to component. 
    ///     If null then using default translator which translate n bytes to 2 bytes.
    /// </param>
    /// <returns>New gamma chain.</returns>
    public static Chain Gamma(Chain betaMatrix, int componentPreimageLength = 3, Func<byte[], byte[]>? translator = null)
    {
        List<byte> structure = new();

        foreach (byte[] preimage in betaMatrix.Body.Skip(betaMatrix.MainChainLength % componentPreimageLength).Chunk(componentPreimageLength))
        {
            structure.AddRange(translator is null ? DefaultTranslator(preimage) : translator(preimage));
        }

        return new(betaMatrix.Head, structure.ToArray());
    }

    private static byte[] DefaultTranslator(byte[] preimage)
    {
        byte[] component = preimage[..2];
        for (int i = 2; i < preimage.Length; ++i)
        {
            component[i % 2] ^= preimage[i];
        }

        return component;
    }
}