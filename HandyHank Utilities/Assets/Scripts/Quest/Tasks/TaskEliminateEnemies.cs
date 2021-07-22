using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TaskEliminateEnemies : MonoBehaviour, IQuestTask
{
    public string Status { get; set; }

    public UnityEvent OnTaskFinished { get; set; } = new UnityEvent();
    public UnityEvent OnTaskFailed   { get; set; } = new UnityEvent();
    public UnityEvent OnTaskUpdated  { get; set; } = new UnityEvent();

    private SerializableDictionary<string, int> descriptionItems;

    public void Init(SerializableDictionary<string, int> enemies)
    {
        this.descriptionItems = enemies;

        foreach (var pair in enemies)
        {
            var entities = FindObjectsOfType<EntityBase>()
                .Where(x => string.Equals(x.Data.Name, pair.Key, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            if (!entities.Any())
                continue;

            foreach (var entity in entities)
            {
                entity.Health.OnDepleted.AddListener(OnEnemyHealthDepleted);
            }
        }

        UpdateInformation();
    }

    private void OnEnemyHealthDepleted(EntityBase owner)
    {
        if (this.descriptionItems.ContainsKey(owner.Data.Name))
        {
            this.descriptionItems[owner.Data.Name]--;

            UpdateInformation();
        }

        //this.enemyCountRemaining--;

        if (this.descriptionItems.Values.Sum() <= 0)
        {
            this.OnTaskFinished?.Invoke();
        }
    }

    private void UpdateInformation()
    {
        this.Status = $"";

        foreach (var pair in this.descriptionItems)
        {
            var suffix = pair.Value > 1 ? "s" : "";
            this.Status += $"Kill {pair.Value} {pair.Key}{suffix}";
        }

        this.OnTaskUpdated?.Invoke();
    }
}
