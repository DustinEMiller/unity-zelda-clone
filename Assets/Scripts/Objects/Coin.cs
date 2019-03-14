using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PowerUp {

    public Inventory playerInventory;

    void Start() {
        powerupSignal.Raise();
    }


    public void OnTriggerEnter2D(Collider2D other) {
  
        if (other.gameObject.GetComponent<PlayerMovement>() && !other.isTrigger) {
            playerInventory.coins++;
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }

    }
}
