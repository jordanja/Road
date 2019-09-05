using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    GameObject _enemy;

    void Awake() {
        instance = this;
    }

    void Start() {
        Coroutine sendEnemy = StartCoroutine(SendEnemies());
    }

    IEnumerator SendEnemies() {
        while(true) {
            sendEnemy();
            yield return new WaitForSeconds(3f);
        }
    }

    void sendEnemy() {
        GameObject enemy = Instantiate(_enemy);
    }
}
