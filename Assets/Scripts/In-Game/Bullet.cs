using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet :MonoBehaviour {

    private MainHouse player;

    void Start() {
        player = FindObjectOfType<MainHouse>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Enemy")) { // Check if the collision is with an object tagged as "Enemy"

            EnemyAI enemy = collision.GetComponent<EnemyAI>();

            bool isCriticalHit = IsCriticalHit();  // Determine if the hit is a critical hit
            float finalDamage = isCriticalHit ? player.damage * 2.5f : player.damage; // Apply critical hit damage if applicable

            Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized; // Calculate knockback direction

            enemy.TakeDamage(finalDamage, knockbackDir, player.knockback); // Apply damage and knockback to the enemy

            Destroy(this.gameObject);// Destroy the bullet itself\

        }
        if(collision.CompareTag("Ground")) { // Check if the bullet hit the ground
            Destroy(this.gameObject);

        }
    }
    private bool IsCriticalHit() {
        float critChance = player.criticChance; // Gets player critical hit chance
        float randomValue = Random.Range(0f, 100f); // Generate a random value between 0 and 100

        return randomValue <= critChance; // Return true if the random value is less than or equal to the critChance
    }
}