using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MainHouse :MonoBehaviour {

    public float life = 50f; // House's health
    public float fireRate = 1f; // Time between shots
    private float _lastFireTime = 0f; // Stores the time when the last shot was fired
    public float fireRange = 4.7f; // The range at which the house can fire
    private float _bulletSpeed = 15f; // Speed of the bullet
                                      //public float knockback = 0.1f; // The force with which the enemy is pushed when hit (currently unused)

    //public float lifeRegeneration = 1f; // Regeneration of life (currently unused)
    //public float damage = 3f; // The damage the house deals (currently unused)

    public GameObject bulletPrefab; // Bullet prefab to instantiate when shooting
    public Transform firePoint; // Position where bullets are instantiated

    private Animator _animator; // Animator to control the animations
    private SpriteRenderer _spriteRenderer; // Sprite renderer to handle the sprite (currently unused)

    private Transform _currentTargetEnemy; // The current enemy the house is targeting
    void Start() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
                float distance = Vector2.Distance(transform.position, enemy.transform.position); // Calculate the distance to the enemy

                if(distance < closestDistance) {//verifies if the collider is an enemy based on the tag
                    closestDistance = distance;
                    closestEnemy = enemy.transform;

                }
            }
        }
        _currentTargetEnemy = closestEnemy; // Assign the closest enemy as the current target
    }
    private void ShootEnemy(Transform enemy) {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Create a bullet object at the fire point position
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        _animator.SetBool("Shot", true); // Activate the shooting animation
        StartCoroutine(ResetShotAnimation()); // Reset the "Shot" animation after a short delay

        if(rb != null) {
            Vector2 direction = (enemy.position - firePoint.position).normalized; // Calculate the direction towards the enemy

            rb.velocity = direction * _bulletSpeed; // Set the bullet's velocity to move towards the enemy


        }
    }
    private IEnumerator ResetShotAnimation() {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds 
        _animator.SetBool("Shot", false); // Disable shooting animation
    }
}





/////
///// NEXT PROYECT STEPS 
//////////
//1.Sistema de Vida de la Casa
//Tarea 1.1: Crear una variable para la vida de la casa.
//Tarea 1.2: Mostrar una barra de vida visualmente en la pantalla (usando UI Slider o una barra personalizada).
//Tarea 1.3: Implementar una función que reduzca la vida de la casa cuando sea atacada por enemigos (puede ser por colisiones o por una habilidad del enemigo).
//Tarea 1.4: Si la vida de la casa llega a 0, terminar el juego o mostrar una pantalla de derrota.
//2. Sistema de Vida de los Enemigos
//Tarea 2.1: Crear una variable para la vida de los enemigos.
//Tarea 2.2: Mostrar una barra de vida para cada enemigo (también con UI Slider o una barra personalizada).
//Tarea 2.3: Reducir la vida del enemigo cuando sea golpeado por la bala.
//Tarea 2.4: Eliminar al enemigo (y su barra de vida) cuando su vida llegue a 0.
//3. Sistema de Mejoras de la Casa
//Tarea 3.1: Crear botones en la interfaz de usuario para mejorar la casa.
//Tarea 3.2: Implementar mejoras que afecten el daño, rango y fire rate de la casa.
//Mejora de Daño: Aumentar el daño que hace la bala a los enemigos.
//Mejora de Rango: Aumentar el rango de detección de enemigos para que la casa dispare antes.
//Mejora de Fire Rate: Reducir el intervalo entre disparos.
//Tarea 3.3: Asegurarte de que cada mejora cueste una cantidad de recursos o dinero que el jugador pueda ganar a lo largo del juego (por ejemplo, cada vez que elimine un enemigo).
//Tarea 3.4: Al mejorar la casa, actualizar las características de disparo y las propiedades visuales de la casa si es necesario (como un cambio en la apariencia).
//4. Creación de Enemigos
//Tarea 4.1: Crear múltiples tipos de enemigos con diferentes características (vida, velocidad, etc.).
//Tarea 4.2: Generar enemigos en oleadas, que lleguen en diferentes momentos y a intervalos regulares.
//Tarea 4.3: Asegurarte de que los enemigos se muevan hacia la casa (utilizando AI para que sigan un camino o se dirijan directamente hacia la casa).
//Tarea 4.4: Implementar diferentes comportamientos para los enemigos, como atacar la casa o solo acercarse.
//5. UI y Visualización
//Tarea 5.1: Crear un sistema de interfaz de usuario (UI) que muestre:
//Barra de vida de la casa.
//Barra de vida de los enemigos.
//Botones para mejorar la casa.
//Texto que indique los recursos actuales del jugador.
//Tarea 5.2: Mostrar información de las mejoras disponibles, como cuánto cuesta cada mejora y cuál es el beneficio.
//6. Gestión de Recursos y Progresión
//Tarea 6.1: Crear un sistema de recursos (dinero o puntos) que se acumulen cuando se eliminen enemigos.
//Tarea 6.2: Asegurarte de que el jugador pueda usar esos recursos para comprar mejoras para la casa.
//Tarea 6.3: Establecer un sistema de progresión donde los enemigos se vuelven más fuertes y el jugador debe seguir mejorando la casa.
//7. Optimización y Pulido
//Tarea 7.1: Optimizar el rendimiento del juego si hay demasiados enemigos en pantalla.
//Tarea 7.2: Asegurarte de que no haya errores en las interacciones entre la casa, los enemigos y la UI.
//Tarea 7.3: Realizar pruebas para verificar que las mejoras y las barras de vida se actualicen correctamente.
//8. Pantallas de Victoria y Derrota
//Tarea 8.1: Implementar una pantalla de victoria cuando la casa sobreviva durante un tiempo o elimine a todos los enemigos.
//Tarea 8.2: Crear una pantalla de derrota si la vida de la casa llega a 0.
//9. Sonidos y Animaciones
//Tarea 9.1: Agregar efectos de sonido cuando la casa dispare, los enemigos mueran y se realicen mejoras.
//Tarea 9.2: Agregar animaciones para los enemigos y para los disparos de la casa.
//10. Pruebas y Ajustes Finales
//Tarea 10.1: Probar el juego en diferentes resoluciones y dispositivos para asegurarse de que funcione bien en todas las condiciones.
//Tarea 10.2: Ajustar la dificultad de los enemigos y las mejoras para que el juego tenga una curva de dificultad adecuada.
////////// 