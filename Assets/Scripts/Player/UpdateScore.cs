using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    public PlayerMotor motor;
    

    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        motor.score = ((int)(motor.gameObject.transform.position.z));
        scoreText.text = (motor.score).ToString();
    }
}
