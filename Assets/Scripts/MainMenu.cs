using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameData gData;
    public Transform[] levels;

    void Start()
    {
        SaveLoad.control.LoadGame();

        UIFader.instance.Fade(UIFader.FADE.FadeIn, 1f, 0f);

        for (int i = 0; i < levels.Length; i++)
        {
            if (gData.levels[i])
            {
                levels[i].GetComponent<Button>().interactable = true;
                levels[i].Find("Text").GetComponent<Text>().text = i.ToString();
                levels[i].Find("Image").gameObject.SetActive(false);                
            }
        }
    }

    public void LoadLevel(string level)
    {
        StartCoroutine(LetsLoad(level));
    }

    public IEnumerator LetsLoad(string level)
    {
        UIFader.instance.Fade(UIFader.FADE.FadeOut, 1f, 0f);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }

    public void ButtonSound()
    {
        AudioSFX.instance.PlaySingle(AudioSFX.instance.button_sound, 1f);
    }
}
