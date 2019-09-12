using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    
    public static LandscapeManager instance;

    [SerializeField]
    GameObject LandscapeBlueprint;

    List<GameObject> landscapes = new List<GameObject>();
    List<GameObject> landscapeParents = new List<GameObject>();

    int numTreesPerLandscape = 40;

    float landscapeMinX = -10f;
    float landscapeMaxX = 10f;

    int numberOfMountainsAdded = 0;

    void Awake() {
        instance = this;
    }

    void Start() {
        addLeftAndRightMountains();
        addLeftAndRightMountains();
        addLeftAndRightMountains();
    }

    public void CreateLandscapeBetweenZPoints(float z1, float z2) {
        GameObject landscapeParent = new GameObject();
        landscapeParent.name = "Landscape Parent " + landscapes.Count;
        landscapeParent.transform.parent = transform;
        landscapeParents.Add(landscapeParent);

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
        
        float x = UnityEngine.Random.Range(GetLandscapeMinX(), GetLandscapeMaxX());
        float z = UnityEngine.Random.Range(z1, z2);

        Vector3 placementPoint = new Vector3(x, 0, z);
        int roadNum = Mathf.FloorToInt(z/RoadManager.instance.GetRoadZLength());

        Bounds roadBounds = RoadManager.instance.GetRoad(roadNum).GetComponent<MeshRenderer>().bounds;
        roadBounds.Expand(1f);

        while (roadBounds.Contains(placementPoint)) {
            x = UnityEngine.Random.Range(GetLandscapeMinX(), GetLandscapeMaxX());
            z = UnityEngine.Random.Range(z1, z2);
            placementPoint = new Vector3(x, 0, z);
            roadNum = Mathf.FloorToInt(z/RoadManager.instance.GetRoadZLength());
            roadBounds = RoadManager.instance.GetRoad(roadNum).GetComponent<MeshRenderer>().bounds;
            roadBounds.Expand(1f);

        }
        float y = GetHeightForLandscape(x);
        placementPoint = new Vector3(x, y, z);
        tree.transform.position = placementPoint;
        
        tree.SetActive(true);


    }

    public void addLeftAndRightMountains(){
        GameObject mountainLeft = MountainsPool.instance.Get();
        GameObject mountainRight = MountainsPool.instance.Get();

        mountainLeft.transform.position = new Vector3(-20f, -0.3f, GetInitialMountainZ() + GetMountainMultiplier() * (numberOfMountainsAdded/2));
        mountainRight.transform.position = new Vector3(+20f, -0.3f, GetInitialMountainZ() + GetMountainMultiplier() * (numberOfMountainsAdded/2));
        
        mountainLeft.SetActive(true);
        mountainRight.SetActive(true);

        numberOfMountainsAdded += 2;

    }

    public void CreateLandscapeForRoad(int roadNum) {
        float z1 = (float)roadNum * RoadManager.instance.GetRoadZLength();
        float z2 = (float)(roadNum + 1) * RoadManager.instance.GetRoadZLength();
        // print("creating landscape between z points: " + z1 + " and " + z2);
        CreateLandscapeBetweenZPoints(z1, z2);
    }

    public void RemoveLandscape(int roadNum) {
        Destroy(landscapes[roadNum]);
        Destroy(landscapeParents[roadNum]);
        FoliagePool.instance.ReturnToPool((float)roadNum * RoadManager.instance.GetRoadZLength(), (float)(roadNum + 1) * RoadManager.instance.GetRoadZLength());
    }

    public float GetHeightForLandscape(float xPos) {
        return (Mathf.Abs(xPos)/6f) - 0.5f;
    }

    public float GetLandscapeMinX() {
        return landscapeMinX;
    }

    public float GetLandscapeMaxX() {
        return landscapeMaxX;
    }

    public int GetNumberOfMountainsAdded() {
        return numberOfMountainsAdded;
    }

    public float GetInitialMountainZ() {
        return 18f;
    }

    public float GetMountainMultiplier() {
        return 32f;
    }
}
