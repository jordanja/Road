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

    float timeBetweenSendingEnemies = 2f;

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
            yield return new WaitForSeconds(timeBetweenSendingEnemies);
        }
    }

    private void sendEnemy() {

        float distanceCoveredByCar = car.GetCurrentRoadNum() + car.GetFractionAlongCurrentRoad();


        GameObject enemy = Instantiate(_enemy);
        enemy.SetActive(false);
        enemy.name = "Enemy " + enemies.Count;
        enemy.transform.parent = transform;

        enemy?.GetComponent<EnemyMovement>().Init(car.GetCurrentRoadNum(), car.GetFractionAlongCurrentRoad(), 2, GetRandomLaneNumber());

        enemies.Add(enemy);

    }

    public float GetTimeForOneRoad() {
        return timeForOneRoad;
    }

    private int GetRandomLaneNumber() {
        return UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
    }
}
