using Chains;

namespace Tests.Chains;

public sealed class EnergyTests
{
    [Fact]
    public void Subtract()
    {
        Energy e = new(0xFF, 10);

        e.Subtract(1);
        Assert.Equal(9, e.Value);

        e.Subtract(9);
        Assert.Equal(0, e.Value);
    }
}