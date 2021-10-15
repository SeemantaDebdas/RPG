using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
    public class UniqueID : MonoBehaviour
    {
        [SerializeField]
        string uID = Guid.NewGuid().ToString();

        public string UID { get { return uID; } }
    }
}
