namespace Domain.Values;

public struct Proficiency
{
    public int Value { get; }

    public Proficiency(int value)
    {
        if (value <= 0 || value > 5)
        {
            throw new ArgumentOutOfRangeException($"{nameof(value)} isn't between 0 and 5");
        }

        Value = value;
    }
}