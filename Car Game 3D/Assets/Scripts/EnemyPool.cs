﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    [SerializeField]
    private GameObject[] enemyPrefabs;

    private Queue<GameObject> enemyQueue = new Queue<GameObject>();

    public static EnemyPool instance {get; private set;}

    int numInstantiated = 0;

    GameObject EnemyParent;

    private void Awake() {
        instance = this;
        
        EnemyParent = new GameObject();
        EnemyParent.name = "Enemy Parent";
        EnemyParent.transform.parent = transform;
        
        AddEnemies(20);
    }

    void Start() {
        
    }

    public GameObject Get() {
        if (enemyQueue.Count == 0) {
            AddEnemies(1);
        }
        GameObject obj = enemyQueue.Dequeue();
        enemyQueue.Enqueue(obj);
        return obj;
    }


    private void AddEnemies(int count) {
        for (int i = 0; i < count; i++) {
            GameObject foliateToInstantiate = Instantiate(enemyPrefabs[numInstantiated % (enemyPrefabs.Length)]);
            foliateToInstantiate.transform.parent = EnemyParent.transform;
            foliateToInstantiate.SetActive(false);
            enemyQueue.Enqueue(foliateToInstantiate);
            numInstantiated++;
        }
    }

    public void ReturnToPool(GameObject obj) {
        obj.SetActive(false);
    }

    
}