﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class RoadManager : MonoBehaviour {

    public static RoadManager instance;

    private List<GameObject> roads = new List<GameObject>();

    List<Vector3> pointsOnRoad;// = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(0, 0, 9), new Vector3(0, 0, 18), new Vector3(0, 0, 27), new Vector3(0, 0, 36) };

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

        // for (int i = 0; i < pointsOnRoad.Count - 1; i++) {
        //     CreateNewRoad(pointsOnRoad[i], pointsOnRoad[i + 1], segments, roadCurviness, numberOfControlPoints);
        // }
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
        roadBox.name = "Road Box " + roads.Count;
        roadBox.transform.parent = roadParent.transform;
        roadBox.GetComponent<RoadBox>()?.Init(road.GetComponent<Road>(), true, true);

        roads.Add(road);
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
        return 32;
    }

    public float GetRoadCurviness() {
        return 5f;
    }

    public int GetNumberOfControlPoints() {
        return 3;
    }

}
