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
    private float heightAboveRoad = 0.2f;
    private float railingWidth = 0.1f;

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

    private Mesh createNewMesh(Mesh baseRoadMesh, (Vector3 Point,Vector3 Direction)[] directionOfVertices)
    {

        Mesh newMesh = new Mesh();
        newMesh.Clear();

        Vector3[] vertices = new Vector3[10 * (baseRoadMesh.vertices.Length / 2)];
        Vector2[] uv = new Vector2[10 * (baseRoadMesh.vertices.Length / 2)];

        Vector3 loweredVertice = new Vector3(0, 0.2f, 0);

        for (int i = 0; i < baseRoadMesh.vertices.Length; i++)
        {
            // vertices[i * 2 + 0] = baseRoadMesh.vertices[i];
            // vertices[i * 2 + 1] = baseRoadMesh.vertices[i] - loweredVertice;

            Vector3 vertexLocation = directionOfVertices[i].Point;
            Vector3 outwardDirection = directionOfVertices[i].Direction;

            vertices[i * 5 + 0] = vertexLocation + outwardDirection * railingWidth + new Vector3(0, heightAboveRoad, 0);
            vertices[i * 5 + 1] = vertexLocation + outwardDirection * railingWidth + new Vector3(0, -depthBelowRoad, 0);
            vertices[i * 5 + 2] = vertexLocation + new Vector3(0, heightAboveRoad, 0);
            vertices[i * 5 + 3] = vertexLocation;
            vertices[i * 5 + 4] = vertexLocation + new Vector3(0, -depthBelowRoad, 0);

            uv[i * 5 + 0] = new Vector2(1f * 0.2f, i/(float)baseRoadMesh.vertices.Length);
            uv[i * 5 + 1] = new Vector2(1f * 0.4f, i/(float)baseRoadMesh.vertices.Length);
            uv[i * 5 + 2] = new Vector2(1f * 0.6f, i/(float)baseRoadMesh.vertices.Length);
            uv[i * 5 + 3] = new Vector2(1f * 0.8f, i/(float)baseRoadMesh.vertices.Length);
            uv[i * 5 + 4] = new Vector2(1f * 1f,   i/(float)baseRoadMesh.vertices.Length);


        }
        newMesh.vertices = vertices;
        newMesh.uv = uv;
        int[] triangles = generateTriangles(baseRoadMesh);

        newMesh.triangles = triangles;

        return newMesh;
    }

    private int[] generateTriangles(Mesh baseRoadMesh) {
        int numberOfSquares = 11;
        int[] triangles = new int[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + (6 * 3)];
        for (int i = 0; i < (baseRoadMesh.vertices.Length / 2) - 1; i++) {

            if (_leftRailing) {
                // Outer left side: top right
                triangles[i * numberOfSquares * 6 + 0] = i * 10 + 0;
                triangles[i * numberOfSquares * 6 + 1] = i * 10 + 1;
                triangles[i * numberOfSquares * 6 + 2] = i * 10 + 10;

                // Outer left side: bottom left
                triangles[i * numberOfSquares * 6 + 3] = i * 10 + 1;
                triangles[i * numberOfSquares * 6 + 4] = i * 10 + 11;
                triangles[i * numberOfSquares * 6 + 5] = i * 10 + 10;

                // Top left side
                triangles[i * numberOfSquares * 6 + 6] = i * 10 + 0;
                triangles[i * numberOfSquares * 6 + 7] = i * 10 + 10;
                triangles[i * numberOfSquares * 6 + 8] = i * 10 + 2;

                triangles[i * numberOfSquares * 6 + 9] = i * 10 + 2;
                triangles[i * numberOfSquares * 6 + 10] = i * 10 + 10;
                triangles[i * numberOfSquares * 6 + 11] = i * 10 + 12;


                // Inner left side: top left
                triangles[i * numberOfSquares * 6 + 12] = i * 10 + 3;
                triangles[i * numberOfSquares * 6 + 13] = i * 10 + 2;
                triangles[i * numberOfSquares * 6 + 14] = i * 10 + 12;

                // Inner left side: bottom right
                triangles[i * numberOfSquares * 6 + 15] = i * 10 + 12;
                triangles[i * numberOfSquares * 6 + 16] = i * 10 + 13;
                triangles[i * numberOfSquares * 6 + 17] = i * 10 + 3;

                // Bottom left side:
                triangles[i * numberOfSquares * 6 + 60] = i * 10 + 1;
                triangles[i * numberOfSquares * 6 + 61] = i * 10 + 4;
                triangles[i * numberOfSquares * 6 + 62] = i * 10 + 11;

                triangles[i * numberOfSquares * 6 + 63] = i * 10 + 4;
                triangles[i * numberOfSquares * 6 + 64] = i * 10 + 14;
                triangles[i * numberOfSquares * 6 + 65] = i * 10 + 11;

                // Front left face: top left
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 0] = 0;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 1] = 2;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 2] = 1;

                // Front left face: bottom right
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 3] = 2;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 4] = 4;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 5] = 1;
            }
           

            if (_rightRailing) {
                // Inner right side: top left
                triangles[i * numberOfSquares * 6 + 18] = i * 10 + 18;
                triangles[i * numberOfSquares * 6 + 19] = i * 10 + 17;
                triangles[i * numberOfSquares * 6 + 20] = i * 10 + 7;

                // Inner right side: bottom right
                triangles[i * numberOfSquares * 6 + 21] = i * 10 + 8;
                triangles[i * numberOfSquares * 6 + 22] = i * 10 + 18;
                triangles[i * numberOfSquares * 6 + 23] = i * 10 + 7;


                // Top right side
                triangles[i * numberOfSquares * 6 + 24] = i * 10 + 7;
                triangles[i * numberOfSquares * 6 + 25] = i * 10 + 17;
                triangles[i * numberOfSquares * 6 + 26] = i * 10 + 15;

                triangles[i * numberOfSquares * 6 + 27] = i * 10 + 5;
                triangles[i * numberOfSquares * 6 + 28] = i * 10 + 7;
                triangles[i * numberOfSquares * 6 + 29] = i * 10 + 15;


                // Outer right side: top left
                triangles[i * numberOfSquares * 6 + 30] = i * 10 + 6;
                triangles[i * numberOfSquares * 6 + 31] = i * 10 + 5;
                triangles[i * numberOfSquares * 6 + 32] = i * 10 + 15;

                // Outer right side: bottom right
                triangles[i * numberOfSquares * 6 + 33] = i * 10 + 6;
                triangles[i * numberOfSquares * 6 + 34] = i * 10 + 15;
                triangles[i * numberOfSquares * 6 + 35] = i * 10 + 16;

                // Bottom right side:
                triangles[i * numberOfSquares * 6 + 48] = i * 10 + 9;
                triangles[i * numberOfSquares * 6 + 49] = i * 10 + 6;
                triangles[i * numberOfSquares * 6 + 50] = i * 10 + 16;

                triangles[i * numberOfSquares * 6 + 51] = i * 10 + 9;
                triangles[i * numberOfSquares * 6 + 52] = i * 10 + 16;
                triangles[i * numberOfSquares * 6 + 53] = i * 10 + 19;  

                // Front right face: top left
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 12] = 7;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 13] = 5;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 14] = 9;

                // Front right face: bottom right
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 15] = 5;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 16] = 6;
                triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 17] = 9;
            }
            


            // Box left side: top right
            triangles[i * numberOfSquares * 6 + 36] = i * 10 + 3;
            triangles[i * numberOfSquares * 6 + 37] = i * 10 + 4;
            triangles[i * numberOfSquares * 6 + 38] = i * 10 + 13;

            // Box left side: bottom left
            triangles[i * numberOfSquares * 6 + 39] = i * 10 + 4;
            triangles[i * numberOfSquares * 6 + 40] = i * 10 + 14;
            triangles[i * numberOfSquares * 6 + 41] = i * 10 + 13;


            // Box right side: top right
            triangles[i * numberOfSquares * 6 + 42] = i * 10 + 8;
            triangles[i * numberOfSquares * 6 + 43] = i * 10 + 18;
            triangles[i * numberOfSquares * 6 + 44] = i * 10 + 9;

            // Box right side: bottom left
            triangles[i * numberOfSquares * 6 + 45] = i * 10 + 9;
            triangles[i * numberOfSquares * 6 + 46] = i * 10 + 18;
            triangles[i * numberOfSquares * 6 + 47] = i * 10 + 19;


            // Bottom middle side:
            triangles[i * numberOfSquares * 6 + 54] = i * 10 + 4;
            triangles[i * numberOfSquares * 6 + 55] = i * 10 + 9;
            triangles[i * numberOfSquares * 6 + 56] = i * 10 + 14;

            triangles[i * numberOfSquares * 6 + 57] = i * 10 + 9;
            triangles[i * numberOfSquares * 6 + 58] = i * 10 + 19;
            triangles[i * numberOfSquares * 6 + 59] = i * 10 + 14;


            

        }

        


        // Front bottom face: top left
        triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 6] = 3;
        triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 7] = 8;
        triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 8] = 4;

        // Front bottom face: bottom right
        triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 9] = 8;
        triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 10] = 9;
        triangles[(6 * numberOfSquares * (baseRoadMesh.vertices.Length / 2)) + 11] = 4;


        

        return triangles;
    }
}
