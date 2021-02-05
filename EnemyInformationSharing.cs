using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 当一个敌人发现玩家时，该系统内的所有敌人相当于都发现了玩家
public class EnemyInformationSharing : MonoBehaviour
{
    public Enemy[] enemies;
    private bool someCanSee;

    // Start is called before the first frame update
    void Start()
    {
        someCanSee = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemies != null)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.canSeePlayer)
                {
                    someCanSee = true;
                    break;
                }
            }

            if (someCanSee)
            {
                foreach (var enemy in enemies)
                {
                    enemy.isInfoShared = true;
                }
            }
        }
    }
}
