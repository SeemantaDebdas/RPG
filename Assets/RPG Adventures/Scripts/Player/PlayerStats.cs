using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG {
    public class PlayerStats : MonoBehaviour,IMessageReceiver
    {
        [SerializeField] int currentLevel;
        [SerializeField] int currentExperience;
        [SerializeField] int maxLevel;
        [SerializeField] int[] availableLevels;

        public int ExperienceToNextLevel { get { return availableLevels[currentLevel] - currentExperience; } }


        // Start is called before the first frame update
        void Awake()
        {
            availableLevels = new int[maxLevel];
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < maxLevel; i++)
            {
                int level = i + 1;
                float levelPow = Mathf.Pow(level, 2);
                int experienceToLevel = Convert.ToInt32(levelPow * maxLevel);
                availableLevels[i] = experienceToLevel;
            }
        }

        public void OnReceiveMessage(MessageType type, object damageable, object message)
        {
            if(type == MessageType.Dead)
                GainExperience((damageable as Damageable).experience);
        }

        public void GainExperience(int experience)
        {
            if (experience > ExperienceToNextLevel)
            {
                int remainderExperience = experience - ExperienceToNextLevel;
                
                currentLevel++;
                currentExperience = 0;

                GainExperience(remainderExperience);
            }
            else if(experience == ExperienceToNextLevel)
            {
                currentLevel++;
                currentExperience = 0;
            }
            else
            {
                currentExperience += experience;
            }
        }
    }
}
