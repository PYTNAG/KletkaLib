namespace Chains;

public static class BetaProducer
{
    /// <summary>
    /// Construct chain based on given alpha matrix
    /// </summary>
    /// <param name="matrix">Alpha chain, genotype</param>
    /// <param name="transcription">Transcripts alpha's body to beta's body. If null then using default transcription which just inverse bytes</param>
    /// <returns>New transcripted chain</returns>
    public static Chain GetBeta(Alpha matrix, Func<byte[], byte[]>? transcription = null)
    {
        byte[] beta = transcription is null ? DefaultTranscription(matrix.RandomGene()) : transcription(matrix.RandomGene());
        return new(Alpha.GeneInitialSequence, beta);
    }

    private static byte[] DefaultTranscription(byte[] gene)
    { 
        return gene
                .Select(b => (byte)~b)
                .ToArray();
    }
}