using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GameSession gameSession;

    [SerializeField] GameObject[] connectingLines;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameSession.SelectedLevel)
        {
            case 0:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 1 || i == 5)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[0].SetActive(false);
                connectingLines[1].SetActive(false);
                break;

            case 1:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 2 || i == 3)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[2].SetActive(false);
                connectingLines[3].SetActive(false);
                break;

            case 2:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 4)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[5].SetActive(false);
                break;

            case 3:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 4)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[4].SetActive(false);
                break;

            case 4:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 9)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[6].SetActive(false);
                break;

            case 5:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 6 || i == 7)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[7].SetActive(false);
                connectingLines[9].SetActive(false);
                break;

            case 6:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 8)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[10].SetActive(false);
                break;

            case 7:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 8)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[8].SetActive(false);
                break;

            case 8:
                for (int i = 0; i < 10; i++)
                {
                    if (i == 9)
                    {
                        gameSession.LevelCanBeReached[i] = true;
                    }
                    else
                    {
                        gameSession.LevelCanBeReached[i] = false;
                    }
                }
                connectingLines[11].SetActive(false);
                break;

            case 9:
                break;
        }
    }
}
