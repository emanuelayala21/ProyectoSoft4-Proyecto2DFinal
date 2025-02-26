using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI :MonoBehaviour {

    public Transform targetHouse; // The house that the enemy will attack

    private float _speed = 1.3f; // Speed at which the enemy moves
    public float _attackRange = 1f; // Range within which the enemy can attack the house
    //private float _enemyLife = 3f; // Enemy's life (currently unused)

    private Animator animator; // Animator to control enemy animations
    private SpriteRenderer spriteRenderer; // Sprite renderer to handle the enemy sprite (currently unused)


    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        EnemyMovement(); EnemyMovement(); // Call the function that controls the enemy movement
    }

    private void EnemyMovement() {
        float distance = Vector2.Distance(transform.position, new Vector2(targetHouse.position.x, transform.position.y)); // Calculate the distance to the house in the x-axis

        if(distance > _attackRange) { // If the enemy is not within attack range
            Vector2 targetPosition = new Vector2(targetHouse.position.x, transform.position.y); // Set the target position in the x-axis (same y as enemy)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); // Move the enemy towards the house
            animator.SetBool("isWalking", true); // Activate walking animation

        } else { // When the enemy reaches the house
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", true); // Activate attack animation

        }
    }
}