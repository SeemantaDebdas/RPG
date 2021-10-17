using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPG
{
    public class JsonHelper
    {
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    }


    public class QuestManager : MonoBehaviour,IMessageReceiver
    {
        public Quest[] quests;

        private void Awake()
        {
            LoadQuestsFromDB();
            AssignQuests();
        }

        private void LoadQuestsFromDB()
        {
            using (StreamReader reader = new StreamReader("Assets/RPG Adventures/Scripts/Quest/QuestDB.json"))
            {
                string json = reader.ReadToEnd();
                var loadedQuests = JsonHelper.GetJsonArray<Quest>(json);
                quests = new Quest[loadedQuests.Length];
                quests = loadedQuests;
            }
        }

        private void AssignQuests()
        {
            QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
            if(questGivers !=null && questGivers.Length > 0)
            {
                foreach (QuestGiver questGiver in questGivers)
                {
                    AssignQuestsToQuestGivers(questGiver);
                }
            }
         
        }

        private void AssignQuestsToQuestGivers(QuestGiver questGiver)
        {
            foreach(var quest in quests)
            {
                if (questGiver.GetComponent<UniqueID>().UID == quest.questGiver)
                {
                    questGiver.quest = quest;
                }
            }
          
        }

        public void OnReceiveMessage(MessageType type, object damageable, object message)
        {
            if (type == MessageType.Dead)
            {
                CheckEnemyDead((Damageable)damageable,(Damageable.DamageMessage)message);
            }
        }

        private void CheckEnemyDead(Damageable damagable, Damageable.DamageMessage message)
        {
            var questLog = message.damageSource.GetComponent<QuestLog>();
            if (questLog != null)
            {
                foreach(var quest in questLog.acceptedQuests)
                {
                    if(quest.questStatus == QuestStatus.ACCEPTED) {
                        if (quest.type == QuestType.HUNT &&
                            Array.Exists(quest.target,(targetUID) => damagable.GetComponent<UniqueID>().UID == targetUID))
                        {
                            quest.amount--;
                            if (quest.amount <= 0)
                            {
                                quest.questStatus = QuestStatus.COMPLETED;
                                Debug.Log("MissionAccomplished: " + quest.uID);
                            }
                        }
                    }
                }
            }
        }
    }
}