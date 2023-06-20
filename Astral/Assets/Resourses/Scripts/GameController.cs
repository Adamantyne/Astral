using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void QuitGame(){
        Application.Quit();
    }

    public void LoadScennes(string _Scene){
        SceneManager.LoadScene(_Scene);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
