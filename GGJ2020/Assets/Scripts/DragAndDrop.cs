using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
    , IPointerClickHandler
, IDragHandler
//, IPointerEnterHandler
// , IPointerExitHandler

{
    [SerializeField] GameObject robotIcon;
    [SerializeField] Sprite robotSprite;
    GameObject newRobotIcon;
    bool isDragged = false;

    RobotSelectionManager robotSelectionManager;

    [SerializeField] GameObject panel1;
    [SerializeField] GameObject panel2;
    [SerializeField] GameObject panel3;

    // Start is called before the first frame update
    void Start()
    {
        robotSelectionManager = GameObject.FindWithTag("RobotSelectionManager").GetComponent<RobotSelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (newRobotIcon != null)
        {
            //Debug.Log("entered");
            newRobotIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0);

            newRobotIcon.GetComponent<RectTransform>().localPosition = new Vector3(
            newRobotIcon.GetComponent<RectTransform>().localPosition.x,
                newRobotIcon.GetComponent<RectTransform>().localPosition.y,
            0);
        }
    }

    private void OnMouseEnter()
    {
        //Debug.Log("ok");
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        //print("I was clicked");
        // GameObject newRobotIcon = Instantiate(robotIcon, Input.mousePosition, Quaternion.identity);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("I'm being dragged!");
        // GameObject newRobotIcon = Instantiate(robotIcon, Input.mousePosition, Quaternion.identity);
    }

    public void OnMouseDown()
    {
        newRobotIcon = Instantiate(robotIcon, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        //newRobotIcon.transform.parent = gameObject.transform.parent;
        newRobotIcon.transform.SetParent(transform.parent, true);
        newRobotIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        newRobotIcon.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newRobotIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newRobotIcon.GetComponent<Image>().sprite = robotSprite;

        //newRobotIcon.GetComponent<RectTransform>().rect = new Rect()
        isDragged = true;
    }

    public void OnMouseUp()
    {
        if (robotSelectionManager.TouchingPanel1)
        {
            Destroy(newRobotIcon.gameObject);
            panel1.GetComponent<Image>().sprite = robotSprite;
            panel1.GetComponent<Image>().color = Color.white;
        }

        if (robotSelectionManager.TouchingPanel2)
        {
            Destroy(newRobotIcon.gameObject);
            panel2.GetComponent<Image>().sprite = robotSprite;
            panel2.GetComponent<Image>().color = Color.white;
        }

        if (robotSelectionManager.TouchingPanel3)
        {
            Destroy(newRobotIcon.gameObject);
            panel3.GetComponent<Image>().sprite = robotSprite;
            panel3.GetComponent<Image>().color = Color.white;
        }
    }


}
