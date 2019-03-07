using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : Interactable {

    public Item contents;
    public Inventory playerInventory;
    public Signal raiseItem;
    public GameObject dialogBox;
    public Text dialogText;
    public bool isOpen;
    public bool empty = false;

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange) {
            if(!isOpen) {
                OpenChest();
            } else {
                ConfirmChest();
            }
        }
    }

    public void OpenChest() {
        dialogBox.SetActive(true);
        dialogText.text = contents.itemDescription;

        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;

        raiseItem.Raise();
        
        context.Raise();
        isOpen = true;
        anim.SetBool("opened", true);
    }

    public void ConfirmChest() {
        if(!empty) {
            dialogBox.SetActive(false);
            raiseItem.Raise();
            empty = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerMovement>() && !other.isTrigger && !isOpen) {
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerMovement>() && !other.isTrigger && !isOpen) {
            context.Raise();
            playerInRange = false;
        }
    }
}
