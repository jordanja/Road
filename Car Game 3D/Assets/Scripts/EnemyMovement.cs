﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    float timeSinceStart = 0f;
    float startingPoint = 0f;

    int _lane;

    float change;

    int currentRoadNum;
    int prevRoadNum;
    Road currentRoad;

    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead, int lane)
    {
        startingPoint = currentCarRoadNum + roadsAhead + currentCarPercentage;
        _lane = lane;



        GetLanePosition();

        SetPosition();
        gameObject.SetActive(true);
    }

    void Update() {
        SetPosition();
    }


    private void SetPosition() {
        timeSinceStart += Time.deltaTime;
        float distanceTraveled = timeSinceStart/EnemyManager.instance.GetTimeForOneRoad();

        float currentPosition = startingPoint - distanceTraveled;

        int currentRoadNum = Mathf.FloorToInt(currentPosition);
        float percentageOnRoad = currentPosition - currentRoadNum;

        if (currentRoadNum != prevRoadNum) {
            currentRoad = RoadManager.instance.GetRoad(currentRoadNum)?.GetComponent<Road>();
        }

        if (currentRoad  == null) {
            Destroy(gameObject);
        } else {
            Vector3 centerOfRoadPosition = currentRoad.GetLocationOnRoad(percentageOnRoad);
            Vector3 facing = currentRoad.GetDerivitiveOnRoad(percentageOnRoad);    

            float angle = 180 + Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
                
            Vector3 normal = new Vector3(facing.z, facing.y, -facing.x);
            Vector3 offset = normal * change;

            transform.position = centerOfRoadPosition + offset;
            transform.eulerAngles = new Vector3(0, angle, 0);

        }

        prevRoadNum = currentRoadNum;
    }



    private void GetLanePosition() {
        if ((_lane >= 0) && (_lane <= RoadManager.instance.NumberOfLanes() - 1)) {
            switch (_lane) {
                case 0:
                    change = -0.75f;
                    break;
                case 1:
                    change = -0.25f;
                    break;
                case 2:
                    change = 0.25f;
                    break;
                case 3:
                    change = 0.75f;
                    break;

            }
            change = change * RoadManager.instance.GetRoadWidth();
        } else {
            change = -0.75f * RoadManager.instance.GetRoadWidth();
        }
        
    }

}
