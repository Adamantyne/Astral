using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private int direction = 1;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    void Update()
    {
        float target = CurrentMovementTarget();
        MovmentPlatform(target);
        DirectionController(target);
    }

    float CurrentMovementTarget(){
        if(direction==-1){
            return startPoint.position.x;
        }
        return endPoint.position.x;
    }

    void MovmentPlatform(float targetX){
        Vector2 _target = new Vector2(targetX, Body.position.y);
        Body.position = Vector2.MoveTowards(Body.position, _target, moveSpeed*Time.deltaTime);
    }

    void DirectionController(float targetX){
        Vector2 _target = new Vector2(targetX, Body.position.y);
        float distance = (_target - (Vector2)Body.position).magnitude;
        if(distance<=0.1f){
            direction*=-1;
            Flip();
        }
    }

    
}
