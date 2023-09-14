using static CustomInput;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "Input Channel", menuName = "Channels/Input Channel", order = 0)]
public class InputChannel : ScriptableObject, IPlayerActions, ICameraActions
{
    CustomInput customInput;
    private void OnEnable()
    {
        if(customInput == null)
        {
            customInput = new CustomInput();
            customInput.Player.SetCallbacks(this);
            customInput.Camera.SetCallbacks(this);
            customInput.Enable();
        }
    }


    public Action<Vector2> MoveEvent;
    public Action ZoomOutEvent;
    public Action ZoomInEvent;
    

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        ZoomOutEvent?.Invoke();
    }
    public void OnZoomIn(InputAction.CallbackContext context)
    {
        ZoomInEvent?.Invoke();
    }
}
