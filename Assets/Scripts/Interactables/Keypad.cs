using Unity.VisualScripting;
using UnityEngine;

// drag in inspector
public class Keypad : Interactable
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    protected override void Interact()
    {
        Debug.Log("Pressed [E] Button on the " +gameObject.name);
    }
}
