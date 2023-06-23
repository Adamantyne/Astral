using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController AudioControllerInstance;
    public AudioSource playerJump;
    public AudioSource playerDead;
    public AudioSource playerGravity;

    void Awake(){
        AudioControllerInstance = this;
    }

    public void PlayAudio(string _audio)
    {
        if (_audio == "playerJump") playerJump.Play();
        else if (_audio == "playerDead") playerDead.Play();
        else if (_audio == "playerGravity") playerGravity.Play();
    } 

    public void PauseAudio(string _audio)
    {
        if (_audio == "playerJump") playerJump.Pause();
        else if (_audio == "playerDead") playerDead.Pause();
        else if (_audio == "playerGravity") playerGravity.Pause();
    } 
}
