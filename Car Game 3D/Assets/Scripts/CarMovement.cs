using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour {

    bool allowCarMovement;
    float initialTime;
    float timeSinceStart;

    float timeForOneRoad;
    float xChange;
    float lastXChange;

    int currentRoadNum;
    int lastRoadNum;

    float fractionAlongCurrentRoad;

    Road currentRoad;

    [SerializeField]
    Transform carTransform;

    [SerializeField]
    MeshRenderer CarBody;

    float carWidth;

    [SerializeField]
    ParticleSystem RightSparks;

    [SerializeField]
    ParticleSystem LeftSparks;
    float clampedXChange;

    float carPosAtLastMountainPlacement = 0;

    private void Start() {
        allowCarMovement = false;
        currentRoadNum = 0;
        lastRoadNum = 0;
        fractionAlongCurrentRoad = 0f;
        carWidth = CarBody.GetComponent<Renderer>().bounds.size.x/2;
        clampedXChange = 0;
        StartCoroutine(Setup());

    }


    IEnumerator Setup() {
        yield return new WaitUntil(() => RoadManager.instance.initialized == true);
        initialTime = Time.time;
        allowCarMovement = true;
        timeForOneRoad = CarManager.instance.GetTimeToTravelOneRoad();
        currentRoad = RoadManager.instance.GetRoad(0).GetComponent<Road>();
        xChange = 0;


    }

    private void Update() {
        if (allowCarMovement) {
            

            timeSinceStart += Time.deltaTime;
            currentRoadNum = Mathf.RoundToInt(Mathf.Floor(timeSinceStart / timeForOneRoad));

            

            if (currentRoadNum <= RoadManager.instance.NumRoads()) { // Should be isValidRoadNum(currentRoadNum)

                if (currentRoadNum != lastRoadNum) {
                    while (currentRoadNum >= RoadManager.instance.NumRoads() - 4) {
                        RoadManager.instance.AddRoad(RoadManager.instance.GetRoadZLength(), RoadManager.instance.GetSegments(), RoadManager.instance.GetRoadCurviness(), RoadManager.instance.GetNumberOfControlPoints());
                        if ((currentRoadNum) * RoadManager.instance.GetRoadZLength() - carPosAtLastMountainPlacement >= LandscapeManager.instance.GetMountainMultiplier()) {
                            carPosAtLastMountainPlacement = currentRoadNum * RoadManager.instance.GetRoadZLength();
                            LandscapeManager.instance.addLeftAndRightMountains();
                        }
                    }
                    currentRoad = RoadManager.instance.GetRoad(currentRoadNum).GetComponent<Road>();

                    RoadManager.instance.RemoveRoad(lastRoadNum - 1);
                    lastRoadNum = currentRoadNum;

                }

                fractionAlongCurrentRoad = (timeSinceStart - (currentRoadNum * timeForOneRoad))/timeForOneRoad;
                
                if (Input.GetMouseButton(0)) {
                    xChange += Input.GetAxis("Mouse X")/15f;

                    if (lastXChange != xChange) {
                        clampedXChange = Mathf.Clamp(xChange, -RoadManager.instance.GetRoadWidth() + carWidth, +RoadManager.instance.GetRoadWidth() - carWidth);
                        SparksParticleSystem();
                    }

                    lastXChange = xChange;
                }


                Vector3 centerOfRoadPosition = currentRoad.GetLocationOnRoad(fractionAlongCurrentRoad);
                Vector3 facing = currentRoad.GetDerivitiveOnRoad(fractionAlongCurrentRoad);

                Vector3 normal = new Vector3(facing.z, facing.y, -facing.x);
                Vector3 offset = normal * clampedXChange;
                
                float angle = Mathf.Rad2Deg * Mathf.Atan2(facing.x, facing.z);

                transform.position = centerOfRoadPosition + offset;
                transform.eulerAngles = new Vector3(0, angle, 0);;
                

            }
        }
    }

    private void SparksParticleSystem() {
        if (clampedXChange >= +RoadManager.instance.GetRoadWidth() - carWidth - 0.025f) {
            if (RightSparks.isPlaying == false) {
                RightSparks.Play();
            }
        } else {
            if (RightSparks.isPlaying == true) {
                RightSparks.Stop();
            }
        }
        if (clampedXChange <= -RoadManager.instance.GetRoadWidth() + carWidth + 0.025f) {
            if (LeftSparks.isPlaying == false) {
                LeftSparks.Play();
            }
        } else {
            if (LeftSparks.isPlaying == true) {
                LeftSparks.Stop();
            }
        }
    }

    public int GetCurrentRoadNum() {
        return currentRoadNum;
    }

    public Road GetCurrentRoad() {
        return RoadManager.instance.GetRoad(currentRoadNum)?.GetComponent<Road>();
    }

    public float GetFractionAlongCurrentRoad(){
        return fractionAlongCurrentRoad;
    } 

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            // GameplayManager.instance.RestartGame();
        }
    }
    
    
}
