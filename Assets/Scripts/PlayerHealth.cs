using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

// drag in inspector

public class PlayerHealth : MonoBehaviour
{
   
    private float health;
    private float lerpTimer;
    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image Front_Healthbar;
    public Image Back_Healthbar;

    [Header("Damage Overlay")]
    public Image overlay; //damageoverlay gameobject
    public float duration;// how long the image stays fully opaque
    public float fadespeed; // how quikly the image will fade

    private float durationTimer; // timer to check against the duration

   
 
   
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
      

    }

   
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
       



            // Taking damage while Pressing down the left mouse

                if (Input.GetKeyDown(KeyCode.Mouse0))
                    
                   

                {
                    TakeDamage(Random.Range(5, 10));
                    Debug.Log(health +" Remaining Health");

                }

                // restore health while pressing the H button
                if (Input.GetKeyDown(KeyCode.H))
                {
                    RestoreHealth(Random.Range(5, 10));
                    Debug.Log(health + " HP healed!");
                }

            //color overlay if less than 30 hp 

            if (overlay.color.a > 0)
            {
                if (health < 30)
                {
                    return;
                }
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    float tempAlpha = overlay.color.a;
                    tempAlpha -= Time.deltaTime * fadespeed;
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);

                }
            }

        
    }



    //health updater & how the HP bar moves while taking damage and restoring health.
    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        float fillF = Front_Healthbar.fillAmount;
        float fillB = Back_Healthbar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            Front_Healthbar.fillAmount = hFraction;
            Back_Healthbar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            Back_Healthbar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);

        }
        if (fillF < hFraction)
        {
            Back_Healthbar.color = Color.green;
            Back_Healthbar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            Front_Healthbar.fillAmount = Mathf.Lerp(fillF, Back_Healthbar.fillAmount, percentComplete);
        }
    }
     //damage health
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }
    
    // restore health
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
