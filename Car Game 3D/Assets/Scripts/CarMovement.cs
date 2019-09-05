﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class CarMovement : MonoBehaviour {

    bool allowCarMovement;
    float initialTime;
    float timeSinceStart;

    float timeToTravelOne = 4f;
    float timeForOneRoad;
    float change;

    int currentRoadNum;

    [SerializeField]
    Transform carTransform;

    private void Start() {
        allowCarMovement = false;
        currentRoadNum = 0;
        StartCoroutine(Setup());
    }


    IEnumerator Setup() {
        yield return new WaitUntil(() => RoadManager.instance.initialized == true);
        initialTime = Time.time;
        allowCarMovement = true;
        timeForOneRoad = timeToTravelOne;
        change = 0;
    }

    private void Update() {
        if (allowCarMovement) {
            timeSinceStart += Time.deltaTime;
            currentRoadNum = Mathf.RoundToInt(Mathf.Floor(timeSinceStart / timeForOneRoad));
            if (currentRoadNum <= RoadManager.instance.NumRoads()) {

                if (currentRoadNum >= RoadManager.instance.NumRoads() -1) {
                    RoadManager.instance.AddRoad();
                }

                float fractionAlongCurrentRoad = (timeSinceStart - (currentRoadNum * timeForOneRoad))/timeForOneRoad;
                GameObject currentRoad = RoadManager.instance.GetRoad(currentRoadNum);
                Vector3 location = currentRoad.GetComponent<Road>().GetLocationOnRoad(fractionAlongCurrentRoad);
                Vector3 centerOfRoadPosition = new Vector3(location.x,location.y + carTransform.localScale.y/2, location.z);

                Vector3 facing = currentRoad.GetComponent<Road>().GetDerivitiveOnRoad(fractionAlongCurrentRoad);

                if (Input.GetMouseButton(0)) {
                    change += Input.GetAxis("Mouse X");
                }

                Vector3 normal = new Vector3(facing.z, facing.y, -facing.x).normalized;
                Vector3 offset = normal * Mathf.Clamp(change,-RoadManager.instance.GetRoadWidth(),+RoadManager.instance.GetRoadWidth());

                float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
               
                transform.position = centerOfRoadPosition + offset;
                transform.eulerAngles = new Vector3(0, angle, 0);;
            }
        }
    }

}
