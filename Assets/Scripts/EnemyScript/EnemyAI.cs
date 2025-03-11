
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask Ground, Player;


   
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


    public PlayerHealth playerHealth;
    public PlayerHealth maxHealth;

    public float lerpTimer;


  
  
    [Header("Damage Overlay")]
    public Image overlay; //damageoverlay gameobject
    public float duration;// how long the image stays fully opaque
    public float fadespeed; // how quikly the image will fade

    private float durationTimer; // timer to check against the duration

   
    private void Awake()
    {

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {

       
     

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
   //     if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

  


        //color overlay if less than 30 hp 

      if (overlay.color.a > 0)
      {
          if (playerHealth.health < 30)
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

    //    if (walkPointSet)
  //          agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < .01f)
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

   // private void ChasePlayer()
  //  {
  //      agent.SetDestination(player.position);
  //  }

    private void AttackPlayer()
    {
        
      //  agent.SetDestination(transform.position);

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
            Debug.Log(playerHealth.health + " Remaining Health");
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }
  
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        playerHealth.health -= damage;
  
  
  
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        if (playerHealth.health <= 0) Invoke(nameof(DestroyPlayer), 0.5f);
    }

    private void DestroyPlayer()
    {
       
            Destroy(GameObject.Find("Player"));
            Debug.Log("You Died!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

}
