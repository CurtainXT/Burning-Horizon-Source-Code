using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankAimming : MonoBehaviour
{
    // 旋转速度
    public float rotateSpeed;
    // 炮塔的Transform
    public Transform turret;
    // 炮管的Transform
    public Transform gun;

    // 火炮瞄准UI图片
    public Image GunAimImage;
    // 炮口的Transform
    public Transform gunPoint;

    // 炮管的仰角
    [Range(0.0f, 90.0f)]
    public float elevation = 25f;
    // 炮管的俯角
    [Range(0.0f, 90.0f)]
    public float depression = 10f;
    
    // 当前正在使用的摄像机
    public Camera currentCamera;

    // 炮塔锁死功能
    private bool isLocked = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 用于存储瞄准的方向
        Vector3 aimPosition/* = Camera.main.transform.TransformPoint(Vector3.forward * 10000.0f)*/;
        // 用于确定射线的落点
        RaycastHit camHit;
        // 射线的最大距离
        float maxDistance = 10000f;
        // 用于Debug.DrawRay
        float camDistance = 0f;
        // 从当前使用的摄像机位置向前发射射线
        if (Physics.Raycast(currentCamera.transform.position,
            currentCamera.transform.forward,
            out camHit, maxDistance, LayerMask.GetMask("Default", "Ground", "Enemy")))
        {
            aimPosition = camHit.point;
            camDistance = camHit.distance;
        }
        else
        {
            aimPosition = currentCamera.transform.forward * maxDistance;
            camDistance = maxDistance;
        }

        // 右键锁死坦克炮塔
        if (Input.GetMouseButton(1))
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }

        // 如果炮塔没有锁死
        if (!isLocked)
        { 
            // 炮塔的实际旋转
            Vector3 turretPos = transform.InverseTransformPoint(aimPosition);
            turretPos.y = 0f;  //过滤掉y轴的信息，防止炮塔出现绕x，z轴旋转的问题
            Quaternion aimRotTurret = Quaternion.RotateTowards(turret.localRotation,
                Quaternion.LookRotation(turretPos), Time.deltaTime * rotateSpeed);

            turret.localRotation = aimRotTurret;

            // 炮管的实际旋转
            Vector3 localTargetPos = turret.InverseTransformPoint(aimPosition);
            localTargetPos.x = 0f; //过滤掉x轴的信息，防止炮塔出现绕y，z轴旋转的问题
            Vector3 clampedLocalVec2Target = localTargetPos;
            // 根据俯仰角限制炮管的旋转角度
            if (localTargetPos.y >= 0.0f)
                clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * elevation, float.MaxValue);
            else
                clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * depression, float.MaxValue);
            Quaternion aimRotGun = Quaternion.RotateTowards(gun.localRotation,
                Quaternion.LookRotation(clampedLocalVec2Target), Time.deltaTime * rotateSpeed);

            gun.localRotation = aimRotGun;
        }

        // 炮管的瞄准UI
        RaycastHit gunHit;
        Vector3 UIPos;
        float gunDistance = 100f;
        if (Physics.Raycast(gunPoint.position,
            gunPoint.TransformDirection(Vector3.forward),
            out gunHit, maxDistance,
            LayerMask.GetMask("Default", "Ground", "Enemy")))
        {
            gunDistance = gunHit.distance;
            UIPos = gunHit.point;
        }
        else
        {
            gunDistance = 100f;
            UIPos = gunPoint.position + gunPoint.forward * gunDistance;
        }

        GunAimImage.rectTransform.position = currentCamera.WorldToScreenPoint(UIPos);

        Debug.DrawRay(gunPoint.position, gunPoint.forward * gunDistance, Color.red);
        Debug.DrawRay(currentCamera.transform.position, currentCamera.transform.TransformDirection(Vector3.forward) * camDistance, Color.blue);
    }
}
