using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public Transform TPCamera;
    public Transform FPCamera;
    public TankAimming tankAimming;

    private bool isThirdPerson = true;

    private void Start()
    {
        tankAimming.currentCamera = TPCamera.GetComponent<Camera>();
    }

    void Update()
    {
        // 左shift键可以切换摄像机
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isThirdPerson = !isThirdPerson;
        }
        //滚轮拉到一定程度也可以
        if (isThirdPerson && TPCamera.GetComponent<ThirdPersonCamera>().distance <= 2.5f)
        {
            isThirdPerson = false;
            TPCamera.GetComponent<ThirdPersonCamera>().distance = 3f;
        }
        if (!isThirdPerson && FPCamera.GetComponent<FPCamera>().fov >= 45f)
        {
            isThirdPerson = true;
            FPCamera.GetComponent<FPCamera>().fov = 40f;
        }

        if (isThirdPerson) //当前使用第三人称摄像机
        {
            // 设置第一人称摄像机
            FPCamera.GetComponent<FPCamera>().isThirdPerson = true;
            FPCamera.GetComponent<FPCamera>().TPCameraMouseSensitivity = TPCamera.GetComponent<ThirdPersonCamera>().actualMouseSensitivity;
            FPCamera.GetComponent<Camera>().enabled = false;
            FPCamera.GetComponent<AudioListener>().enabled = false;

            // 设置第三人称摄像机
            TPCamera.GetComponent<ThirdPersonCamera>().isFirstPerson = false;
            TPCamera.GetComponent<Camera>().enabled = true;
            TPCamera.GetComponent<AudioListener>().enabled = true;

            tankAimming.currentCamera = TPCamera.GetComponent<Camera>();
        }
        else //当前使用第一人称摄像机
        {
            // 设置第一人称摄像机
            FPCamera.GetComponent<FPCamera>().isThirdPerson = false;
            FPCamera.GetComponent<Camera>().enabled = true;
            FPCamera.GetComponent<AudioListener>().enabled = true;

            // 设置第三人称摄像机
            TPCamera.GetComponent<ThirdPersonCamera>().isFirstPerson = true;
            TPCamera.GetComponent<ThirdPersonCamera>().FPCameraMouseSensitivity = FPCamera.GetComponent<FPCamera>().actualMouseSensitivity;
            TPCamera.GetComponent<Camera>().enabled = false;
            TPCamera.GetComponent<AudioListener>().enabled = false;

            tankAimming.currentCamera = FPCamera.GetComponentInChildren<Camera>();
        }
    }
}
