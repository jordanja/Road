using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public static class BezierCurve {




    public static Point3D[] GenerateControlPoints(Point3D firstPoint, Point3D lastPoint, int controlPointsPerCurve, float roadCurviness) {
        Point3D[] controlPoints = new Point3D[controlPointsPerCurve];


        controlPoints[0] = firstPoint;
        controlPoints[controlPointsPerCurve - 1] = lastPoint;

        Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]).toVector();
        for (int remainingPoints = 1; remainingPoints < controlPoints.Length - 1; remainingPoints++) {

            float offset = GetRandomOffset(roadCurviness);
            controlPoints[remainingPoints] = new Point3D(controlPoints[0].getX() + (remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1) + offset, 0, controlPoints[0].getZ() + ((remainingPoints * vecBetween.z) / (controlPointsPerCurve - 1)));


        }

        return controlPoints;


    }
    public static Point3D[] GenerateControlPoints(Point3D firstPoint, Point3D lastPoint, Vector3 lastVector, int controlPointsPerCurve, float roadCurviness) {
        Point3D[] controlPoints = new Point3D[controlPointsPerCurve];

        controlPoints[0] = firstPoint;
        controlPoints[controlPointsPerCurve - 1] = lastPoint;

        controlPoints[1] = controlPoints[0] + lastVector;

        Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]).toVector();

        for (int remainingPoints = 2; remainingPoints < controlPoints.Length - 1; remainingPoints++) {
            float offset = GetRandomOffset(roadCurviness);
            controlPoints[remainingPoints] = new Point3D(controlPoints[0].getX() + ((remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1)) + offset, 0, controlPoints[0].getZ() + (remainingPoints * vecBetween.z / (controlPointsPerCurve - 1)));

        }

        return controlPoints;

    }

    public static Point3D GetLocationOnRoad(float t, Point3D[] controlPoints) {

        int numControlPoints = controlPoints.Length;

        Point3D sum = new Point3D(0, 0, 0);

        for (int i = 0; i < numControlPoints; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 1);

            double term = binCof * Math.Pow((1 - t), (numControlPoints - 1 - i)) * Math.Pow(t, i);


            sum = sum + (controlPoints[i] * term);

        }

        return sum;


    }


    public static Vector3 GetDerivitiveOnRoad(float t, Point3D[] controlPoints) {

        int numControlPoints = controlPoints.Length;
        Point3D sum = new Point3D(0, 0, 0);

        for (int i = 0; i < numControlPoints - 1; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 2);

            double term = binCof * Math.Pow(t, i) * Math.Pow((1 - t), (numControlPoints - 2 - i));

            sum = sum + ((controlPoints[i + 1] - controlPoints[i]) * term);

        }
        return sum.toVector().normalized;
    }


    public static Vector3 FindLastVector(Point3D[] lastRoadControlPoints, int lastRoadControlPointsPerCurve) {
        Vector3 lastVector = (lastRoadControlPoints[lastRoadControlPointsPerCurve - 1] - lastRoadControlPoints[lastRoadControlPointsPerCurve - 2]).toVector();
        return lastVector;
    }

    private static int BinomialCoefficient(int k, int n) {

        int result = 1;
        for (int i = 1; i <= k; i++) {
            result *= n - (k - i);
            result /= i;
        }
        return result;

    }


    private static float GetRandomOffset(float roadCurviness) {
        return roadCurviness * UnityEngine.Random.Range(-1f, +1f);
    }



}
