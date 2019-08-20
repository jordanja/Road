using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point2D[] controlPoints = { new Point2D(0, 0), new Point2D(3, 3), new Point2D(-3, 3), new Point2D(-3, -3) };
    int segments = 12;

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

        //LineRenderRoad(pointsAlongRoad);

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] points = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[(segments + 1) * 6];


        for (int i = 0; i < segments + 1; i++) {
            float percentageThroughRoad = (float)(((float)i) / ((float)segments));
            Vector3 derivitive = GetDerivitiveOnRoad(percentageThroughRoad);

            print("derivitive = " + derivitive);

            Point2D side1 = pointsAlongRoad[i] + derivitive;
            Point2D side2 = pointsAlongRoad[i] - derivitive;
            points[2 * i] = side1.getPosition();
            points[2 * i + 1] = side2.getPosition();

        }

        for (int i = 0; i < segments + 1; i++) {

            triangles[i*6] = i;
            triangles[i*6 + 2] = i + 2;
            triangles[i*6 + 1] = i + 1;


        }

        mesh.vertices = points;
        mesh.triangles = triangles;
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

        float multiple = (1 - t) * (1 - t) * (1 - t);
        Point2D p0 = new Point2D(controlPoints[0].getX() * multiple, controlPoints[0].getY() * multiple);

        multiple = 3 * (1 - t) * (1 - t) * t;
        Point2D p1 = new Point2D(controlPoints[1].getX() * multiple, controlPoints[1].getY() * multiple);

        multiple = 3 * (1 - t) * t * t;
        Point2D p2 = new Point2D(controlPoints[2].getX() * multiple, controlPoints[2].getY() * multiple);

        multiple = t * t * t;
        Point2D p3 = new Point2D(controlPoints[3].getX() * multiple, controlPoints[3].getY() * multiple);

        return new Point2D(p0.getX() + p1.getX() + p2.getX() + p3.getX(), p0.getY() + p1.getY() + p2.getY() + p3.getY());
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
