using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float armor = 3f;
    public PlayerDestoryed playerDestoryed;

    // Bailed Out特效
    public GameObject bailedOutFireFx;
    public Transform bialedOutFireTrans;
    public float bailedOutTime = 5f;
    [HideInInspector]
    public bool isBailedOut;
    [HideInInspector]
    public bool startBailedOut; //专门用来启动Invoke

    public Transform turret;

    private void BailedOutOver()
    {
        isBailedOut = false;
    }

    private void Start()
    {
        playerDestoryed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDestoryed>();
        isBailedOut = false;
        startBailedOut = false;
    }

    private void Update()
    {
        if (startBailedOut)
        {
            GameObject fireTemp = Instantiate(bailedOutFireFx, bialedOutFireTrans.position, Quaternion.identity);
            GameObject.Destroy(fireTemp, bailedOutTime);
            Invoke("BailedOutOver", bailedOutTime);
            startBailedOut = false;
        }

        if(isBailedOut)
        {
            this.GetComponent<TankController>().enabled = false;
            this.GetComponent<TankAimming>().enabled = false;
            this.GetComponent<FireShell>().enabled = false;
        }
        else
        {
            this.GetComponent<TankController>().enabled = true;
            this.GetComponent<TankAimming>().enabled = true;
            this.GetComponent<FireShell>().enabled = true;
        }
    }
}
