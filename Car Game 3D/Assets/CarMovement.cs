using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class CarMovement : MonoBehaviour {

    bool allowCarMovement;
    float initialTime;
    float timeSinceStart;

    float timeToTravelWhole = 15f;
    float timeForOneRoad;
    float change;


    private void Start() {
        allowCarMovement = false;
        StartCoroutine(Setup());
    }


    IEnumerator Setup() {
        yield return new WaitUntil(() => RoadManager.instance.initialized == true);
        initialTime = Time.time;
        allowCarMovement = true;
        timeForOneRoad = timeToTravelWhole / RoadManager.instance.NumRoads();
        change = 0;
    }

    private void Update() {
        if (allowCarMovement) {
            timeSinceStart += Time.deltaTime;
            int currentRoadNum = Mathf.RoundToInt(Mathf.Floor(timeSinceStart / timeForOneRoad));
            if (currentRoadNum <= RoadManager.instance.NumRoads()) {

                float fractionAlongCurrentRoad = (timeSinceStart - (currentRoadNum * timeForOneRoad))/timeForOneRoad;
                GameObject currentRoad = RoadManager.instance.GetRoad(currentRoadNum);
                Vector3 location = currentRoad.GetComponent<Road>().GetLocationOnRoad(fractionAlongCurrentRoad);
                Vector3 centerOfRoadPosition = new Vector3(location.x,location.y + transform.localScale.y/2, location.z);

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
