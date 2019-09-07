using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    float timeSinceStart = 0f;
    float startingPoint = 0f;


    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead) {
        startingPoint = currentCarRoadNum + roadsAhead + currentCarPercentage;
    }

    void Update() {
        timeSinceStart += Time.deltaTime;
        float distanceTraveled = timeSinceStart/CarManager.instance.GetTimeToTravelOneRoad();

        float currentPosition = startingPoint - distanceTraveled;

        int roadNum = Mathf.FloorToInt(currentPosition);
        float percentageOnRoad = currentPosition - roadNum;

        GameObject currentRoad = RoadManager.instance.GetRoad(roadNum);

        Vector3 centerOfRoadPosition = currentRoad.GetComponent<Road>().GetLocationOnRoad(percentageOnRoad);
        Vector3 facing = currentRoad.GetComponent<Road>().GetDerivitiveOnRoad(percentageOnRoad);    

        float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
               
        transform.position = centerOfRoadPosition;
        transform.eulerAngles = new Vector3(0, angle, 0);

    }



}
