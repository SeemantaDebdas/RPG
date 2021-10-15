using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
    public class QuestGiver : MonoBehaviour
    {
        public Quest quest;

        private void OnMouseEnter()
        {
            Debug.Log("Mouse Enter");
        }

        private void OnMouseExit()
        {
            Debug.Log("Mouse Exit");
        }
    }
}
