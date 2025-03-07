using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpear :EnemyAI {
    protected int attackCounter = 0; // Counts the number of attacks
    protected bool isBurstAttack = false; // Determines if the enemy is in burst mode
    protected override void Start() {
        _speed = 0.6f;
        _attackRange = 2.8f;
        _damage = 1f;
        timeBetweenAttacks = 2f;
        enemyHealth = 30f;
        base.Start(); // Call the base Start method 
    }
    public override void EnemyMovement() {
        float distance = Vector2.Distance(transform.position, new Vector2(_targetHouse.position.x, transform.position.y)); // Distance to the house in the x-axis

        if(enemyHealth <= 0) { // If the enemy is dead
            animator.SetBool("isWalking", false);
            animator.SetBool("Attack", false);
            animator.SetBool("isHit", false);
            return; // Exit the method if the enemy is dead
        }

        if(animator.GetBool("isHit")) {
            return; // If hit animation is playing, do not move
        }

        if(distance > _attackRange) { // If the enemy is not within attack range
            Vector2 targetPosition = new Vector2(_targetHouse.position.x, transform.position.y); // Target position in the x-axis (same y as the enemy)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); // Move the enemy

            animator.SetBool("isWalking", true); // Resume walking
            animator.SetBool("Attack", false);
            animator.SetBool("isHit", false);

        } else { // When the enemy reaches the house
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", true); // Activate attack animation

            timeSinceLastAttack += Time.deltaTime;

            if(timeSinceLastAttack >= timeBetweenAttacks) {
                timeSinceLastAttack = 0f;

                if(!isBurstAttack) {
                    AttackHouse(); // Perform the first normal attack
                    isBurstAttack = true; // Activate burst mode
                    attackCounter = 0; // Reset burst attack counter
                } else {
                    StartCoroutine(BurstAttack()); // Start burst attack sequence
                }
            }
        }
    }

    protected virtual IEnumerator BurstAttack() {
        int burstCount = 6; // Number of rapid attacks
        float burstInterval = 0.2f; // Time between burst attacks

        for(int i = 0; i < burstCount; i++) {
            AttackHouse(); // Deal damage multiple times
            yield return new WaitForSeconds(burstInterval); // Short delay between attacks
        }
    }
    public override void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance) {
        if(enemyHealth <= 0) {
            return; // If the enemy is already dead, don't process further damage
        }

        enemyHealth -= damageAmount; // Subtract damage from health
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth);
        animator.SetBool("isHit", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("Attack", false);

        animator.Play("enemy_hit");

        Vector2 adjustedKnockback = new Vector2(-knockbackDirection.x, 0); // Apply knockback only in the X axis
        StartCoroutine(ApplyKnockback(adjustedKnockback, knockbackDistance));

        if(enemyHealth <= 0) {
            GameObject coin = Instantiate(coin_reward, transform.position, Quaternion.identity); // Spawn a coin
            animator.SetBool("isWalking", false);
            animator.SetBool("isHit", false);
            animator.SetBool("Attack", false);
            animator.Play("enemy_dead"); // Play the death animation
            StartCoroutine(DieAfterDelay()); // Wait and destroy the enemy
            playerScript.IncreaseCoins(2); // Reward the player
        } else {
            StartCoroutine(DisabledHit()); // Delay reset of isHit
        }
    }
}
