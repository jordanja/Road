using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application;

public class CarMovement : MonoBehaviour {

    bool allowCarMovement;
    float initialTime;
    float timeSinceStart;

    float timeToTravelWhole = 200f;
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
                Vector3 positionOnRoad = new Vector3(location.getX(),location.getY() + transform.localScale.y/2, location.getZ());

                Vector3 facing = road.GetComponent<Road>().GetDerivitiveOnRoad(fractionAlongCurrentRoad);
                
                float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
                // transform.eulerAngles = new Vector3(0, angle, 0);

                if (Input.GetMouseButton(0)) {
                    change += Input.GetAxis("Mouse X");
                    print(change);
                }

                transform.position = new Vector3(positionOnRoad.x + Mathf.Clamp(change * Mathf.Cos(angle * Mathf.Deg2Rad),-RoadManager.instance.GetRoadWidth(),+RoadManager.instance.GetRoadWidth()) , 
                                                 positionOnRoad.y, 
                                                 positionOnRoad.z + Mathf.Clamp(change * Mathf.Sin(angle * Mathf.Deg2Rad),-RoadManager.instance.GetRoadWidth(),+RoadManager.instance.GetRoadWidth()));
                transform.eulerAngles = new Vector3(0, angle, 0);;

            }
        }
    }

}
