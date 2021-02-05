using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour 
{
    public GameObject shell;
    public Transform gunPoint;
    public float reactionForce = 10f;
    public float LoadTime = 5f; //装填时间
    public GameObject fireFX;
    public AudioSource fireAudioPlayer;

    private bool isLoaded; //用于标记是否装填完成
    private bool startLoading; //用于标记是否要进行装填

    private void Loaded()
    {
        isLoaded = true;
    }

    private void Start()
    {
        isLoaded = true;
        startLoading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLoaded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(shell, gunPoint.position, gunPoint.rotation);
                this.GetComponent<Rigidbody>().AddForceAtPosition(-gunPoint.forward * reactionForce, gunPoint.position, ForceMode.Impulse);
                GameObject fireFXTemp = Instantiate(fireFX, gunPoint.position, gunPoint.rotation);
                GameObject.Destroy(fireFXTemp, 3f);
                startLoading = true;
                isLoaded = false;
                fireAudioPlayer.Play();
            }
        }

        if(startLoading)
        {
            Invoke("Loaded", LoadTime);
            startLoading = false;
        }
    }
}