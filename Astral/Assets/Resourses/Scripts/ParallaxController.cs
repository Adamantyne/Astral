using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    // public static ParallaxController Parallaxinstance;
    // public int smoothSpeed;

    // void Awake(){
    //     Parallaxinstance = this;
    // }
    // public void MoveScenery(Vector3 _cenaryposition){
    //     Vector3 desiredPosition = _cenaryposition*-1;
    //     Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    //     transform.position = smoothedPosition;
    // }
    public float parallaxEffect;
    public Transform Cam;

    private Vector3 startPosition;
    //private Vector3 size;

    void Awake(){
        startPosition = transform.position;
        //size = GetComponent<BoxCollider2D>().size;
    }

    void Update(){
        Vector3 distance = Cam.transform.position*parallaxEffect + new Vector3(0,0,10);
        transform.position = startPosition+ distance;
    }
}
