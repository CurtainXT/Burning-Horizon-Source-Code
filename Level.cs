using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 关卡管理器
public class Level : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject MissionCompleteUI;
    public GameObject gamePauseUI;
    public string currentScene;
    public string nextScene;

    private bool isMissionComplete;
    private bool isGamePaused;

    private void Start()
    {
        isMissionComplete = false;
        isGamePaused = false;
    }

    void Update()
    {
        // 判断任务失败条件
        if (this.GetComponent<PlayerDestoryed>().isDestoryed)
        {
            gameOverUI.SetActive(true);

            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(currentScene);
            }
        }

        // 判断任务完成条件
        if(GameObject.FindGameObjectWithTag("Enemy") == null) // 敌人已被消灭完全
        {
            isMissionComplete = true;
        }

        // 加载下一关
        if(isMissionComplete)
        {
            MissionCompleteUI.SetActive(true);

            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(nextScene);
            }
        }

        // 游戏暂停中
        if (isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Quit");
                Application.Quit();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                isGamePaused = false;
                gamePauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        // 游戏暂停判断
        if (Input.GetKey(KeyCode.Escape))
        {
            isGamePaused = true;
            gamePauseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
