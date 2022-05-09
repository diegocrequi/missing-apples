using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.tag == "PlayerCollider")
        {
            gameObject = GameObject.FindWithTag("Player");
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            gameObject.GetComponent<Animator>().SetBool("isDeath", true);
            GameObject.FindWithTag("GameController").GetComponent<GameManagerController>().resetLevel(0.3f);
        }
    }
}
