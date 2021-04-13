using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Player player;
    private Transform hpbarhelper;
    private Transform hpbar;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        hpbarhelper = transform.GetChild(0);
        hpbar = transform.GetChild(1);
        Invoke(nameof(transformbar), 0.01F);
    }
    
    private void transformbar()
    {
        transform.DOScaleX(transform.lossyScale.x * player.Hp_Max, 0);  
    }

    public void Refresh()
    {
        if (player.Hp >= 0)
        {
            hpbar.DOScaleX(player.Hp / player.Hp_Max, 0);
            hpbarhelper.DOScaleX(player.Hp / player.Hp_Max, 0.5F);

        }
    }
}
