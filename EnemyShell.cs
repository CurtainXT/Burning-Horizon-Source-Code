using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShell : MonoBehaviour
{
    public float force = 8000f;
    public GameObject impactFX;
    private Rigidbody m_rb;

    // 火力测试
    private TankDestoryed.DamageType FirePowerTest(Player player)
    {
        TankDestoryed.DamageType damage = TankDestoryed.DamageType.NoEffect;
        float firePower = Random.Range(2f, 7.5f);
        Debug.Log(firePower);
        if (firePower < player.armor)
        {
            damage = TankDestoryed.DamageType.NoEffect;
        }
        else
        {
            if (firePower < player.armor + 1)
            {
                damage = TankDestoryed.DamageType.BailedOut;
            }
            else
            {
                if (firePower <= player.armor + 3)
                {
                    damage = TankDestoryed.DamageType.Destoryed;
                }
                else
                {
                    if (firePower > player.armor + 3)
                        damage = TankDestoryed.DamageType.AmmoDetonation;
                }
            }
        }

        return damage;
    }

    void Start()
    {
        m_rb = this.GetComponent<Rigidbody>();
        m_rb.velocity = this.transform.forward * force;
        Destroy(this.gameObject, 10f);
    }

    void FixedUpdate()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + transform.forward, Color.red, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            PlayerDestoryed playerDestoryed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerDestoryed>();
            // 伤害判定在炮弹脚本中进行
            TankDestoryed.DamageType damage = FirePowerTest(player);
            switch (damage)
            {
                case TankDestoryed.DamageType.NoEffect:
                    Debug.Log("NoEffect");
                    break;
                case TankDestoryed.DamageType.BailedOut:
                    player.startBailedOut = true;
                    player.isBailedOut = true;
                    Debug.Log("BailedOut");
                    break;
                case TankDestoryed.DamageType.Destoryed:
                    playerDestoryed.isDestoryed = true;
                    break;
                case TankDestoryed.DamageType.AmmoDetonation:
                    playerDestoryed.isAmmoDetonation = true;
                    playerDestoryed.isDestoryed = true;
                    break;
            }

            if (damage == TankDestoryed.DamageType.NoEffect)
            {
                // 没有产生效果，炮弹不能立刻删除
                Destroy(this.gameObject, 0.5f);
            }
            else
            {
                // 产生了效果，炮弹立刻删除
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        // 产生了效果 生成冲击特效
        GameObject temp = Instantiate(impactFX, collision.contacts[0].point, Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal));
        Destroy(temp, 3f);
    }
}
