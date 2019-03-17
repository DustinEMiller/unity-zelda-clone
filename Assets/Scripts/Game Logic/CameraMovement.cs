using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [Header("Position Variables")]
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition; 
    public Vector2 minPosition;

    [Header("Animator")]
    public Animator anim;

    [Header("Position Reset")]
    public VectorValue cameraMin;
    public VectorValue cameraMax;

    // Use this for initialization
    void Start () {
        maxPosition = cameraMax.initialValue;
        minPosition = cameraMin.initialValue;
        anim = GetComponent<Animator>();
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(transform.position != target.position) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);


        }
	}

    public void BeginKick() {
        anim.SetBool("kickActive", true);
        StartCoroutine(KickCo());
    }

    public IEnumerator KickCo() {
        yield return null;
        anim.SetBool("kickActive", false);
    }
}
