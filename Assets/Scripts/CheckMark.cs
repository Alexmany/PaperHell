using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMark : MonoBehaviour
{
    public Paper paper;
    public GameObject check;
    public bool firstCheck;

    private void Start()
    {
        firstCheck = true;
        check.SetActive(false);
    }

    public void CheckMate()
    {
        if (firstCheck)
        {
            paper.ReadyToGo();
            check.SetActive(true);
            firstCheck = false;
        }
    }
}
