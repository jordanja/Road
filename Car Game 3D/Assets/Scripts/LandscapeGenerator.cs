using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGenerator : MonoBehaviour {

    float _z1;
    float _z2;

    int vertices = 8;

    [SerializeField]
    Material landscapeMat;

    public void Init(float z1, float z2) {
        _z1 = z1;
        _z2 = z2;

        generateMesh();
    }
    
    private void generateMesh() {
        if (gameObject.GetComponent<MeshFilter>() == null) {
            gameObject.AddComponent<MeshFilter>();
        }
        if (gameObject.GetComponent<MeshRenderer>() == null) {
            gameObject.AddComponent<MeshRenderer>();
        }

        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        Mesh mesh = mf.mesh;
        mesh.Clear();

        mr.material = landscapeMat;

        Vector3[] points = new Vector3[vertices * vertices];
        int[] triangles = new int[(vertices - 1) * (vertices - 1) * 6];
        Vector2[] uv = new Vector2[vertices * vertices];

        for (int i = 0; i < vertices; i++) {
            for (int j = 0; j < vertices; j++) {

                float x = MathHelper.Remap(j, 0, vertices, -10, +10);
                float z = MathHelper.Remap(i, 0, vertices, _z1, _z2);
                int index = i * vertices + j;
                points[index] = new Vector3(x,-0.1f, z);
                print("points[" + index + "] = " + points[index]);

            }
        }

        for (int i = 0; i < (vertices - 1) * (vertices - 1); i++) {
            triangles[i * 6 + 0] = i + 0;
            triangles[i * 6 + 1] = i + vertices;
            triangles[i * 6 + 2] = i + 1;
            
            triangles[i * 6 + 3] = i + 1;
            triangles[i * 6 + 4] = i + vertices;
            triangles[i * 6 + 5] = i + vertices + 1;   
        }


        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uv;

    }

}
