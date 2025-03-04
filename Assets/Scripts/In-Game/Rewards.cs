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
        if(transform.position.y <= -3.76f) {
            transform.position = new Vector3(transform.position.x, -3.76f, transform.position.z); // Mantener la moneda en la posición de la barrera
        }
    }
    private IEnumerator DisappearAfterTime() {
        yield return new WaitForSeconds(3.0f);

        // Destruir el objeto después de esperar
        Destroy(gameObject);
    }
}
