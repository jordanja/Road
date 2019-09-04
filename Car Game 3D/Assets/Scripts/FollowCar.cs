using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{

    [SerializeField]
    Transform Car;

    Vector3 translationOffset = new Vector3(0,1,-2);
    Vector3 rotationOffset = new Vector3(30,0,0);


    void LateUpdate() {
        
        transform.position = Car.position;
        transform.rotation = Car.rotation;

        transform.Translate(translationOffset);
        transform.Rotate(rotationOffset);
    }
}
