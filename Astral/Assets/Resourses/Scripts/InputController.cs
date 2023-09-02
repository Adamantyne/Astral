using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class InputController : MonoBehaviour
{
    private float moveX = 0;
    private float moveY = 0;
    private bool usingMobile = false;

    void Update()
    {
        SendInputs();
        KeyInputs();
    }

    public void KeyInputs()
    {
        if(usingMobile) return;
        float keyMoveX = Input.GetAxisRaw("Horizontal");
        float keyMoveY = Input.GetAxisRaw("Vertical");
        bool keyGravtyPress = Input.GetKeyDown("f");
        bool keyPausePress = Input.GetKeyDown(KeyCode.Escape);
        bool keyJumpPress = Input.GetButtonDown("Jump");
        bool LeftMouseDown = Input.GetMouseButtonDown(0);
        if(keyPausePress){
            SetPauseStatus();
        }
        if(keyGravtyPress){
            SetGravityStatus();
        }
        if(keyJumpPress){
            keyMoveY = 1;
        }
        PlayerController.PlayerInstance.InputActions(keyMoveX, keyMoveY);
    }

    public void HorizontalMove(int _direction)
    {
        moveX = _direction;
    }

    public void VerticalMove(int _direction)
    {
        moveY = _direction;
    }

    public void SetGravityStatus()
    {
        if(!PlayerController.PlayerInstance.GetIsAlive()) return;
        PlayerController.PlayerInstance.SetGravityStatus(!PlayerController.PlayerInstance.GetGravityStatus());
    }

    public void SetPauseStatus()
    {
        GameController.ControllerInstance.PauseGame();
    }

    private void SendInputs(){
        PlayerController.PlayerInstance.InputActions(moveX, moveY);
    }
}
