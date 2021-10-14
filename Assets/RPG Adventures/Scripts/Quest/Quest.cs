using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    HUNT,
    GATHER,
    TALK,
    EXPLORE
}

[System.Serializable]
public class Quest 
{
    public string uID;
    public string title;
    public string description;
    public int experience;
    public int gold;

    public int amount;
    public string[] target;

    public string talkTo;
    public Vector3 explore;

    public string questGiver;
    public QuestType type;
}
