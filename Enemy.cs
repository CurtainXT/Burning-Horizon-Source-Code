using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public enum TankType { Sherman, Tiger, T34 };
    public EnemyDestoryed tankDestoryed;

    // 炮塔旋转
    public float rotateSpeed; //炮塔旋转速度
    public Transform turret;
    public Transform gun;
    public Transform watchTower;
    [Range(0.0f, 90.0f)]
    public float elevation = 25f;
    [Range(0.0f, 90.0f)]
    public float depression = 10f;
    [HideInInspector]
    public bool canSeePlayer; // 能否看见玩家？
    [HideInInspector]
    public bool isInfoShared; // 是否有信息共享

    // Bailed Out特效
    public GameObject bailedOutFireFx;
    public Transform bialedOutFireTrans;
    public float bailedOutTime = 10f;
    [HideInInspector]
    public bool isBailedOut = false;
    [HideInInspector]
    public bool startBailedOut = false; //专门用来启动Invoke

    // 敌人的装甲
    [HideInInspector]
    public float armor = 3f;

    // 敌人的瞄准
    public float detectDistance = 200f;
    public Transform aimmingPosition; // 瞄准玩家的点（炮塔）

    // 敌人开火
    public GameObject shell;
    public Transform gunPoint;
    public float reactionForce = 10f;
    public float LoadTime = 20f; //装填时间
    public GameObject fireFX;
    public AudioSource fireAudioPlayer;
    private bool isLoaded; //用于标记是否装填完成
    private bool startLoading; //用于标记是否要进行装填
    private bool aimedAtPlayer; //是否已瞄准？

    //public TankDestoryed.DamageType damageType;



    private void BailedOutOver()
    {
        isBailedOut = false;
        this.GetComponent<EnemyController>().enabled = true;
    }

    private void Loaded()
    {
        isLoaded = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        tankDestoryed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyDestoryed>();
        //aimmingPosition = GameObject.FindGameObjectWithTag("Player").transform;

        isLoaded = false;
        startLoading = true;
        canSeePlayer = false;
        isInfoShared = false;
        aimedAtPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 是否能直接看到玩家（射线检测）
        RaycastHit hit;
        // 能否看见玩家
        if(Physics.Raycast(watchTower.position, aimmingPosition.position - watchTower.position, out hit, detectDistance))
        {
            if(hit.transform.tag == "Player")
            {
                // 能看到玩家
                canSeePlayer = true;
                // 是否已经瞄准
                if (Physics.Raycast(gunPoint.position, gunPoint.forward, out hit, detectDistance))
                {
                    if(hit.transform.tag == "Player")
                    {
                        // 已经瞄准玩家
                        aimedAtPlayer = true;
                    }
                    else
                    {
                        aimedAtPlayer = false;
                    }
                }
                else
                {
                    aimedAtPlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }

        // BailedOut判定
        if (startBailedOut)
        {
            GameObject fireTemp = Instantiate(bailedOutFireFx, bialedOutFireTrans.position, Quaternion.identity);
            GameObject.Destroy(fireTemp, bailedOutTime);
            Invoke("BailedOutOver", bailedOutTime);
            startBailedOut = false;
        }

        // 没有在BailedOut状态下敌人才能行动
        if(!isBailedOut)
        {
           if(canSeePlayer || isInfoShared)
            {
                // 看得到玩家或有队友的信息共享时才会尝试瞄准玩家
                Vector3 aimPosition = aimmingPosition.position;
                Vector3 turretPos = transform.InverseTransformPoint(aimPosition);
                turretPos.y = 0f;  //过滤掉y轴的信息，防止炮塔出现绕x，z轴旋转的问题
                Vector3 LocalVec2Target = turretPos;
                Quaternion aimRotTurret = Quaternion.RotateTowards(turret.localRotation,
                Quaternion.LookRotation(turretPos), Time.deltaTime * rotateSpeed);
                turret.localRotation = aimRotTurret;
                Vector3 localTargetPos = turret.InverseTransformPoint(aimPosition);
                localTargetPos.x = 0f;
                Vector3 clampedLocalVec2Target = localTargetPos;
                if (localTargetPos.y >= 0.0f)
                    clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * elevation, float.MaxValue);
                else
                    clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * depression, float.MaxValue);
                Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
                Quaternion aimRotGun = Quaternion.RotateTowards(gun.localRotation, rotationGoal, Time.deltaTime * rotateSpeed);
                gun.localRotation = aimRotGun;

                // 瞄准玩家并且装填完毕就会向玩家开火
                if (isLoaded && aimedAtPlayer)
                {
                    Instantiate(shell, gunPoint.position, gunPoint.rotation);
                    this.GetComponent<Rigidbody>().AddForceAtPosition(-gunPoint.forward * reactionForce, gunPoint.position, ForceMode.Impulse);
                    GameObject fireFXTemp = Instantiate(fireFX, gunPoint.position, gunPoint.rotation);
                    GameObject.Destroy(fireFXTemp, 3f);
                    fireAudioPlayer.Play();
                    startLoading = true;
                    isLoaded = false;
                }
           }


           // 装填弹药
            if (startLoading)
            {
                Invoke("Loaded", LoadTime);
                startLoading = false;
            }
        }
        else
        {
            this.GetComponent<EnemyController>().enabled = false;
        }

    }


}
