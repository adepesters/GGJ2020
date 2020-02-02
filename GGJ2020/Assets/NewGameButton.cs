using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] GameObject roomPanel;
    [SerializeField] Image roomLightsOff;
    [SerializeField] Image roomLightsOn;
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject newGamePanel;
    [SerializeField] GameObject mapContainer;
    [SerializeField] AudioClip neonSound;

    GameSession gameSession;

    [SerializeField] AnimationCurve myAnimationCurve;
    private float timer = 0f;
    private bool animationRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animationRunning)
        {
            timer += Time.deltaTime;
        }
        roomLightsOn.color = new Color(1, 1, 1, myAnimationCurve.Evaluate(timer));
    }

    private void OnMouseDown()
    {
        //roomPanel.GetComponent<Image>().sprite = roomLightsOn;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        newGamePanel.GetComponent<Text>().fontSize += 20;
        yield return new WaitForSeconds(0.1f);
        newGamePanel.GetComponent<Text>().fontSize -= 20;
        yield return new WaitForSeconds(0.3f);

        titlePanel.GetComponent<Text>().enabled = false;
        newGamePanel.GetComponent<Text>().enabled = false;
        gameSession.GameHasStarted = true;
        mapContainer.SetActive(true);
        animationRunning = true;
        GetComponent<AudioSource>().PlayOneShot(neonSound);
    }



}
