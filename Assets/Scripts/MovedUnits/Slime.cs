using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Slime : MoveUnit
{
   protected void Start()
   {
      max_hp = 30;
      hp = max_hp;
     
      dmg[TE.Physic] = 1;  //Физический урон
      dmg[TE.Fire] = 0;   //Огненный урон
      dmg[TE.Cold] = 0;   //Ледяной урон
      dmg[TE.Poison] = 9;   //Отравляющий урон
      dmg[TE.Mystic] = 0;   //Мистический урон
      dmg[TE.Divine] = 0;   //Божественный урон   

      defence[TE.Physic] = 0.75F;    //Физическая защита
      defence[TE.Fire] = 0.1F;    //Огненная защита
      defence[TE.Cold] = 0.1F;    //Ледяная защита
      defence[TE.Poison] = 0.9F;    //Отравляющая защита
      defence[TE.Mystic] = 0;    //Мистическая защита
      defence[TE.Divine] = 0;    //Божественная защита
   }
}
