using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public class Road : MonoBehaviour {

    Point2D[] controlPoints = { new Point2D(0, 0), new Point2D(3, 3), new Point2D(-3, 3), new Point2D(-3, -3) };
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




    private void DrawControlPointGizmos() {
        foreach (Point2D point in controlPoints) {

            GameObject circle = Instantiate(circleGizmo, point.getPosition(), Quaternion.identity, this.transform);
            circle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

    }


}
