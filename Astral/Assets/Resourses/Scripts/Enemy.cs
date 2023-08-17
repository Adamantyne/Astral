using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private int direction = 1;

    void Update()
    {
        if(Body.velocity.x == 0) {
            direction*=-1;
            Flip();
        }
        Move(direction);
    }
}
