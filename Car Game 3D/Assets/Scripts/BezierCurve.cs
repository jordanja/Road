using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public static class BezierCurve {




    public static Vector3[] GenerateControlPoints(Vector3 firstPoint, Vector3 lastPoint, int controlPointsPerCurve, float roadCurviness) {
        Vector3[] controlPoints = new Vector3[controlPointsPerCurve];


        controlPoints[0] = firstPoint;
        controlPoints[controlPointsPerCurve - 1] = lastPoint;

        Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]);
        for (int remainingPoints = 1; remainingPoints < controlPoints.Length - 1; remainingPoints++) {

            float offset = GetRandomOffset(roadCurviness);
            controlPoints[remainingPoints] = new Vector3(controlPoints[0].x + (remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1) + offset, 0, controlPoints[0].z + ((remainingPoints * vecBetween.z) / (controlPointsPerCurve - 1)));


        }

        return controlPoints;


    }
    public static Vector3[] GenerateControlPoints(Vector3 firstPoint, Vector3 lastPoint, Vector3 lastVector, int controlPointsPerCurve, float roadCurviness) {
        Vector3[] controlPoints = new Vector3[controlPointsPerCurve];

        controlPoints[0] = firstPoint;
        controlPoints[controlPointsPerCurve - 1] = lastPoint;

        controlPoints[1] = controlPoints[0] + lastVector;

        Vector3 vecBetween = (controlPoints[controlPointsPerCurve - 1] - controlPoints[0]);

        for (int remainingPoints = 2; remainingPoints < controlPoints.Length - 1; remainingPoints++) {
            float offset = GetRandomOffset(roadCurviness);
            controlPoints[remainingPoints] = new Vector3(controlPoints[0].x + ((remainingPoints * vecBetween.x) / (controlPointsPerCurve - 1)) + offset, 0, controlPoints[0].z + (remainingPoints * vecBetween.z / (controlPointsPerCurve - 1)));

        }

        return controlPoints;

    }

    public static Vector3 GetLocationOnRoad(float t, Vector3[] controlPoints) {

        int numControlPoints = controlPoints.Length;

        Vector3 sum = new Vector3(0, 0, 0);

        for (int i = 0; i < numControlPoints; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 1);

            double term = binCof * efficientPow((1 - t), (numControlPoints - 1 - i)) * efficientPow(t, i);


            sum = sum + (controlPoints[i] * (float)term);

        }

        return sum;


    }


    public static Vector3 GetDerivitiveOnRoad(float t, Vector3[] controlPoints) {

        int numControlPoints = controlPoints.Length;
        Vector3 sum = new Vector3(0, 0, 0);

        for (int i = 0; i < numControlPoints - 1; i++) {
            int binCof = BinomialCoefficient(i, numControlPoints - 2);

            double term = binCof * efficientPow(t, i) * efficientPow((1 - t), (numControlPoints - 2 - i));

            sum = sum + ((controlPoints[i + 1] - controlPoints[i]) * (float)term);

        }

        return sum.normalized;
    }


    public static Vector3 FindLastVector(Vector3[] lastRoadControlPoints, int lastRoadControlPointsPerCurve) {
        Vector3 lastVector = (lastRoadControlPoints[lastRoadControlPointsPerCurve - 1] - lastRoadControlPoints[lastRoadControlPointsPerCurve - 2]);
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

    private static float efficientPow(float num, int exp) {
        float result = 1f;
        while (exp > 0) {
            if (exp % 2 == 1) {
                result *= num;
            }
            exp >>= 1;
            num *= num;
        }
        return result;
    }



    private static float GetRandomOffset(float roadCurviness) {
        return roadCurviness * UnityEngine.Random.Range(-1f, +1f);
    }



}
