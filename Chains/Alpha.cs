namespace Chains;

public class Alpha : Chain
{
    public static readonly byte[] GeneInitialSequence = { 0b_0000_1000 };
    public static readonly byte[] GeneTerminalSequence = { 0b_0001_0000 };

    private const int _headLength = 1;
    private const int _bodyLength = 32;

    private readonly List<byte[]> _genes;

    public Alpha() : base(_headLength, _bodyLength)
    {
        _genes = new();
        ParseGenes();
    }

    private Alpha(Alpha ancestor) : base(_headLength, _bodyLength)
    {
        _head = ancestor._head;
        _body = ancestor._body;

        _genes = new();

        for (int i = 0; i < _body.Length; ++i)
        {
            byte @byte = _body[i];
            byte mutationMask = GenerateMutationMask();

            _body[i] = (byte)((~@byte & mutationMask) | (@byte & ~mutationMask));
        }

        ParseGenes();
    }

    private static byte GenerateMutationMask()
    {
        byte mask = 0;

        for (int i = 0; i < 8; ++i)
        {
            if (Random.Shared.NextSingle() <= 0.1f)
            {
                mask |= (byte)(1 << i);
            }
        }

        return mask;
    }

    private void ParseGenes()
    {
        List<byte> currentGene = new();

        for (int pos = 0; pos + GeneInitialSequence.Length - 1 < MainChainLength; ++pos)
        {
            int geneStartIndex = pos + GeneInitialSequence.Length;

            if (!_body[pos .. geneStartIndex].SequenceEqual(GeneInitialSequence))
            {
                continue;
            }

            for (int i = geneStartIndex; i + GeneTerminalSequence.Length - 1 < MainChainLength; ++i)
            {
                currentGene.Add(_body[i]);

                if (_body[^GeneTerminalSequence.Length ..].SequenceEqual(GeneTerminalSequence))
                {
                    _genes.Add(currentGene.ToArray()[.. (^GeneTerminalSequence.Length)]);
                    currentGene.Clear();
                    break;
                }
            }

            if (currentGene.Count != 0)
            {
                _genes.Add(currentGene.ToArray());
                currentGene.Clear();
            }
        }
    }

    public Alpha UnreliableCopy() => new(this);

    public byte[] RandomGene() => _genes[Random.Shared.Next(_genes.Count)];
    public bool HasGenes() => _genes.Count > 0;
}