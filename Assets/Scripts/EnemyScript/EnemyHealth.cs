using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float Enemyhealth = 100;
    public float EmaxHealths = 100;

    private float lerpTimer;
    [Header("Health Bar")]

    public float chipSpeed = 2f;
    public Image Front_Healthbar;
    public Image Back_Healthbar;





   
    void Start()
    {
        Enemyhealth = EmaxHealths;

      



    }


    void Update()
    {
        Enemyhealth = Mathf.Clamp(Enemyhealth, 0, EmaxHealths);
        UpdateHealthUI();


        

    }

    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        float fillF = Front_Healthbar.fillAmount;
        float fillB = Back_Healthbar.fillAmount;
        float hFraction = Enemyhealth / EmaxHealths;
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

    public void TakeDamage(int damage)
    {
        Enemyhealth -= damage;



        lerpTimer = 0f;

        if (Enemyhealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Died!!");
        }



    }

}
