using Unity.VisualScripting;
using UnityEngine;
// dont forget to change Layer into Interactable
// Use as a Prompt message on a object

public class EventOnlyInteractable : Interactable
{
    
    void Start()
    {

    }

   
    void Update()
    {

    }
    
    protected override void Interact()
    {
        Debug.Log(" ");
    }
}
