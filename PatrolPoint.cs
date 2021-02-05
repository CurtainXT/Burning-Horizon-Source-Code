using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public EnemyController enemyController = null;
    public Transform nextPatrolPoint = null;

    // Update is called once per frame
    void Update()
    {
        // 如果。。。
        if (enemyController != null &&  //当该巡逻点会被某个敌人使用
            enemyController.patrolIsActive && //敌人激活了巡逻
            enemyController.patrolPoint == this.transform && //敌人目前正在行驶向该巡逻点
            enemyController.rotateComplete && //敌人已经完成转向
            enemyController.moveComplete //敌人已经完成向前移动
            ) 
        {
            if (nextPatrolPoint != null)
            {
                // 将下一个巡逻点设置给敌人
                enemyController.patrolPoint = nextPatrolPoint;
                enemyController.moveComplete = false;
                enemyController.rotateComplete = false;
            }
            else
            {
                // 如果没有下一个巡逻点，就保持这设置为该巡逻点
                enemyController.patrolPoint = this.transform;
            }
        }
    }

}
