using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController ControllerInstance;
    public GameObject PauseMenu;

    public void Awake(){
        ControllerInstance = this;
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void LoadScennes(string _Scene){
        SceneManager.LoadScene(_Scene);
    }
    public void PauseGame(){
        if(Time.timeScale==0f){
            Time.timeScale=1f;
        }else{
            Time.timeScale=0f;
        }
        PauseMenuController(Time.timeScale==0f);
    }

    void PauseMenuController(bool _status){
        PauseMenu.SetActive(_status);
    }
}
