using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour
{
    public Camera camera;
    public float mouseSentivity;
    public Vector2 maxMinAngle;
    public float mouseScrollSpeed = 5f;

    // 当前是否是第三人称
    public bool isThirdPerson;
    // 第三人称摄像机的鼠标灵敏度
    public float TPCameraMouseSensitivity;
    // 摄像机转换
    public CameraShift shifter;
    // 旋转运动平滑时间
    public float rotationSmoothTime = 0.12f;

    private Vector3 m_mouseInputValue;
    private float mouseScrollWheel;
    // 当前的旋转
    Vector3 currentRotation;
    // 旋转运动平滑速度
    Vector3 rotationSmoothVelocity;

    // 实际鼠标灵敏度
    [HideInInspector]
    public float actualMouseSensitivity;
    [HideInInspector]
    public float fov = 40f;

    private void Start()
    {
        m_mouseInputValue = new Vector3();
        camera = this.GetComponent<Camera>();
        fov = 40f;
    }

    void Update()
    {
        // 判断当前是否正在使用第三人称摄像机
        if(isThirdPerson)
        {
            actualMouseSensitivity = TPCameraMouseSensitivity;
        }
        else
        {
            actualMouseSensitivity = mouseSentivity;

            // 只有在第一人称时会设置摄像机fov
            mouseScrollWheel = -Input.GetAxis("Mouse ScrollWheel");
            actualMouseSensitivity = 0.5f + (fov / 10);
            fov += mouseScrollWheel * mouseScrollSpeed;
            fov = Mathf.Clamp(fov, 5f, 45f); //实际到45f时会切换为第三人称视角
            camera.fieldOfView = Mathf.Clamp(fov, 5f, 40f);

        }

        m_mouseInputValue.y += Input.GetAxis("Mouse X") * actualMouseSensitivity;
        m_mouseInputValue.x -= Input.GetAxis("Mouse Y") * actualMouseSensitivity;

        
        // 限制垂直旋转的角度（以符合现实情况）
        m_mouseInputValue.x = Mathf.Clamp(m_mouseInputValue.x, maxMinAngle.x, maxMinAngle.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(m_mouseInputValue.x, m_mouseInputValue.y, 0f), ref rotationSmoothVelocity, rotationSmoothTime);
    }

    private void LateUpdate()
    {
        Vector3 targetRotation = currentRotation;
        transform.eulerAngles = targetRotation;
    }
}
