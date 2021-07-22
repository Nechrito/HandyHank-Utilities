using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [AllowNesting]
    [Expandable]
    public QuestLine CurrentQuest;
    
    [AllowNesting]
    [Expandable]
    public List<QuestLine> ActiveQuests = new List<QuestLine>();

    private void Start()
    {
        if (ActiveQuests != null && ActiveQuests.Any())
        {
            foreach (var quest in this.ActiveQuests.Where(quest => quest))
            {
                this.InitializeQuestLine(quest);
            }
        }

        this.SetCurrentQuest();

        Instance = this;
    }

    private void InitializeQuestLine(QuestLine quest)
    {
        quest.Init();

        if (quest.Tasks.Any())
        {
            foreach (var task in quest.Tasks)
            {
                task.Init(this);
            }
        }
    }

    private void SetCurrentQuest()
    {
        if (!this.CurrentQuest && this.ActiveQuests.Any())
        {
            this.CurrentQuest = this.ActiveQuests.FirstOrDefault();

            if (this.CurrentQuest)
            {
                if (string.IsNullOrEmpty(this.CurrentQuest.Title))
                {
                    this.CurrentQuest.Title = this.CurrentQuest.Tasks.FirstOrDefault()?.Title;
                }

                if (string.IsNullOrEmpty(this.CurrentQuest.Description))
                {
                    this.CurrentQuest.Description = this.CurrentQuest.Tasks.FirstOrDefault()?.Description;
                }
            }
           
        }
    }

    public void AddQuest(QuestLine questLine)
    {
        questLine.OnQuestComplete.AddListener(OnQuestComplete);

        this.ActiveQuests.Add(questLine);

        this.CurrentQuest ??= questLine;
    }

    private void OnQuestComplete(QuestLine questLine, QuestReward reward)
    {
        if (this.ActiveQuests.Any(x => x.Title == questLine.Title))
        {
            this.ActiveQuests.Remove(questLine);
        }

        if (this.CurrentQuest.Title == questLine.Title && this.ActiveQuests.Count > 0)
        {
            this.ChangeCurrentQuest(this.ActiveQuests.ElementAtOrDefault(0));
        }

        // Todo: Implement your reward stuff here

    }

    public void ChangeCurrentQuest(QuestLine questLine)
    {
        this.CurrentQuest = questLine;
        questLine.OnQuestComplete.AddListener(OnQuestComplete);
    }
}
