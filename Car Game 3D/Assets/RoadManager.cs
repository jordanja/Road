using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour {

    public static RoadManager instance;

    private List<GameObject> roads = new List<GameObject>();

    [SerializeField]
    GameObject roadBlueprint;

    void Awake() {
        instance = this;
    }

    public void CreateNewRoad() {
        GameObject road = Instantiate(roadBlueprint, transform, false);
        road.GetComponent<Road>()?.Init();
    }

}
