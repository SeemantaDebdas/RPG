using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {

    [System.Serializable]
    public class DialogueAnswer
    {
        [TextArea(3,15)]
        public string text;
        public bool forceDialogueQuit;
        public string questID;
    }

    [System.Serializable]
    public class DialogueQuestion
    {
        [TextArea(3, 15)]
        public string text;
        public DialogueAnswer answer;
        public bool isAsked;
        public bool isAlwaysAsked;
    }

    [System.Serializable]
    public class Dialogue
    {
        [TextArea(3, 15)]
        public string welcomeText;
        [TextArea(3, 15)]
        public string postInteractionText;
        public DialogueQuestion[] queries;
    }
}
