using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Paper : MonoBehaviour
{
    public bool wrong;
    public bool fly;
    public bool oneTime;
    public bool ready;
    public bool made_it;

    [Space]
    public float death_time;
    public bool death_one;
    SpriteRenderer sp;

    [Space]
    public GameObject ready_alert;

    [Space]
    public bool dragged;

    Vector2 difference = Vector2.zero;
    public Vector3 startPos;
    public Vector3 endPos;
    float zed = 0;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        ready = false;
        ready_alert.SetActive(false);

        transform.position = startPos;
    }
    
    void Update()
    {
        if(GameController.gc.currentState == GameController.GAMESTATE.EXPLORE)
        {
            if (!made_it)
            {
                if (fly)
                    LetFly();

                if (GameController.gc.gameMODE != GameController.GAMEMODE.relax)
                {
                    death_time -= Time.deltaTime;

                    if (death_time < 3 && death_one)
                    {
                        StartCoroutine(ShowDeath());
                        death_one = false;
                    }

                    if (death_time < 0)
                        Death();

                }                

                if (dragged)
                {
                    if (Input.touchCount > 0)
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));

                        transform.position = new Vector3(pos.x, pos.y, zed) - (Vector3)difference;
                        
                        fly = false;
                    }                    
                }
            }
            else
            {
                LetFly();
            }
        } 
        else if(GameController.gc.currentState == GameController.GAMESTATE.PAUSE)
        {
            if (made_it)
            {
                LetFly();
            }              
        }
        else if (GameController.gc.currentState == GameController.GAMESTATE.END)
        {
            if (!made_it)
            {
                Destroy(gameObject);
            } 
            else
            {
                LetFly();
            }          
        }
    }

    void LetFly()
    {
        transform.position = Vector3.Lerp(transform.position, endPos, 5f * Time.deltaTime);
    }

    public void MouseDrag()
    {
        zed -= 0.1f;
        GetComponent<SortingGroup>().sortingOrder += 10;        
        dragged = true;

        if (Input.touchCount > 0)
        {            
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));
            difference = worldPoint - (Vector2)transform.position;
        }            
    }

    public void MouseStop()
    {
        dragged = false;        
    }

    public void ReadyToGo()
    {
        if (oneTime)
        {
            sp.enabled = true;
            StopAllCoroutines();
            ready = true;
            ready_alert.SetActive(true);
            oneTime = false;
            death_time += 10;
            AudioSFX.instance.PlaySingle(AudioSFX.instance.ready_paper_sound, 1f);
        }        
    }

    public IEnumerator ShowEnd()
    {
        made_it = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void Death()
    {
        GameController.gc.papersOnTable--;

        if (!wrong)
        {
            GameController.gc.DoDamge();              
        }            

        Destroy(gameObject);
    }

    public IEnumerator ShowDeath()
    {
        sp.enabled = false;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = true;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = false;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = true;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = false;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = true;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = false;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = true;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = false;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = true;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = false;
        yield return new WaitForSeconds(0.25f);
        sp.enabled = true;
        yield return new WaitForSeconds(0.25f);
    }
}
