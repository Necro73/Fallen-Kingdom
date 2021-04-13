using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class MoveUnit : StandUnit
{
    [SerializeField] protected GameObject player;
    
    [SerializeField] protected float speed = 2.0F;
    [SerializeField] protected  AnimancerComponent _Animancer;
    [SerializeField] protected AnimationClip Idle_Clip;
    [SerializeField] protected AnimationClip Jump_Clip;
    [SerializeField] protected AnimationClip Move_Clip;
    [SerializeField] protected AnimationClip Die_Clip;
    [SerializeField] protected AnimationClip Attack_Clip;
    
    protected AnimancerState state;
    
    protected bool isGrounded = false;
    protected bool isDamaged = false;
    protected bool isEvade = false;
    
    protected Vector3 direction;
    private SpriteRenderer sprite;
    
    private void FixedUpdate()
    {
        CheckGround();
    }
    protected override void Update()
    {
        if (hp <= 0)
        {
            state = _Animancer.Play(Die_Clip);
            state.Events.OnEnd = () =>
            {
                Debug.Log("Mob is  dead");
                Die();
            };
        }

        if (state.Clip == Idle_Clip)
        {
            if (isGrounded) Move();
        }
        
    }

    private Sword sword;
    protected virtual void Awake()
    {
        state = _Animancer.Play(Idle_Clip);
        rigidbody = GetComponent<Rigidbody2D>(); 
        sprite = GetComponentInChildren<SpriteRenderer>();
        sword = Resources.Load<Sword>("Sword");
    }

    protected virtual void Move()
    {
        if (isGrounded) state = _Animancer.Play(Move_Clip);
        direction = transform.right;
        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position = Vector3.MoveTowards(transform.position,transform.position - direction, speed * Time.deltaTime);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        }

        state.Events.OnEnd = () =>
        {
            state = _Animancer.Play(Idle_Clip);
        };
    }
    
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
        isGrounded = colliders.Length > 1;

        if (!isGrounded) _Animancer.Play(Jump_Clip);
    }

    public override void RecieveDamage(float[] dmg)
    {
        if (player.GetComponent<Player>().isAttacking)
        {
            if (!isDamaged)
            {
                isDamaged = true;
                sprite.color = UnityEngine.Color.HSVToRGB(0, 0.75F, 1F, false);
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(transform.up * 2.0F, ForceMode2D.Impulse);
                rigidbody.AddForce(transform.right * (transform.localScale == new Vector3(-1, 1, 1) ? 1 : -1) * 2.0F, ForceMode2D.Impulse);
                float dmg_sum = 0;
                for (int i=0; i<dmg.Length; i++)
                {
                    dmg_sum = dmg_sum + dmg[i] - dmg[i] * defence[i];
                }
                hp = hp - dmg_sum;
                Debug.Log("Mob_hp: " + hp);  
                Invoke(nameof(EndImmortal), 0.1F); 
            }
        }
    }
    
    public void EndImmortal()
    {
        isDamaged = false;
        isEvade = false;
        sprite.color = UnityEngine.Color.white;
    }
}