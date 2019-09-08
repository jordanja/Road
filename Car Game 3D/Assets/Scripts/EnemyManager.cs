using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<GameObject> enemies;

    [SerializeField]
    CarMovement car;

    [SerializeField]
    GameObject _enemy;

    int distanceAheadToSendEnemyFrom = 2;

    float timeForOneRoad = 4f;

    void Awake() {
        instance = this;
    }

    void Start() {
        enemies = new List<GameObject>();
        StartCoroutine(SendEnemies());
        
    }

    IEnumerator SendEnemies() {

        while (true) {
            sendEnemy();
            yield return new WaitForSeconds(2f);
        }
    }

    private void sendEnemy() {

        float distanceCoveredByCar = car.GetCurrentRoadNum() + car.GetFractionAlongCurrentRoad();


        GameObject enemy = Instantiate(_enemy);
        enemy.SetActive(false);
        enemy.name = "Enemy " + enemies.Count;
        enemy.transform.parent = transform;

        enemy?.GetComponent<EnemyMovement>().Init(car.GetCurrentRoadNum(), car.GetFractionAlongCurrentRoad(), 2, 3);

        enemies.Add(enemy);

    }

    public float GetTimeForOneRoad() {
        return timeForOneRoad;
    }
}
