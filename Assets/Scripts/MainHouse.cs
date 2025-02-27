using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MainHouse :MonoBehaviour {

    public float health = 50f; // House's health
    public float healthMax; // House's health
    public float fireRate = 1f; // Time between shots
    private float _lastFireTime = 0f; // Stores the time when the last shot was fired
    public float fireRange = 4.7f; // The range at which the house can fire
    private float _bulletSpeed = 15f; // Speed of the bullet
    //public float knockback = 0.1f; // The force with which the enemy is pushed when hit (currently unused)

    //public float lifeRegeneration = 1f; // Regeneration of life (currently unused)
    public float damage = 3f; // The damage the house deals

    public GameObject bulletPrefab; // Bullet prefab to instantiate when shooting
    public Transform firePoint; // Position where bullets are instantiated

    private Animator _animator; // Animator to control the animations
    private SpriteRenderer _spriteRenderer; // Sprite renderer to handle the sprite (currently unused)

    private Transform _currentTargetEnemy; // The current enemy the house is targeting

    private FloatingHealthBar _healthBar;
    private void Awake() {
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    void Start() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        healthMax = health;
        _healthBar.UpdateHealthBar(health, healthMax);
    }

    void Update() {
        DetectEnemy();// Continuously checks for enemies in range

        if(_currentTargetEnemy != null && Time.time - _lastFireTime >= fireRate) { // If there's a target and enough time has passed since the last shot, shoot again
            ShootEnemy(_currentTargetEnemy); // Fire at the closest enemy
            _lastFireTime = Time.time; // Update the last fire time

            _currentTargetEnemy = null; // Clear the current target
            Debug.Log("Shot enemy ");
        }
    }
    private void DetectEnemy() {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, fireRange); // Detects all colliders within the fire range

        float closestDistance = Mathf.Infinity; // Stores the closest distance to an enemy
        Transform closestEnemy = null; // Stores the closest enemy's transform

        foreach(Collider2D enemy in enemiesInRange) {  // If this enemy is closer than the previous closest enemy, update the closest enemy
            if(enemy.CompareTag("Enemy")) { // Check if the collider is an enemy using the "Enemy" tag

                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>(); // Get the EnemyAI component
                if(enemyAI != null && enemyAI.enemyHealth > 0) {
                    Debug.Log("enemy with more than 0 life: or it aint" + enemyAI.name + "     " + enemyAI.enemyHealth);

                    Vector2 colliderCenter = enemy.bounds.center + new Vector3(0.40f, 0f, 0f);
                    float distance = Vector2.Distance(transform.position, colliderCenter);

                    if(distance < closestDistance) {//verifies if the collider is an enemy based on the tag
                        closestDistance = distance;
                        closestEnemy = enemy.transform;

                    }
                }
            }
        }
        if(closestEnemy != _currentTargetEnemy) {  // Check if the target has changed
            _currentTargetEnemy = closestEnemy; // Assign the new closest enemy as the current target
            Debug.Log("Target updated to: " + (_currentTargetEnemy != null ? _currentTargetEnemy.name : "None"));
        }
    }
    private void ShootEnemy(Transform enemy) {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Create a bullet object at the fire point position
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        _animator.SetBool("Shot", true); // Activate the shooting animation
        StartCoroutine(ResetShotAnimation()); // Reset the "Shot" animation after a short delay

        if(rb != null) {
            Vector2 direction = (enemy.position - firePoint.position).normalized; // Calculate the direction towards the enemy
            rb.velocity = direction * _bulletSpeed; // Set the bullet's velocity to move towards the enemy

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Rotate the bullet to face the enemy
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    private IEnumerator ResetShotAnimation() {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds 
        _animator.SetBool("Shot", false); // Disable shooting animation
    }

    private void TakeDamage(float damageAmount) {
        health -= damageAmount;
        if(health < 0) {
            ///GAME OVER
        }
    }
}