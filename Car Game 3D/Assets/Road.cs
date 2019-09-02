﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point3D[] controlPoints;

    int _segments;

    float _roadCurviness;

    int _numberOfControlPoints;

    [SerializeField]
    GameObject circleGizmo;

    Point3D firstPoint;
    Point3D lastPoint;
    Vector3 lastVector;

    [SerializeField]
    Material roadMat;


    internal void Init(Point3D first, Point3D last, int segments, float roadCurviness, int numberOfControlPoints) {
        firstPoint = first;
        lastPoint = last;
        bool firstRoad = true;
        _segments = segments;
        _roadCurviness = roadCurviness;
        _numberOfControlPoints = numberOfControlPoints;
        CommonInit(firstRoad);
    }

    internal void Init(Point3D first, Point3D last, Road lastRoad, int segments, float roadCurviness, int numberOfControlPoints) {
        firstPoint = first;
        lastPoint = last;
        lastVector = BezierCurve.FindLastVector(lastRoad.GetControlPoints(), lastRoad.GetControlPointsPerCurve());
        _segments = segments;
        _roadCurviness = roadCurviness;
        _numberOfControlPoints = numberOfControlPoints;

        bool firstRoad = false;
        CommonInit(firstRoad);

    }

    private void CommonInit(bool firstRoad) {
        if (firstRoad) {
            controlPoints = BezierCurve.GenerateControlPoints(firstPoint, lastPoint, _numberOfControlPoints, _roadCurviness);
        } else {
            controlPoints = BezierCurve.GenerateControlPoints(firstPoint, lastPoint, lastVector, _numberOfControlPoints, _roadCurviness);
        }
        GenerateMesh();

    }




    internal Point3D GetLocationOnRoad(float fractionAlongCurrentRoad) {
        return BezierCurve.GetLocationOnRoad(fractionAlongCurrentRoad, controlPoints);
    }

    internal Vector3 GetDerivitiveOnRoad(float fractionAlongCurrentRoad) {
        return BezierCurve.GetDerivitiveOnRoad(fractionAlongCurrentRoad, controlPoints);
    }




    private void GenerateMesh() {


        Point3D[] fractionalPointsAlongBezier = new Point3D[_segments + 1];
        for (int i = 0; i < _segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)_segments));
            fractionalPointsAlongBezier[i] = BezierCurve.GetLocationOnRoad(percentageThroughRoad, controlPoints);
        }


        transform.parent = this.transform;


        gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = roadMat;
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] points = new Vector3[(_segments + 1) * 2];
        int[] triangles = new int[(_segments + 1) * 6];
        Vector2[] uv = new Vector2[(_segments + 1) * 2];


        for (int i = 0; i < _segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)_segments));
            Vector3 derivitive = BezierCurve.GetDerivitiveOnRoad(percentageThroughRoad, controlPoints);

            Vector3 rightAngle = RightAngleVector(derivitive) * RoadManager.instance.GetRoadWidth();

            Point3D side1 = fractionalPointsAlongBezier[i] + rightAngle;
            Point3D side2 = fractionalPointsAlongBezier[i] - rightAngle;

            points[2 * i + 0] = side1.toVector();
            points[2 * i + 1] = side2.toVector();

            uv[2 * i + 0] = new Vector2(0, percentageThroughRoad);
            uv[2 * i + 1] = new Vector2(1f, percentageThroughRoad);


           

        }

        for (int i = 0; i < (_segments); i++) {

            triangles[i * 6 + 0] = i * 2;
            triangles[i * 6 + 1] = i * 2 + 2;
            triangles[i * 6 + 2] = i * 2 + 1;

            triangles[i * 6 + 3] = i * 2 + 1;
            triangles[i * 6 + 4] = i * 2 + 2;//could be 1
            triangles[i * 6 + 5] = i * 2 + 3;
        }



        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uv;



    }

    private Vector3 RightAngleVector(Vector3 derivitive) {
        return new Vector3(-derivitive.z, derivitive.y, derivitive.x);

    }


    private void DrawControlPointGizmos() {

        for (int j = 0; j < controlPoints.Length; j++) {
            GameObject circle = Instantiate(circleGizmo, controlPoints[j].toVector(), Quaternion.identity, this.transform);
            circle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            circle.name = "Gizmo " + j;
        }




    }

    public Point3D[] GetControlPoints() {
        return controlPoints;
    }

    public int GetControlPointsPerCurve() {
        return _numberOfControlPoints;
    }



}
