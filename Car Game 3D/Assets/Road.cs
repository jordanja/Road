using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point3D[] pointsOnRoad = { new Point3D(0, 0, 0), new Point3D(0, 0, 2), new Point3D(0, 0, 4), new Point3D(0, 0, 6), new Point3D(0, 0, 8) };

    int segments = 32;

    float roadCurviness = 4;

    float roadWidth = 0.25f;

    int controlPointsPerCurve = 4;

    [SerializeField]
    GameObject circleGizmo;

    [SerializeField]
    Material roadMat;

    [SerializeField]
    GameObject SubRoads;

    [SerializeField]
    GameObject Gizmos;

    void Start() {
        Point3D[][] controlPoints = GenerateControlPoints();
        DrawControlPointGizmos(controlPoints);

        DrawRoad(controlPoints);

    }

    private Point3D[][] GenerateControlPoints() {
        Point3D[][] controlPoints = new Point3D[pointsOnRoad.Length-1][];

        for (int i = 0; i < pointsOnRoad.Length-1; i++) {

            Point3D[] points = new Point3D[controlPointsPerCurve];


            if (i == 0) {
                points[0] = pointsOnRoad[0];
                points[controlPointsPerCurve-1] = pointsOnRoad[i + 1];

                Vector3 vecBetween = (points[controlPointsPerCurve - 1] - points[0]).toVector();
                for (int remainingPoints = 1; remainingPoints < points.Length - 1; remainingPoints++) {

                    float offset = GetRandomOffset();
                    points[remainingPoints] = new Point3D(points[0].getX() + (remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1) + offset, 0, points[0].getZ() + ((remainingPoints * vecBetween.z) / (controlPointsPerCurve - 1)));

                
                }



            } else {
                points[0] = pointsOnRoad[i];
                points[controlPointsPerCurve - 1] = pointsOnRoad[i + 1];
                Vector3 lastVector = (controlPoints[i - 1][controlPointsPerCurve - 1] - controlPoints[i - 1][controlPointsPerCurve - 2]).toVector();


                points[1] = points[0] + lastVector;

                Vector3 vecBetween = (points[controlPointsPerCurve - 1] - points[0]).toVector();

                for (int remainingPoints = 2; remainingPoints < points.Length - 1; remainingPoints++) {
                    float offset = GetRandomOffset();
                    points[remainingPoints] = new Point3D(points[0].getX() + ((remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1)) + offset, 0, points[0].getZ() + (remainingPoints * vecBetween.z / (controlPointsPerCurve - 1)));

                }



            }
            controlPoints[i] = points;
        }

        return controlPoints;

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


    private void DrawRoad(Point3D[][] controlPoints) {

        for (int curve = 0; curve < controlPoints.Length; curve++) {

            Point3D[] fractionalPointsAlongBezier = new Point3D[segments + 1];
            for (int i = 0; i < segments + 1; i++) {
                float percentageThroughRoad = (float)(((float)i) / ((float)segments));
                fractionalPointsAlongBezier[i] = GetLocationOnRoad(percentageThroughRoad, controlPoints[curve]);
            }


            GameObject road = new GameObject();
            road.name = "road " + curve.ToString();
            road.transform.parent = SubRoads.transform;


            road.AddComponent<MeshFilter>();
            MeshRenderer mr = road.AddComponent<MeshRenderer>();
            mr.material = roadMat;
            Mesh mesh = road.GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            
            Vector3[] points = new Vector3[(segments + 1) * 2];
            int[] triangles = new int[(segments + 1) * 6];
            Vector2[] uv = new Vector2[(segments + 1) * 2];
            
            for (int i = 0; i < segments + 1; i++) {
                float percentageThroughRoad = (float)(((float)i) / ((float)segments));
                Vector3 derivitive = GetDerivitiveOnRoad(percentageThroughRoad, controlPoints[curve]);
                
                Vector3 rightAngle = RightAngleVector(derivitive) * roadWidth;

                Point3D side1 = fractionalPointsAlongBezier[i] + rightAngle;
                Point3D side2 = fractionalPointsAlongBezier[i] - rightAngle;
                points[2 * i] = side1.toVector();
                points[2 * i + 1] = side2.toVector();
                
                uv[2 * i] = new Vector2(0, percentageThroughRoad);
                uv[2 * i + 1] = new Vector2(0.5f, percentageThroughRoad);
                
            }
            
            for (int i = 0; i < (segments); i++) {
                
                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = i * 2 + 2;
                triangles[i * 6 + 2] = i * 2 + 1;
                
                triangles[i * 6 + 3] = i * 2 + 1;
                triangles[i * 6 + 5] = i * 2 + 3;
                triangles[i * 6 + 4] = i * 2 + 2;
            }

            mesh.vertices = points;
            mesh.triangles = triangles;
            mesh.uv = uv;
        }


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
            int binCof = BinomialCoefficient(i, numControlPoints-1);

            double term = binCof * Math.Pow((1-t), (numControlPoints-1 - i)) * Math.Pow(t, i);

           
            sum = sum + (curveControlPoints[i] * term);

        }

        return sum;


    }


    private Vector3 GetDerivitiveOnRoad(float t, Point3D[] curveControlPoints) {

        int numControlPoints = curveControlPoints.Length;
        Point3D sum = new Point3D(0, 0, 0);

        for (int i = 0; i < numControlPoints - 1; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 2);

            double term = binCof * Math.Pow(t,i) * Math.Pow((1 - t),(numControlPoints - 2 - i));

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

    private void DrawControlPointGizmos(Point3D[][] controlPoints) {
        for (int i = 0; i < controlPoints.Length; i++) {

            for (int j = 0; j < controlPoints[i].Length; j++) {
                GameObject circle = Instantiate(circleGizmo, controlPoints[i][j].toVector(), Quaternion.identity, Gizmos.transform);
                circle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                circle.name = "Gizmo " + i + ":" + j;
            }


        }

    }


}
