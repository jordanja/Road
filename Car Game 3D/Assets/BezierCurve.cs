using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;
using System;

public static class BezierCurve {



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


    private static int BinomialCoefficient(int k, int n) {

        int result = 1;
        for (int i = 1; i <= k; i++) {
            result *= n - (k - i);
            result /= i;
        }
        return result;

    }






}
