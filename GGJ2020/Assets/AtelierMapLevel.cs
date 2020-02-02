using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtelierMapLevel : MonoBehaviour
{
    GameSession gameSession;
    [SerializeField] int levelID;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        gameSession.SelectedLevel = levelID;
    }
}
