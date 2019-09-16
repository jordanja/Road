using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{

    [SerializeField]
    Transform Car;

    Vector3 translationOffset = new Vector3(0,0.6f,-2);
    Vector3 rotationOffset = new Vector3(16.55f,0,0);

    float subtraction = 0.3f;

    Road currentRoad;

    void LateUpdate() {
        
        int roadNum = CarManager.instance.getPlayerRoadNum();
        float percentage = CarManager.instance.getPercentageAlongRoad();

        float carPosition = ((float)roadNum) + percentage;
        float total = carPosition - subtraction;

        if (total < 0) {
            
        } else {
            int camRoad = Mathf.FloorToInt(total);
            float camPercent = total - camRoad;
            
            currentRoad = RoadManager.instance.GetRoad(camRoad).GetComponent<Road>();
            Vector3 centerOfRoadPosition = currentRoad.GetLocationOnRoad(camPercent);
            Vector3 facing = currentRoad.GetDerivitiveOnRoad(camPercent);

            float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);

            transform.position = centerOfRoadPosition + new Vector3(0,0.6f,0);
            transform.eulerAngles = new Vector3(0, angle, 0);
            transform.Translate(0,0,-1);


        }

        
    }
}
