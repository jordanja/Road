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



    }
}
