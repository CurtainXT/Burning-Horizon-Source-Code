using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour 
{
	public float force = 8000f;
	public GameObject impactFX;
	private Rigidbody m_rb;

    // 火力测试
    private TankDestoryed.DamageType FirePowerTest(Enemy enemy)
    {
        TankDestoryed.DamageType damage = TankDestoryed.DamageType.NoEffect;
        float firePower = Random.Range(2f, 7f);
        if (firePower < enemy.armor)
        {
            damage = TankDestoryed.DamageType.NoEffect;
        }
        else
        {
            if (firePower < enemy.armor + 1)
            {
                damage = TankDestoryed.DamageType.BailedOut;
            }
            else
            {
                if (firePower <= enemy.armor + 3)
                {
                    damage = TankDestoryed.DamageType.Destoryed;
                }
                else
                {
                    if (firePower > enemy.armor + 3)
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

    void FixedUpdate () 
	{
		Debug.DrawLine(this.transform.position,this.transform.position + transform.forward,Color.red, 0.1f);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            // 伤害判定在炮弹脚本中进行
            TankDestoryed.DamageType damage = FirePowerTest(enemy);
            switch (damage)
            {
                case TankDestoryed.DamageType.NoEffect:
                    Debug.Log("NoEffect");
                    break;
                case TankDestoryed.DamageType.BailedOut:
                    enemy.startBailedOut = true;
                    enemy.isBailedOut = true;
                    Debug.Log("BailedOut");
                    break;
                case TankDestoryed.DamageType.Destoryed:
                    enemy.tankDestoryed.isTankDestoryed = true;
                    enemy.tankDestoryed.isAmmoDetonation = false;
                    enemy.tankDestoryed.destoryedTank = enemy.transform;
                    break;
                case TankDestoryed.DamageType.AmmoDetonation:
                    enemy.tankDestoryed.isTankDestoryed = true;
                    enemy.tankDestoryed.isAmmoDetonation = true;
                    enemy.tankDestoryed.destoryedTank = enemy.transform;
                    enemy.tankDestoryed.destoryedTankTurretTrans = enemy.turret;
                    break;
            }

			if(damage == TankDestoryed.DamageType.NoEffect)
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
            Destroy(this.gameObject, 0.5f);
        }

		// 产生了效果 生成冲击特效
		GameObject temp = Instantiate(impactFX, collision.contacts[0].point, Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal));
		Destroy(temp, 3f);
	}
}
