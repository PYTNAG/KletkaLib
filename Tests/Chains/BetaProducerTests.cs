using Chains;

namespace Tests.Chains;

public sealed class BetaProduceratests
{
    [Fact]
    public void GetBeta_MatrixWithGenes_ReturnsTranscriptedGene()
    {
        Alpha matrix;

        do
        {
            matrix = new();
        }
        while (!matrix.HasGenes());

        Chain beta = BetaProducer.GetBeta(matrix)!;

        Assert.True(beta.Head.SequenceEqual(Alpha.GeneInitialSequence));
    }

    [Fact]
    public void GetBeta_MatrixWithoutGenes_ReturnsNull()
    {
        Alpha matrix;

        do
        {
            matrix = new();
        }
        while (matrix.HasGenes());

        Assert.Null(BetaProducer.GetBeta(matrix));
    }
}