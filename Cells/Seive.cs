using Chains;

namespace Cells;

public class Seive : Cell
{
    protected readonly int _seivePeriod;

    protected Dictionary<byte, Energy> _energy = new();
    protected const int _splitGate = 20;

    public Seive(int seivePeriod, Alpha? genotype = null) : base(genotype)
    {
        _seivePeriod = seivePeriod;
    }

    public override sealed void Tick(IEnumerable<Chain> neighbourhoodChains, Energy energy)
    {
        Filter();
        Capture(energy);

        // Absorbing
        foreach (Chain chain in neighbourhoodChains)
        {
            if (chain.MainChainLength <= _seivePeriod)
            {
                _freeChains.Add(chain);
            }
        }

        Synthesis();
        Split();
    }

    protected virtual void Filter()
    {
        if (_freeChains.Count == 0)
        {
            return;
        }

        List<Chain> chainsToRelease = new();

        foreach(Chain chain in _freeChains)
        {
            if (chain.MainChainLength <= _seivePeriod)
            {
                chainsToRelease.Add(chain);
            }
        }

        _ = chainsToRelease.Select(_freeChains.Remove);
        ReleaseChains(chainsToRelease);
    }

    protected virtual void Capture(Energy energy)
    {
        _energy.Add(energy.Type, energy);
    }

    protected virtual void Synthesis()
    {
        Chain[] freeChains = _freeChains.ToArray();
        HashSet<int> acceptedChainsIndexes = new();

        for (int acceptorPointer = 0; acceptorPointer < _freeChains.Count; ++acceptorPointer)
        for (int donorPointer = 0; donorPointer < _freeChains.Count; ++donorPointer)
        {
            if (!acceptedChainsIndexes.Contains(acceptorPointer) && !acceptedChainsIndexes.Contains(donorPointer))
            {
                byte energyType = freeChains[acceptorPointer].Body[0];
                if (_energy.TryGetValue(energyType, out Energy? energy) && energy.Value > 0)
                {
                    if (freeChains[acceptorPointer].TryAccept(freeChains[donorPointer]))
                    {
                        _energy[energyType].Subtract(1);
                        _freeChains.Remove(freeChains[donorPointer]);
                        acceptedChainsIndexes.Add(donorPointer);
                    }
                }
            }
        }
    }

    protected virtual void Split()
    {
        foreach (Chain chain in _freeChains)
        foreach ((byte type, Energy energy) in _energy)
        {
            if (energy.Value > byte.MaxValue - type && chain.TrySplit(type, out Chain? splittedPart))
            {
                energy.Subtract(byte.MaxValue - type + 1);
                _freeChains.Add(splittedPart!);
            }
        }

        if (_freeChains.Count >= _splitGate)
        {
            SplitClone();
        }
    }

    protected override Cell GetClone()
    {
        Seive clone = new(_seivePeriod, _genotype.UnreliableCopy());
        return clone;
    }
}
