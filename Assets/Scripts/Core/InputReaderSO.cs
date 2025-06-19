using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

[CreateAssetMenu(fileName = "InputReaderSO", menuName = "Scriptable Objects/InputReaderSO")]
public class InputReaderSO : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;//event with datatype to pass Bool
    public event Action<Vector2> MovingEvent;//event with datatype to pass Vector2
    //public event Action<Vector2> AimingEvent;//better not to use event because it will continuesly trigger event with every mouse move
    public Vector2 aimPosition{ get; private set; }//we can get this variable anywhee, but set it only here
    private PlayerControls controls;//variable for Controls instance
    private void OnEnable()
    {
        //initiating controls:
        if (controls == null)
        {
            controls = new PlayerControls();
            //after creating an instance of the controls we need to assign the Input Reader script to controls:
            controls.Player/*generated field, if not defined not exists*/.SetCallbacks(this);//this contains: ScriptableObject, IPlayerActions
        }
        //enabling controls:
        controls.Enable();//these simply means that we can enabale/disable some inputs depending on the game state
    }
    public void OnMove(InputAction.CallbackContext context)
    {

        MovingEvent?.Invoke(context.ReadValue<Vector2>());

    }

    public void OnPrimaryfire(InputAction.CallbackContext context)
    {
        //here we"ll implement event by checking context state
        if (context.performed/*is pressed in our case*/)
        {
            PrimaryFireEvent?/*will defend us from null ref, 
                               in case if there's no listener*/.Invoke(true);
        }
        else if (context.canceled/*button released in our case*/)
        {
            PrimaryFireEvent?/*will defend us from null ref, 
                               in case if there's no listener*/.Invoke(false);
        }
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        //Debug.Log("AimingEvent" + context.ReadValue<Vector2>());
        // AimingEvent?.Invoke(context.ReadValue<Vector2>());
       aimPosition=context.ReadValue<Vector2>();  
    }
}
