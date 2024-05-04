using Chains;

namespace Tests.Chains;

public sealed class AlphaTests
{
    [Fact]
    public void UnreliableCopy_ReturnsMutatedAlpha()
    {
        Alpha @base = new();

        Alpha copy = @base.UnreliableCopy();

        Assert.Equal(@base.Body.Length, copy.Body.Length);
        Assert.True(@base.Head.SequenceEqual(copy.Head));
    }
}