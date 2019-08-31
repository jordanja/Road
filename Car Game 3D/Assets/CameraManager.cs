using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    Camera GodCamera;

    [SerializeField]
    Camera ThirdPersonCamera;

    public void ChooseGodCamera() {
        GodCamera.enabled = true;
        ThirdPersonCamera.enabled = false;
    }

    public void ChooseThirdPersonCamera() {
        ThirdPersonCamera.enabled = true;
        GodCamera.enabled = false;
    }


}
