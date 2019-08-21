using System;
using UnityEngine;

namespace Application {

    public class Point2D {

        private float _x;
        private float _y;


        public Point2D(float x, float y) {
            this._x = x;
            this._y = y;
        }



        public float getX() {
            return _x;
        }

        public float getY() {
            return _y;
        }


        public Vector3 getPosition() {
            return new Vector3(this.getX(), this.getY());
        }

        public static Point2D operator +(Point2D p1, Point2D p2) {
            return new Point2D(p1.getX() + p2.getX(), p1.getY() + p2.getY());
        }
        public static Point2D operator +(Point2D p1, Vector3 vec) {
            return new Point2D(p1.getX() + vec.x, p1.getY() + vec.y);
        }

        public static Point2D operator -(Point2D p1, Point2D p2) {
            return new Point2D(p1.getX() - p2.getX(), p1.getY() - p2.getY());
        }
        public static Point2D operator -(Point2D p1, Vector3 vec) {
            return new Point2D(p1.getX() - vec.x, p1.getY() - vec.y);
        }

        public static Point2D operator *(Point2D point, float multiple) {
            return new Point2D(point.getX() * multiple, point.getY() * multiple);
        }

        public static Point2D operator *(Point2D point, double multiple) {
            return new Point2D((float)(point.getX() * multiple), (float)(point.getY() * multiple));
        }

        public override string ToString() {
            return ("[x: " + _x + ", y: " + _y + "]");
        }
    }
}
