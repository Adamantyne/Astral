using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header ("Valores:")]
    public GameObject player;
    public float smoothSpeed = 0.125f;

    [Header ("Elementos externos:")]
    public Vector3 offset;

    void FixedUpdate(){
        MoveCam();
    }

    private void MoveCam(){
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
