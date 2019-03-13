using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUp {

    public FloatValue playerHealth;
    public float amountToIncrease;
    public FloatValue heartContainers;

    public void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.GetComponent<PlayerMovement>() && !other.isTrigger) {
            playerHealth.RuntimeValue += amountToIncrease;

            if(playerHealth.initialValue > heartContainers.RuntimeValue * 2f) {
                playerHealth.initialValue = heartContainers.RuntimeValue * 2f;
            }
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }

    }

}
