using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public int scene;

    public void OnPress()
    {
        SceneManager.LoadScene(scene);
    }
}
