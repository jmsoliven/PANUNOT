using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// drag in inspector

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    public bool Action = true;
    public GameObject AnimeObject;
    //public GameObject ThisTrigger;



  
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
      

    }
  
  //raycast 
    void Update()
    {
       playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {

            if (hitInfo.collider.GetComponent<Interactable>()!= null) 
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);


         // door switch when interacted (press [E] to open the door)

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();

                        if (Action == true)
                        {

                            AnimeObject.GetComponent<Animator>().Play("DoorOpen");
                         
                            Action = false;
                               Debug.Log("Door is Open");

                        }
                        else
                        {

                            AnimeObject.GetComponent<Animator>().Play("DoorClosed");
                            Action = true;
                            Debug.Log("Door is Close");
                        }
                    
                   

                }

       


            }
        }
    }

    }

