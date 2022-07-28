using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public float minTime;
    public float maxTime;
    public Vector3 startPos;
    public Vector3 endPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paper"))
        {
            if (collision.GetComponent<Paper>().ready && !collision.GetComponent<Paper>().made_it)
            {
                GameController.gc.papersOnTable--;
                StartCoroutine(GameController.gc.SpawnAnotherPaper(Random.Range(minTime, maxTime)));
                collision.GetComponent<Paper>().transform.position = startPos;
                collision.GetComponent<Paper>().endPos = endPos;
                collision.GetComponent<Paper>().made_it = true;
                StartCoroutine(collision.GetComponent<Paper>().ShowEnd());
                AudioSFX.instance.PlaySingle(AudioSFX.instance.collect_paper_sound, 0.75f);

                if (collision.GetComponent<Paper>().wrong)
                    GameController.gc.DoDamge();
            }
        }
    }
}
