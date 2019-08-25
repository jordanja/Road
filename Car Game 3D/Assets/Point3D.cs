using UnityEngine;
using System;

namespace Application {

    public class Point3D {

        private float _x;
        private float _y;
        private float _z;


        public Point3D(float x, float y, float z) {
            _x = x;
            _y = y;
            _z = z;
        }



        public float getX() {
            return _x;
        }

        public float getY() {
            return _y;
        }

        public float getZ() {
            return _z;
        }

        public Vector3 toVector() {
            return new Vector3(this.getX(), this.getY(), this.getZ());
        }



        public static Point3D operator +(Point3D p1, Point3D p2) {
            return new Point3D(p1.getX() + p2.getX(), p1.getY() + p2.getY(), p1.getZ() + p2.getZ());
        }
        public static Point3D operator +(Point3D p1, Vector3 vec) {
            return new Point3D(p1.getX() + vec.x, p1.getY() + vec.y, p1.getZ() + vec.z);
        }

        public static Point3D operator -(Point3D p1, Point3D p2) {
            return new Point3D(p1.getX() - p2.getX(), p1.getY() - p2.getY(), p1.getZ() - p2.getZ());
        }
        public static Point3D operator -(Point3D point, Vector3 vec) {
            return new Point3D(point.getX() - vec.x, point.getY() - vec.y, point.getZ() - vec.z);
        }

        public static Point3D operator *(Point3D point, float multiple) {
            return new Point3D(point.getX() * multiple, point.getY() * multiple, point.getZ() * multiple);
        }

        public static Point3D operator *(Point3D point, double multiple) {
            return new Point3D((float)(point.getX() * multiple), (float)(point.getY() * multiple), (float)(point.getZ() * multiple));
        }

        public override string ToString() {
            return ("[x: " + this.getX().ToString("0.##") + ", y: " + this.getY().ToString("0.##") + ", z: " + this.getZ().ToString("0.##") + "]");
        }



    }

}
