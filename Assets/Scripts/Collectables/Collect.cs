using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public AudioSource coinSound;
    PlayerMotor player;
    

    private void Start()
    {
        player = FindObjectOfType<PlayerMotor>();
    }

    void OnTriggerEnter(Collider other)
    {
        coinSound.Play();
        gameObject.SetActive(false);
        player.health += player.gainHealth;
    }

}
