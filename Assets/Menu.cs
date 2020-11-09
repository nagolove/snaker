using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene[] scenes = SceneManager.GetAllScenes();
        foreach(Scene scn in scenes)
        {
            Debug.Log(string.Format("scene {0}", scn.name));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
