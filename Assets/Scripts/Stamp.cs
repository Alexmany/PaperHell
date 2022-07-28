using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    [Space]
    public bool dragged;

    Vector2 difference = Vector2.zero;

    void Update()
    {
        if (dragged)
        {
            if (Input.touchCount > 0)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));

                transform.position = pos - difference;                
            }
        }            
    }

    public void MouseDrag()
    {
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
}
