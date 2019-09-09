using System;
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

    float timeToChangeLanes = 0.5f;
    bool currentlyChangingLanes;
    float timeStartedChangingLanes;
    int laneToChangeTo;
    float newChange;

    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead, int lane)
    {
        startingPoint = currentCarRoadNum + roadsAhead + currentCarPercentage;
        _lane = lane;

        change = GetLanePosition(_lane);

        SetPosition();

        gameObject.SetActive(true);

        float laneChangerDelay = UnityEngine.Random.Range(0f, 0.5f);
        currentlyChangingLanes = false;
        StartCoroutine(laneChanger(laneChangerDelay));
    }

    void Update() {
        SetPosition();
    }

    IEnumerator laneChanger(float laneChangerDelay) {
        yield return new WaitForSeconds(laneChangerDelay);
        while (true) {
            currentlyChangingLanes = true;
            timeStartedChangingLanes = Time.time;
            laneToChangeTo = getNewLane();
            newChange = GetLanePosition(laneToChangeTo);
            float randomDelay = UnityEngine.Random.Range(0f, 2f);
            yield return new WaitForSeconds(timeToChangeLanes + randomDelay);
        }
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
            Vector3 offset = normal;
            if (currentlyChangingLanes == true) {
                float t = (Time.time - timeStartedChangingLanes)/timeToChangeLanes;
                offset *= Mathf.Lerp(change, newChange,t);
                if (t >= 1) {
                    currentlyChangingLanes = false;
                    _lane = laneToChangeTo;
                    change = GetLanePosition(_lane);
                }

            } else {
                offset *= change;
            }

            transform.position = centerOfRoadPosition + offset;
            transform.eulerAngles = new Vector3(0, angle, 0);

        }

        prevRoadNum = currentRoadNum;
    }



    private float GetLanePosition(float lane) {
    
        float xOffset = ((lane - 1.5f)/(2f)) * RoadManager.instance.GetRoadWidth();
        return xOffset;
    }

    private int getNewLane(){ 
        int newLane = UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
        while (newLane == _lane) {
            newLane = UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
        }

        return newLane;
    }

}
