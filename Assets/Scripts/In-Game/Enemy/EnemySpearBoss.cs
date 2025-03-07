using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpearBoss :EnemySpear {
    protected override void Start() {
        base.Start(); // Llamar al m�todo Start() de EnemySword

        // Personaliza el "boss"
        _speed = 0.4f;  // M�s lento que el enemigo normal
        enemyHealth = 90f;  // M�s vida que un enemigo normal
        _damage = 20f;  // M�s da�o
        _attackRange = 4f;
        timeBetweenAttacks = 3.5f;  // Ataques m�s lentos
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth);
        // Otros valores de personalizaci�n para hacerlo m�s desafiante
    }

    // Puedes sobrescribir el TakeDamage si deseas agregar una mec�nica especial
    public override void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance) {
        if(enemyHealth <= 0) {
            return; // Si el boss est� muerto, no procesamos m�s da�o
        }

        enemyHealth -= damageAmount; // Restar da�o
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth); // Actualizar barra de salud

        animator.SetBool("isHit", true);  // Reproducir animaci�n de ser golpeado
        animator.SetBool("isWalking", false);
        animator.SetBool("Attack", false);
        animator.Play("enemy_hit");  // Animaci�n de da�o

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
            animator.Play("enemy_dead");  // Reproducir animaci�n de muerte
            StartCoroutine(DieAfterDelay());
            playerScript.IncreaseCoins(20);  // Recompensa m�s grande por matar al boss
        } else {
            StartCoroutine(DisabledHit());  // Deshabilitar el golpe durante un tiempo
        }
    }
}
