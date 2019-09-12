using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainsPool : MonoBehaviour {

    [SerializeField]
    private GameObject mountainPrefab;

    private Queue<GameObject> mountainQueue = new Queue<GameObject>();

    public static MountainsPool instance {get; private set;}

    int numInstantiated = 0;

    GameObject MountainParent;

    private void Awake() {
        instance = this;
        
        MountainParent = new GameObject();
        MountainParent.name = "Mountain Parent";
        MountainParent.transform.parent = transform;
        
        AddMountains(8);
    }

    void Start() {
        
    }

    public GameObject Get() {
        if (mountainQueue.Count == 0) {
            AddMountains(1);
        }
        GameObject obj = mountainQueue.Dequeue();
        mountainQueue.Enqueue(obj);
        return obj;
    }


    private void AddMountains(int count) {
        for (int i = 0; i < count; i++) {
            GameObject mountainToInstantiate = Instantiate(mountainPrefab);
            mountainToInstantiate.transform.parent = MountainParent.transform;
            mountainToInstantiate.SetActive(false);
            mountainQueue.Enqueue(mountainToInstantiate);
            numInstantiated++;
        }
    }

    public void ReturnToPool(GameObject obj) {
        obj.SetActive(false);
    }

    
}
