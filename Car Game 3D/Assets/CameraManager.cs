using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    Camera GodCamera;

    [SerializeField]
    Camera ThirdPersonCamera;

    [SerializeField]
    Button GodButton;

    [SerializeField]
    Button ThirdPersonButton;

    void Start() {
        ChooseGodCamera();
    }

    public void ChooseGodCamera() {
        GodCamera.enabled = true;
        ThirdPersonCamera.enabled = false;
        
    }

    public void ChooseThirdPersonCamera() {
        ThirdPersonCamera.enabled = true;
        GodCamera.enabled = false;
    }


}
