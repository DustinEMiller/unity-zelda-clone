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

    [Header("State Machine")]
    public PlayerState currentState;

    [Header("Stats")]
    public float speed;
    public FloatValue currentHealth;
    
    [Header("Components")]
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    [Header("Signals")]
    public Signal playerHealthSignal;
    public Signal playerHit;

    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;

    // Use this for initialization
    void Start () {
        currentState = PlayerState.Walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }
	
	// Update is called once per frame
	void Update () {

        if(currentState == PlayerState.Interact) {
            return;
        }

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

        if(currentState != PlayerState.Interact) {
            currentState = PlayerState.Walk;
        }
    }

    public void RaiseItem() {
        if(playerInventory.currentItem != null) {
            if (currentState != PlayerState.Interact) {
                animator.SetBool("receiveItem", true);
                currentState = PlayerState.Interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else {
                animator.SetBool("receiveItem", false);
                currentState = PlayerState.Idle;
                receivedItemSprite.sprite = null;
            }
        }        
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
        playerHit.Raise();
        if (myRigidBody != null) {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            myRigidBody.velocity = Vector2.zero;
        }
    }

    public void Knock(float knockTime, float damage) {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();

        if (currentHealth.RuntimeValue > 0) {
            StartCoroutine(KnockCo(knockTime));
        } else {
            this.gameObject.SetActive(false);
        }

    }
}
