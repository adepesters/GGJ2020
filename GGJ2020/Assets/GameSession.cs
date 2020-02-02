using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    bool gameHasStarted;
    bool introDialogOver;

    int selectedLevel;

    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public bool IntroDialogOver { get => introDialogOver; set => introDialogOver = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
