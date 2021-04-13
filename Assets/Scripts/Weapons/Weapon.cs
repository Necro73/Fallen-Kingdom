using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Player player;
    
    protected int Combo = 0;
    protected int OldCombo = 0;

    protected bool block = false;
    protected int vect;
    protected Sequence mySequence;
    
    [SerializeField] protected float needstamina;
    
    protected float[] base_dmg = new float[6];
    protected float[] dmg = new float[6];
    protected int[] bonusdamagehero = new int[3];
    
    public float[] DMG
    {
        get { return dmg; }
    }

    public void DamageInit()
    {
        for(int i=0; i<6; i++)
        {
           dmg[i] = base_dmg[i] + base_dmg[i] * 0.01F * (player.Strength * bonusdamagehero[BonusDmgType.Strength] + player.Agility * bonusdamagehero[BonusDmgType.Agility] + player.Sapience * bonusdamagehero[BonusDmgType.Sapience]);
        }
    }
    
    public void Attack()
    {
        player = GetComponentInParent<Player>();
        DamageInit();
        player.cooldown = true;
        player.isAttacking = true;
        player.frezestamina = 0;
        player.Stamina = player.Stamina - needstamina;
        PlayAnimation();
    }

    protected virtual void PlayAnimation()
    {
        
    }

    protected void EA()
    {
        player.EndAttack(); 
    }

    protected void EC()
    {
        player.EndCooldown();
    }

    protected void EF()
    {
        player.EndFrezeStamina();
    }
}
