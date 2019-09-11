using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;


    [SerializeField]
    CarMovement car;

    [SerializeField]
    GameObject _enemy;

    int distanceAheadToSendEnemyFrom = 2;

    float timeForOneRoad = 4f;

    // float timeBetweenSendingEnemies = 2f;

    [SerializeField]
    AnimationCurve timeBetweenSendingEnemiesCurve;

    void Awake() {
        instance = this;
    }

    void Start() {
        StartCoroutine(SendEnemies());
        
    }

    IEnumerator SendEnemies() {

        while (true) {
            sendEnemy();
            yield return new WaitForSeconds(timeBetweenSendingEnemiesCurve.Evaluate(Time.time/60f));
        }
    }

    private void sendEnemy() {

        float distanceCoveredByCar = car.GetCurrentRoadNum() + car.GetFractionAlongCurrentRoad();


        GameObject enemy = EnemyPool.instance.Get();

        enemy?.GetComponent<EnemyMovement>().Init(car.GetCurrentRoadNum(), car.GetFractionAlongCurrentRoad(), 2, GetRandomLaneNumber());

        enemy.SetActive(true);

    }

    public float GetTimeForOneRoad() {
        return timeForOneRoad;
    }

    private int GetRandomLaneNumber() {
        return UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
    }
}
