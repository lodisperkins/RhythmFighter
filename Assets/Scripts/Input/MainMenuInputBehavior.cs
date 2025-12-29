using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuInputBehavior : MonoBehaviour
{

    private PlayerActions _menuInputs;

    private string[] _buttonArray = new string[3];
    private int _arrayIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _menuInputs = new PlayerActions();

        _menuInputs.MainMenu.MenuMoveUp.performed += MenuUP;
        _menuInputs.MainMenu.MenuMoveDown.performed += MenuDown;
        _menuInputs.MainMenu.MenuSelect.performed += MenuSelect;
    }

    private void MenuUP(InputAction.CallbackContext context)
    {
        //deslect the start button
        //_buttonArray[_arrayIndex].unhovered;

        if (_arrayIndex == 0)
            _arrayIndex = _buttonArray.Length;
        else
            _arrayIndex--;

        //select the settings button
        //_buttonArray[_arrayIndex].hovered;
    }

    private void MenuDown(InputAction.CallbackContext context)
    {
        //deslect the start button
        //_buttonArray[_arrayIndex].unhovered;

        if (_arrayIndex == _buttonArray.Length)
            _arrayIndex = 0;
        else
            _arrayIndex++;

        //select the settings button
        //_buttonArray[_arrayIndex].hovered;
    }

    private void MenuSelect(InputAction.CallbackContext context)
    {
        //_buttonArray[_arrayIndex].selected;

        //if(_arrayIndex = 0)
            
        //if (_arrayIndex = 1)
            
        //if (_arrayIndex = 2)
            
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
