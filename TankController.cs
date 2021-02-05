using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Rigidbody rb;
    //坦克左边的所有轮子
    public GameObject[] LeftWheels;
    //坦克右边的所有轮子
    public GameObject[] RightWheels;

    //坦克左边的履带
    public GameObject LeftTrack;
    //坦克右边的履带
    public GameObject RightTrack;

    public float wheelSpeed = 2f;
    public float trackSpeed = 2f;
    public float rotateSpeed = 10f;
    public float moveSpeed = 2f;

    public AudioSource movementAudioPlayer;
    public AudioClip move;
    public AudioClip idle;

    private void Update()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 音效播放
        if (horizontal == 0 && vertical == 0)
        {
            movementAudioPlayer.clip = idle;
            if (!movementAudioPlayer.isPlaying)
            {
                movementAudioPlayer.volume = 0.2f;
                movementAudioPlayer.Play();
            }
        }
        else
        {
            movementAudioPlayer.clip = move;
            if(!movementAudioPlayer.isPlaying)
            {
                movementAudioPlayer.volume = 0.6f;
                movementAudioPlayer.Play();
            }
        }

        // 限制倒车的速度
        vertical = Mathf.Clamp(vertical, -0.3f, 1f);

        // 这些都是为了让履带和轮子看上去在动
        //坦克左右两边车轮转动
        foreach (var wheel in LeftWheels)
        {
            wheel.transform.Rotate(new Vector3(wheelSpeed * vertical, 0f, 0f));
            wheel.transform.Rotate(new Vector3(wheelSpeed * 0.6f * horizontal, 0f, 0f));
        }
        foreach (var wheel in RightWheels)
        {
            wheel.transform.Rotate(new Vector3(wheelSpeed * vertical, 0f, 0f));
            wheel.transform.Rotate(new Vector3(wheelSpeed * 0.6f * - horizontal, 0f, 0f));
        }
        //履带滚动效果
        // 前后
        LeftTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, -trackSpeed * vertical * Time.deltaTime);
        RightTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, -trackSpeed * vertical * Time.deltaTime);
        // 左右
        LeftTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, 0.6f * -trackSpeed * horizontal * Time.deltaTime);
        RightTrack.transform.GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, 0.6f * trackSpeed * horizontal * Time.deltaTime);


        // 坦克本体的移动
        rb.MovePosition(rb.position + transform.forward * moveSpeed * vertical * Time.deltaTime);
        // 坦克本体的旋转
        Quaternion turnRotation = Quaternion.Euler(0f, horizontal * rotateSpeed * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
