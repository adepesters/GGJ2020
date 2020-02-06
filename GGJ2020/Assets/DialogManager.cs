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

    [SerializeField] AudioClip[] voicesBoy;
    [SerializeField] AudioClip[] voicesGirl;

    bool canDisplayDialogs;

    public bool CanDisplayDialogs { get => canDisplayDialogs; set => canDisplayDialogs = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();

        dialogPanel.GetComponent<Image>().enabled = false;

        line[0] = "Ce soir...je vais conquérir le monde !";
        line[1] = "Même pas en rêve tronche de cake.";
        line[2] = "Si! Je vais gagner, sale caca boudin !";
        line[3] = "Et puis t’es trop moche c’est toi la plus nulle.";
        line[4] = "Mais toi t’as été adopté !!!";
        line[5] = "...";
        line[6] = "J'en ai marre, je commence! ";
        line[7] = "Attends que je prépare mes robots..";
        line[8] = "avec quelques gemmes rouges de puissance..";
        line[9] = "et quelques gemmes bleues de défense...";
        line[10] = "ah oui et puis des vertes et des oranges aussi..";
        line[11] = "je mets tout ça sur mon robot et elle aura aucune chance!!!!!";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.GameHasStarted && dialogIntroCoroutine == null && CanDisplayDialogs)
            {
            dialogIntroCoroutine = StartCoroutine(DialogIntro());
            }
        else if (!CanDisplayDialogs)
        {
            dialogPanel.GetComponent<Image>().enabled = false;
            dialogLineDisplay.GetComponent<Text>().text = "";
        }
        //else
        //{
        //    dialogPanel.GetComponent<Image>().enabled = false;
        //}

        //if (GameObject.Find("MainRoom") == null)
        //{
        //    //StopCoroutine(dialogCoroutine);
        //    //GetComponent<AudioSource>().Stop();
        //    canDisplayDialogs = false;
        //}

        //if (!canDisplayDialogs)
        //{
        //    if (dialogCoroutine != null) {
        //        StopCoroutine(dialogCoroutine);
        //    }
        //    dialogPanel.GetComponent<Image>().enabled = false;
        //    GetComponent<AudioSource>().Stop();
        //}

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
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogGirlCalm;
        StartCoroutine(ReadLine(line[1], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(341, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesGirl[Random.Range(0, voicesGirl.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        StartCoroutine(ReadLine(line[2], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        StartCoroutine(ReadLine(line[3], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogGirlAngry;
        StartCoroutine(ReadLine(line[4], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(341, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesGirl[Random.Range(0, voicesGirl.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyAngry;
        dialogCoroutine = StartCoroutine(ReadLine(line[5], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        //GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[6], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[7], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[8], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[9], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[10], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(3f);
        dialogPanel.GetComponent<Image>().sprite = dialogBoyCalm;
        dialogCoroutine = StartCoroutine(ReadLine(line[11], 0.02f));
        dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(480, dialogLineDisplay.GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<AudioSource>().PlayOneShot(voicesBoy[Random.Range(0, voicesBoy.Length)]);
        yield return new WaitForSeconds(5f);

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
