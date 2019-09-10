using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class RoadManager : MonoBehaviour {

    public static RoadManager instance;

    private List<GameObject> roads = new List<GameObject>();
    private List<GameObject> roadBoxs = new List<GameObject>();
    private List<GameObject> roadParents = new List<GameObject>();


    List<Vector3> pointsOnRoad;

    [HideInInspector]
    public bool initialized = false;

    [SerializeField]
    GameObject roadBlueprint;

    [SerializeField]
    GameObject roadBoxBlueprint;

    private float roadWidth = 0.7f;

    int numberOfLanes = 4;

    void Awake() {
        instance = this;
    }

    private void Start() {
    
        
        pointsOnRoad = new List<Vector3>();

        for (int i = 0; i < 5; i++) {
            AddRoad(GetRoadZLength(), GetSegments(), GetRoadCurviness(), GetNumberOfControlPoints());
        }

        initialized = true;
    }

    public void CreateNewRoad(Vector3 firstPoint, Vector3 lastPoint, int segments, float roadCurviness, int numberOfControlPoints) {
        GameObject roadParent = new GameObject();
        roadParent.transform.parent = this.transform;
        roadParent.name = "Road Parent " + roads.Count;

        GameObject road = Instantiate(roadBlueprint, roadParent.transform, false);
        road.name = "Road " + roads.Count;
        
        if (roads.Count == 0) {
            road.GetComponent<Road>()?.Init(firstPoint, lastPoint, segments, roadCurviness, numberOfControlPoints, numberOfLanes);
        } else {
            road.GetComponent<Road>()?.Init(firstPoint, lastPoint, roads[roads.Count - 1].GetComponent<Road>(), segments, roadCurviness, numberOfControlPoints, numberOfLanes);

        }

        GameObject roadBox = Instantiate(roadBoxBlueprint, roadParent.transform, false);
        roadBox.name = "Road Box " + roadBoxs.Count;
        roadBox.transform.parent = roadParent.transform;
        roadBox.GetComponent<RoadBox>()?.Init(road.GetComponent<Road>(), true, true);

        roadBoxs.Add(roadBox);
        roads.Add(road);
        roadParents.Add(roadParent);
    }



    public GameObject GetRoad(int roadNum) {
        if ((roadNum < roads.Count) && (roadNum >= 0)){
            if (roads[roadNum] != null) {
                return roads[roadNum];
            }
        }
        return null;
    }

    public List<GameObject> GetAllRoads() {
        return roads;
    }

    public int NumRoads() {
        return roads.Count - 1;
    }

    public float GetRoadWidth() {
        return roadWidth;
    }

    public void AddRoad(float roadZLength, int segments, float roadCurviness, int numberOfControlPoints) {
        float xLocation = 0;
        float yLocation = 0;
        float zLocation = (roads.Count+1) * roadZLength;
        LandscapeManager.instance.CreateLandscapeForRoad(roads.Count);

        Vector3 newPointOnRoad = new Vector3(xLocation, yLocation, zLocation);
        if (roads.Count == 0) {
            CreateNewRoad(new Vector3(0,0,0), newPointOnRoad, segments, roadCurviness, numberOfControlPoints);
        } else {
            CreateNewRoad(roads[roads.Count - 1].GetComponent<Road>().GetLastPoint(), newPointOnRoad, segments, roadCurviness, numberOfControlPoints);

        }



    }

    public float GetRoadZLength() {
        return 9;
    }

    public int GetSegments() {
        return 16;
    }

    public float GetRoadCurviness() {
        return 2f;
    }

    public int GetNumberOfControlPoints() {
        return 3;
    }

    public int NumberOfLanes() {
        return numberOfLanes;
    }

    public void RemoveRoad(int roadNum) {

        if (roadNum >= 0) {
            Destroy(roads[roadNum]);
            Destroy(roadBoxs[roadNum]);
            Destroy(roadParents[roadNum]);

        }

    }

    public float GetApproxPointOnRoadAtZPosition(float z) {
        int roadNum = Mathf.FloorToInt(z/GetRoadZLength());
        return 0f;
    }

}
