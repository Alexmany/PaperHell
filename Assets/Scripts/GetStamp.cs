using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStamp : MonoBehaviour
{
    public Stamp _stamp;
    public bool stamp;
    public bool stamped;
    public Transform seal;

    Paper paper;

    private void Start()
    {
        paper = GetComponent<Paper>();
        seal.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(stamp  && !stamped)
        {
            if(_stamp.dragged == false)
            {
                GetStamped();
            }
        }
    }

    void GetStamped()
    {
        AudioSFX.instance.PlaySingle(AudioSFX.instance.stamp_sound, 0.5f);
        seal.position = _stamp.transform.position;
        seal.gameObject.SetActive(true);
        paper.ReadyToGo();
        stamped = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stamp") && !stamped)
        {
            _stamp = collision.GetComponent<Stamp>();

            if(_stamp.dragged)
                stamp = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Stamp") && !stamped)
        {
            _stamp = collision.GetComponent<Stamp>();

            if (_stamp.dragged)
                stamp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stamp"))
        {
            _stamp = null;
            stamp = false;
        }
    }
}
