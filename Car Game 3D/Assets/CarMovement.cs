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
            int currentRoad = Mathf.RoundToInt(Mathf.Floor(timeSinceStart / timeForOneRoad));
            if (currentRoad <= RoadManager.instance.NumRoads()) {

                float fractionAlongCurrentRoad = (timeSinceStart - (currentRoad * timeForOneRoad))/timeForOneRoad;
                GameObject road = RoadManager.instance.GetRoad(currentRoad);
                Point3D location = road.GetComponent<Road>().GetLocationOnRoad(fractionAlongCurrentRoad);
                Vector3 centerOfRoadPosition = new Vector3(location.getX(),location.getY() + transform.localScale.y/2, location.getZ());

                Vector3 facing = road.GetComponent<Road>().GetDerivitiveOnRoad(fractionAlongCurrentRoad);

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
