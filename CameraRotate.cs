using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform target;

    private void RotateCamera() //摄像机围绕目标旋转操作
    {
        transform.RotateAround(target.transform.position, Vector3.up, 6f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();

    }
}
