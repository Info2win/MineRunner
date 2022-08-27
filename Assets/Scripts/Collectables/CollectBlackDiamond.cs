using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectBlackDiamond : MonoBehaviour
{
    public AudioSource blackDiamondSound;
    public GameObject blackDiamonds;
    private TextMeshProUGUI blackDiamondsText;
    PlayerMotor player;
    private void Start()
    {
        player = FindObjectOfType<PlayerMotor>();
        blackDiamondsText = blackDiamonds.GetComponent<TextMeshProUGUI>();
    }


    void OnTriggerEnter(Collider other)
    {
        blackDiamondSound.Play();
        gameObject.SetActive(false);
        player.blackDiamondCount++;
        blackDiamondsText.text = (player.blackDiamondCount).ToString();
    }
}
