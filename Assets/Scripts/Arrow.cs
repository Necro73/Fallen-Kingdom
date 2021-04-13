using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;

public class Arrow : Unit
{
    private float speed = 0.37F;
    private Vector3 direction;
    private Vector3 scalyar;
    private Rigidbody2D rigidbody;
    public Vector3 Direction { set { direction = value; } }
    public float[] dmg = new float[6];

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        StandUnit st = collider.GetComponent<StandUnit>();
        Unit ar = collider.GetComponent<Arrow>();
        Player pl = collider.GetComponent<Player>();
        Sword sw = collider.GetComponent<Sword>();
        if (!st && !ar)
        {

            if (!pl & !sw) StartCoroutine(Timer((float) 0));
            else StartCoroutine(Timer((float) 0.1));
        }
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit is Player)
        {
            unit.RecieveDamage(dmg);
        }
    }
    
    private IEnumerator Timer(float x)
    {
        var coll = GetComponent<BoxCollider2D>();
        coll.enabled = !coll.enabled;
        rigidbody.AddForce(direction * -0.1F, ForceMode2D.Impulse);
        yield return new WaitForSeconds(x);
        RecieveDamage(dmg);
    }
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
        Destroy(gameObject, 10.0F);
    }

    private void Update()
    {
        float angle = Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
