using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point2D[] controlPoints = { new Point2D(0, 0), new Point2D(3, 3), new Point2D(-3, 3), new Point2D(-3, -3) };
    //Point2D[] controlPoints = { new Point2D(0, 0), new Point2D(0, 1), new Point2D(0, 2), new Point2D(3, 3) };

    int segments = 32;

    [SerializeField]
    GameObject circleGizmo;

    void Start() {
        DrawControlPointGizmos();

        DrawRoad();

    }

    private void DrawRoad() {
        Point2D[] pointsAlongRoad = new Point2D[segments + 1];
        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            pointsAlongRoad[i] = GetLocationOnRoad(percentageThroughRoad);
        }

        LineRenderRoad(pointsAlongRoad);

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] points = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[(segments + 1) * 6];


        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            Vector3 derivitive = GetDerivitiveOnRoad(percentageThroughRoad);

            Vector3 rightAngle = RightAngleVector(derivitive);
            
            Point2D side1 = pointsAlongRoad[i] + rightAngle;
            Point2D side2 = pointsAlongRoad[i] - rightAngle;
            points[2 * i] = side1.getPosition();
            points[2 * i + 1] = side2.getPosition();

        }

        for (int i = 0; i < (segments); i++) {

            triangles[i * 6] = i*2;
            triangles[i * 6 + 1] = i * 2 + 2;
            triangles[i * 6 + 2] = i*2 + 1;

            triangles[i * 6 + 3] = i*2 + 1;
            triangles[i * 6 + 5] = i*2 + 3;
            triangles[i * 6 + 4] = i*2 + 2;
        }

        mesh.vertices = points;
        mesh.triangles = triangles;
    }

    private Vector3 RightAngleVector(Vector3 derivitive) {
        return new Vector3(-derivitive.y, derivitive.x);
        
    }

    private void LineRenderRoad(Point2D[] pointsAlongRoad) {
        LineRenderer lRend = gameObject.AddComponent<LineRenderer>();
        lRend.positionCount = segments + 1;
        lRend.startWidth = 0.1f;
        lRend.endWidth = 0.1f;

        for (int i = 0; i < pointsAlongRoad.Length; i++) {
            lRend.SetPosition(i, pointsAlongRoad[i].getPosition());
        }
    }

    private Point2D GetLocationOnRoad(float t) {

        float multiple;

        multiple = (1 - t) * (1 - t) * (1 - t);
        Point2D p0 = controlPoints[0] * multiple;
        print("origonal multiple 1: " + multiple);

        multiple = 3 * (1 - t) * (1 - t) * t;
        Point2D p1 = controlPoints[1] * multiple;
        print("origonal multiple 2: " + multiple);

        multiple = 3 * (1 - t) * t * t;
        Point2D p2 = controlPoints[2] * multiple;
        print("origonal multiple 3: " + multiple);

        multiple = t * t * t;
        Point2D p3 = controlPoints[3] * multiple;
        print("origonal multiple 4: " + multiple);


        print("origonal sum: " + (p0 + p1 + p2 + p3));
        //return p0 + p1 + p2 + p3;

        int numControlPoints = controlPoints.Length;

        Point2D sum = new Point2D(0,0);

        for (int i = 0; i < numControlPoints; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints-1);

            double term = binCof * Math.Pow((1-t), (numControlPoints-1 - i)) * Math.Pow(t, i);

            print("multiple " + (i+1) + ": " + term);
            print("the above multiple = (1-t)^" + (numControlPoints - 1 - i) + " * t^" + i);

            sum = sum + (controlPoints[i] * term);

        }

        print("new sum: " + sum);

        print("\n");
        return sum;


    }

    private int BinomialCoefficient(int K, int N) {

        int result = 1;
        for (int i = 1; i <= K; i++) {
            result *= N - (K - i);
            result /= i;
        }
        return result;

    }

    private Vector3 GetDerivitiveOnRoad(float t) {

        float multiple;
        Point2D point;

        multiple = 3 * (1 - t) * (1 - t);
        point = controlPoints[1] - controlPoints[0];
        Point2D p0 = point * multiple;

        multiple = 6 * (1 - t) * t;
        point = controlPoints[2] - controlPoints[1];
        Point2D p1 = point * multiple;

        multiple = 3 * t * t;
        point = controlPoints[3] - controlPoints[2];
        Point2D p2 = point * multiple;

        return (p0 + p1 + p2).getPosition().normalized/4;

    }



    private void DrawControlPointGizmos() {
        foreach (Point2D point in controlPoints) {

            GameObject circle = Instantiate(circleGizmo, point.getPosition(), Quaternion.identity, this.transform);
            circle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

    }


}
