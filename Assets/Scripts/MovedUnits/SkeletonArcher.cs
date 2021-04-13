using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = System.Random;

public class SkeletonArcher : MoveUnit
{
    [SerializeField] private float time_reload = 2;
    
    private Arrow arrow;
    private int ischarged = 1;

    protected void Start()
    {
        max_hp = 50;
        hp = max_hp;
        dmg[0] = 10;
    }

    protected override void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        state = _Animancer.Play(Idle_Clip);
        arrow = Resources.Load<Arrow>("Objects/Arrow");
    }

    protected void Reload()
    {
        if (ischarged < 1)
        {
            ischarged++;
        }
    }

    
    protected override void Update()
    {
        if (hp <= 0)
        {
            state = _Animancer.Play(Die_Clip);
            state.Events.OnEnd = () =>
            {
                Die();
            };
        }

        if (state.Clip == Idle_Clip)
        {
            if ((player.transform.position - transform.position).magnitude > 7.5F)
            {
                Move();
            }
            else
            {
                if (ischarged > 0)
                {
                    ischarged--;
                    Shoot();
                }
            }
        }
    }
    
    private void Shoot()
    {
        state = _Animancer.Play(Attack_Clip);
        state.Events.OnEnd = () =>
        {
            Move();
            Vector3 position = transform.position;
            position.y += 0.5F;
            Arrow newArrow = Instantiate(arrow, position, arrow.transform.rotation);
            newArrow.dmg = dmg;
            newArrow.Direction = (newArrow.transform.position.x > player.transform.position.x ? -1 : 1) * Vector3.right + Vector3.up / 4 * Math.Abs(player.transform.position.y - newArrow.transform.position.y) + Vector3.up / UnityEngine.Random.Range(10, 100);
            Invoke(nameof(Reload), time_reload);
            state = _Animancer.Play(Idle_Clip);
        };
    }
}
