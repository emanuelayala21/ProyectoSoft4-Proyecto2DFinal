using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewards :MonoBehaviour {

    private void Start() {
        StartCoroutine(DisappearAfterTime());
    }

    void Update() {
        Limits();
    }
    private void Limits() {
        if(transform.position.y <= -3.76f) { // Check if the object's Y position is below the lower limit
            transform.position = new Vector3(transform.position.x, -3.76f, transform.position.z); // Clamp the Y position to the lower limit (-3.76f)
        }
    }
    private IEnumerator DisappearAfterTime() {
        yield return new WaitForSeconds(3.0f); // Wait for 3 seconds before destroying the object
        Destroy(gameObject); // Destroy the object after the delay
    }
}
