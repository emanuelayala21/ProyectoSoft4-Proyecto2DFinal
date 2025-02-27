using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet :MonoBehaviour {

    private Rigidbody2D _rb;
    private MainHouse player;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MainHouse>();
    }
   
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Enemy")) { // Check if the collision is with an object tagged as "Enemy"

            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            Debug.Log("Bullet hits enemy ");
            enemy.TakeDamage(player.damage);

            Destroy(this.gameObject);// Destroy the bullet itself\

        }
        if(collision.CompareTag("Ground")) { // Destroy the bullet when it hits the ground
            Destroy(this.gameObject);

        }
    }
}
