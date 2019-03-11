using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    Key,
    Enemy,
    Button
}

public class Door : Interactable {

    [Header("Door Variables")]
    public DoorType doorType;
    public bool open;
    public Inventory playerInventory;
    private SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    private void Start() {
        doorSprite = gameObject.GetComponentInParent<SpriteRenderer>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(playerInRange && doorType == DoorType.Key) {
                if(playerInventory.numberOfKeys > 0) {
                    playerInventory.numberOfKeys--;
                    Open();
                }
            }
        }
    }

    public void Open() {
        doorSprite.enabled = false;
        open = true;
        physicsCollider.enabled = false;
    }

    public void Closed() {

    }
}
