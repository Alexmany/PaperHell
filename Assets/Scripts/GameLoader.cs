using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{      
    public void ChooseLang(string lng)
    {
        PlayerPrefs.SetString("Lang", lng);

        if (File.Exists(Application.persistentDataPath + "/gamedata"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("lvl0");
        }
    }
}
