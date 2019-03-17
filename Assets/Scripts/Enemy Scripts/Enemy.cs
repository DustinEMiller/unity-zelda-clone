using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Stagger
}


public class Enemy : MonoBehaviour {

    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Stats")]
    public FloatValue maxHealth;
    public float health;

    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;


    private void Awake() {
        health = maxHealth.initialValue;
    }
    // Use this for initialization
    void Start () {
        health = maxHealth.initialValue;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void TakeDamage(float damage) {

        health -= damage;
        if(health <= 0) {
            DeathEffect();
            this.gameObject.SetActive(false);
        }
    }

    private void DeathEffect() {
        if(deathEffect != null) {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }

    public void Knock(Rigidbody2D hitObject, float knockTime, float damage) {
        StartCoroutine(KnockCo(hitObject, knockTime));
        TakeDamage(damage);
    } 

    protected void ChangeState(EnemyState newState) {
        if (currentState != newState) {
            currentState = newState;
        }
    }

    private IEnumerator KnockCo(Rigidbody2D hitObject, float knockTime) {
        if (hitObject != null) {
            yield return new WaitForSeconds(knockTime);
            hitObject.velocity = Vector2.zero;
            hitObject.GetComponent<Enemy>().currentState = EnemyState.Idle;
        }
    }
}
