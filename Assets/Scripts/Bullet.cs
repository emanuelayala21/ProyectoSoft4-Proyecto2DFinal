using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet :MonoBehaviour {

    private Rigidbody2D _rb;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();

    }
   
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Enemy")) { // Check if the collision is with an object tagged as "Enemy"

            Destroy(collision.gameObject); // Destroy the enemy object when the bullet hits it
            Destroy(this.gameObject);// Destroy the bullet itself

        }
        if(collision.CompareTag("Ground")) { // Destroy the bullet when it hits the ground
            Destroy(this.gameObject);

        }
    }
}
