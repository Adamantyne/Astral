using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint; 
    [SerializeField] private float speed = 4f;
    private int direction = 1;

    void Awake(){
        platform.position = startPoint.position;
    }
    void Update()
    {
        Vector2 target = CurrentMovementTarget();
        MovmentPlatform(target);
        DirectionController(target);
    }

    Vector2 CurrentMovementTarget(){
        if(direction==1){
            return startPoint.position;
        }
        return endPoint.position;
    }

    void MovmentPlatform(Vector2 target){
        platform.position = Vector2.MoveTowards(platform.position, target, speed*Time.deltaTime);
    }

    void DirectionController(Vector2 target){
        float distance = (target - (Vector2)platform.position).magnitude;
        if(distance<=0.1f){
            direction*=-1;
        }
    }
    
    private void OnDrawGizmos(){
        if(platform && startPoint && endPoint){
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }
}
