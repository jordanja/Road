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
        

        int[] loweredTris = new int[((6 + 6 + 6 + 6) * baseRoadMesh.vertices.Length/2) + 6 + 6];
        for (int i = 0; i < baseRoadMesh.vertices.Length/2 - 1; i++) {

            // left face: top right
            loweredTris[i * 24 + 0] = i * 4 + 0;
            loweredTris[i * 24 + 1] = i * 4 + 1;
            loweredTris[i * 24 + 2] = i * 4 + 4;
        
            // left face: bottom left
            loweredTris[i * 24 + 3] = i * 4 + 4;
            loweredTris[i * 24 + 4] = i * 4 + 1;
            loweredTris[i * 24 + 5] = i * 4 + 5;
        
            // right face: top left
            loweredTris[i * 24 + 6] = i * 4 + 2;
            loweredTris[i * 24 + 7] = i * 4 + 6;
            loweredTris[i * 24 + 8] = i * 4 + 3;
        
            // right face: bottom right
            loweredTris[i * 24 + 9] = i * 4 + 6;
            loweredTris[i * 24 + 10] = i * 4 + 7;
            loweredTris[i * 24 + 11] = i * 4 + 3;

            // bottom face: top left
            loweredTris[i * 24 + 12] = i * 4 + 1;
            loweredTris[i * 24 + 13] = i * 4 + 3;
            loweredTris[i * 24 + 14] = i * 4 + 5;

            // bottom face: bottom right
            loweredTris[i * 24 + 15] = i * 4 + 3;
            loweredTris[i * 24 + 16] = i * 4 + 7;
            loweredTris[i * 24 + 17] = i * 4 + 5;
        
        }

        // front face: bottom left
        loweredTris[loweredTris.Length - 1 - 5] = 0;
        loweredTris[loweredTris.Length - 1 - 4] = 3;
        loweredTris[loweredTris.Length - 1 - 3] = 1;

        // front face: top right
        loweredTris[loweredTris.Length - 1 - 2] = 2;
        loweredTris[loweredTris.Length - 1 - 1] = 3;
        loweredTris[loweredTris.Length - 1 - 0] = 0;

        newMesh.triangles = loweredTris;

        return newMesh;
    }
}
