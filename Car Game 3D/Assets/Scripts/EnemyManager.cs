using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<GameObject> enemies;

    [SerializeField]
    GameObject _enemy;

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

        GameObject enemy = Instantiate(_enemy);
        

        enemies.Add(enemy);

    }
}
