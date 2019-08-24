using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point2D[] pointsOnRoad = { new Point2D(-6, 0), new Point2D(-3, 0), new Point2D(0, 0), new Point2D(3, 0), new Point2D(6, 0)};

    int segments = 32;

    float roadCurveHeight = 4;

    [SerializeField]
    GameObject circleGizmo;

    [SerializeField]
    Material roadMat;

    [SerializeField]
    Texture roadTexture;

    void Start() {
        Point2D[,] controlPoints = GenerateControlPoints();

        //DrawControlPointGizmos(controlPoints);

        DrawRoad(controlPoints);

    }

    private Point2D[,] GenerateControlPoints() {
        Point2D[,] controlPoints = new Point2D[pointsOnRoad.Length-1,4];

        for (int i = 0; i < pointsOnRoad.Length-1; i++) {
            Point2D p0;
            Point2D p1;
            Point2D p2;
            Point2D p3;

            if (i == 0) {
                p0 = pointsOnRoad[0];
                p3 = pointsOnRoad[1];

                Vector2 vecBetween = (p3 - p0).getPosition();
                float offset = GetRandomOffset();

                p1 = new Point2D(p0.getX() + vecBetween.x / 3, p0.getY() + (vecBetween.y / 3) + offset);

                offset = GetRandomOffset();
                p2 = new Point2D(p0.getX() + (2 * vecBetween.x / 3), p0.getY() + (2 * vecBetween.y / 3) + offset);



            } else {
                p0 = pointsOnRoad[i];
                p3 = pointsOnRoad[i + 1];

                Vector2 p3ToP2 = (controlPoints[i - 1,3] - controlPoints[i - 1,2]).toVector();
                Vector2 p2ToP3 = -p3ToP2;
                p1 = p0 + p3ToP2;

                Vector2 vecBetween = (p3 - p0).getPosition();
                float offset = GetRandomOffset();
                p2 = new Point2D(p0.getX() + (2 * vecBetween.x / 3), p0.getY() + (2 * vecBetween.y / 3) + offset);

            }
            controlPoints[i, 0] = p0;
            controlPoints[i, 1] = p1;
            controlPoints[i, 2] = p2;
            controlPoints[i, 3] = p3;
        }

        return controlPoints;

    }

    private float GetRandomOffset() {
        return roadCurveHeight * UnityEngine.Random.Range(-1f, +1f);
    }

    private void printControlPoints(Point2D[,] controlPoints) {

        for (int i = 0; i < controlPoints.GetLength(0); i++) {
            string line = "";
            for (int j = 0; j < controlPoints.GetLength(1); j++) {
                line = line + controlPoints[i, j] + ", ";
            }
            print(line);
        }
    }


    private void DrawRoad(Point2D[,] controlPoints) {

        for (int curve = 0; curve < controlPoints.GetLength(0); curve++) {

            Point2D[] fractionalPointsAlongBezier = new Point2D[segments + 1];
            for (int i = 0; i < segments + 1; i++) {
                float percentageThroughRoad = (float)(((float)i) / ((float)segments));
                fractionalPointsAlongBezier[i] = GetLocationOnRoad(percentageThroughRoad, GetRow(controlPoints,curve));
            }



            GameObject road = new GameObject();
            road.name = "road " + curve.ToString();
            road.transform.parent = this.transform;


            road.AddComponent<MeshFilter>();
            MeshRenderer mr = road.AddComponent<MeshRenderer>();
            mr.material = roadMat;
            //mr.
            Mesh mesh = road.GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            
            Vector3[] points = new Vector3[(segments + 1) * 2];
            int[] triangles = new int[(segments + 1) * 6];
            Vector2[] uv = new Vector2[(segments + 1) * 2];
            
            for (int i = 0; i < segments + 1; i++) {
                float percentageThroughRoad = (float)(((float)i) / ((float)segments));
                Vector3 derivitive = GetDerivitiveOnRoad(percentageThroughRoad,GetRow(controlPoints,curve));
                
                Vector3 rightAngle = RightAngleVector(derivitive)/4;
                
                
                
                Point2D side1 = fractionalPointsAlongBezier[i] + rightAngle;
                Point2D side2 = fractionalPointsAlongBezier[i] - rightAngle;
                points[2 * i] = side1.getPosition();
                points[2 * i + 1] = side2.getPosition();
                
                uv[2 * i] = new Vector2(0, percentageThroughRoad);
                uv[2 * i + 1] = new Vector2(1, percentageThroughRoad);
                
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
        return new Vector3(-derivitive.y, derivitive.x);
        
    }


    private Point2D[] GetRow(Point2D[,] matrix, int row) {
        var rowLength = matrix.GetLength(1);
        var rowVector = new Point2D[rowLength];

        for (var i = 0; i < rowLength; i++)
            rowVector[i] = matrix[row, i];

        return rowVector;
    }
     
    private Point2D GetLocationOnRoad(float t, Point2D[] curveControlPoints) {

        int numControlPoints = curveControlPoints.Length;

        Point2D sum = new Point2D(0,0);

        for (int i = 0; i < numControlPoints; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints-1);

            double term = binCof * Math.Pow((1-t), (numControlPoints-1 - i)) * Math.Pow(t, i);

           
            sum = sum + (curveControlPoints[i] * term);

        }

        return sum;


    }


    private Vector3 GetDerivitiveOnRoad(float t, Point2D[] curveControlPoints) {

        float multiple;
        Point2D point;

        multiple = 3 * (1 - t) * (1 - t);
        point = curveControlPoints[1] - curveControlPoints[0];
        Point2D p0 = point * multiple;

        multiple = 6 * (1 - t) * t;
        point = curveControlPoints[2] - curveControlPoints[1];
        Point2D p1 = point * multiple;

        multiple = 3 * t * t;
        point = curveControlPoints[3] - curveControlPoints[2];
        Point2D p2 = point * multiple;

        return (p0 + p1 + p2).getPosition().normalized;

    }


    private int BinomialCoefficient(int k, int n) {
        
        int result = 1;
        for (int i = 1; i <= k; i++) {
            result *= n - (k - i);
            result /= i;
        }
        return result;
        
    }

    private void DrawControlPointGizmos(Point2D[,] controlPoints) {
        for (int i = 0; i < controlPoints.GetLength(0); i++) {

            for (int j = 0; j < controlPoints.GetLength(1); j++) {
                GameObject circle = Instantiate(circleGizmo, controlPoints[i,j].getPosition(), Quaternion.identity, this.transform);
                circle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }


        }

    }


}
