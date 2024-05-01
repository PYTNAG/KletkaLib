using Chains;

namespace Cells;

public abstract class Cell
{
    public event Action<IEnumerable<Chain>>? ChainsReleased;
    public event Action<Cell>? CloneSplitted;
    public event Action<Cell>? Died;

    protected readonly Alpha _genotype;
    public byte[] Genotype => _genotype.Body;

    protected readonly List<Chain> _freeChains; // includes betas and gammas
    public IEnumerable<Chain> FreeChains => _freeChains;

    public Cell(Alpha? genotype = null)
    {
        _genotype = genotype ?? new();
        _freeChains = new();
    }

    public abstract void Tick(IEnumerable<Chain> neighbourhoodChains, Energy energy);

    protected void ReleaseChains(IEnumerable<Chain> chains) => ChainsReleased?.Invoke(chains);
    
    protected abstract Cell GetClone();
    protected void SplitClone() => CloneSplitted?.Invoke(GetClone());

    protected void Die() => Died?.Invoke(this);
}
