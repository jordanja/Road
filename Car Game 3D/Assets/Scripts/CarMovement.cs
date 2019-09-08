using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {

    bool allowCarMovement;
    float initialTime;
    float timeSinceStart;

    float timeForOneRoad;
    float change;

    int currentRoadNum;
    int lastRoadNum;

    float fractionAlongCurrentRoad;

    Road currentRoad;

    [SerializeField]
    Transform carTransform;

    private void Start() {
        allowCarMovement = false;
        currentRoadNum = 0;
        lastRoadNum = 0;
        fractionAlongCurrentRoad = 0f;
        StartCoroutine(Setup());
    }


    IEnumerator Setup() {
        yield return new WaitUntil(() => RoadManager.instance.initialized == true);
        initialTime = Time.time;
        allowCarMovement = true;
        timeForOneRoad = CarManager.instance.GetTimeToTravelOneRoad();
        currentRoad = RoadManager.instance.GetRoad(0).GetComponent<Road>();
        change = 0;
    }

    private void Update() {
        if (allowCarMovement) {
            timeSinceStart += Time.deltaTime;
            currentRoadNum = Mathf.RoundToInt(Mathf.Floor(timeSinceStart / timeForOneRoad));

            

            if (currentRoadNum <= RoadManager.instance.NumRoads()) { // Should be isValidRoadNum(currentRoadNum)

                if (currentRoadNum != lastRoadNum) {
                    if (currentRoadNum >= RoadManager.instance.NumRoads() -1) {
                        RoadManager.instance.AddRoad(RoadManager.instance.GetRoadZLength(), RoadManager.instance.GetSegments(), RoadManager.instance.GetRoadCurviness(), RoadManager.instance.GetNumberOfControlPoints());
                    }
                    currentRoad = RoadManager.instance.GetRoad(currentRoadNum).GetComponent<Road>();

                    RoadManager.instance.RemoveRoad(lastRoadNum - 1);
                }

                fractionAlongCurrentRoad = (timeSinceStart - (currentRoadNum * timeForOneRoad))/timeForOneRoad;
                
                Vector3 centerOfRoadPosition = currentRoad.GetLocationOnRoad(fractionAlongCurrentRoad);
                Vector3 facing = currentRoad.GetDerivitiveOnRoad(fractionAlongCurrentRoad);

                if (Input.GetMouseButton(0)) {
                    change += Input.GetAxis("Mouse X");
                }

                Vector3 normal = new Vector3(facing.z, facing.y, -facing.x);
                Vector3 offset = normal * Mathf.Clamp(change,-RoadManager.instance.GetRoadWidth(),+RoadManager.instance.GetRoadWidth());

                float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
               
                transform.position = centerOfRoadPosition + offset;
                transform.eulerAngles = new Vector3(0, angle, 0);;
            }
        }
        lastRoadNum = currentRoadNum;
    }

    public int GetCurrentRoadNum() {
        return currentRoadNum;
    }

    public Road GetCurrentRoad() {
        return RoadManager.instance.GetRoad(currentRoadNum)?.GetComponent<Road>();
    }

    public float GetFractionAlongCurrentRoad(){
        return fractionAlongCurrentRoad;
    } 

    void OnCollisionEnter(Collision collision) {
        print("collided!");
    }
    
    
}
