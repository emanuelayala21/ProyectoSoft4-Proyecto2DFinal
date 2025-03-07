using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel = 1;
    public int currentWave = 1;

    public EnemySpawn enemySpawn;
    public  MainHouse house;

    private float enemy1Prob;
    private float enemy2Prob;
    private float enemy3Prob;

    public UIManager ui_manager;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener GameManager en todas las escenas
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        UpdateEnemyProbabilities();
        InitializeGame();
    }
    public void InitializeGame() {
        currentLevel = 1;
        currentWave = 1;
        ResetPlayerStats();
        ClearEnemies();
        UpdateEnemyProbabilities();
        StartNextWave();
    }

    public void ResetGame() {
        Debug.Log("Reiniciando juego...");
        currentLevel = 2;
        currentWave = 1;
        ResetPlayerStats();
        ClearEnemies();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetPlayerStats() {
        if(house == null) {
            house = FindObjectOfType<MainHouse>(); // Asegurar que house está referenciada
        }

        if(house != null) {
            house.health = 50f;
            house.healthMax = 50f;
            house.coins = 0;
            house.damage = 3f;
            house.criticChance = 1f;
            house.fireRate = 1f;
            house.fireRange = 4.7f;
            Debug.Log("Stats del jugador restablecidos.");
        }
    }

    private void ClearEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies) {
            Destroy(enemy);
        }
        Debug.Log("Todos los enemigos eliminados.");
    }
    public void StartNextWave() {
        Debug.Log("Iniciando siguiente ola...");

        if(enemySpawn == null) {
            enemySpawn = FindObjectOfType<EnemySpawn>();
        }

        if(enemySpawn != null) {
            currentWave++;
            Debug.Log("Ola actual: " + currentWave);  // Verifica que la ola se incremente correctamente

            if(currentWave > 3) { // Después de 3 oleadas, se sube de nivel
                currentLevel++;
                currentWave = 1;
                UpdateEnemyProbabilities();
                ui_manager.LevelPassed();
                StartCoroutine(Waiting());
            }

            enemySpawn.StartWave(currentWave, enemy1Prob, enemy2Prob, enemy3Prob);
        } else {
            Debug.LogError("No se encontró EnemySpawn en la escena.");
        }
    }

    private void UpdateEnemyProbabilities() {
        switch(currentLevel) {
            case 1:
                enemy1Prob = 80;
                enemy2Prob = 20;
                enemy3Prob = 0;
                break;
            case 2:
                enemy1Prob = 30;
                enemy2Prob = 50;
                enemy3Prob = 20;
                break;
            case 3:
                enemy1Prob = 20;
                enemy2Prob = 40;
                enemy3Prob = 40;
                break;
            case 4:
                enemy1Prob = 10;
                enemy2Prob = 30;
                enemy3Prob = 60;
                break;
            default:
                enemy1Prob = 10;
                enemy2Prob = 20;
                enemy3Prob = 70;
                break;
        }
    }
    private IEnumerator Waiting() {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the "no funds" message

    }
}