using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour {

    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("breakable") && 
            this.gameObject.GetComponent<PlayerMovement>()) {
            other.GetComponent<Pot>().Smash();
        }

        if (other.gameObject.CompareTag("enemy") || other.gameObject.GetComponent<PlayerMovement>()) {
            Rigidbody2D hitObject = other.GetComponent<Rigidbody2D>();
            if (hitObject != null) {

                Vector2 difference = hitObject.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hitObject.AddForce(difference, ForceMode2D.Impulse);

                Debug.Log("Variables- Thrust: " + thrust + " knockTime: " + knockTime + " difference: " + difference);

                // This stuff is almost the same. It should probably use some sort of interface to test against
                // We Can consolodate code then
                if (other.gameObject.CompareTag("enemy")) {
                    hitObject.GetComponent<Enemy>().currentState = EnemyState.Stagger;
                    other.GetComponent<Enemy>().Knock(hitObject, knockTime, 0);
                }

                if (other.gameObject.GetComponent<PlayerMovement>()) {
                    if (other.GetComponent<PlayerMovement>().currentState != PlayerState.Stagger) {
                        hitObject.GetComponent<PlayerMovement>().currentState = PlayerState.Stagger;
                        other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                }

                
            }
        }
    }


  
}
