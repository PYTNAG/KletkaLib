using Chains;

namespace Tests.Chains;

public sealed class ChainTests
{
    [Fact]
    public void TryAccept_AcceptableChain_ReturnsTrue()
    {
        Chain chain = new(1, 1);
        Chain acceptable = new([ (byte)~chain.Body[0] ], [ 0xAA ]);

        Assert.True(chain.TryAccept(acceptable));
    }

    [Fact]
    public void TryAccept_UnacceptableChain_ReturnsFalse()
    {
        Chain chain = new(1, 1);
        Chain acceptable = new([ chain.Body[0] ], [ 0xAA ]);

        Assert.False(chain.TryAccept(acceptable));
    }
}