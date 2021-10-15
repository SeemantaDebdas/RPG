using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG {
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] GameObject DialogueUI;
        [SerializeField] TextMeshProUGUI WelcomeText;
        bool dialogueActive = false;

        private void Awake()
        {
            DialogueUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(CharacterInput.Instance.HitTarget!=null && Character.Instance != null && !dialogueActive)
            {
                if(CharacterInput.Instance.HitTarget.CompareTag("QuestGiver"))
                    StartDialogue();
            }
        }

        void StartDialogue()
        {
            dialogueActive = true;
            DialogueUI.SetActive(true);
            WelcomeText.text = "Just Testing";
        }
    }
}
