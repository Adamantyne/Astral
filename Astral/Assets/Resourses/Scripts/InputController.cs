using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        Inputs();
    }

    public void Inputs ()
    {
        float MoveX = Input.GetAxisRaw ("Horizontal");       
        float MoveY = Input.GetAxisRaw ("Vertical");
        bool WPress = Input.GetKeyDown ("w");
        bool FPress = Input.GetKeyDown ("f");
        bool JumpPress = Input.GetButtonDown("Jump");
        bool LeftMouseDown = Input.GetMouseButtonDown (0);
        bool pause = Input.GetKeyDown(KeyCode.Escape);
        PlayerController.PlayerInstance.InputActions(MoveX, MoveY, FPress, WPress, JumpPress);
        if(pause){
            GameController.ControllerInstance.PauseGame();
        }
    }
}
