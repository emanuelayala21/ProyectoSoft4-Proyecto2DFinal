using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI :MonoBehaviour {

    private Transform _targetHouse; // The house that the enemy will attack

    private float _speed = 0.6f; // Speed at which the enemy moves
    private float _attackRange = 1.7f; // Range within which the enemy can attack the house
    private float _damage = 3f;

    private Animator animator; // Animator to control enemy animations
    private SpriteRenderer spriteRenderer; // Sprite renderer to handle the enemy sprite (currently unused)

    private FloatingHealthBar _healthBar;
    public float enemyHealth = 10;
    private float _enemyMaxHealth; // Enemy's life max

    public GameObject coin_reward;

    private MainHouse playerScript; // Referencia al script de la casa para llamar TakeDamage

    private float timeBetweenAttacks = 2f; // Tiempo en segundos entre ataques
    private float timeSinceLastAttack = 0f;

    private void Awake() {
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        _targetHouse = FindObjectOfType<MainHouse>().transform;
        playerScript = _targetHouse.GetComponent<MainHouse>();

        _enemyMaxHealth = enemyHealth;
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth);
    }

    void Update() {
        EnemyMovement(); EnemyMovement(); // Call the function that controls the enemy movement
    }

    private void EnemyMovement() {
        float distance = Vector2.Distance(transform.position, new Vector2(_targetHouse.position.x, transform.position.y)); // Calculate the distance to the house in the x-axis

        if(enemyHealth <= 0) { // If the enemy is dead, it cannot attack
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", false); // Deactivate attack animation
            return; // Exit the method if the enemy is dead
        }

        if(distance > _attackRange) { // If the enemy is not within attack range
            Vector2 targetPosition = new Vector2(_targetHouse.position.x, transform.position.y); // Set the target position in the x-axis (same y as enemy)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); // Move the enemy towards the house
            animator.SetBool("isWalking", true); // Activate walking animation

        } else { // When the enemy reaches the house
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", true); // Activate attack animation

            timeSinceLastAttack += Time.deltaTime; // Increase time since last attack

            if(timeSinceLastAttack >= timeBetweenAttacks) { // If enough time has passed
                timeSinceLastAttack = 0f; // Reset the timer
                AttackHouse(); // Call the function that damages the house
            }
        }
    }
    private void AttackHouse() {
        if(playerScript != null) { // Check if the playerScript is assigned
            playerScript.TakeDamage(_damage); // Call TakeDamage from the house (example with 2 damage)
        }
    }
    public void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance) {
        if(enemyHealth <= 0) { // If the enemy is already dead, exit the method
            return;
        }

        enemyHealth -= damageAmount; // Subtract the damage from the enemy's health
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth); // Update the health bar with the current health

        Vector2 adjustedKnockback = new Vector2(-knockbackDirection.x, 0); // Only apply knockback in the X axis
        StartCoroutine(ApplyKnockback(adjustedKnockback, knockbackDistance)); // Apply knockback effect

        if(enemyHealth <= 0) { // If the enemy's health reaches zero or below, handle death
            GameObject coin = Instantiate(coin_reward, transform.position, Quaternion.identity); // Spawn a coin reward

            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", false); // Deactivate attack animation
            animator.Play("enemy_dead"); // Play death animation
            StartCoroutine(DieAfterDelay()); // Call the coroutine to destroy the enemy after a delay

            playerScript.IncreaseCoins(1); // Give coins to the player
        }
    }
    private IEnumerator DieAfterDelay() {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        Destroy(gameObject); // Destroy the enemy GameObject
    }

    private IEnumerator ApplyKnockback(Vector2 direction, float distance) {
        float maxKnockbackDuration = 0.2f; // Maximum duration for the knockback effect
        float knockbackDuration = Mathf.Clamp(maxKnockbackDuration * (distance / 2), 0.1f, maxKnockbackDuration); // Calculate the knockback duration based on distance
        float elapsedTime = 0f;

        Vector2 startPosition = transform.position; // Record the starting position of the enemy

        Vector2 targetPosition = new Vector2(// Calculate the target knockback position (only on the X axis)
            startPosition.x + (direction.normalized.x * distance), // Apply knockback to the X axis
            startPosition.y // Keep the Y position the same to avoid the issue where enemy rotates haha
        );

        while(elapsedTime < knockbackDuration) {  // Move the enemy to the knockback position smoothly
            transform.position = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / knockbackDuration)); // Smoothly move the enemy
            elapsedTime += Time.deltaTime; // Increase the elapsed time
            yield return null; // Wait for the next frame
        }
        transform.position = targetPosition; // Ensure the enemy reaches the final knockback position
    }
}