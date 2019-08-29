using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class RoadManager : MonoBehaviour {

    public static RoadManager instance;

    private List<GameObject> roads = new List<GameObject>();

    Point3D[] pointsOnRoad = { new Point3D(0, 0, 0), new Point3D(0, 0, 2), new Point3D(0, 0, 4), new Point3D(0, 0, 6), new Point3D(0, 0, 8) };

    public bool initialized = false;

    [SerializeField]
    GameObject roadBlueprint;

    void Awake() {
        instance = this;
    }

    private void Start() {
        for (int i = 0; i < pointsOnRoad.Length - 1; i++) {
            CreateNewRoad(pointsOnRoad[i], pointsOnRoad[i + 1]);
        }
        initialized = true;
    }

    public void CreateNewRoad(Point3D firstPoint, Point3D lastPoint) {
        GameObject roadParent = new GameObject();
        roadParent.transform.parent = this.transform;
        roadParent.name = "Road Parent " + roads.Count;

        GameObject road = Instantiate(roadBlueprint, roadParent.transform, false);
        road.name = "Road " + roads.Count;

        if (roads.Count == 0) {
            road.GetComponent<Road>()?.Init(firstPoint, lastPoint);
        } else {
            road.GetComponent<Road>()?.Init(firstPoint, lastPoint, roads[roads.Count - 1]);

        }

        GameObject roadBox = new GameObject();
        roadBox.name = "Road Box " + roads.Count;
        roadBox.transform.parent = roadParent.transform;


        roads.Add(road);
    }

    public GameObject GetRoad(int roadNum) {
        if (roads[roadNum] != null) {
            return roads[roadNum];
        }
        return null;
    }

    public List<GameObject> GetAllRoads() {
        return roads;
    }

    public int NumRoads() {
        return roads.Count - 1;
    }

}
