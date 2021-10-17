using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Called");
    }

    public void Started()
    {
        Debug.Log("Clicked");
    }
}
