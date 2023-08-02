using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenueInputBehavior : MonoBehaviour
{
    private UIActions _player1Controls;
    private UIActions _player2Controls;

    // Start is called before the first frame update
    void Awake()
    {
        _player1Controls.UIInputs.UIMovement.performed += UpdateHUDCursor;
    }

    private void UpdateHUDCursor(InputAction.CallbackContext context)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
