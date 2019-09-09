using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    
    public LandscapeManager instance;

    void Awake() {
        instance = this;
    }

    public void CreateLandscapeBetweenZPoints(float z1, float z2) {
        Mesh landscapeMesh = new Mesh();
        



    }


}
