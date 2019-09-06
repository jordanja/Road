﻿using System;
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

    float distanceAheadToSendEnemyFrom = 2f;

    void Start() {
        enemies = new List<GameObject>();
        StartCoroutine(SendEnemies());
        
    }

    IEnumerator SendEnemies() {

        while (true) {
            sendEnemy();
            yield return new WaitForSeconds(3f);
        }
    }

    private void sendEnemy() {

        float positionOfEnemy = car.GetCurrentRoadNum() + car.GetFractionAlongCurrentRoad() + distanceAheadToSendEnemyFrom;
        

        GameObject enemy = Instantiate(_enemy);
        enemy?.GetComponent<EnemyMovement>().Init(positionOfEnemy);

        enemies.Add(enemy);

    }
}
