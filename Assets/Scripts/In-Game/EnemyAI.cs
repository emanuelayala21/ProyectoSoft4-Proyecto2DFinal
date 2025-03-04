using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI :MonoBehaviour {

    private Transform _targetHouse; // The house that the enemy will attack

    private float _speed = 0.9f; // Speed at which the enemy moves
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

        if(enemyHealth <= 0) {
            // Si el enemigo está muerto, no puede atacar
            animator.SetBool("isWalking", false); // Desactivar la animación de caminar
            animator.SetBool("Attack", false); // Desactivar la animación de ataque
            return; // No hacer nada más si el enemigo está muerto
        }

        if(distance > _attackRange) { // If the enemy is not within attack range
            Vector2 targetPosition = new Vector2(_targetHouse.position.x, transform.position.y); // Set the target position in the x-axis (same y as enemy)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); // Move the enemy towards the house
            animator.SetBool("isWalking", true); // Activate walking animation

        } else { // When the enemy reaches the house
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", true); // Activate attack animation

            timeSinceLastAttack += Time.deltaTime; // Aumentar el tiempo desde el último ataque

            if(timeSinceLastAttack >= timeBetweenAttacks) { // Si ha pasado suficiente tiempo
                timeSinceLastAttack = 0f; // Resetear el temporizador
                AttackHouse(); // Llamar a la función que daña a la casa
            }
        }
    }
    private void AttackHouse() {
        if(playerScript != null) {
            playerScript.TakeDamage(_damage); // Llamar a TakeDamage de la casa (ejemplo con 2 de daño)
        }
    }

    public void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance) {
        Debug.Log("enemy gets damage ");

        if(enemyHealth <= 0) {
            return;
        }

        enemyHealth -= damageAmount;
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth); // Actualiza la barra de vida

        Vector2 adjustedKnockback = new Vector2(-knockbackDirection.x, 0); // Only in X axis
        StartCoroutine(ApplyKnockback(adjustedKnockback, knockbackDistance));

        if(enemyHealth <= 0) {
            GameObject coin = Instantiate(coin_reward, transform.position, Quaternion.identity);

            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", false); // Deactivate attack animation
            animator.Play("enemy_dead");
            StartCoroutine(DieAfterDelay());

            playerScript.IncreaseMoney(1);
        }
    }
    private IEnumerator DieAfterDelay() {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        Destroy(gameObject); // Destruye el GameObject del enemigo
    }
    private IEnumerator ApplyKnockback(Vector2 direction, float distance) {
        float maxKnockbackDuration = 0.2f;
        float knockbackDuration = Mathf.Clamp(maxKnockbackDuration * (distance / 2), 0.1f, maxKnockbackDuration);
        float elapsedTime = 0f;

        Vector2 startPosition = transform.position;

        // Calculate target position, only on X axis
        Vector2 targetPosition = new Vector2(
            startPosition.x + (direction.normalized.x * distance), // Only apply knockback on X
            startPosition.y // Keep Y position the same
        );

        // Move the enemy to the knockback position smoothly
        while(elapsedTime < knockbackDuration) {
            transform.position = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / knockbackDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Enemy stays in the knocked-back position
        transform.position = targetPosition;
    }
}