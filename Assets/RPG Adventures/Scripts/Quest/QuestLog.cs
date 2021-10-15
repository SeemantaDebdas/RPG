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

    public class AcceptedQuest : Quest
    {
        QuestStatus questStatus;
    }

    public class QuestLog : MonoBehaviour
    {
        public List<AcceptedQuest> acceptedQuests;
    }
}
