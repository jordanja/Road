using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliagePool : MonoBehaviour {
    [SerializeField]
    private GameObject[] foliagePrefabs;

    private Queue<GameObject> foliageQueue = new Queue<GameObject>();

    public static FoliagePool instance {get; private set;}

    int numInstantiated = 0;

    GameObject FoliageParent;

    private void Awake() {
        instance = this;
        
        FoliageParent = new GameObject();
        FoliageParent.name = "Foliage Parent";
        FoliageParent.transform.parent = transform;
        
        AddFoliage(20);
    }

    void Start() {
        
    }

    public GameObject Get() {
        if (foliageQueue.Count == 0) {
            AddFoliage(1);
        }
        return foliageQueue.Dequeue();
    }


    private void AddFoliage(int count) {
        for (int i = 0; i < count; i++) {
            GameObject foliateToInstantiate = Instantiate(foliagePrefabs[numInstantiated % (foliagePrefabs.Length)]);
            foliateToInstantiate.transform.parent = FoliageParent.transform;
            foliateToInstantiate.SetActive(false);
            foliageQueue.Enqueue(foliateToInstantiate);
            numInstantiated++;
        }
    }

    public void ReturnToPool(GameObject foliageToReturn) {
        foliageToReturn.SetActive(false);
        foliageQueue.Enqueue(foliageToReturn);
    }
}
