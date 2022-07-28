using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager dm;

    [Space]    
    public Text dialogText;
    public Image pic;
    public GameObject Dialog_ui;

    [Space]       
    public Dialog currentDialog;
    public AudioClip sfx;

    public int index;
    bool started = false;    

    void Awake()
    {
        dm = this;
    }

    void Update()
    {
        if (started && GameController.gc.currentState == GameController.GAMESTATE.DIALOGUE)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (currentDialog.sentences.Length - 1 > index)
                {
                    index++;
                    NextSentence();
                } 
                else
                {
                    StartCoroutine(uiAnimationEnd());
                }                                    
            }              
        }
    }

    public void StartDialogue()
    {
        GameController.gc.currentState = GameController.GAMESTATE.DIALOGUE;

        Dialog_ui.SetActive(true);        

        index = 0;

        NextSentence();
        StartCoroutine(uiAnimationStart());
    }

    void NextSentence()
    {
        pic.sprite = currentDialog.sentences[index].pic;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(LocalizationManager.instance.GetLocalizedValue(currentDialog.sentences[index].text)));                
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        //AudioSFX.instance.PlaySingle(sfx);

        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(0.010f);

            dialogText.text += letter;

            yield return null;
        }
    }

    IEnumerator uiAnimationStart()
    {
        yield return new WaitForSeconds(0.5f);
        started = true;
    }

    IEnumerator uiAnimationEnd()
    {        
        Dialog_ui.SetActive(false);
        GameController.gc.StartLevel();
        yield return new WaitForSeconds(0.1f);                               
    }
}
