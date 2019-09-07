using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    float timeSinceStart = 0f;
    float startingPoint = 0f;

    int _lane;


    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead, int lane) {
        startingPoint = currentCarRoadNum + roadsAhead + currentCarPercentage;
        _lane = lane;
    }

    void Update() {
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
                
            transform.position = centerOfRoadPosition;
            transform.eulerAngles = new Vector3(0, angle, 0);

        }

    }



}
