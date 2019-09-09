using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    
    public static LandscapeManager instance;

    [SerializeField]
    GameObject LandscapeBlueprint;

    List<GameObject> landscapes = new List<GameObject>();

    void Awake() {
        instance = this;
    }

    

    public void CreateLandscapeBetweenZPoints(float z1, float z2) {
        GameObject landscapeParent = new GameObject();
        landscapeParent.name = "Landscape Parent " + landscapes.Count;
        landscapeParent.transform.parent = transform;

        GameObject landscape = Instantiate(LandscapeBlueprint, landscapeParent.transform, false);
        landscape.name = "Landscape " + landscapes.Count;

        landscape.GetComponent<LandscapeGenerator>().Init(z1, z2);
        landscapes.Add(landscape);

        GameObject tree = FoliagePool.instance.Get();
        // tree.transform.position = new Vector3(0,0,0);

    }

    public void CreateLandscapeForRoad(int roadNum) {
        float z1 = (float)roadNum * RoadManager.instance.GetRoadZLength();
        float z2 = (float)(roadNum + 1) * RoadManager.instance.GetRoadZLength();
        // print("creating landscape between z points: " + z1 + " and " + z2);
        CreateLandscapeBetweenZPoints(z1, z2);
    }

}
