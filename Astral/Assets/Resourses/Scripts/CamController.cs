using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header ("Valores:")]
    public GameObject player;
    public float smoothSpeed = 0.125f;
    public static CamController CamInstance;

    [Header ("Elementos externos:")]
    public Vector3 offset;

    void Awake(){
        CamInstance = this;
    }

    void FixedUpdate(){
        MoveCam();
    }

    private void MoveCam(){
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        //ParallaxController.Parallaxinstance.MoveScenery(transform.position);
    }
}
