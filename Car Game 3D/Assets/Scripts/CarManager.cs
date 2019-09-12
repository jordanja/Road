using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour {

    public static CarManager instance;

    float timeToTravelOneRoad = 4f;

    [SerializeField]
    CarMovement carMovement;
    
    void Awake() {
        instance = this;
    }


    public float GetTimeToTravelOneRoad() {
        return timeToTravelOneRoad;
    }

    public float getPercentageAlongRoad() {
        return carMovement.GetFractionAlongCurrentRoad();
    }


    public int getPlayerRoadNum() {
        return carMovement.GetCurrentRoadNum();
    }
}
