using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_InputField inputName;
    public static string inputNameText;

    private void Start()
    {

    }
    

    public void StartMenu()
    {
        inputNameText = inputName.text;
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadSceneAsync(1);
    }

    public void Exit()
    {

    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
        Application.Quit(); // original code to quit Unity player
    #endif
    }

}