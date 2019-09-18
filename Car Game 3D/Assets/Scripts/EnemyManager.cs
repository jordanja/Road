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

    float timeForOneRoad = 3f;
    float startTime;

    // float timeBetweenSendingEnemies = 2f;

    [SerializeField]
    AnimationCurve timeBetweenSendingEnemiesCurve;

    void Awake() {
        instance = this;
    }

    void Start() {
        StartCoroutine(SendEnemies());
        startTime = Time.time;
    }

    IEnumerator SendEnemies() {

        while (true) {
            sendEnemy(2);
            // sendEnemy();
            yield return new WaitForSeconds(timeBetweenSendingEnemiesCurve.Evaluate((Time.time - startTime)/60f));
        }
    }

    private void sendEnemy(int numberOfEnemies) {

        // float distanceCoveredByCar = car.GetCurrentRoadNum() + car.GetFractionAlongCurrentRoad();

        int enemy1Lane = GetRandomLaneNumber();

        GameObject enemy1 = EnemyPool.instance.Get();
        enemy1?.GetComponent<EnemyMovement>().Init(car.GetCurrentRoadNum(), car.GetFractionAlongCurrentRoad(), 2, enemy1Lane);
        enemy1.SetActive(true);

        if (numberOfEnemies == 2) {
            int enemy2Lane = GetRandomLaneNumber(enemy1Lane);
            GameObject enemy2 = EnemyPool.instance.Get();
            enemy2?.GetComponent<EnemyMovement>().Init(car.GetCurrentRoadNum(), car.GetFractionAlongCurrentRoad(), 2, enemy2Lane);
            enemy2.SetActive(true);
        } 




    }

    public float GetTimeForOneRoad() {
        return timeForOneRoad;
    }

    private int GetRandomLaneNumber() {
        return UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
    }

    private int GetRandomLaneNumber(int otherLane) {
        int lane = UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
        while (lane == otherLane) {
            lane = UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
        }
        return lane;
    }
}
