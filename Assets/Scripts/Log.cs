﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{

    private Rigidbody2D myRigidbody;

    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;

    // Use this for initialization
    void Start() {
        ChangeState(EnemyState.Idle);
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        CheckDistance();
    }

    void CheckDistance() {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && 
            Vector3.Distance(target.position, transform.position) > attackRadius) {

            if(currentState == EnemyState.Idle || currentState == EnemyState.Walk && currentState != EnemyState.Stagger) {

                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.Walk);
                anim.SetBool("wakeUp", true);
            }
            
        } else if(Vector3.Distance(target.position, transform.position) > chaseRadius) {
            ChangeState(EnemyState.Idle);
            anim.SetBool("wakeUp", false);
        }
    }

    private void SetAnimFloat(Vector2 setVector) {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    private void ChangeAnim(Vector2 direction) {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x > 0) {
                SetAnimFloat(Vector2.right);
            } else if (direction.x < 0) {
                SetAnimFloat(Vector2.left);
            }
        } else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
            if (direction.y > 0) {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0) {
                SetAnimFloat(Vector2.down);
            }
        }
    }


}
