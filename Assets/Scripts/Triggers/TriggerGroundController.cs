using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerGroundController : MonoBehaviour
{
    public UnityEvent onEnterEvent;
    public UnityEvent onExitEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            onEnterEvent.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            onExitEvent.Invoke();
        }
    }
}
