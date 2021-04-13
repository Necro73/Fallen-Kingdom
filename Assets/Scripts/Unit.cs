using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public virtual void RecieveDamage(float[] dmg)
    {
        Die();  
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
