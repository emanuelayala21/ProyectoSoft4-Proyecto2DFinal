using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword :EnemyAI {
    protected override void Start() {
        _speed = 0.6f;
        _attackRange = 1.7f;
        _damage = 3f;
        timeBetweenAttacks = 2f;
        enemyHealth = 10f;
        base.Start(); // Call the base Start method 
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
            playerScript.IncreaseCoins(1); // Reward the player
        } else {
            StartCoroutine(DisabledHit()); // Delay reset of isHit
        }
    }
}
