using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDestoryed : MonoBehaviour
{
    public Player player;


    public GameObject TankBody_AmmoDetonated;
    public GameObject TankDestoryed;
    public GameObject TankTurret_AmmoDetonated;

    public GameObject TPCamera;

    public float ammoDetonateForce = 800f;

    [HideInInspector]
    public bool isDestoryed;

    [HideInInspector]
    public bool isAmmoDetonation = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isDestoryed = false;

    }

    private void Update()
    {
        if(isDestoryed)
        {
            TPCamera.GetComponent<Camera>().enabled = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isDestoryed)
        {
            DestoryPlayer();
        }
    }

    private void DestoryPlayer()
    {
        if (isAmmoDetonation)
        {
            Transform body = player.transform;
            Instantiate(TankBody_AmmoDetonated, body.position, body.rotation);
            // 殉爆的炮塔受力受随机数影响
            Instantiate(TankTurret_AmmoDetonated,
                player.turret.position,
                player.turret.rotation).GetComponent<Rigidbody>().AddForce((
                Vector3.up + new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.05f, 0.05f)))
                * Random.Range(ammoDetonateForce * 0.8f, ammoDetonateForce * 1.2f),
                ForceMode.Impulse);
        }
        else
        {
            Instantiate(TankDestoryed, player.transform.position, player.transform.rotation);
        }

        Destroy(player.gameObject);
        //isDestoryed = false;
    }
}
