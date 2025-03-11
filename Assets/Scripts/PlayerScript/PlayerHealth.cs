using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{


    public float health = 100;
    public float maxHealths = 100;

    private float lerpTimer;
    [Header("Health Bar")]

    public float chipSpeed = 2f;
    public Image Front_Healthbar;
    public Image Back_Healthbar;

    public float healrange;



    public TextMeshProUGUI healthText;
    void Start()
    {
        health = maxHealths;

        healrange = Random.Range(50, 100);
   

        
    }

   
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealths);
        UpdateHealthUI();

        if (healthText != null)
            healthText.SetText(health+"");

        //heal
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            if (health < 1000) 
           {
               RestoreHealth(healrange);
              Debug.Log(healrange + " HP restored!");
           }

        }

        

    }

    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        float fillF = Front_Healthbar.fillAmount;
        float fillB = Back_Healthbar.fillAmount;
        float hFraction = health / maxHealths;
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
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }



}
