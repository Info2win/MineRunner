using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
   private float maxHealth =100f;
   private Image Healthbar;
   public float currentHeath;
   PlayerMotor player;

   private void Start()
   {
       
       Healthbar = GetComponent<Image>();
       player = FindObjectOfType<PlayerMotor>();

   }

   private void Update()
   {
        currentHeath = player.health;
       Healthbar.fillAmount = currentHeath / maxHealth;
       if(currentHeath <=30) Healthbar.color= Color.red;
       else Healthbar.color = new Color(0.2745098f,0.5058824f,0.9803922f,1f);
   }
}
