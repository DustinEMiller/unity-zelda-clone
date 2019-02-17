using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Walk,
    Attack,
    Interact,
    Stagger,
    Idle
}

public class PlayerMovement : MonoBehaviour {

    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;

	// Use this for initialization
	void Start () {
        currentState = PlayerState.Walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }
	
	// Update is called once per frame
	void Update () {

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("attack") && currentState != PlayerState.Attack && currentState != PlayerState.Stagger) {
            StartCoroutine(AttackCo());
        } else if(currentState == PlayerState.Walk || currentState == PlayerState.Idle) {
            UpdateAnimationAndMove();
        }
        
	}

    private IEnumerator AttackCo() {
        animator.SetBool("attacking", true);
        currentState = PlayerState.Attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.Walk;
    }

    void UpdateAnimationAndMove() {
        if (change != Vector3.zero) {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter() {
        change.Normalize();
        myRigidBody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    private IEnumerator KnockCo(float knockTime) {
        if (myRigidBody != null) {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            myRigidBody.velocity = Vector2.zero;
        }
    }

    public void Knock(float knockTime, float damage) {
        currentHealth.initialValue -= damage;
        if (currentHealth.initialValue > 0) {
            playerHealthSignal.Raise();
            StartCoroutine(KnockCo(knockTime));
        }
    }
}
