
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask Ground, Player;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

   


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

   
    private void Awake()
    {

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);


        


    }

    private void Update()
    {

        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        //heal
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

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            //random damage 
            TakeDamage(Random.Range(5, 10));
            Debug.Log(health + " Remaining Health");
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }
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
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        lerpTimer = 0f;
         durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        if (health <= 0) Invoke(nameof(DestroyPlayer), 0.5f);
    }
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
    private void DestroyPlayer()
    {
       
            Destroy(GameObject.Find("Player"));
            Debug.Log("You Died!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

}
