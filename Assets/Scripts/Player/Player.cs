using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Animancer;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Color = System.Drawing.Color;
using Vector3 = UnityEngine.Vector3;

public class Player : Unit
{
    [SerializeField] private float speed = 4.0F;
    [SerializeField] private float jumpForce = 10.0F;

    protected AnimancerState state;
    [SerializeField] protected  AnimancerComponent _Animancer;
    [SerializeField] protected AnimationClip Idle_Clip;
    [SerializeField] protected AnimationClip Jump_Clip;
    [SerializeField] protected AnimationClip Move_Clip;
    [SerializeField] protected AnimationClip Die_Clip;

    [SerializeField] private int health_power;
    [SerializeField] private int stamina_power;
    [SerializeField] private int magic_power;
    [SerializeField] private int strength;
    [SerializeField] private int agility;
    [SerializeField] private int sapience;

    public int Health_power
    {
        get { return health_power; }
        set { int health_power = value; }
    }
    public int Stamina_power
    {
        get { return stamina_power; }
        set { int stamina_power = value; }
    }
    public int Magic_power
    {
        get { return magic_power; }
        set { int magic_power = value; }
    }
    public int Strength
    {
        get { return strength; }
        set { int strength = value; }
    }
    public int Agility
    {
        get { return agility; }
        set { int agility = value; }
    }
    public int Sapience
    {
        get { return sapience; }
        set { int sapience = value; }
    }
    
    private int hp_max;
    private int stamina_max;
    private int mana_max;

    private int hp;
    private int stamina;
    private float mana;

    private float[] defence = new float[6];

    public int frezestamina = 0;

    public bool isAttacking = false;
    public bool cooldown = false;
    private bool isDamaged = false;
    private bool isGrounded = false;
    private bool isEvade = false;

    private Sword sword;
    private HealthBar hpbar;
    private StaminaBar staminabar;
    private ManaBar manabar;

    public float Hp_Max
    {
        get { return hp_max; }
    }
    public float Hp
    {
        get { return hp; }
        set
        {
            if (value < hp_max) hp = (int)value;
            hpbar.Refresh();
        }
    }
    
    public float Stamina_Max
    {
        get { return stamina_max; }
    }
    public float Stamina
    {
        get { return stamina; }
        set
        {
            if (value < stamina_max) stamina = (int)value;
            staminabar.Refresh();
        }
    }

    public float Mana_Max
    {
        get { return mana_max; }
    }
    public float Mana
    {
        get { return mana; }
        set
        {
            if (value < mana_max) mana = value;
            manabar.Refresh();
        }
    }
    
    public Rigidbody2D rigidbody;
    public SpriteRenderer sprite;

    private void Awake()
    {
        hp_max = health_power * 5;
        stamina_max = stamina_power * 5;
        mana_max = magic_power * 5;
        
        
        hp = hp_max;
        stamina = stamina_max;
        mana = mana_max;
        
        hpbar = FindObjectOfType<HealthBar>();
        staminabar = FindObjectOfType<StaminaBar>();
        manabar = FindObjectOfType<ManaBar>();

        defence[TE.Physic] = 0.1F;    //Физическая защита
        defence[TE.Fire] = 0.1F;    //Огненная защита
        defence[TE.Cold] = 0.1F;    //Ледяная защита
        defence[TE.Poison] = 0.1F;    //Отравляющая защита
        defence[TE.Mystic] = 0.1F;    //Мистическая защита
        defence[TE.Divine] = 0;     //Божественная защита
        
        state = _Animancer.Play(Idle_Clip);
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        InvokeRepeating(nameof(StaminaRecovery), 0.1F, 0.1F);
        InvokeRepeating(nameof(ManaRecovery), 0.1F, 0.1F);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Hp = 0;
            isDamaged = true;
            state = _Animancer.Play(Die_Clip);
            state.Events.OnEnd = () =>
            {
                //Die();
            };
        }
        else
        {
            if (!isGrounded) state = _Animancer.Play(Jump_Clip);
            if (Input.GetButtonDown("Fire1")) Attack();
            if (isGrounded && Input.GetButtonDown("Jump")) Jump();
            if (Input.GetButtonDown("Evade")) Evade();
            if (Input.GetButtonDown("Cast")) Cast();
            
            if (Input.GetButton("Horizontal"))
            {
                Run();
            }
            else
            {
                _Animancer.Play(Idle_Clip);
            } 
        }

        if (frezestamina < 1000) EndFrezeStamina();



    }

    private void Run()
    {
        if (isGrounded) state = _Animancer.Play(Move_Clip);
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;
        if (sprite.flipX == true) transform.Find("Sword").localScale = new Vector3(-1, 1, 1);
        else transform.Find("Sword").gameObject.transform.localScale = new Vector3(1, 1, 1);
        
        state.Events.OnEnd = () =>
        {
            state = _Animancer.Play(Idle_Clip);
        };
        
    }

    private void Jump()
    {
        if (stamina >= 10)
        {
            frezestamina = 0;
            Stamina = Stamina - 10;
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            Invoke(nameof(EndFrezeStamina), 2F);
        }
    }

    private void Attack()
    {
        sword = GetComponentInChildren<Sword>();
        if (cooldown == false)
        {
            if (stamina >= 5)
            {
                sword.Attack();
            }
        }
    }

    private void Evade()
    {
        if (stamina >= 7)
        {
            if (!isEvade)
            {
                isEvade = true;
                isDamaged = true;
                frezestamina = 0;
                Stamina = Stamina - 7;
                sprite.color = new UnityEngine.Color(255, 255, 255, 0.75F);
                rigidbody.AddForce(transform.right * jumpForce / 2 * (sprite.flipX == true ? -1: 1), ForceMode2D.Impulse);
                rigidbody.AddForce(transform.up * jumpForce / 3, ForceMode2D.Impulse);
                Invoke(nameof(EndImmortal), 0.3F);  
                Invoke(nameof(EndFrezeStamina), 2F); 
            }
        }
    }

    private void StaminaRecovery()
    {
        if (frezestamina >= 1000)
            if (stamina < stamina_max)
                Stamina = Stamina + 2;
        if (stamina > stamina_max) Stamina = Stamina_Max;
    }

    private void ManaRecovery()
    {
        Mana = Mana + 0.1F;
    }

    private void Cast()
    {
        if (mana >= 3)
        {
            Mana = Mana - 3;
        }
    }
    
    public override void RecieveDamage(float[] dmg)
    {
        if (isDamaged == false)
        {
            isDamaged = true;
            float dmg_sum = 0;
            for (int i=0; i<dmg.Length; i++)
            {
                dmg_sum = dmg_sum + dmg[i] - dmg[i] * defence[i];
            }
            Hp = Hp - dmg_sum;

            sprite.color = UnityEngine.Color.HSVToRGB(0, 0.75F, 1F, false);
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(transform.up * 6.0F, ForceMode2D.Impulse);
            rigidbody.AddForce(transform.right * (sprite.flipX == true ? 1 : -1) * 6.0F, ForceMode2D.Impulse);
            Debug.Log(hp);
            Invoke(nameof(EndImmortal), 0.1F);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
    public void EndCooldown()
    {
        cooldown = false;
    }

    public void EndImmortal()
    {
        isDamaged = false;
        isEvade = false;
        sprite.color = UnityEngine.Color.white;
    }
    
    public void EndFrezeStamina()
    {
        frezestamina++;
    }
    
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
        isGrounded = colliders.Length > 1;
    }
}