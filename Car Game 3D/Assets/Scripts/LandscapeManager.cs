using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    
    public static LandscapeManager instance;

    [SerializeField]
    GameObject LandscapeBlueprint;

    List<GameObject> landscapes = new List<GameObject>();

    int numTreesPerLandscape = 10;

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
        for (int i = 0; i < numTreesPerLandscape; i++) {
            // addFoliage(z1, z2);
            StartCoroutine(addFoliage(z1,z2));
        }

    }

    IEnumerator addFoliage(float z1, float z2) {
        
        yield return new WaitUntil(() => RoadManager.instance.initialized == true);

        GameObject tree = FoliagePool.instance.Get();
        
        float x = UnityEngine.Random.Range(-10f, 10f);
        float z = UnityEngine.Random.Range(z1, z2);
        // float y = GetHeightForLandscape(x);
        Vector3 placementPoint = new Vector3(x, 0, z);
        int roadNum = Mathf.FloorToInt(z/RoadManager.instance.GetRoadZLength());
        print("getting road bounds for road number: " + roadNum);
        Bounds roadBounds = RoadManager.instance.GetRoad(roadNum).GetComponent<MeshRenderer>().bounds;
        while (roadBounds.Contains(placementPoint)) {
            x = UnityEngine.Random.Range(-10f, 10f);
            z = UnityEngine.Random.Range(z1, z2);
            // y = GetHeightForLandscape(x);
            placementPoint = new Vector3(x, 0, z);
            roadNum = Mathf.FloorToInt(z/RoadManager.instance.GetRoadZLength());
            roadBounds = RoadManager.instance.GetRoad(roadNum).GetComponent<MeshRenderer>().bounds;
        }
        float y = GetHeightForLandscape(x);
        placementPoint = new Vector3(x, y, z);
        tree.transform.position = placementPoint;
        
        tree.SetActive(true);


    }

    public void CreateLandscapeForRoad(int roadNum) {
        float z1 = (float)roadNum * RoadManager.instance.GetRoadZLength();
        float z2 = (float)(roadNum + 1) * RoadManager.instance.GetRoadZLength();
        // print("creating landscape between z points: " + z1 + " and " + z2);
        CreateLandscapeBetweenZPoints(z1, z2);
    }

    public void RemoveLandscape(int roadNum) {
        Destroy(landscapes[roadNum]);
        FoliagePool.instance.ReturnToPool((float)roadNum * RoadManager.instance.GetRoadZLength(), (float)(roadNum + 1) * RoadManager.instance.GetRoadZLength());
    }

    public float GetHeightForLandscape(float xPos) {
        return (Mathf.Abs(xPos)/6f) - 0.5f;
    }

}
