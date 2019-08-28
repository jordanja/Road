using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class CarMovement : MonoBehaviour {

    bool allowCarMovement;
    float initialTime;
    float timeSinceStart;

    float timeToTravelWhole = 10f;
    float timeForOneRoad;
    private void Start() {
        allowCarMovement = false;
        StartCoroutine(Setup());
    }


    IEnumerator Setup() {
        yield return new WaitUntil(() => RoadManager.instance.initialized == true);
        initialTime = Time.time;
        allowCarMovement = true;
        timeForOneRoad = timeToTravelWhole / RoadManager.instance.NumRoads();

    }

    private void Update() {
        if (allowCarMovement) {
            timeSinceStart += Time.deltaTime;
            int currentRoad = Mathf.RoundToInt(Mathf.Floor(timeSinceStart / timeForOneRoad));
            float fractionAlongCurrentRoad = (timeSinceStart - (currentRoad * timeForOneRoad))/timeForOneRoad;
            GameObject road = RoadManager.instance.GetRoad(currentRoad);
            Point3D location = road.GetComponent<Road>().GetLocationOnRoad(fractionAlongCurrentRoad);
            transform.position = location.toVector();
        }
    }

}
