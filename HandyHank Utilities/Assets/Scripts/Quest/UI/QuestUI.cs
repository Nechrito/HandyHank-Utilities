using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public QuestManager Manager;

    public TMP_Text Text;

    private void Update()
    {
        if (this.Manager && this.Manager.CurrentQuest)
        {
            var quest = this.Manager.CurrentQuest;
            var title       = quest.Title.SplitCamelCase();
            var description = quest.Description.SplitCamelCase();

            if (title.Length > 0 && description.Length > 0)
            {
                this.Text.text = $"{description}";
            }
            else
            {
                this.Text.text = string.Empty;
            }
        }
    }
}
