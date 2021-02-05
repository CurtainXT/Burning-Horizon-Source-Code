using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    // 鼠标灵敏度
    public float mouseSensitivity = 10f;
    // 滚轮灵敏度
    public float mouseScrollSensitivity = 5f;
    // 第三人称目标
    public Transform target;
    // 摄像机离目标的位置
    public float distanceFromTarget = 2f;
    // y方向的限制
    public Vector2 pitchMinMax = new Vector2(-40f, 85f);
    // 旋转运动平滑时间
    public float rotationSmoothTime = 0.12f;
    // 是否锁定鼠标
    public bool lockCursor;
    // 当前是否是第一人称
    public bool isFirstPerson;
    // 第一人称摄像机的鼠标灵敏度
    public float FPCameraMouseSensitivity;
    // 摄像机转换
    public CameraShift shifter;

    // x方向
    private float yaw;
    // y方向
    private float pitch;

    // 旋转运动平滑速度
    Vector3 rotationSmoothVelocity;
    // 当前的旋转
    Vector3 currentRotation;
    // 实际鼠标灵敏度
    [HideInInspector]
    public float actualMouseSensitivity = 10f;
    // 滚轮拉近拉远摄像机
    [HideInInspector]
    public float distance;

    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        distance = distanceFromTarget;
    }

    private void Update()
    {
        // 当前是否是第一人称
        if(isFirstPerson)
        {
            actualMouseSensitivity = FPCameraMouseSensitivity;
        }
        else
        {
            actualMouseSensitivity = mouseSensitivity;
            // 只有在第三人称时才考虑摄像机拉近拉远
            distance -= Input.GetAxis("Mouse ScrollWheel") * mouseScrollSensitivity;
            distance = Mathf.Clamp(distance, 2.5f, 16f); // 实际到2.5f时会切换到第一人称视角
            distanceFromTarget = Mathf.Clamp(distance, 3f, 16f);
        }

        yaw += Input.GetAxis("Mouse X") * actualMouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * actualMouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw, 0f), ref rotationSmoothVelocity, rotationSmoothTime);
    }

    // 使用LateUpdate在target.position设置好以后设置摄像机的位置
    void LateUpdate()
    {
        Vector3 targetRotation = currentRotation;
        transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
