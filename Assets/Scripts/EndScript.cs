using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class EndScript : MonoBehaviour
{
    public GameObject menu;
    public GameData gData;

    void Start()
    {
        SaveLoad.control.LoadGame();
        UIFader.instance.Fade(UIFader.FADE.FadeIn, 1f, 0f);
        gData.game_ended = true;
        gData.coins += 2500;
        AnalyticsEvent.Custom("End Game");

        SaveLoad.control.SaveGame();
    }    

    public void LoadLevel(string level)
    {
        SaveLoad.control.SaveGame();
        StartCoroutine(LoadNextLevelAnim(level));
    }

    public IEnumerator LoadNextLevelAnim(string level)
    {
        UIFader.instance.Fade(UIFader.FADE.FadeOut, 1f, 0f);
        menu.SetActive(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }
}
