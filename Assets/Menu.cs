using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scn = SceneManager.GetSceneAt(i);
            Debug.Log(string.Format("scene {0}", scn.name));
        }
    }

    public void ByeBye()
    {
        Application.Quit();
    }

    public void Study()
    {
        SceneManager.LoadScene("Main");
    }

    public void ViewStudied()
    {
        SceneManager.LoadScene("SearchDishExample");
    }
}
