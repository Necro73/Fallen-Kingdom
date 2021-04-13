using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StandUnit : Unit
{
    [SerializeField] protected float max_hp;
    [SerializeField] protected float hp;
    
    protected float[] dmg = new float[6];
    protected float[] defence = new float[6];
    
    protected Rigidbody2D rigidbody;
    protected Animator animator;
    protected SpriteRenderer sprite;
    
    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Sword sword = collider.GetComponent<Sword>();

        if (sword)
        {
            RecieveDamage(sword.DMG);
        }
        
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit is Player)
        {
            unit.RecieveDamage(dmg);
        }
    }

    public override void RecieveDamage(float[] dmg)
    { 
        float dmg_sum = 0;
        for (int i=0; i<dmg.Length; i++)
        {
            dmg_sum = dmg_sum + dmg[i] - dmg[i] * defence[i];
        }
        hp = hp - dmg_sum;
        if (hp <= 0) Die();
    }
}
