using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG {
    public class CinemachineCam : MonoBehaviour
    {
        public CinemachineFreeLook cineCam;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                cineCam.m_XAxis.m_MaxSpeed = 400;
                cineCam.m_YAxis.m_MaxSpeed = 10;
            }
            if (Input.GetMouseButtonUp(1))
            {
                cineCam.m_XAxis.m_MaxSpeed = 0;
                cineCam.m_YAxis.m_MaxSpeed = 0;
            }
        }
    }
}

