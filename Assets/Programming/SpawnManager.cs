using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour {
    [SerializeField] GameObject enemySpawnObject;
    [SerializeField]Transform enemySpawnLocation;
    [SerializeField]Transform enemyWalkLocation;
    public bool isSpawning = true;

    #region Singleton
        public static SpawnManager instance;
        public void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion

    void Start() {
        StartCoroutine(checkSpawn());
    }

    public void SpawnEnemy() {
        Debug.Log("SpawnEnemy");
        if(enemySpawnObject) {
            GameObject newEnemyObject = Instantiate(enemySpawnObject, enemySpawnLocation.position, Quaternion.identity);
            Enemy newEnemy = newEnemyObject.GetComponent<Enemy>();
            newEnemy.MoveTowards(enemyWalkLocation);
        }
    }

    IEnumerator checkSpawn() {
        while(isSpawning) {
            SpawnEnemy();
            yield return new WaitForSeconds(10f);
        }
    }
}