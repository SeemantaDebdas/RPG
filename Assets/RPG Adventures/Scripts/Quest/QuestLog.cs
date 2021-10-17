using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {

    public enum QuestStatus
    {
        ACCEPTED,
        FAILED,
        COMPLETED
    }

    [System.Serializable]
    public class AcceptedQuest : Quest
    {
        public QuestStatus questStatus;

        public AcceptedQuest(Quest quest)
        {
            uID = quest.uID;
            title = quest.title;
            description = quest.description;
            experience = quest.experience;
            gold = quest.gold;

            amount = quest.amount;
            target = quest.target; 

            talkTo = quest.talkTo;
            explore = quest.explore;

            questGiver = quest.questGiver;
            type = quest.type;

            questStatus = QuestStatus.ACCEPTED;
    }
    }

    public class QuestLog : MonoBehaviour
    {
        public List<AcceptedQuest> acceptedQuests = new List<AcceptedQuest>();

        public void AddQuest(Quest quest)
        {
            acceptedQuests.Add(new AcceptedQuest(quest));
        }
    }
}
