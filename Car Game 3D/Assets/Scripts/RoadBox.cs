using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBox : MonoBehaviour {

    [SerializeField]
    Material SideMat;

    private bool _leftRailing;
    private bool _rightRailing;

    private float depthBelowRoad = 0.2f;
    private float heightAboveRoad = 0.3f;
    private float railingWidth = 0.2f;

    internal void Init(Road baseRoad, bool leftRailing, bool rightRailing) {
        _leftRailing = leftRailing;
        _rightRailing = rightRailing;

        MeshFilter origMF = baseRoad.gameObject.GetComponent<MeshFilter>();
        if (origMF != null) {
            MeshFilter newMf = gameObject.AddComponent<MeshFilter>();
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = SideMat;

            Mesh newMesh = createNewMesh(origMF.mesh,baseRoad.GetDirectionOfVertices());

            newMf.mesh = newMesh;
        } 
    }

    private Mesh createNewMesh(Mesh baseRoadMesh, (Vector3 Point,Vector3 Direction)[] directionOfVertices) {

        Mesh newMesh = new Mesh();
        newMesh.Clear();

        Vector3[] vertices = new Vector3[10 * (baseRoadMesh.vertices.Length/2)];

        Vector3 loweredVertice = new Vector3(0,0.2f,0);

        for (int i = 0; i < baseRoadMesh.vertices.Length; i++) {
            // vertices[i * 2 + 0] = baseRoadMesh.vertices[i];
            // vertices[i * 2 + 1] = baseRoadMesh.vertices[i] - loweredVertice;

            Vector3 vertexLocation = directionOfVertices[i].Point;
            Vector3 outwardDirection = directionOfVertices[i].Direction;

            vertices[i * 5 + 0] = vertexLocation + outwardDirection * railingWidth + new Vector3(0, heightAboveRoad, 0);
            vertices[i * 5 + 1] = vertexLocation + outwardDirection * railingWidth + new Vector3(0, -depthBelowRoad, 0);
            vertices[i * 5 + 2] = vertexLocation + new Vector3(0, heightAboveRoad, 0);
            vertices[i * 5 + 3] = vertexLocation;
            vertices[i * 5 + 4] = vertexLocation + new Vector3(0, -depthBelowRoad, 0);


        }
        newMesh.vertices = vertices;
        

        int[] triangles = new int[6 * 7 * (baseRoadMesh.vertices.Length/2)];
        for (int i = 0; i < baseRoadMesh.vertices.Length/2 - 1; i++) {

            
        
        }

     

        newMesh.triangles = triangles;

        return newMesh;
    }
}
