using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        Coroutine sendEnemy = StartCoroutine(SendEnemies());
    }

    IEnumerator SendEnemies() {
        while(true) {

            yield return new WaitForSeconds(3f);
        }
    }

}
