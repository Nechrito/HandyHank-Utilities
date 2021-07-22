using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    public EntityData Data;

    public ValueStat Health   { get; private set; }
    public ValueStat Shield   { get; private set; }
    public ValueStat Mana     { get; private set; }
    public ValueStat Stamina  { get; private set; }
    public ValueStat Strength { get; private set; }

    public Vector2 Position { get; private set; }

    public BoxCollider2D BoxCollider     { get; private set; }
    public Rigidbody2D Rigidbody         { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    protected void Awake()
    {
        this.Rigidbody      = GetComponent<Rigidbody2D>();
        this.BoxCollider    = GetComponent<BoxCollider2D>();
        this.SpriteRenderer = GetComponent<SpriteRenderer>();

        this.Health   = new ValueStat(this.Data.Health)   { Owner = this };
        this.Stamina  = new ValueStat(this.Data.Stamina)  { Owner = this };
        this.Shield   = new ValueStat(this.Data.Shield)   { Owner = this };
        this.Mana     = new ValueStat(this.Data.Mana)     { Owner = this };
        this.Strength = new ValueStat(this.Data.Strength) { Owner = this };
    }

    protected virtual void Update()
    {
        this.Position = this.transform.position.ToVector2();
    }

    public void LevelUp()
    {
        this.Data.Level++;
        this.Health.AddModifier(  new StatModifier(this.Data.LevelUpMultiplier, StatModType.PercentAdd));
        this.Stamina.AddModifier( new StatModifier(this.Data.LevelUpMultiplier, StatModType.PercentAdd));
        this.Strength.AddModifier(new StatModifier(this.Data.LevelUpMultiplier, StatModType.PercentAdd));
    }
}
