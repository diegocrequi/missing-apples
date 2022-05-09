using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerController : MonoBehaviour
{
    public int numberOfWorlds;
    public int numberOfLevelsPerWorld;
    LinkedListNode<string> currentLevel;
    string currentWorld;
    Dictionary<string, LinkedList<string>> levels;
    
    void Start()
    {
        if (levels == null)
        {
            levels = new Dictionary<string, LinkedList<string>>();
            createLevels();
            currentLevel = levels["1"].First;
            currentWorld = "1";
        }
    }

    void createLevels()
    {
        for(int worldNum = 1; worldNum <= numberOfWorlds; worldNum++)
        {
            LinkedList<string> world = new LinkedList<string>();
            currentLevel = new LinkedListNode<string>("1");
            world.AddFirst(currentLevel);
            for (int levelNum = 2; levelNum <= numberOfLevelsPerWorld; levelNum++)
            {
                LinkedListNode<string> level = new LinkedListNode<string>(levelNum.ToString());
                world.AddAfter(currentLevel, level);
                currentLevel = level;
            }
            levels[worldNum.ToString()] = world;
        }
    }

    public string getNextLevel()
    {
        LinkedListNode<string> nextLevel = currentLevel.Next;
        if(nextLevel != null)
        {
            currentLevel = nextLevel;
        } else
        {
            currentWorld = (int.Parse(currentWorld) + 1).ToString();
            if (int.Parse(currentWorld) > numberOfWorlds)
            {
                currentWorld = "1";
            }
            currentLevel = levels[currentWorld].First;
        }
        return currentWorld + "-" + currentLevel.Value;
    }
}
