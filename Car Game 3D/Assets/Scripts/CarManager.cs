using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour {

    public static CarManager instance;

    float timeToTravelOneRoad = 4f;
    
    void Awake() {
        instance = this;
    }


    public float GetTimeToTravelOneRoad() {
        return timeToTravelOneRoad;
    }

}
