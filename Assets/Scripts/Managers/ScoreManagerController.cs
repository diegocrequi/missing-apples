using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerController : MonoBehaviour
{
    int score;
    
    public void increaseScore(int points) 
    {
        score += points;
    }
}
