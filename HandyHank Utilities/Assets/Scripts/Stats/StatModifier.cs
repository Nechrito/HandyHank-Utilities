public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

public class StatModifier
{
    public float value;
    public readonly StatModType Type;

    public StatModifier(float value, StatModType type)
    {
        this.value = value;
        this.Type = type;
    }
}