using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI :MonoBehaviour {

    // Reference and target related attributes
    protected Transform _targetHouse; // The house that the enemy will attack
    protected MainHouse playerScript; // Reference to the house script to call TakeDamage

    // Movement and attack related attributes
    protected float _speed; // Speed at which the enemy moves
    protected float _attackRange; // Range within which the enemy can attack the house
    protected float _damage; // Damage dealt by the enemy
    protected float timeBetweenAttacks; // Time in seconds between attacks
    protected float timeSinceLastAttack; // Time since last attack

    // Health related attributes
    protected FloatingHealthBar _healthBar;
    public float enemyHealth; // Enemy health value
    protected float _enemyMaxHealth; // Enemy's max health

    // Visual and animation related attributes
    protected Animator animator; // Animator to control enemy animations
    protected SpriteRenderer spriteRenderer; // Sprite renderer to handle the enemy sprite (currently unused)

    // Other related attributes
    public GameObject coin_reward; // Coin reward when the enemy dies

    protected virtual void Awake() {
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    protected virtual void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        _targetHouse = FindObjectOfType<MainHouse>().transform;
        playerScript = _targetHouse.GetComponent<MainHouse>();

        _enemyMaxHealth = enemyHealth;
        _healthBar.UpdateHealthBar(enemyHealth, _enemyMaxHealth);
    }
    protected virtual void Update() {
        EnemyMovement(); // Call the movement method
    }
    public virtual void EnemyMovement() {
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

        } else { // When the enemy reaches the house
            animator.SetBool("isWalking", false); // Deactivate walking animation
            animator.SetBool("Attack", true); // Activate attack animation

            timeSinceLastAttack += Time.deltaTime;

            if(timeSinceLastAttack >= timeBetweenAttacks) {
                timeSinceLastAttack = 0f;
                AttackHouse(); // Damage the house
            }
        }
    }
    protected virtual void AttackHouse() {
        if(playerScript != null) {
            playerScript.TakeDamage(_damage); // Damage the house
        }
    }
    public virtual void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackDistance){}
    protected virtual IEnumerator DieAfterDelay() {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject); // Destroy the enemy after delay
    }
    protected virtual IEnumerator ApplyKnockback(Vector2 direction, float distance) {
        float maxKnockbackDuration = 0.2f;
        float knockbackDuration = Mathf.Clamp(maxKnockbackDuration * (distance / 2), 0.1f, maxKnockbackDuration);
        float elapsedTime = 0f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = new Vector2(startPosition.x + (direction.normalized.x * distance), startPosition.y);

        while(elapsedTime < knockbackDuration) {
            transform.position = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / knockbackDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
    protected virtual IEnumerator DisabledHit() {
        while(true) { // Continuously regenerate health while the game is running
            yield return new WaitForSeconds(0.1f); // Wait for 1 second before checking health again
            animator.SetBool("isHit", false);


        }
    
    }
}