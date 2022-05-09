using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerCollider")
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManagerController>().nextLevel();
        }
    }
}
