using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {



    public void Init(float distanceOfEnemy, int distanceAheadToSendEnemyFrom) {
        int currentRoadNum = Mathf.RoundToInt(Mathf.Floor(distanceOfEnemy / CarManager.instance.GetTimeToTravelOneRoad())) + distanceAheadToSendEnemyFrom;
        print("Roadnum of enemy = " + currentRoadNum);
        if (currentRoadNum <= RoadManager.instance.NumRoads()) {
            float fractionAlongCurrentRoad = (distanceOfEnemy - (currentRoadNum * CarManager.instance.GetTimeToTravelOneRoad()))/CarManager.instance.GetTimeToTravelOneRoad();
            GameObject currentRoad = RoadManager.instance.GetRoad(currentRoadNum);
            Vector3 location = currentRoad.GetComponent<Road>().GetLocationOnRoad(fractionAlongCurrentRoad);
            Vector3 centerOfRoadPosition = new Vector3(location.x,location.y, location.z);

            Vector3 facing = currentRoad.GetComponent<Road>().GetDerivitiveOnRoad(fractionAlongCurrentRoad);    

            float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
               
            transform.position = centerOfRoadPosition;
            transform.eulerAngles = new Vector3(0, angle, 0);;
        }   

    }




    void Update() {
        
    }


}
