using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG {
    public class DialogueManager : MonoBehaviour
    {
        public bool HasActiveDialogue { get { return activeDialogue != null; } }
        public float DialogueDistance { get { return Vector3.Distance(characterInput.transform.position, NPC.transform.position); } }

        [SerializeField] GameObject dialogueUI;
        [SerializeField] TextMeshProUGUI answerText;
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] float interactableDistance = 5f;

        [SerializeField] Button dialogueButtonPrefab;
        [SerializeField] Button closeDialogueButton;
        [SerializeField] GameObject dialogueOptionList;

        [SerializeField] float readingTimer = 5f;
        float readingTimerCounter;

        CharacterInput characterInput;
        Dialogue activeDialogue;
        QuestGiver NPC;

        bool forceQuitDialogue;

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

            if (readingTimerCounter > 0)
            {
                readingTimerCounter += Time.deltaTime;
                if (readingTimerCounter >= readingTimer)
                {
                    readingTimerCounter = 0;
                    if (forceQuitDialogue)
                    {
                        StopDialogue();
                    }
                    else
                    {
                        DisplayDialogue();
                    }
                }
            }
        }

        
        
        void CreateDialogueMenu()
        {
            var queries = Array.FindAll(activeDialogue.queries, query => !query.isAsked);

            foreach(var query in queries)
            {
                var dialogueButton = CreateDialogueMenuButtons(query.text);
                RegisterOptionClickerHandler(dialogueButton,query);
            }
        }

        Button CreateDialogueMenuButtons(string buttonText)
        {
            Button buttonInstance =Instantiate(dialogueButtonPrefab, dialogueOptionList.transform);
            buttonInstance.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            return buttonInstance;
        }

        void RegisterOptionClickerHandler(Button dialogueButton,DialogueQuestion query)
        {
            //adding event trigger like OnClick and adding it to the button
            EventTrigger trigger = dialogueButton.gameObject.AddComponent<EventTrigger>();
            //creating a pointer down event trigger. Mouse CLick
            var pointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            pointerDown.callback.AddListener((e) =>
            {
                if (!string.IsNullOrEmpty(query.answer.questID))
                {
                    characterInput.GetComponent<QuestLog>().AddQuest(NPC.quest);
                }
                if (query.answer.forceDialogueQuit)
                {
                    forceQuitDialogue = true;
                }
                if (!query.isAlwaysAsked)
                {
                    query.isAsked = true;
                }

                ClearDialogueOptions();
                DisplayAnswerText(query.answer.text);
                TriggerDialgueReadingTimerCounter();
            });

            trigger.triggers.Add(pointerDown);
        }

        void StartDialogue()
        {
            activeDialogue = NPC.dialogue;
            headerText.text = NPC.name;
            dialogueUI.SetActive(true);

            DisplayDialogue();
            NPC.interacted = true;
        }

        public void StopDialogue()
        {
            readingTimerCounter = 0;
            forceQuitDialogue = false;
            activeDialogue = null;
            NPC = null;
            dialogueUI.SetActive(false);
        }

        void TriggerDialgueReadingTimerCounter()
        {
            readingTimerCounter = 0.001f;
        }

        void DisplayDialogue()
        {
            DisplayAnswerText((NPC.interacted)?activeDialogue.postInteractionText:activeDialogue.welcomeText);
            ClearDialogueOptions();
            CreateDialogueMenu();
        }

        void DisplayAnswerText(string text)
        {
            answerText.text = text;
        }

        void ClearDialogueOptions() { 
            
            foreach(Transform child in dialogueOptionList.transform)
            {
                if(child!=null)
                    Destroy(child.gameObject);
            }
        }
    }
}
