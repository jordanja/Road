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

    [SerializeField]
    Texture2D roadTexture;

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
        Vector2[] uv = new Vector2[(segments + 1) * 2];

        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            Vector3 derivitive = GetDerivitiveOnRoad(percentageThroughRoad);

            Vector3 rightAngle = RightAngleVector(derivitive)/4;


            
            Point2D side1 = pointsAlongRoad[i] + rightAngle;
            Point2D side2 = pointsAlongRoad[i] - rightAngle;
            points[2 * i] = side1.getPosition();
            points[2 * i + 1] = side2.getPosition();

            uv[2 * i] = new Vector2(0, percentageThroughRoad);
            uv[2 * i + 1] = new Vector2(1, percentageThroughRoad);

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
        mesh.uv = uv;
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

        int numControlPoints = controlPoints.Length;

        Point2D sum = new Point2D(0,0);

        for (int i = 0; i < numControlPoints; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints-1);

            double term = binCof * Math.Pow((1-t), (numControlPoints-1 - i)) * Math.Pow(t, i);

           
            sum = sum + (controlPoints[i] * term);

        }

        return sum;


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

    private void DrawControlPointGizmos() {
        foreach (Point2D point in controlPoints) {

            GameObject circle = Instantiate(circleGizmo, point.getPosition(), Quaternion.identity, this.transform);
            circle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

    }


}
