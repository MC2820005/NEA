using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitMainMenu : MonoBehaviour
{
    public void Quit()// on main menu screen
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");


    }
    void Start()
    {


    }
}
