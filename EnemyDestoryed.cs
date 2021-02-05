using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestoryed : MonoBehaviour
{
    // 是否有坦克被击毁
    public bool isTankDestoryed = false;
    // 是否为殉爆
    public bool isAmmoDetonation = false;
    public float ammoDetonateForce = 800f;

    [HideInInspector]
    public Transform destoryedTank;
    [HideInInspector]
    public Transform destoryedTankTurretTrans;

    // 坦克残骸预制体
    public GameObject currentTankBody_AmmoDetonated;
    public GameObject currentTankDestoryed;
    public GameObject currentTankTurret_AmmoDetonated;

    // Start is called before the first frame update
    void Start()
    {
        isTankDestoryed = false;
        isAmmoDetonation = false;
        destoryedTank = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTankDestoryed)
        {
            if (isAmmoDetonation)
            {
                Transform body = destoryedTank.transform;
                Instantiate(currentTankBody_AmmoDetonated, body.position, body.rotation);
                // 殉爆的炮塔受力受随机数影响
                Instantiate(currentTankTurret_AmmoDetonated,
                    destoryedTankTurretTrans.position,
                    destoryedTankTurretTrans.rotation).GetComponent<Rigidbody>().AddForce((
                    Vector3.up + new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.05f, 0.05f)))
                    * Random.Range(ammoDetonateForce * 0.8f, ammoDetonateForce * 1.2f),
                    ForceMode.Impulse);
                Destroy(destoryedTank.gameObject);
                isAmmoDetonation = false;
            }
            else
            {
                Instantiate(currentTankDestoryed, destoryedTank.position, destoryedTank.rotation);
                Destroy(destoryedTank.gameObject);
            }
            isTankDestoryed = false;
            destoryedTank = null;
        }
    }
}
