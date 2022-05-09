using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    [SerializeField]
    LevelManagerController levelManagerController;
    Coroutine coroutine;

    public void resetLevel(float length)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(resetLevelCoroutine(length));
        }
    }

    IEnumerator resetLevelCoroutine(float length)
    {
        yield return new WaitForSeconds(length);
        coroutine = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void nextLevel()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        string nextLevel = levelManagerController.getNextLevel();
        Debug.Log("NEXT LEVEL: " + nextLevel);
        SceneManager.LoadScene(nextLevel);
    }
}
