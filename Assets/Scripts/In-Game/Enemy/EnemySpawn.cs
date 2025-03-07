using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class EnemySpawn :MonoBehaviour {

    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public GameObject bossPrefab1;
    public GameObject bossPrefab2;
    public GameObject bossPrefab3;

    public float timeBetweenWaves = 10f; // Tiempo entre oleadas
    private Transform spawnPoint;

    private bool isSpawning = false;

    void Start() {
        spawnPoint = this.transform;

    }
    private void Update() {
        
    }
    public void StartWave(int waveNumber, float enemy1Prob, float enemy2Prob, float enemy3Prob) {
        Debug.Log("Starting wave " + waveNumber);  // Verifica si este log aparece en la consola
        if(!isSpawning) {
            StartCoroutine(SpawnWave(waveNumber, enemy1Prob, enemy2Prob, enemy3Prob));
        }
    }

    IEnumerator SpawnWave(int waveNumber, float enemy1Prob, float enemy2Prob, float enemy3Prob) {
        isSpawning = true;
        int enemiesInWave = Mathf.RoundToInt(0 + (waveNumber * 1.2f));  // Incrementa la cantidad de enemigos

        for(int i = 0; i < enemiesInWave; i++) {
            SpawnEnemy(enemy1Prob, enemy2Prob, enemy3Prob);
            yield return new WaitForSeconds(2.5f);
        }

        Debug.Log("Oleada terminada."); // Verifica cuando se termine de generar la oleada

        if(waveNumber == 3) { // Spawnear un boss al final de cada nivel
            SpawnBoss(GameManager.Instance.currentLevel);
        }

        isSpawning = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        GameManager.Instance.StartNextWave(); // Llama a la siguiente ola después de un tiempo
    }

    void SpawnEnemy(float enemy1Prob, float enemy2Prob, float enemy3Prob) {
        int randomValue = Random.Range(1, 101);

        if(randomValue <= enemy1Prob) {
            Instantiate(enemyPrefab1, spawnPoint.position, Quaternion.Euler(0f, 180f, 0f)); // Rotar el enemigo 180 grados
        } else if(randomValue <= enemy1Prob + enemy2Prob) {
            Instantiate(enemyPrefab2, spawnPoint.position, Quaternion.Euler(0f, 180f, 0f)); // Rotar el enemigo 180 grados
        } else {
            Instantiate(enemyPrefab3, spawnPoint.position, Quaternion.Euler(0f, 180f, 0f)); // Rotar el enemigo 180 grados
        }
    }

    void SpawnBoss(int bossLevel) {
        switch(bossLevel) {
            case 1:
                Instantiate(bossPrefab1, new Vector3(spawnPoint.position.x, spawnPoint.position.y + 0.7f, spawnPoint.position.z), Quaternion.Euler(0f, 180f, 0f)); // Rotar el boss 180 grados
                break;
            case 2:
                Instantiate(bossPrefab2, new Vector3(spawnPoint.position.x, spawnPoint.position.y + 0.75f, spawnPoint.position.z), Quaternion.Euler(0f, 180f, 0f)); // Rotar el boss 180 grados
                break;
            case 3:
                Instantiate(bossPrefab3, new Vector3(spawnPoint.position.x, spawnPoint.position.y + 0.8f, spawnPoint.position.z), Quaternion.Euler(0f, 180f, 0f)); // Rotar el boss 180 grados
                break;
            default:
                Instantiate(bossPrefab3, new Vector3(spawnPoint.position.x, spawnPoint.position.y + 0.6f, spawnPoint.position.z), Quaternion.Euler(0f, 180f, 0f)); // Rotar el boss 180 grados
                break;
        }
    }
}