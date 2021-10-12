using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] int damageVal;

    public void Damage()
    {
        Debug.Log("ApplyDamage");
    }
}
