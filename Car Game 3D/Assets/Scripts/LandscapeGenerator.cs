using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGenerator : MonoBehaviour {

    float _z1;
    float _z2;

    int vertices = 8;

    [SerializeField]
    Material landscapeMat;

    float textureMultiplier = 3f;

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
        float xMin = -10f;
        float xMax = +10f;
        for (int i = 0; i < vertices; i++) {
            for (int j = 0; j < vertices; j++) {

                float x = MathHelper.Remap(j, 0, (vertices - 1), xMin, xMax);
                float z = MathHelper.Remap(i, 0, (vertices - 1), _z1, _z2);
                float y = (Mathf.Abs(x)/6f) - 0.5f;
                // float distanceFromRoad = RoadManager.instance.GetApproxPointOnRoadAtZPosition(z);

                int index = i * vertices + j;
                points[index] = new Vector3(x, y , z);
                uv[index] = textureMultiplier * new Vector2(((float)j) / ((float)vertices - 1),((float)i) / ((float)vertices - 1));
                // print("points[" + index + "] = " + points[index]); 

            }
        }

        for (int width = 0; width < (vertices - 1); width++) {
            for (int depth = 0; depth < (vertices - 1); depth++) {
            
                triangles[(width * (vertices - 1) + depth) * 6 + 0] = width * vertices + depth + 1;
                triangles[(width * (vertices - 1) + depth) * 6 + 1] = width * vertices + depth;
                triangles[(width * (vertices - 1) + depth) * 6 + 2] = width * vertices + depth + (vertices);
                
                triangles[(width * (vertices - 1) + depth) * 6 + 3] = width * vertices + depth + vertices;
                triangles[(width * (vertices - 1) + depth) * 6 + 4] = width * vertices + depth + vertices + 1;
                triangles[(width * (vertices - 1) + depth) * 6 + 5] = width * vertices + depth + 1;
            }
        }


        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uv;

    }

}
