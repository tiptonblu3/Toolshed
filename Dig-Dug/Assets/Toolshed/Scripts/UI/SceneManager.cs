using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string Level1)
    {
        SceneManager.LoadScene(Level1);
    }
}
