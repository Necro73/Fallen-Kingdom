using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprike : MonoBehaviour
{
    private float[] dmg = new float[6];

    protected void Start()
    {
        dmg[0] = 25;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit is Player)
        {
            unit.RecieveDamage(dmg);
        }
    }
}
