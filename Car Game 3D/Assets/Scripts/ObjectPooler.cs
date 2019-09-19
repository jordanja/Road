using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        
    }


}
