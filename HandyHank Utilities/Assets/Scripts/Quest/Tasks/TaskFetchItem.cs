using UnityEngine;
using UnityEngine.Events;

// Todo: Not yet implemented
public class TaskFetchItem : MonoBehaviour, IQuestTask
{
    public string Status { get; set; }

    public UnityEvent OnTaskFinished { get; set; }
    public UnityEvent OnTaskFailed   { get; set; }
    public UnityEvent OnTaskUpdated  { get; set; }
}
