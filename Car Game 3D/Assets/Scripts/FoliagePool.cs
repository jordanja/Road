using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliagePool : MonoBehaviour {
    [SerializeField]
    private GameObject[] foliagePrefabs;

    private Queue<GameObject> foliage = new Queue<GameObject>();

    public static FoliagePool instance {get; private set;}

    int numInstantiated = 0;

    private void Awake() {
        instance = this;
        
    }

    public GameObject Get() {
        if (foliage.Count == 0) {
            AddFoliage(1);
        }
        return foliage.Dequeue();
    }


    private void AddFoliage(int count) {
        for (int i = 0; i < count; count++) {
            GameObject foliateToInstantiate = Instantiate(foliagePrefabs[numInstantiated % (foliage.Count)]);
            foliage.Enqueue(foliateToInstantiate);
            numInstantiated++;
        }
    }

    public void ReturnToPool(GameObject foliageToReturn) {
        foliageToReturn.SetActive(false);
        foliage.Enqueue(foliageToReturn);
    }
}
