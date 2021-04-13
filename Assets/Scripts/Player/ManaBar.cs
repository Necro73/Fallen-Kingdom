using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    private Player player;
    private Transform manabarhelper;
    private Transform manabar;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        manabarhelper = transform.GetChild(0);
        manabar = transform.GetChild(1);
        Invoke(nameof(transformbar), 0.01F); 
    }

    private void transformbar()
    {
        transform.DOScaleX(transform.lossyScale.x * player.Mana_Max, 0);  
    }

    public void Refresh()
    {
        if (player.Mana >= 0)
        {
            manabar.DOScaleX(player.Mana / player.Mana_Max, 0);
            manabarhelper.DOScaleX(player.Mana / player.Mana_Max, 0.5F);
        }
    }
}
