using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {



    public void Init(int currentCarRoadNum, float currentCarPercentage, int roadsAhead) {
        int enemyRoadNum = currentCarRoadNum + roadsAhead;
        
        if (enemyRoadNum <= RoadManager.instance.NumRoads()) {
            
            
            GameObject currentRoad = RoadManager.instance.GetRoad(enemyRoadNum);
            
            Vector3 centerOfRoadPosition = currentRoad.GetComponent<Road>().GetLocationOnRoad(currentCarPercentage);

            Vector3 facing = currentRoad.GetComponent<Road>().GetDerivitiveOnRoad(currentCarPercentage);    

            float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);
               
            transform.position = centerOfRoadPosition;
            transform.eulerAngles = new Vector3(0, angle, 0);;
        }   

    }



}
