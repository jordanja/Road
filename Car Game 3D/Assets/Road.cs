using System.Collections;
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

    [SerializeField]
    GameObject SubRoads;

    [SerializeField]
    GameObject Gizmos;



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
        DrawControlPointGizmos();
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

            //Vector3 lastVector = (controlPoints[i - 1][controlPointsPerCurve - 1] - controlPoints[i - 1][controlPointsPerCurve - 2]).toVector();

            controlPoints[1] = controlPoints[0] + lastVector;

            Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]).toVector();

            for (int remainingPoints = 2; remainingPoints < controlPoints.Length - 1; remainingPoints++) {
                float offset = GetRandomOffset();
                controlPoints[remainingPoints] = new Point3D(controlPoints[0].getX() + ((remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1)) + offset, 0, controlPoints[0].getZ() + (remainingPoints * vecBetween.z / (controlPointsPerCurve - 1)));

            }



        }


    }

    private float GetRandomOffset() {
        return roadCurviness * UnityEngine.Random.Range(-1f, +1f);
    }

    private void printControlPoints(Point3D[][] controlPoints) {

        for (int i = 0; i < controlPoints.Length; i++) {
            string line = "";
            for (int j = 0; j < controlPoints[i].Length; j++) {
                line = line + controlPoints[i][j] + ", ";
            }
            print(line);
        }
    }


    private void DrawRoad() {


        Point3D[] fractionalPointsAlongBezier = new Point3D[segments + 1];
        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            fractionalPointsAlongBezier[i] = GetLocationOnRoad(percentageThroughRoad, controlPoints);
        }


        GameObject road = new GameObject();
        road.name = "road (add number)";
        //road.transform.parent = SubRoads.transform;


        road.AddComponent<MeshFilter>();
        MeshRenderer mr = road.AddComponent<MeshRenderer>();
        mr.material = roadMat;
        Mesh mesh = road.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] points = new Vector3[(segments + 1) * 4];
        int[] triangles = new int[(segments + 1) * 18];
        Vector2[] uv = new Vector2[(segments + 1) * 4];


        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            Vector3 derivitive = GetDerivitiveOnRoad(percentageThroughRoad, controlPoints);

            Vector3 rightAngle = RightAngleVector(derivitive) * roadWidth;

            Point3D side1 = fractionalPointsAlongBezier[i] + rightAngle;
            Point3D side2 = fractionalPointsAlongBezier[i] - rightAngle;

            points[4 * i + 0] = side1.toVector();
            points[4 * i + 1] = side2.toVector();

            uv[2 * i + 0] = new Vector2(0, percentageThroughRoad);
            uv[2 * i + 1] = new Vector2(0.5f, percentageThroughRoad);


            Point3D bottom1 = side1 - new Vector3(0, roadDepth, 0);
            Point3D bottom2 = side2 - new Vector3(0, roadDepth, 0);
            points[4 * i + 2] = bottom1.toVector();
            points[4 * i + 3] = bottom2.toVector();

            uv[2 * i + 2] = new Vector2(1f, 0);
            uv[2 * i + 3] = new Vector2(1f, 0);


        }

        for (int i = 0; i < (segments); i++) {

            triangles[i * 18 + 0] = i * 4;
            triangles[i * 18 + 1] = i * 4 + 4;
            triangles[i * 18 + 2] = i * 4 + 1;

            triangles[i * 18 + 3] = i * 4 + 1;
            triangles[i * 18 + 4] = i * 4 + 4;
            triangles[i * 18 + 5] = i * 4 + 5;


            triangles[i * 18 + 6] = i * 4 + 1;
            triangles[i * 18 + 7] = i * 4 + 7;
            triangles[i * 18 + 8] = i * 4 + 3;

            triangles[i * 18 + 9] = i * 4 + 1;
            triangles[i * 18 + 10] = i * 4 + 5;
            triangles[i * 18 + 11] = i * 4 + 7;



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

    private Point3D GetLocationOnRoad(float t, Point3D[] curveControlPoints) {

        int numControlPoints = curveControlPoints.Length;

        Point3D sum = new Point3D(0, 0, 0);

        for (int i = 0; i < numControlPoints; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 1);

            double term = binCof * Math.Pow((1 - t), (numControlPoints - 1 - i)) * Math.Pow(t, i);


            sum = sum + (curveControlPoints[i] * term);

        }

        return sum;


    }


    private Vector3 GetDerivitiveOnRoad(float t, Point3D[] curveControlPoints) {

        int numControlPoints = curveControlPoints.Length;
        Point3D sum = new Point3D(0, 0, 0);

        for (int i = 0; i < numControlPoints - 1; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 2);

            double term = binCof * Math.Pow(t, i) * Math.Pow((1 - t), (numControlPoints - 2 - i));

            sum = sum + ((curveControlPoints[i + 1] - curveControlPoints[i]) * term);

        }
        return sum.toVector().normalized;
    }


    private int BinomialCoefficient(int k, int n) {

        int result = 1;
        for (int i = 1; i <= k; i++) {
            result *= n - (k - i);
            result /= i;
        }
        return result;

    }

    private void DrawControlPointGizmos() {

        for (int j = 0; j < controlPoints.Length; j++) {
            GameObject circle = Instantiate(circleGizmo, controlPoints[j].toVector(), Quaternion.identity, Gizmos.transform);
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
