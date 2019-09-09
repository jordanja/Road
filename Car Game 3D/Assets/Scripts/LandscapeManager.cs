using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    
    public static LandscapeManager instance;

    [SerializeField]
    GameObject LandscapeBlueprint;

    List<GameObject> landscapes;

    void Awake() {
        instance = this;
    }

    void Start() {
        landscapes = new List<GameObject>();
        CreateLandscapeBetweenZPoints(0,9);
    }

    public void CreateLandscapeBetweenZPoints(float z1, float z2) {
        GameObject landscapeParent = new GameObject();
        landscapeParent.name = "Landscape Parent " + landscapes.Count;
        landscapeParent.transform.parent = transform.parent;

        GameObject landscape = Instantiate(LandscapeBlueprint, landscapeParent.transform, false);
        landscape.name = "Landscape " + landscapes.Count;

        landscape.GetComponent<LandscapeGenerator>().Init(z1, z2);

    }


}
