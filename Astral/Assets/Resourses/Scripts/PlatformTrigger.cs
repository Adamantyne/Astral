using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            other.transform.SetParent(this.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            other.transform.SetParent(null);
        }
    }
}

// void OnCollisionEnter2D(Collision2D _other){
//         if(_other.gameObject.CompareTag("Player")){
//             _other.transform.SetParent(this.transform);
//         }
//     }
//     void OnCollisionExit2D(Collision2D _other){
//         if(_other.gameObject.CompareTag("Player")){
//             _other.transform.SetParent(null);
//         }
//     }
