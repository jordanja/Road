using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    float timeSinceStart = 0f;
    float startingPoint = 0f;

    int _lane;

    float change;
    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead, int lane) {
        startingPoint = currentCarRoadNum + roadsAhead + currentCarPercentage;
        _lane = lane;
        
        switch (lane) {
            case 0:
                change = -0.375f;
                break;
            case 1:
                change = -0.125f;
                break;
            case 2:
                change = 0.125f;
                break;
            case 3:
                change = 0.375f;
                break;

        }
        change = change * RoadManager.instance.GetRoadWidth() * 2;

        SetPosition();
        gameObject.SetActive(true);
    }

    private void SetPosition() {
        timeSinceStart += Time.deltaTime;
        float distanceTraveled = timeSinceStart/EnemyManager.instance.GetTimeForOneRoad();

        float currentPosition = startingPoint - distanceTraveled;

        int roadNum = Mathf.FloorToInt(currentPosition);
        float percentageOnRoad = currentPosition - roadNum;

        GameObject currentRoad = RoadManager.instance.GetRoad(roadNum);
        if (currentRoad  == null) {
            Destroy(gameObject);
        } else {
            Vector3 centerOfRoadPosition = currentRoad.GetComponent<Road>().GetLocationOnRoad(percentageOnRoad);
            Vector3 facing = currentRoad.GetComponent<Road>().GetDerivitiveOnRoad(percentageOnRoad);    

            float angle = 180 + Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
                
            Vector3 normal = new Vector3(facing.z, facing.y, -facing.x).normalized;
            Vector3 offset = normal * change;

            transform.position = centerOfRoadPosition + offset;
            transform.eulerAngles = new Vector3(0, angle, 0);

        }
    }

    void Update() {
        SetPosition();

    }



}
