using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBox : MonoBehaviour {

    [SerializeField]
    Material SideMat;

    internal void Init(Road baseRoad) {
        MeshFilter origMF = baseRoad.gameObject.GetComponent<MeshFilter>();
        if (origMF != null) {
            MeshFilter newMf = gameObject.AddComponent<MeshFilter>();
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = SideMat;

            Mesh newMesh = createNewMesh(origMF.mesh);

            newMf.mesh = newMesh;
        } 
    }

    private Mesh createNewMesh(Mesh baseRoadMesh) {

        Mesh newMesh = new Mesh();
        newMesh.Clear();

        Vector3[] vertices = new Vector3[baseRoadMesh.vertices.Length * 2];

        Vector3 loweredVertice = new Vector3(0,0.2f,0);

        for (int i = 0; i < baseRoadMesh.vertices.Length; i++) {
            vertices[i * 2 + 0] = baseRoadMesh.vertices[i];
            vertices[i * 2 + 1] = baseRoadMesh.vertices[i] - loweredVertice;
        }
        newMesh.vertices = vertices;
        

        int[] loweredTris = new int[(6 + 6 + 6 + 6) * baseRoadMesh.vertices.Length/2];
        for (int i = 0; i < baseRoadMesh.vertices.Length/2 - 1; i++) {
            loweredTris[i * 24 + 0] = i * 4 + 0;
            loweredTris[i * 24 + 1] = i * 4 + 1;
            loweredTris[i * 24 + 2] = i * 4 + 4;
        
            loweredTris[i * 24 + 3] = i * 4 + 4;
            loweredTris[i * 24 + 4] = i * 4 + 1;
            loweredTris[i * 24 + 5] = i * 4 + 5;
        
            loweredTris[i * 24 + 6] = i * 4 + 2;
            loweredTris[i * 24 + 7] = i * 4 + 6;
            loweredTris[i * 24 + 8] = i * 4 + 3;
        
            loweredTris[i * 24 + 9] = i * 4 + 6;
            loweredTris[i * 24 + 10] = i * 4 + 7;
            loweredTris[i * 24 + 11] = i * 4 + 3;

            loweredTris[i * 24 + 12] = i * 4 + 1;
            loweredTris[i * 24 + 13] = i * 4 + 3;
            loweredTris[i * 24 + 14] = i * 4 + 5;
        
            loweredTris[i * 24 + 15] = i * 4 + 3;
            loweredTris[i * 24 + 16] = i * 4 + 7;
            loweredTris[i * 24 + 17] = i * 4 + 5;
        
        }
        newMesh.triangles = loweredTris;

        return newMesh;
    }
}
