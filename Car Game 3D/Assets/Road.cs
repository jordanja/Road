﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point3D[] controlPoints;

    int segments = 32;

    float roadCurviness = 4;

    float roadWidth = 0.25f;

    int controlPointsPerCurve = 4;

    float roadDepth = 1;

    [SerializeField]
    GameObject circleGizmo;

    Point3D firstPoint;
    Point3D lastPoint;
    Vector3 lastVector;

    [SerializeField]
    Material roadMat;


    internal void Init(Point3D first, Point3D last) {
        firstPoint = first;
        lastPoint = last;
        bool firstRoad = true;
        CommonInit(firstRoad);
    }

    internal void Init(Point3D first, Point3D last, Vector3 lastV) {
        firstPoint = first;
        lastPoint = last;
        lastVector = lastV;
        bool firstRoad = false;
        CommonInit(firstRoad);

    }

    private void CommonInit(bool firstRoad) {
        GenerateControlPoints(firstRoad);
        //DrawControlPointGizmos();
        DrawRoad();

    }


    private void GenerateControlPoints(bool firstRoad) {
        controlPoints = new Point3D[controlPointsPerCurve];


        if (firstRoad == true) {
            controlPoints[0] = firstPoint;
            controlPoints[controlPointsPerCurve - 1] = lastPoint;

            Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]).toVector();
            for (int remainingPoints = 1; remainingPoints < controlPoints.Length - 1; remainingPoints++) {

                float offset = GetRandomOffset();
                controlPoints[remainingPoints] = new Point3D(controlPoints[0].getX() + (remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1) + offset, 0, controlPoints[0].getZ() + ((remainingPoints * vecBetween.z) / (controlPointsPerCurve - 1)));


            }



        } else {
            controlPoints[0] = firstPoint;
            controlPoints[controlPointsPerCurve - 1] = lastPoint;
            
            controlPoints[1] = controlPoints[0] + lastVector;

            Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]).toVector();

            for (int remainingPoints = 2; remainingPoints < controlPoints.Length - 1; remainingPoints++) {
                float offset = GetRandomOffset();
                controlPoints[remainingPoints] = new Point3D(controlPoints[0].getX() + ((remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1)) + offset, 0, controlPoints[0].getZ() + (remainingPoints * vecBetween.z / (controlPointsPerCurve - 1)));

            }



        }


    }

    internal Point3D GetLocationOnRoad(float fractionAlongCurrentRoad) {
        return BezierCurve.GetLocationOnRoad(fractionAlongCurrentRoad, controlPoints);
    }

    internal Vector3 GetDerivitiveOnRoad(float fractionAlongCurrentRoad) {
        return BezierCurve.GetDerivitiveOnRoad(fractionAlongCurrentRoad, controlPoints);
    }

    private float GetRandomOffset() {
        return roadCurviness * UnityEngine.Random.Range(-1f, +1f);
    }


    private void DrawRoad() {


        Point3D[] fractionalPointsAlongBezier = new Point3D[segments + 1];
        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            fractionalPointsAlongBezier[i] = BezierCurve.GetLocationOnRoad(percentageThroughRoad, controlPoints);
        }


        transform.parent = this.transform;


        gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = roadMat;
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] points = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[(segments + 1) * 6];
        Vector2[] uv = new Vector2[(segments + 1) * 2];


        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            Vector3 derivitive = BezierCurve.GetDerivitiveOnRoad(percentageThroughRoad, controlPoints);

            Vector3 rightAngle = RightAngleVector(derivitive) * roadWidth;

            Point3D side1 = fractionalPointsAlongBezier[i] + rightAngle;
            Point3D side2 = fractionalPointsAlongBezier[i] - rightAngle;

            points[2 * i + 0] = side1.toVector();
            points[2 * i + 1] = side2.toVector();

            uv[2 * i + 0] = new Vector2(0, percentageThroughRoad);
            uv[2 * i + 1] = new Vector2(1f, percentageThroughRoad);


           

        }

        for (int i = 0; i < (segments); i++) {

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


    private Point2D[] GetRow(Point2D[,] matrix, int row) {
        var rowLength = matrix.GetLength(1);
        var rowVector = new Point2D[rowLength];

        for (var i = 0; i < rowLength; i++)
            rowVector[i] = matrix[row, i];

        return rowVector;
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
        return controlPointsPerCurve;
    }


}
