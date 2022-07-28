using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int tut_int;

    [Space]
    public Transform hand1;
    public GameObject tut_text1;

    [Space]
    public Transform hand2;
    public GameObject tut_text2;
    public Vector3 endPos;

    [Space]
    public GameObject tut_text3;

    [Space]
    public Paper paper;

    public bool spawn_paper = true;   

    void Start()
    {
        hand1.gameObject.SetActive(true);
        tut_text1.SetActive(true);
        hand2.gameObject.SetActive(false);
        tut_text2.SetActive(false);
        tut_text3.SetActive(false);
    }
    
    void Update()
    {
        if (paper.ready && paper != null)
        {
            hand1.gameObject.SetActive(false);
            hand2.gameObject.SetActive(true);
            tut_text2.SetActive(true);
            tut_text1.SetActive(false);
            LetFly();
        }        

        if(paper == null && spawn_paper)
        {
            GameController.gc.SpawnPaper();                        

            tut_text1.SetActive(false);
            hand2.gameObject.SetActive(false);
            tut_text2.SetActive(false);
            tut_text3.SetActive(true);

            spawn_paper = false;
            tut_int = 1;
        }        

        if (tut_int == 1 && GameController.gc.papersOnTable == 0)
        {
            GameController.gc.WinLvl();
            tut_text3.SetActive(false);
            tut_int = 2;
        }
    }

    void LetFly()
    {
        hand2.transform.position = Vector3.Lerp(hand2.transform.position, endPos, 1f * Time.deltaTime);

        if(hand2.transform.position.x > endPos.x - 1)
        {
            hand2.transform.position = paper.transform.position;
        }
    }
}
