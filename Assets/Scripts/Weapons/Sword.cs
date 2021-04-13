using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Sword : Weapon
{
   protected void Start()
   {
      base_dmg[TE.Physic] = 10;  //Физический урон
      base_dmg[TE.Fire] = 0;   //Огненный урон
      base_dmg[TE.Cold] = 0;   //Ледяной урон
      base_dmg[TE.Poison] = 0;   //Отравляющий урон
      base_dmg[TE.Mystic] = 0;   //Мистический урон
      base_dmg[TE.Divine] = 0;   //Божественный урон  

      bonusdamagehero[BonusDmgType.Strength] = BonusDmgMod.C;
      bonusdamagehero[BonusDmgType.Agility] = BonusDmgMod.C;
      bonusdamagehero[BonusDmgType.Sapience] = BonusDmgMod.F;

      needstamina = 5;
   }

   protected override void PlayAnimation()
   {
      Combo++;
      var mySequence = DOTween.Sequence();
      var vect = player.sprite.flipX == true ? 1: -1;
      switch (Combo)
      {
         case 1:
         {
            
            transform.DORotate(new Vector3(0, 0,110 * vect), 0.2F);
            transform.DOLocalMoveX(0.3F * -vect, 0.15F);
            Invoke(nameof(EC), 0.2F);
            if (!block)
            {
               block = true;
               OldCombo = Combo;
               Invoke(nameof(EndAnim), 0.4F);
            }
            break;
         }
         case 2:
         {
            transform.DORotate(new Vector3(0, 0,30 * vect), 0.15F);
            Invoke(nameof(EC), 0.2F);
            if (!block)
            {
               block = true;
               OldCombo = Combo;
               Invoke(nameof(EndAnim), 0.4F);
            }
            break;
         }
         case 3:
         {
            mySequence.Append(transform.DORotate(new Vector3(0, 0,75 * vect), 0.075F));
            mySequence.Join(transform.DOScaleX(0.2F, 0.075F));
            mySequence.Append(transform.DORotate(new Vector3(0, 0,120 * vect), 0.075F));
            mySequence.Join(transform.DOScaleX(1F, 0.075F));
            Invoke(nameof(EC), 0.4F);
            Invoke(nameof(EndCombo), 0.4F);
            Invoke(nameof(EndAnim), 0.4F);
            break;
         }
      }



      Invoke(nameof(EA), 0.15F);
      Invoke(nameof(EF), 2F);
   }

   private void EndAnim()
   {
      block = false;
      if (Combo == OldCombo)
      {
         EndCombo();
         mySequence.Append(transform.DORotate(new Vector3(0, 0, 0), (float)0.15F));
         mySequence.Join(transform.DOLocalMoveX(0, 0.15F));
         transform.DOScaleX(1, 0.2F);
      }
   }

   private void EndCombo()
   {
      Combo = 0;
      OldCombo = 0;
   }
}
