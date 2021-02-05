using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //坦克左边的所有轮子
    public GameObject[] LeftWheels;
    //坦克右边的所有轮子
    public GameObject[] RightWheels;

    //坦克左边的履带
    public GameObject LeftTrack;
    //坦克右边的履带
    public GameObject RightTrack;

    // 坦克刚体
    public Rigidbody rb;

    // 旋转速度
    public float rotateSpeed = 0.3f;
    // 移动速度
    public float moveSpeed = 3f;

    // 巡逻控制
    public float precision = 0.999f;
    [HideInInspector]
    public bool rotateComplete = false;
    [HideInInspector]
    public bool moveComplete = false;

    // 巡逻点
    public bool patrolIsActive = false; //巡逻点是否激活
    public Transform patrolPoint; //巡逻点，使用外部的GameObject.Transform
    private Transform patrolPointActived; //如果激活了巡逻点，这个对象将被赋值

    // 巡逻模式
    public bool activePatrolByPlayerDetected = false; //启用这个类型，敌人会在看到玩家时前往巡逻点
    public bool stopPatrolByPlayerDetected = false; //启用这个类型，敌人会在看到玩家时停止巡逻
    //public bool patrolLoop = false; //启用这个类型，敌人会在多个巡逻点循环往返

    //public NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rotateComplete = false;
        moveComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 已经抛弃了Navmesh Agent方案
        //agent.SetDestination(patrolPoint.position);

        //根据是否检测到玩家来激活巡逻点
        if (activePatrolByPlayerDetected) //检测到玩家时开始巡逻
        {
            if(this.GetComponent<Enemy>().canSeePlayer || this.GetComponent<Enemy>().isInfoShared)
            {
                patrolIsActive = true;
            }
        }

        if(stopPatrolByPlayerDetected) // 检测到玩家时停止巡逻
        {
            if(this.GetComponent<Enemy>().canSeePlayer)
            {
                patrolIsActive = false;
            }
            else
            {
                patrolIsActive = true;
            }
        }

        // 路径是否激活
        if(patrolIsActive)
        {
            patrolPointActived = patrolPoint;
        }
        else
        {
            patrolPointActived = null;
        }

        // 执行巡逻
        if (patrolPointActived != null) //如果有巡逻点的话
        {
            Vector3 path = patrolPointActived.position - this.transform.position;
            Vector3 direction = new Vector3(path.x, 0f, path.z).normalized;
            
            // 首先转向巡逻点的方向
            if (transform.InverseTransformDirection(direction).z <= precision)
            {
                rotateComplete = false;
                float leftOrRight = Mathf.Sign(Vector3.Cross(transform.forward, direction).y); //左转还是右转？

                // 实际旋转
                Quaternion turnRotation = Quaternion.Euler(0f, leftOrRight * rotateSpeed * Time.deltaTime, 0f);
                rb.MoveRotation(rb.rotation * turnRotation);

                //坦克左右两边车轮转动
                foreach (var wheel in LeftWheels)
                {
                    wheel.transform.Rotate(new Vector3(leftOrRight * rotateSpeed * 0.06f, 0f, 0f));
                }
                foreach (var wheel in RightWheels)
                {
                    wheel.transform.Rotate(new Vector3(-leftOrRight * rotateSpeed * 0.06f, 0f, 0f));
                }
                //履带滚动效果
                // 左右
                LeftTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, -leftOrRight * 0.06f * rotateSpeed * Time.deltaTime);
                RightTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, leftOrRight * 0.06f * rotateSpeed * Time.deltaTime);
            }
            else
            {
                rotateComplete = true;
            }

            // 旋转完成后再移动坦克
            if (rotateComplete && path.sqrMagnitude >= 2f)
            {
                moveComplete = false;
                // 实际移动
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);

                // 车轮向前
                foreach (var wheel in LeftWheels)
                {
                    wheel.transform.Rotate(new Vector3(rotateSpeed * 0.06f, 0f, 0f));
                }
                foreach (var wheel in RightWheels)
                {
                    wheel.transform.Rotate(new Vector3(rotateSpeed * 0.06f, 0f, 0f));
                }
                // 履带向前
                LeftTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, 0.1f * -rotateSpeed * Time.deltaTime);
                RightTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, 0.1f * -rotateSpeed * Time.deltaTime);
            }
            else
            {
                moveComplete = true;
            }
        }
    }
}
