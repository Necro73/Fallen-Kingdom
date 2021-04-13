using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    private Player player;
    private Transform staminabarhelper;
    private Transform staminabar;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        staminabarhelper = transform.GetChild(0);
        staminabar = transform.GetChild(1);
        Invoke(nameof(transformbar), 0.01F);
    }
    
    private void transformbar()
    {
        transform.DOScaleX(transform.lossyScale.x * player.Stamina_Max, 0);  
    }

    public void Refresh()
    {
        if (player.Stamina >= 0)
        {
            staminabar.DOScaleX(player.Stamina / player.Stamina_Max, 0);
            staminabarhelper.DOScaleX(player.Stamina / player.Stamina_Max, 0.5F);
        }
    }
}
