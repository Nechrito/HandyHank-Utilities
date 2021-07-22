using UnityEngine;
using UnityEngine.Events;

public class TaskInitiateDialog : MonoBehaviour, IQuestTask
{
    public string Status { get; set; }
    public UnityEvent OnTaskFinished { get; set; }
    public UnityEvent OnTaskFailed { get; set; }
    public UnityEvent OnTaskUpdated { get; set; }

    public void Init()
    {

    }
}
