using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour, IUnityAdsListener
{
    #region singleton
    public static GameController gc;

    void Awake()
    {
        gc = this;
    }

    public GAMESTATE currentState;

    public enum GAMESTATE
    {
        START,
        EXPLORE,
        DIALOGUE,
        PAUSE,
        END,        
    }

    public GAMEMODE gameMODE;

    public enum GAMEMODE
    {
        normal,
        relax,
        survive,        
        tutorial,
    }
    #endregion

    [Space]
    public GameData gData;

    [Space]
    public bool start_dialog;
    public Dialog dialog;

    [Space]
    public AudioSource music;
    public AudioClip music_level;
    public AudioClip music_win;
    public AudioClip music_lose;

    [Space]
    public int day;
    public int reward;
    public string next_level;

    [Space]
    public bool coffee;    
    public bool finger;
    public bool healthpak;
    float coffee_timer = 20;
    float health_timer = 20;
    float finger_timer = 20;

    [Space]
    public Button coffee_button;
    public Button finger_button;
    public Button health_button;     

    [Space]
    public int health;
    public Text health_text;

    [Space]
    public float day_timer;
    float current_time;
    public Text text_timer_ui;

    [Space]
    public int goal_score;
    public int current_score;

    [Space]
    public GameObject menu;
    public GameObject win_menu;
    public GameObject lose_menu;
    public GameObject start_menu;
    public GameObject pause_button;
    public GameObject game_ui;
    public GameObject shop_ui;
    public GameObject ads_ui;
    public GameObject reward_ui;

    [Space]
    public int papersOnTable;
    public int max_papers_on_table;

    [Space]
    public float min_spawn_rate;
    public float max_spawn_rate;

    [Space]
    public float stop_time;
    public GameObject[] papers;
    float spawn_timer;
    int p_index;

    [Space]
    public float high_score;
    
    [HideInInspector]
    public bool drawing;
    [HideInInspector]
    public TestDraw brush;
    [HideInInspector]
    public bool dragged;
    [HideInInspector]
    public Paper currentPaper;
    [HideInInspector]
    public bool stamped;
    [HideInInspector]
    public Stamp currentStamp;
    [HideInInspector]
    public bool touch_me = false;

    Scene thisScene;
    string store_id = "3900803";
    string reward_id = "rewardedVideo";    

    void Start()
    {
        SaveLoad.control.LoadGame();

        UIFader.instance.Fade(UIFader.FADE.FadeIn, 1f, 0f);
        music.clip = music_level;

        if (start_dialog)
        {
            currentState = GAMESTATE.DIALOGUE;
            DialogManager.dm.currentDialog = dialog;
            DialogManager.dm.StartDialogue();
            start_menu.SetActive(false);
        } 
        else
        {
            StartLevel();
        }

        if (gameMODE == GAMEMODE.tutorial)
        {
            current_time = day_timer;
        }

        thisScene = SceneManager.GetActiveScene();
        AnalyticsEvent.LevelStart(thisScene.name, thisScene.buildIndex);

        Advertisement.AddListener(this);
        Advertisement.Initialize(store_id, false); 
    }

    public void StartLevel() 
    {
        if (gameMODE == GAMEMODE.relax)
        {
            start_menu.transform.Find("DayText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("zen_button");
            start_menu.transform.Find("addText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("zen_button2");
        }

        if (gameMODE == GAMEMODE.survive)
        {            
            start_menu.transform.Find("DayText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("survive_button");
            
            string minutes = Mathf.Floor(gData.highscore / 60).ToString("00");
            string seconds = (gData.highscore % 60).ToString("00");

            start_menu.transform.Find("addText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("highscore") + minutes + "-" + seconds;
        }

        if (gameMODE == GAMEMODE.normal)
        {
            start_menu.transform.Find("DayText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("day_text") + day.ToString();
        }

        if(gameMODE != GAMEMODE.tutorial)
        {
            currentState = GAMESTATE.START;
            start_menu.SetActive(true);
            pause_button.SetActive(false);
            game_ui.SetActive(false);            
        }       
    }

    public void StartGame()
    {
        start_menu.SetActive(false);
        pause_button.SetActive(true);
        game_ui.SetActive(true);
        SetupButtons();
        music.Play();

        currentState = GAMESTATE.EXPLORE;
        current_time = day_timer;
        spawn_timer = 3;
        p_index = 0;

        SpawnPaper();
        StartCoroutine(SpawnAnotherPaper(8f));

        coffee_button.transform.Find("Text").GetComponent<Text>().text = gData.coffee.ToString();
    }
    
    void Update()
    {
        if(currentState == GAMESTATE.EXPLORE)
        {
            TouchStuff();

            if (gameMODE == GAMEMODE.normal)
                NormalMode();

            if (gameMODE == GAMEMODE.relax)
                RelaxMode();

            if (gameMODE == GAMEMODE.survive)
                SurviveMode();

            if (coffee)
            {
                coffee_button.transform.Find("Fill").GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, 20, coffee_timer);
                
                coffee_timer -= Time.deltaTime;

                if (coffee_timer <= 0)
                {
                    coffee_timer = 0;
                    coffee = false;
                    coffee_button.interactable = true;
                    coffee_button.transform.Find("Fill").gameObject.SetActive(false);
                    SetupButtons();
                }
            }

            if (healthpak)
            {
                health_button.transform.Find("Fill").GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, 20, health_timer);

                health_timer -= Time.deltaTime;

                if (health_timer <= 0)
                {
                    health_timer = 0;
                    healthpak = false;
                    health_button.interactable = true;
                    health_button.transform.Find("Fill").gameObject.SetActive(false);
                    SetupButtons();
                }
            }

            if (finger)
            {
                finger_button.transform.Find("Fill").GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, 20, finger_timer);

                finger_timer -= Time.deltaTime;

                if (finger_timer <= 0)
                {
                    finger_timer = 0;
                    finger = false;
                    finger_button.interactable = true;
                    finger_button.transform.Find("Fill").gameObject.SetActive(false);
                    SetupButtons();
                }
            }
        }        
    }

    void TouchStuff()
    {
        if (Input.touchCount > 0 && !touch_me)
        {
            touch_me = true;
            Touch touch = Input.GetTouch(0);
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Stamp"))
                {
                    hit.collider.GetComponent<Stamp>().MouseDrag();
                    currentStamp = hit.collider.GetComponent<Stamp>();
                    stamped = true;
                }

                if (hit.collider.CompareTag("Paper"))
                {
                    hit.collider.GetComponent<Paper>().MouseDrag();
                    currentPaper = hit.collider.GetComponent<Paper>();
                    dragged = true;
                    AudioSFX.instance.PlaySingle(AudioSFX.instance.grab_paper_sound, 1f);

                    if (finger)
                        currentPaper.ReadyToGo();

                }

                if (hit.collider.CompareTag("Brush"))
                {
                    hit.collider.GetComponent<TestDraw>().letsDraw = true;
                    brush = hit.collider.GetComponent<TestDraw>();
                    drawing = true;
                    AudioSFX.instance.pencilSound.Play();
                }

                if (hit.collider.CompareTag("Check"))
                {
                    hit.collider.GetComponent<CheckMark>().CheckMate();
                }
            }
        }

        if (Input.touchCount == 0)
        {
            touch_me = false;

            if (dragged)
            {
                currentPaper.MouseStop();
                currentPaper = null;
                dragged = false;
            }

            if (stamped)
            {
                currentStamp.MouseStop();
                currentStamp = null;
                stamped = false;
            }

            if (drawing)
            {
                brush.letsDraw = false;
                brush = null;
                drawing = false;
                AudioSFX.instance.pencilSound.Stop();
            }
        }
    }

    void NormalMode()
    {
        if (health <= 0)
            LoseLvl();        

        if (current_time >= 0)
        {
            current_time -= Time.deltaTime;            

            spawn_timer -= Time.deltaTime;

            if (spawn_timer <= 0 && !coffee)
            {
                SpawnPaper();
                spawn_timer = Random.Range(min_spawn_rate, max_spawn_rate);
            }

            string minutes = Mathf.Floor(current_time / 60).ToString("00");
            string seconds = (current_time % 60).ToString("00");

            text_timer_ui.text = minutes + "-" + seconds;
        }

        if (current_time <= 0)
        {
            current_time = 0;

            if (papersOnTable == 0)
            {
                WinLvl();
            }

            text_timer_ui.text = "00-00";
        }

        health_text.text = health.ToString();
    }

    void RelaxMode()
    {
        spawn_timer -= Time.deltaTime;

        if (spawn_timer <= 0 &&!coffee)
        {
            SpawnPaper();
            spawn_timer = Random.Range(min_spawn_rate, max_spawn_rate);
        }
    }

    void SurviveMode()
    {
        if (current_time > 60)
            max_papers_on_table = 6;

        health_text.text = health.ToString();

        current_time += Time.deltaTime;

        spawn_timer -= Time.deltaTime;

        string minutes = Mathf.Floor(current_time / 60).ToString("00");
        string seconds = (current_time % 60).ToString("00");

        text_timer_ui.text = minutes +"-"+ seconds;

        if (spawn_timer <= 0 && !coffee)
        {
            SpawnPaper();
            spawn_timer = Random.Range(min_spawn_rate, max_spawn_rate);
        }

        if (health <= 0)
            LoseLvl();
    }

    public IEnumerator SpawnAnotherPaper(float t)
    {
        yield return new WaitForSeconds(t);

        if(!coffee)
            SpawnPaper();
    }

    public void SpawnPaper()
    {
        if(current_time > stop_time)
        {
            if (papersOnTable < max_papers_on_table)
            {
                AudioSFX.instance.PlaySingle(AudioSFX.instance.spawn_paper_sound, 1f);
                GameObject go = Instantiate(papers[p_index]);
                go.GetComponent<Paper>().startPos = new Vector3(Random.Range(-15, 15), 15, 0);
                go.GetComponent<Paper>().endPos = new Vector3(Random.Range(-9, 9), Random.Range(2, 0), 0);
                papersOnTable++;
                p_index++;

                if (p_index >= papers.Length)
                    p_index = 0;
            }
        }               
    }

    public void PauseGame()
    {
        currentState = GAMESTATE.PAUSE;
        menu.SetActive(true);
        game_ui.SetActive(false);
        music.Pause();
    }

    public void ResumeGame()
    {
        currentState = GAMESTATE.EXPLORE;
        music.Play();
        menu.SetActive(false);

        if(gameMODE != GAMEMODE.relax)
            game_ui.SetActive(true);

        SetupButtons();
    }

    public void LoadLevel(string level)
    {
        StartCoroutine(LoadNextLevelAnim(level));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadNextLevelAnim(SceneManager.GetActiveScene().name));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelAnim(next_level));
    }

    public IEnumerator LoadNextLevelAnim(string level)
    {
        UIFader.instance.Fade(UIFader.FADE.FadeOut, 1f, 0f);
        win_menu.SetActive(false);
        start_menu.SetActive(false);
        lose_menu.SetActive(false);
        menu.SetActive(false);
        reward_ui.SetActive(false);
        ads_ui.SetActive(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }

    public void WinLvl()
    {
        currentState = GAMESTATE.END;

        music.Stop();
        music.loop = false;
        music.clip = music_win;
        music.Play();

        if (gameMODE == GAMEMODE.normal)
        {
            win_menu.transform.Find("ScoreText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("reward1") + reward.ToString() + LocalizationManager.instance.GetLocalizedValue("reward2");
        }

        gData.coins += reward;

        if(day < 30)
            gData.levels[day + 1] = true;           

        pause_button.SetActive(false);
        game_ui.SetActive(false);
        win_menu.SetActive(true);

        SaveLoad.control.SaveGame();

        AnalyticsEvent.LevelComplete(thisScene.name, thisScene.buildIndex);
    }

    void LoseLvl()
    {
        currentState = GAMESTATE.END;

        music.Stop();
        music.loop = false;
        music.clip = music_lose;
        music.Play();


        if (gameMODE == GAMEMODE.normal)
        {
            AnalyticsEvent.LevelFail(thisScene.name, thisScene.buildIndex);
            lose_menu.transform.Find("LoseText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("day_lose");
            lose_menu.transform.Find("ScoreText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("day_lose2");
        }

        if (gameMODE == GAMEMODE.survive)
        {
            AnalyticsEvent.Custom("Survive Fail");

            if (current_time > gData.highscore)
                gData.highscore = current_time;

            string minutes = Mathf.Floor(gData.highscore / 60).ToString("00");
            string seconds = (gData.highscore % 60).ToString("00");

            string minutes2 = Mathf.Floor(current_time / 60).ToString("00");
            string seconds2 = (current_time % 60).ToString("00");

            lose_menu.transform.Find("LoseText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("highscore2") + minutes2 + "-" + seconds2;
            lose_menu.transform.Find("ScoreText").GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedValue("highscore") + minutes + "-" + seconds;

            SaveLoad.control.SaveGame();
        }

        pause_button.SetActive(false);
        game_ui.SetActive(false);
        lose_menu.SetActive(true);

        AnalyticsEvent.LevelFail(thisScene.name, thisScene.buildIndex);
    }

    public void ButtonSound()
    {
        AudioSFX.instance.PlaySingle(AudioSFX.instance.button_sound, 1f);
    }

    public void CoffeeButton()
    {
        if(gData.coffee > 0 && !coffee)
        {
            coffee_timer = 20f;
            coffee = true;
            gData.coffee--;
            coffee_button.interactable = false;
            coffee_button.transform.Find("Text").GetComponent<Text>().text = gData.coffee.ToString();
            coffee_button.transform.Find("Fill").gameObject.SetActive(true);            
            AudioSFX.instance.PlaySingle(AudioSFX.instance.button_sound, 1f);

            AnalyticsEvent.Custom("Coffee used");
        }        
    }

    public void HealthButton()
    {
        if (gData.healthpak > 0 && !healthpak)
        {
            health_timer = 20f;
            healthpak = true;
            health++;
            gData.healthpak--;
            health_button.interactable = false;
            health_button.transform.Find("Text").GetComponent<Text>().text = gData.coffee.ToString();
            health_button.transform.Find("Fill").gameObject.SetActive(true);
            StartCoroutine(UpdateHealth(Color.green));            
            AudioSFX.instance.PlaySingle(AudioSFX.instance.button_sound, 1f);

            AnalyticsEvent.Custom("Health used");
        }        
    }

    public void FingerButton()
    {
        if (gData.finger > 0 && !finger)
        {
            finger_timer = 20f;
            finger = true;            
            gData.finger--;
            finger_button.interactable = false;
            finger_button.transform.Find("Text").GetComponent<Text>().text = gData.coffee.ToString();
            finger_button.transform.Find("Fill").gameObject.SetActive(true);            
            AudioSFX.instance.PlaySingle(AudioSFX.instance.button_sound, 1f);

            AnalyticsEvent.Custom("Finger used");
        }        
    }

    void SetupButtons()
    {
        if(gData.coffee > 0)
        {
            coffee_button.transform.Find("Text").gameObject.SetActive(true);
            coffee_button.transform.Find("Text").GetComponent<Text>().text = gData.coffee.ToString();            
        } 
        else
        {
            coffee_button.gameObject.SetActive(false);
        }

        if (gData.healthpak > 0)
        {
            health_button.transform.Find("Text").gameObject.SetActive(true);
            health_button.transform.Find("Text").GetComponent<Text>().text = gData.healthpak.ToString();            
        }
        else
        {
            health_button.gameObject.SetActive(false);
        }

        if (gData.finger > 0)
        {
            finger_button.transform.Find("Text").gameObject.SetActive(true);
            finger_button.transform.Find("Text").GetComponent<Text>().text = gData.finger.ToString();            
        }
        else
        {
            finger_button.gameObject.SetActive(false);
        }
    }

    public IEnumerator UpdateHealth(Color color)
    {
        health_text.color = color;
        yield return new WaitForSeconds(0.5f);

        health_text.color = Color.white;
    }

    public void DoDamge()
    {
        health--;
        AudioSFX.instance.PlaySingle(AudioSFX.instance.damage_sound, 1f);
        StartCoroutine(UpdateHealth(Color.red));
    }

    public void lastlevel()
    {
        if(gData.game_ended)
            StartCoroutine(LoadNextLevelAnim("MainMenu"));
        else
            StartCoroutine(LoadNextLevelAnim("end"));
    }

    public void checkAds()
    {
        if (Advertisement.IsReady(reward_id))
        {
            ads_ui.SetActive(true);
            win_menu.SetActive(false);
            lose_menu.SetActive(false);
        }
        else
        {
            reward_ui.SetActive(false);
            ads_ui.SetActive(false);
            StartCoroutine(LoadNextLevelAnim(next_level));
        }
    }   

    public void ShowAd()
    {
        Advertisement.Show(reward_id);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {        
        if (showResult == ShowResult.Finished)
        {
            reward_ui.SetActive(true);
            ads_ui.SetActive(false);
            gData.coins += 2500;
            SaveLoad.control.SaveGame();
        }
        else if (showResult == ShowResult.Skipped)
        {            
            StartCoroutine(LoadNextLevelAnim(next_level));
        }
        else if (showResult == ShowResult.Failed)
        {            
            StartCoroutine(LoadNextLevelAnim(next_level));
        }        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
