using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Reward", menuName = "ScriptableObjects/Quest/Reward Data")]
public class QuestReward : ScriptableObject
{
    [AllowNesting]
    public float Gold = 15;
    
    [AllowNesting]
    public float Experience = 20;
}
