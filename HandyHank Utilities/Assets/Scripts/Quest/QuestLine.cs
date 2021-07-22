using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu(fileName = "New QuestLine", menuName = "ScriptableObjects/Quest/QuestLine Data")]
public class QuestLine : ScriptableObject
{
    public string Title;
    
    [TextArea] 
    public string Description;

    [Expandable]
    [Space]
    public QuestReward Reward;

    [Expandable]
    public List<QuestTask> Tasks = new List<QuestTask>();
    
    [Header("Events")]
    public UnityEvent<QuestLine, QuestReward> OnQuestComplete;
    public UnityEvent<QuestLine> OnQuestFailed;

    public void Init()
    {
        foreach (var task in this.Tasks)
        {
            task.OnTaskFinished?.AddListener(OnTaskFinished);
            task.OnTaskFailed?.AddListener(OnTaskFailed);
        }
    }
  
    private void OnTaskFailed(QuestTask task)
    {
        this.OnQuestFailed?.Invoke(this);
    }

    private void OnTaskFinished(QuestTask task)
    {
        this.Tasks.Remove(task);

        if (this.Tasks.Count <= 0)
        {
            this.OnQuestComplete?.Invoke(this, Reward);
        }
    }

    public void Update()
    {
        foreach (var task in this.Tasks)
        {
            task.Update();
        }
    }
}
