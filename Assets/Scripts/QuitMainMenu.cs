using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitMainMenu : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas HomePage;
    public void Quit()// on main menu screen
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");


    }
    public void OnLoadProgressClick()
    {
        MainMenu.gameObject.SetActive(false);
        HomePage.gameObject.SetActive(true);
    }
    void Start()
    {


    }
}
