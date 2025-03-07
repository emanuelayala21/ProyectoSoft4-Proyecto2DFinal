using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield :EnemyAI {
    protected bool isBlocking = false; // Determines if the enemy is blocking
    protected override void Start() {
        _speed = 0.8f; // Slightly faster than normal enemies
        _attackRange = 1.5f;
        _damage = 2f;
        timeBetweenAttacks = 1f;
        enemyHealth = 25f; // Slightly higher health
        base.Start();
    }
    public override void EnemyMovement() {
        float distance = Vector2.Distance(transform.position, new Vector2(_targetHouse.position.x, transform.position.y)); // Distance to the house in the x-axis

        if(enemyHealth <= 0) {
            animator.SetBool("isWalking", false);
            animator.SetBool("Attack", false);
            animator.SetBool("isHit", false);
            return;
        }

        if(animator.GetBool("isHit")) {

            return; // If hit animation is playing, do not move
        }

        if(distance > _attackRange) {
            Vector2 targetPosition = new Vector2(_targetHouse.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            animator.SetBool("isWalking", true);
            animator.SetBool("Attack", false);

        } else {
            animator.SetBool("isWalking", false);
            animator.SetBool("Attack", true);

            timeSinceLastAttack += Time.deltaTime;

            if(timeSinceLastAttack >= timeBetweenAttacks) {
                timeSinceLastAttack = 0f;
                AttackHouse();
            }
        }
    }
    public override void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance) {
        if(enemyHealth <= 0) {
            return;
        }

        // Si no está atacando, solo reproduce la animación de bloqueo y vuelve a caminar
        if(!animator.GetBool("Attack") && enemyHealth > 0) {
            animator.Play("block"); // Reproducir la animación de bloqueo
            StartCoroutine(ResumeWalkingAfterBlock());
            return; // No recibe daño
        }

        // Si está atacando, recibe daño normalmente
        enemyHealth -= damageAmount;
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth);
        animator.SetBool("isHit", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("Attack", false);
        animator.Play("enemy_hit");

        Vector2 adjustedKnockback = new Vector2(-knockbackDirection.x, 0);
        StartCoroutine(ApplyKnockback(adjustedKnockback, knockbackDistance));

        if(enemyHealth <= 0) {
            GameObject coin = Instantiate(coin_reward, transform.position, Quaternion.identity);
            animator.SetBool("isWalking", false);
            animator.SetBool("isHit", false);
            animator.SetBool("Attack", false);
            animator.Play("enemy_dead");
            StartCoroutine(DieAfterDelay());
            playerScript.IncreaseCoins(3);
        } else {
            StartCoroutine(DisabledHit());
        }
    }

    // Corrutina para que el enemigo vuelva a caminar después de bloquear
    protected virtual IEnumerator ResumeWalkingAfterBlock() {
        yield return new WaitForSeconds(0.2f); // Duración de la animación de "Block"
        animator.SetBool("isWalking", true);
    }
}
