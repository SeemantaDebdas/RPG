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


    public class QuestManager : MonoBehaviour
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
    }
}