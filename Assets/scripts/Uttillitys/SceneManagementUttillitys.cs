using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneManagementUttillitys : MonoBehaviour
{
    public static bool SceneNameContaints(Scene scene, string name)
    {
        int chatCount = name.Length;
        string nameOfScene = string.Empty; 
        for (int i = 0; i < chatCount; ++i)
        {
            nameOfScene += scene.name[i]; 
        }

        return name == nameOfScene; 
    }

    public static bool CompareScene(Scene a, Scene b)
    {
        return a == b; 
    }
}
