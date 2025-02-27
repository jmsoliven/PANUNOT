using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

// drag in inspector

// input action
public class InputManager : MonoBehaviour
{


    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
   

    private PlayerMotor motor;
    private PlayerLook look;
   
    
    public static object OnFoot { get; internal set; }

    
    void Awake()
    {
       playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        
       

    
        //callback context jump function (performed,canceled,started)
        onFoot.Jump.performed += ctx => motor.Jump();

    }

    void FixedUpdate() =>
        // tell the playermotor to move using the value from our movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    private void LateUpdate() { 
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        
      
    }



    private void OnEnable() { onFoot.Enable(); }
    private void OnDisable() { onFoot.Disable(); }
}