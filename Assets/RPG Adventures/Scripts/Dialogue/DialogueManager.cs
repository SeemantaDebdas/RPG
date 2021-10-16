using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace RPG {
    public class DialogueManager : MonoBehaviour
    {
        public bool HasActiveDialogue { get { return activeDialogue != null; } }
        public float DialogueDistance { get { return Vector3.Distance(characterInput.transform.position, NPC.transform.position); } }

        [SerializeField] GameObject dialogueUI;
        [SerializeField] TextMeshProUGUI answerText;
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] float interactableDistance = 5f;

        [SerializeField] Button buttonPrefab;
        [SerializeField] GameObject dialogueOptionList;

        CharacterInput characterInput;
        Dialogue activeDialogue;
        QuestGiver NPC;

        private void Awake()
        {
            dialogueUI.SetActive(false);
        }

        private void Start()
        {
            characterInput = CharacterInput.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if(characterInput.HitTarget!=null && !HasActiveDialogue)
            {
                if (characterInput.HitTarget.CompareTag("QuestGiver"))
                {
                    NPC = characterInput.HitTarget.GetComponent<QuestGiver>();
                    if (DialogueDistance < interactableDistance)
                    {
                        StartDialogue();
                    }
                }
                    
            }

            if(HasActiveDialogue && DialogueDistance > interactableDistance + 0.5f)
            {
                StopDialogue();
            }
        }

        private void StopDialogue()
        {
            activeDialogue = null;
            NPC = null;
            dialogueUI.SetActive(false);

        }
        
        void CreateDialogueMenu()
        {
            var queries = Array.FindAll(activeDialogue.queries, query => !query.isAsked);

            foreach(var query in queries)
            {
                var dialogueButton = CreateDialogueMenuButtons(query.text);
            }
        }

        Button CreateDialogueMenuButtons(string buttonText)
        {
            Button buttonInstance =Instantiate(buttonPrefab, dialogueOptionList.transform);
            buttonInstance.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            return buttonInstance;
        }

        void StartDialogue()
        {
            activeDialogue = NPC.dialogue;
            dialogueUI.SetActive(true);
            headerText.text = NPC.name;
            answerText.text = activeDialogue.welcomeText;


            CreateDialogueMenu();
        }
    }
}
