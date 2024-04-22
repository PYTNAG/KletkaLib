namespace Chains;

public static class BetaProducer
{
    public static Chain GetBeta(Alpha matrix)
    {
        byte[] beta = matrix.RandomGene()
            .Select(b => (byte)~b)
            .ToArray();

        return new(Alpha.GeneInitialSequence, beta);
    }
}