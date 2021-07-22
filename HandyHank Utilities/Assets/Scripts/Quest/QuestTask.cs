using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public interface IQuestTask
{
    string Status { get; set; }

    UnityEvent OnTaskFinished { get; set; }
    UnityEvent OnTaskFailed   { get; set; }
    UnityEvent OnTaskUpdated  { get; set; }
}

public enum TaskType
{
    EliminateEnemies,
    InitiateDialogue,
    FetchItem,
}

[Serializable]
[CreateAssetMenu(fileName = "New Task", menuName = "ScriptableObjects/Quest/Task Data")]
public class QuestTask : ScriptableObject
{
    public IQuestTask Task;

    public string Title;

    [AllowNesting]
    [TextArea]
    public string Description;

    [AllowNesting]
    [CanBeNull]
    public QuestReward TaskReward;

    [AllowNesting]
    [Space]
    public TaskType TaskType;


#region EliminateEnemies

    public bool EliminateEnemies => TaskType == TaskType.EliminateEnemies;

    [ShowIf(nameof(EliminateEnemies))]
    [Tooltip("Key: The name of the enemy | Value: The amount to eliminate")]
    public SerializableDictionary<string, int> EnemiesToEliminate;

#endregion


#region InitiateDialogue

    public bool InitiateDialogue => TaskType == TaskType.InitiateDialogue;

    [ShowIf(nameof(InitiateDialogue))]
    public bool DialogueInitiated;

#endregion

    [Space]
    [ReadOnly]
    [TextArea]
    public string Status;

    [Space]
    public UnityEvent<QuestTask> OnTaskFinished;
    public UnityEvent<QuestTask> OnTaskFailed;

    private void Start()
    {
        this.Title ??= $"{GetType().ToString().Replace("Task", "").SplitCamelCase()}";
    }

    public void Init(MonoBehaviour behaviour)
    {
        switch (this.TaskType)
        {
            case TaskType.EliminateEnemies:
                var taskEliminateEnemies = behaviour.gameObject.AddComponent<TaskEliminateEnemies>();
                taskEliminateEnemies.Init(this.EnemiesToEliminate);
                this.Task = taskEliminateEnemies;
                break;

            case TaskType.InitiateDialogue:
                var taskInitiateDialog = behaviour.gameObject.AddComponent<TaskInitiateDialog>();
                taskInitiateDialog.Init();
                this.Task = taskInitiateDialog;
                break;
            case TaskType.FetchItem:

                break;
        }

        if (this.Task == null)
        {
            return;
        }

        this.Task.OnTaskUpdated.AddListener(() => this.Status = this.Task.Status);
        this.Task.OnTaskFailed.AddListener(() => this.OnTaskFailed.Invoke(this));
        this.Task.OnTaskFinished.AddListener(() => this.OnTaskFinished.Invoke(this));
    }

    public virtual void Update()
    {
        
    }

}
