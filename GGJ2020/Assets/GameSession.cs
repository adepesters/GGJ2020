using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    bool gameHasStarted;
    bool introDialogOver;

    int selectedLevel = -1;
    bool[] levelCanBeReached = new bool[10];

    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public bool IntroDialogOver { get => introDialogOver; set => introDialogOver = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }
    public bool[] LevelCanBeReached { get => levelCanBeReached; set => levelCanBeReached = value; }

    // Start is called before the first frame update
    void Start()
    {
        levelCanBeReached[0] = true;
        selectedLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(selectedLevel);
    }
}
