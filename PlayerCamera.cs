using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 该脚本已被弃用
public class PlayerCamera : MonoBehaviour
{
//-------------------------------------------第三人称----------------------------------------------//
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

    // x方向
    private float yaw;
    // y方向
    private float pitch;
    // 滚轮拉近拉远摄像机
    private float distance;
    // 旋转运动平滑速度
    private Vector3 rotationSmoothVelocity;
    // 当前的旋转
    private Vector3 currentRotation;

//-------------------------------------------第一人称----------------------------------------------//
    public Transform cameraTransform;
    public float mouseSentivity;
    public Vector2 maxMinAngle;
    public float mouseScrollSpeed = 5f;

    private Vector3 m_mouseInputValue;
    private float mouseScrollWheel;
    private float fov;
    //----------------------------------------摄像机切换参数-------------------------------------------//
    // 摄像机是否要转换为第三人称
    private bool changeThirdPerson;
    // 摄像机是否要转换为第一人称
    private bool changeFirstPerson;
    // 摄像机是否为第三人称
    private bool isThirdPerson;
    // 摄像机是否为第一人称
    private bool isFirstPerson;

    void Start()
    {
        // 锁定并隐藏鼠标
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        isThirdPerson = true;
        isFirstPerson = false;
        distance = distanceFromTarget;

        m_mouseInputValue = new Vector3();
    }

    private void Update()
    {
        // 获取输入
        if(isThirdPerson)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            distance -= Input.GetAxis("Mouse ScrollWheel") * mouseScrollSensitivity;

            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            if(distance < 3f)
            {
                isFirstPerson = true;
                isThirdPerson = false;
            }
            distance = Mathf.Clamp(distance, 3f, 16f);
        }
        if(isFirstPerson)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            mouseScrollWheel = -Input.GetAxis("Mouse ScrollWheel");
            m_mouseInputValue.y += mouseX * mouseSentivity;
            m_mouseInputValue.x -= mouseY * mouseSentivity;

            fov = cameraTransform.GetComponent<Camera>().fieldOfView;
            mouseSentivity = 0.5f + (fov / 10);
            fov += mouseScrollWheel * mouseScrollSpeed;
            if(fov > 60f)
            {
                isFirstPerson = false;
                isThirdPerson = true;
            }
            fov = Mathf.Clamp(fov, 5f, 60f);
            // 限制垂直旋转的角度（以符合现实情况）
            m_mouseInputValue.x = Mathf.Clamp(m_mouseInputValue.x, maxMinAngle.x, maxMinAngle.y);
        }
    }

    // 使用LateUpdate在target.position设置好以后设置摄像机的位置
    void LateUpdate()
    {
        if(isThirdPerson)
        {
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw, 0f), ref rotationSmoothVelocity, rotationSmoothTime);

            Vector3 targetRotation = currentRotation;
            transform.eulerAngles = targetRotation;

            distanceFromTarget = distance;

            // 摄像机的位置设置在目标位置减去自身z轴方向上的特定距离
            transform.position = target.position - transform.forward * distanceFromTarget;
        }
        if(isFirstPerson)
        {
            // 水平旋转整个Controller
            this.transform.localRotation = Quaternion.Euler(0, m_mouseInputValue.y, 0);
            // 垂直只旋转摄像机
            cameraTransform.localRotation = Quaternion.Euler(m_mouseInputValue.x, 0, 0);
            // 摄像机缩放
            cameraTransform.GetComponent<Camera>().fieldOfView = fov;
        }
    }
}
