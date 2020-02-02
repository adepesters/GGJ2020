using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    Coroutine dialogIntroCoroutine;
    Coroutine dialogCoroutine;
    string[] line = new string[30];
    [SerializeField] GameObject dialogPanel;
    [SerializeField] GameObject dialogLineDisplay;

    [SerializeField] Sprite dialogBoyCalm;
    [SerializeField] Sprite dialogBoyAngry;
    [SerializeField] Sprite dialogGirlCalm;
    [SerializeField] Sprite dialogGirlAngry;

    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();

        line[0] = "Ce soir...je vais conquérir le monde !";
        line[1] = "Je vais gagner, sale caca boudin !";
        line[2] = "Même pas en rêve tronche de cake.";
        line[3] = "T’es trop moche c’est toi la plus nulle.";
        line[4] = "Mais toi t’as été adopté !!!";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.GameHasStarted)
        {
            if (dialogIntroCoroutine == null)
            {
                dialogIntroCoroutine = StartCoroutine(DialogIntro());
            }
        }
        else
        {
            dialogPanel.GetComponent<Image>().enabled = false;
        }

        //if (dialogCoroutine != null)
        //{
        //    StopCoroutine(dialogCoroutine);
        //}
        //if (dialogCoroutine == null)
        //{

        //    dialogCoroutine = StartCoroutine(ReadLine(line0, 0.02f));
        //}
    }

    IEnumerator DialogIntro()
    {
        yield return new WaitForSeconds(2.2f);
        dialogPanel.GetComponent<Image>().enabled = true;
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[0], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        StartCoroutine(ReadLine(line[1], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogGirlCalm;
        StartCoroutine(ReadLine(line[2], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(341, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        StartCoroutine(ReadLine(line[3], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogGirlAngry;
        StartCoroutine(ReadLine(line[4], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(341, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().enabled = false;
        dialogLineDisplay.GetComponent<Text>().text = "";
        gameSession.IntroDialogOver = true;
    }


    IEnumerator ReadLine(string _dialogLine, float _textDisplaySpeed)
    {

        //if (gameEventManager.CurrentNPCEmotion == "angry")
        //{
        //    SFXNPCAudioSource.PlayOneShot(voiceFragmentsMaxAngry[Random.Range(0, voiceFragmentsMaxAngry.Length)]);
        //}
        //if (gameEventManager.CurrentNPCEmotion == "neutral")
        //{
        //    SFXNPCAudioSource.PlayOneShot(voiceFragmentsMaxNeutral[Random.Range(0, voiceFragmentsMaxNeutral.Length)]);
        //}

        dialogLineDisplay.GetComponent<Text>().text = "";
        foreach (char caracter in _dialogLine)
        {
            dialogLineDisplay.GetComponent<Text>().text += caracter;
            yield return new WaitForSeconds(_textDisplaySpeed);
        }
        if (dialogLineDisplay.GetComponent<Text>().text == _dialogLine)
        {
            //yield return new WaitForSeconds(1f);
        }
    }
}
