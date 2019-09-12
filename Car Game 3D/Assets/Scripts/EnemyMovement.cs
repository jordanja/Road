using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class EnemyMovement : MonoBehaviour {

    float timeSinceStart = 0f;
    float startingPoint = 0f;

    int _lane;

    float change;

    int currentRoadNum;
    int prevRoadNum;
    Road currentRoad;

    float timeToChangeOneLane = 0.5f;
    bool currentlyChangingLanes;
    float timeStartedChangingLanes;
    int laneToChangeTo;
    float newChange;

    float maxLaneChangeAngle = 30f;
    bool pastPlayer = false;

    float totalLaneChangingTime;


    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead, int lane) {
        startingPoint = currentCarRoadNum + roadsAhead + currentCarPercentage;
        _lane = lane;

        pastPlayer = false;
        change = GetLanePosition(_lane);

        SetPosition();

        gameObject.SetActive(true);

        float laneChangerDelay = UnityEngine.Random.Range(0f, 0.5f);
        currentlyChangingLanes = false;
        StartCoroutine(laneChanger(laneChangerDelay));
    }

    void Update() {
        if (gameObject.activeInHierarchy == true) {
            SetPosition();
        }
    }

    IEnumerator laneChanger(float laneChangerDelay) {
        yield return new WaitForSeconds(laneChangerDelay);
        while (true) {
            currentlyChangingLanes = true;
            timeStartedChangingLanes = Time.time;
            laneToChangeTo = getNewLane();
            newChange = GetLanePosition(laneToChangeTo);
            float randomDelay = UnityEngine.Random.Range(1f, 2f);
            totalLaneChangingTime = (Mathf.Abs(laneToChangeTo - _lane) * timeToChangeOneLane);
            yield return new WaitForSeconds(timeToChangeOneLane * Mathf.Abs(laneToChangeTo - _lane) + randomDelay);
        }
    }

    private void SetPosition() {
        timeSinceStart += Time.deltaTime;
        float distanceTraveled = timeSinceStart/EnemyManager.instance.GetTimeForOneRoad();

        float currentPosition = startingPoint - distanceTraveled;

        currentRoadNum = Mathf.FloorToInt(currentPosition);
        float percentageOnRoad = currentPosition - currentRoadNum;

        if (currentRoadNum != prevRoadNum) {
            currentRoad = RoadManager.instance.GetRoad(currentRoadNum)?.GetComponent<Road>();
            
        }
        if (currentRoadNum == CarManager.instance.getPlayerRoadNum()) {
            if ((pastPlayer == false)) {
                checkIfPastPlayer(percentageOnRoad);
            }
        }

        if (currentRoad  == null) {
            EnemyPool.instance.ReturnToPool(this.gameObject);
        } else {
            Vector3 centerOfRoadPosition = currentRoad.GetLocationOnRoad(percentageOnRoad);
            Vector3 facing = currentRoad.GetDerivitiveOnRoad(percentageOnRoad);    

            float angle = 180 + Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
                
            Vector3 normal = new Vector3(facing.z, facing.y, -facing.x);
            Vector3 offset = normal;

            float laneChangeAngle = 0f;
            if (currentlyChangingLanes == true) {
                float t = (Time.time - timeStartedChangingLanes) / totalLaneChangingTime;
                offset *= Mathf.Lerp(change, newChange, t);
                laneChangeAngle = ChangeLanes(t);

            }
            else {
                offset *= change;
            }

            transform.position = centerOfRoadPosition + offset;
            transform.eulerAngles = new Vector3(0, angle + laneChangeAngle, 0);

        }

        prevRoadNum = currentRoadNum;
    }

    private float ChangeLanes(float t) {
        float laneChangeAngle;
        
        if (t < 0.5) {
            laneChangeAngle = Mathf.Lerp(0, maxLaneChangeAngle, t * 2);
            if (laneToChangeTo > _lane) {
                laneChangeAngle *= -1;
            }
        } else {
            laneChangeAngle = Mathf.Lerp(maxLaneChangeAngle, 0, (t - 0.5f) * 2);
            if (laneToChangeTo > _lane) {
                laneChangeAngle *= -1;
            }

        }

        if (t >= 1) {
            currentlyChangingLanes = false;
            _lane = laneToChangeTo;
            change = GetLanePosition(_lane);
        }

        return laneChangeAngle;
    }

    void checkIfPastPlayer(float percentageOnRoad) {
        // print(this.name + ": " + percentageOnRoad);
        if (percentageOnRoad < CarManager.instance.getPercentageAlongRoad()) {
            // print(this.name + " has percentage " + percentageOnRoad + ". I have percentage " + CarManager.instance.getPercentageAlongRoad());
            pastPlayer = true;
            GameplayManager.instance.IncreaseScore();
        }
    }

    private float GetLanePosition(float lane) {
        return ((lane - 1.5f)/(2f)) * RoadManager.instance.GetRoadWidth();
    }

    private int getNewLane(){ 
        int newLane = UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
        while (newLane == _lane) {
            newLane = UnityEngine.Random.Range(0,RoadManager.instance.NumberOfLanes());
        }

        return newLane;
    }

}
