using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MainHouse :MonoBehaviour {

    // House's Stats
    public float health = 50f;       // Current health of the house
    public float healthMax = 50f;    // Maximum health of the house
    public float healthRegen = 0.5f; // Regeneration of life (currently unused)

    // Attack Properties
    public float damage = 3f;        // Damage dealt by the house
    public float criticChance = 1f;  // Critical hit chance
    public float fireRate = 1f;      // Time between shots
    private float _lastFireTime = 0f;// Time when the last shot was fired
    public float fireRange = 4.7f;   // Range at which the house can fire
    private float _bulletSpeed = 15f;// Speed of the bullet
    public float knockback = 0.0f;   // Force applied to enemies when hit

    // Resources & UI
    public int coins = 0;            // Player's collected coins
    public UIManager uiManager;      // Reference to the UI manager
    private FloatingHealthBar _healthBar; // Health bar UI reference

    // Shooting System
    public GameObject bulletPrefab;  // Prefab of the bullet to shoot
    public Transform firePoint;      // Position where bullets are instantiated

    // Animation & Targeting
    private Animator _animator;      // Animator for animations
    private Transform _currentTargetEnemy; // Current enemy being targeted

    private void Awake() {
        _healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    void Start() {
        _animator = GetComponent<Animator>();

        healthMax = health;
        _healthBar.UpdateHealthBar(health, healthMax);

        StartCoroutine(HouseRegeneration());
    }

    void Update() {
        if (health >= 0f) {
            DetectEnemy();// Continuously checks for enemies in range
            ShootEnemy(_currentTargetEnemy); // Fire at the closest enemy
        }
        _healthBar.UpdateHealthBar(health, healthMax);

    }
    private void DetectEnemy() {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, fireRange); // Detects all colliders within the fire range

        float closestDistance = Mathf.Infinity; // Stores the closest distance to an enemy
        Transform closestEnemy = null; // Stores the closest enemy's transform

        foreach(Collider2D enemy in enemiesInRange) {  // If this enemy is closer than the previous closest enemy, update the closest enemy
            if(enemy.CompareTag("Enemy")) { // Check if the collider is an enemy using the "Enemy" tag

                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>(); // Get the EnemyAI component
                if(enemyAI != null && enemyAI.enemyHealth > 0) {

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
        if(_currentTargetEnemy != null && Time.time - _lastFireTime >= fireRate) {
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
            _lastFireTime = Time.time; // Update the last fire time

            _currentTargetEnemy = null; // Clear the current target
        }
    }
    private IEnumerator ResetShotAnimation() {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds 
        _animator.SetBool("Shot", false); // Disable shooting animation
    }

    public void TakeDamage(float damageAmount) {
        health -= damageAmount; // Deduct the damage amount from the current health
        if(health < 0) { // Check if the health goes below 0
            uiManager.GameOver();
            
        }
    }

    private IEnumerator HouseRegeneration() {
        while(true) { // Continuously regenerate health while the game is running
            yield return new WaitForSeconds(1f); // Wait for 1 second before checking health again

            if(health < healthMax) { // Only regenerate health if it's below the max
                health += healthRegen; // Increase health by the regeneration amount
                health = Mathf.Min(health, healthMax); // Ensure health does not exceed the maximum health
                _healthBar.UpdateHealthBar(health, healthMax); // Update the health bar UI with the new health value
            }
        }
    }
    public bool BuyUpgrades(int cost, float upgradeAmount, int upgradeType) {
        if(coins - cost >= 0) { // Check if the player has enough coins to buy the upgrade
            coins -= cost;

            uiManager.ShowCoinsUI(coins); // Update the UI to reflect the new coin count
            ApplyUpgrade(upgradeType, upgradeAmount); // Apply the upgrade based on the type and the amount
            return true;

        } else {
            uiManager.ShowNoFundsMsg(); // If the player doesn't have enough coins, show a "no funds" message
            return false;
        }
    }
    private void ApplyUpgrade(int upgradeType, float upgradeAmount) {
        // Aplica la mejora dependiendo del tipo
        switch(upgradeType) {
            case 0:
                damage += upgradeAmount; ///apply more damage
                break;
            case 1:
                fireRate = Mathf.Max(0.1f, fireRate + upgradeAmount);///apply a decrease on fire  rate 
                break;
            case 2:
                criticChance += upgradeAmount; ///apply more critic chance
                break;
            case 3:
                fireRange += upgradeAmount; ///apply more fire range 
                break;
            case 4:
                healthMax += upgradeAmount; ///apply more fire range 
                health += upgradeAmount;
                break;
            case 5:
                healthRegen += upgradeAmount; ///apply more fire range 
                break;
            case 6:
                knockback += upgradeAmount; ///apply more fire range 
                break;
            default:
                break;
        }
    }
    public void IncreaseCoins(int enemy) {
        switch(enemy) { // Check which type of enemy is defeated
            case 1:
                coins += 1; // Increase coins by 1 for defeating enemy type 1
                uiManager.ShowCoinsUI(coins); // Update the UI with the new coin count

                break;
        }
    }
}