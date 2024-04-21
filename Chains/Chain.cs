namespace Chains;

public class Chain
{
    protected byte[] _head;
    public byte[] Head => _head;

    protected byte[] _body;
    public byte[] Body => _body;

    private readonly Dictionary<int, Chain> _radicals = new();

    public int MainChainLength => _body.Length;

    /// <summary>
    /// Generates a new random chain with [headLength] bytes in head and [bodyLength] bytes in body.
    /// </summary>
    /// <param name="headLength">Length of head sequence</param>
    /// <param name="bodyLength">Length of body sequence</param>
    public Chain(int headLength, int bodyLength)
    {
        _head = new byte[headLength];
        Random.Shared.NextBytes(_head);

        _body = new byte[bodyLength];
        Random.Shared.NextBytes(_body);
    }

    /// <summary>
    /// Generates a new chain with [head] and [body].
    /// </summary>
    /// <param name="head">Head sequence</param>
    /// <param name="body">Body sequence</param>
    public Chain(byte[] head, byte[] body)
    {
        _head = head;
        _body = body;
    }

    public bool TryAccept(Chain other)
    {
        Dictionary<Chain, int> pointers = new(){
            [this] = 0
        };

        while (pointers.Count != 0)
        {
            foreach ((Chain acceptor, int pos) in pointers)
            {
                if (acceptor.IsSequenceConjugateAt(other._head, pos))
                {
                    acceptor._radicals.Add(pos, other);
                    return true;
                }

                if (acceptor._radicals.TryGetValue(pos, out Chain? radical))
                {
                    pointers.Add(radical, 0);
                }
            }

            pointers
                .AsParallel()
                .ForAll(
                    (pair) => {
                        (Chain chain, int pointer) = pair;

                        if (pointer + 1 >= chain.MainChainLength)
                        {
                            pointers.Remove(chain);
                            return;
                        }

                        pointers[chain] += 1;
                    }
                );
        }

        return false;
    }

    private bool IsSequenceConjugateAt(byte[] sequence, int position)
    {
        for (int i = 0; i < sequence.Length; ++i)
        {
            if ((sequence[i] & _body[position + i]) == 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool TrySplit(Energy energy, out Chain? splittedPart)
    {
        for (int i = 0; i < _body.Length; ++i)
        {
            if (IsSequenceConjugateAt(new byte[] { energy.Type }, i))
            {
                energy.Subtract(byte.MaxValue - energy.Type + 1);

                splittedPart = new(_head, _body[..i]);
                foreach (var radical in _radicals.Where(r => r.Key < i))
                {
                    splittedPart._radicals.Add(radical.Key, radical.Value);
                    _radicals.Remove(radical.Key);
                }

                // byte with index [i] is broke
                _body = _body[(i+1)..];
                return true;
            }
        }

        splittedPart = null;
        return false;
    }
}