namespace Chains;

public class Energy
{
    public byte Type { get; }
    public int Value { get; private set; }

    public Energy(byte type, int value)
    {
        Type = type;
        Value = value;
    }

    public bool Subtract(int value)
    {
        if (Value < value)
        {
            return false;
        }

        Value -= value;
        return true;
    }
}