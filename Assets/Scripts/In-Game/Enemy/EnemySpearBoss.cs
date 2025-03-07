using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpearBoss :EnemySpear {
    protected override void Start() {
        base.Start(); // Llamar al método Start() de EnemySword

        // Personaliza el "boss"
        _speed = 0.4f;  // Más lento que el enemigo normal
        enemyHealth = 90f;  // Más vida que un enemigo normal
        _damage = 20f;  // Más daño
        _attackRange = 4f;
        timeBetweenAttacks = 3.5f;  // Ataques más lentos
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth);
        // Otros valores de personalización para hacerlo más desafiante
    }

    // Puedes sobrescribir el TakeDamage si deseas agregar una mecánica especial
    public override void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance) {
        if(enemyHealth <= 0) {
            return; // Si el boss está muerto, no procesamos más daño
        }

        enemyHealth -= damageAmount; // Restar daño
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth); // Actualizar barra de salud

        animator.SetBool("isHit", true);  // Reproducir animación de ser golpeado
        animator.SetBool("isWalking", false);
        animator.SetBool("Attack", false);
        animator.Play("enemy_hit");  // Animación de daño

        // Reducir la distancia de knockback a la mitad
        float reducedKnockbackDistance = knockbackDistance / 2;

        Vector2 adjustedKnockback = new Vector2(-knockbackDirection.x, 0);
        StartCoroutine(ApplyKnockback(adjustedKnockback, reducedKnockbackDistance));

        // Si la salud llega a cero, muere el boss
        if(enemyHealth <= 0) {
            GameObject coin = Instantiate(coin_reward, transform.position, Quaternion.identity);  // Recompensa al jugador
            animator.SetBool("isWalking", false);
            animator.SetBool("isHit", false);
            animator.SetBool("Attack", false);
            animator.Play("enemy_dead");  // Reproducir animación de muerte
            StartCoroutine(DieAfterDelay());
            playerScript.IncreaseCoins(20);  // Recompensa más grande por matar al boss
        } else {
            StartCoroutine(DisabledHit());  // Deshabilitar el golpe durante un tiempo
        }
    }
}
