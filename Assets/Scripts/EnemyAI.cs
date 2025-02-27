using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI :MonoBehaviour {

    private Transform _targetHouse; // The house that the enemy will attack

    private float _speed = 0.9f; // Speed at which the enemy moves
    public float _attackRange = 1.7f; // Range within which the enemy can attack the house

    private Animator animator; // Animator to control enemy animations
    private SpriteRenderer spriteRenderer; // Sprite renderer to handle the enemy sprite (currently unused)

    private FloatingHealthBar _healthBar;
    public float enemyHealth = 10;
    private float _enemyMaxHealth; // Enemy's life max

    private void Awake() {
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        _targetHouse = FindObjectOfType<MainHouse>().transform;

        _enemyMaxHealth = enemyHealth;
        _healthBar.UpdateHealthBar(enemyHealth,_enemyMaxHealth);
    }

    void Update() {
        EnemyMovement(); EnemyMovement(); // Call the function that controls the enemy movement
    }

    private void EnemyMovement() {
        float distance = Vector2.Distance(transform.position, new Vector2(_targetHouse.position.x, transform.position.y)); // Calculate the distance to the house in the x-axis

        if(distance > _attackRange) { // If the enemy is not within attack range
            Vector2 targetPosition = new Vector2(_targetHouse.position.x, transform.position.y); // Set the target position in the x-axis (same y as enemy)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); // Move the enemy towards the house
            animator.SetBool("isWalking", true); // Activate walking animation

        } else { // When the enemy reaches the house
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", true); // Activate attack animation

        }
    }
    public void TakeDamage(float damageAmount) {
        Debug.Log("enemy gets damage ");

        enemyHealth -= damageAmount;
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth); // Actualiza la barra de vida

        if(enemyHealth <= 0) {
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", false); // Deactivate attack animation
            animator.Play("enemy_dead");
            StartCoroutine(DieAfterDelay());
        }
    }
    private IEnumerator DieAfterDelay() {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        Destroy(gameObject); // Destruye el GameObject del enemigo
    }
}