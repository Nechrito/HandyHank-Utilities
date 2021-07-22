using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
public class EntityData : ScriptableObject
{
    [Header("General")]
    public string Name;

    [Header("Attributes")]
    public int Health   = 100;
    public int Shield   = 100;
    public int Mana     = 100;
    public int Stamina  = 100;
    public int Strength = 100;

    [Header("Leveling")]
    public int Level = 1;
    public int Experience;
    public float LevelUpMultiplier = 1.50f;

    [Header("SFX")]
    public AudioClip AudioWalk;
    public AudioClip AudioAttackHit;
    public AudioClip AudioAttackMiss;

    [Header("Other")]
    public float PathRadius = 2;
    public float MoveSpeed  = 2;

    [Range(1.0f, 2.0f)]
    public float SprintSpeedMultiplier = 1.50f;
}