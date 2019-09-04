using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEdge : MonoBehaviour {


    [SerializeField]
    Material EdgeMat;

    float heightAboveRoad;

    internal void Init(Road baseRoad) {
        MeshFilter origMF = baseRoad.gameObject.GetComponent<MeshFilter>();
        if (origMF != null) {
            MeshFilter newMf = gameObject.AddComponent<MeshFilter>();
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = EdgeMat;

            Mesh newMesh = createNewMesh(origMF.mesh);

            newMf.mesh = newMesh;
        } 
    }

    private Mesh createNewMesh(Mesh baseRoadMesh) {




        return null;

    }
}
