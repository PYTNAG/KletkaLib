namespace Chains;

internal static class AlphaConvertor
{
    public static Chain Convert(Alpha matrixAlpha)
    {
        return BetaProducer.Beta(matrixAlpha.RandomGene());
    }
}