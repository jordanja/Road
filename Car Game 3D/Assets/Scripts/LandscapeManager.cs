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
            addFoliage(z1, z2);
        }

    }

    private void addFoliage(float z1, float z2) {

        GameObject tree = FoliagePool.instance.Get();
        
        float x = UnityEngine.Random.Range(-10f, 10f);
        float z = UnityEngine.Random.Range(z1, z2);
        float y = GetHeightForLandscape(x);
        tree.transform.position = new Vector3(x, y, z);
        
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
