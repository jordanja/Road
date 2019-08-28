using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class RoadManager : MonoBehaviour {

    public static RoadManager instance;

    private List<GameObject> roads = new List<GameObject>();

    Point3D[] pointsOnRoad = { new Point3D(0, 0, 0), new Point3D(0, 0, 2), new Point3D(0, 0, 4), new Point3D(0, 0, 6), new Point3D(0, 0, 8) };


    [SerializeField]
    GameObject roadBlueprint;

    void Awake() {
        instance = this;
    }

    private void Start() {
        for (int i = 0; i < pointsOnRoad.Length - 1; i++) {
            CreateNewRoad(pointsOnRoad[i], pointsOnRoad[i + 1]);
        }
    }

    public void CreateNewRoad(Point3D firstPoint, Point3D lastPoint) {
        GameObject road = Instantiate(roadBlueprint, transform, false);
        road.name = "Road " + roads.Count;
        if (roads.Count == 0) {
            road.GetComponent<Road>()?.Init(firstPoint, lastPoint);
        } else {
            GameObject lastRoad = roads[roads.Count - 1];
            Point3D[] controlPoints = lastRoad.GetComponent<Road>().GetControlPoints();
            int controlPointsPerCurve = lastRoad.GetComponent<Road>().GetControlPointsPerCurve();
            Vector3 lastVector = (controlPoints[controlPointsPerCurve - 1] - controlPoints[controlPointsPerCurve - 2]).toVector();

            road.GetComponent<Road>()?.Init(firstPoint, lastPoint,lastVector);

        }
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

}
