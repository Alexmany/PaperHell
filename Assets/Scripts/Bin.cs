using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    public float minTime;
    public float maxTime;
    public Vector3 startPos;
    public Vector3 endPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paper"))
        {
            StartCoroutine(GameController.gc.SpawnAnotherPaper(Random.Range(minTime, maxTime)));
            GameController.gc.papersOnTable--;
            collision.GetComponent<Paper>().transform.position = startPos;
            collision.GetComponent<Paper>().endPos = endPos;
            collision.GetComponent<Paper>().made_it = true;
            StartCoroutine(collision.GetComponent<Paper>().ShowEnd());
            AudioSFX.instance.PlaySingle(AudioSFX.instance.collect_paper_sound, 0.75f);

            if (!collision.GetComponent<Paper>().wrong)
                GameController.gc.DoDamge();

        }
    }
}
