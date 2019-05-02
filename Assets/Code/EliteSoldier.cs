using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;

public class EliteSoldier : Enemy
{
    public float speed = 2f;
    private Transform target;
    public Animator animator;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>() ;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
   
        if (Vector2.Distance(transform.position, target.position) < 6f)
        {
            
            Vector3 moveDirection = target.position - transform.position;
            if (moveDirection != Vector3.zero)
            {
               
                if(moveDirection.x<0)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                else transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if(Vector2.Distance(transform.position, target.position) > 3f) animator.SetInteger("ES", 1);
        }
        
        else if (Vector2.Distance(transform.position, target.position) > 3f) animator.SetInteger("ES", 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Hero hero = collision.collider.GetComponent<Hero>();
        if (hero)
        {
            animator.SetInteger("ES", 2);
            hero.GetDamage();
        }
    }
}
