using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropCrystal : MonoBehaviour
    , IPointerClickHandler
, IDragHandler
//, IPointerEnterHandler
// , IPointerExitHandler

{
    [SerializeField] GameObject crystalIcon;
    [SerializeField] Sprite crystalSprite;
    GameObject newCrystalIcon;
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
        if (newCrystalIcon != null)
        {
            //Debug.Log("entered");
            newCrystalIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0);

            newCrystalIcon.GetComponent<RectTransform>().localPosition = new Vector3(
            newCrystalIcon.GetComponent<RectTransform>().localPosition.x,
                newCrystalIcon.GetComponent<RectTransform>().localPosition.y,
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
        newCrystalIcon = Instantiate(crystalIcon, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        //newRobotIcon.transform.parent = gameObject.transform.parent;
        newCrystalIcon.transform.SetParent(transform.parent, true);
        newCrystalIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        newCrystalIcon.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newCrystalIcon.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newCrystalIcon.GetComponent<Image>().sprite = crystalSprite;

        //newRobotIcon.GetComponent<RectTransform>().rect = new Rect()
        isDragged = true;
    }

    public void OnMouseUp()
    {
        if (robotSelectionManager.TouchingPanel1Crystal)
        {
            Destroy(newCrystalIcon.gameObject);
            //panel1.GetComponent<Image>().sprite = crystalSprite;
            //panel1.GetComponent<Image>().color = Color.white;
        }

        if (robotSelectionManager.TouchingPanel2Crystal)
        {
            Destroy(newCrystalIcon.gameObject);
            //panel2.GetComponent<Image>().sprite = crystalSprite;
            //panel2.GetComponent<Image>().color = Color.white;
        }

        if (robotSelectionManager.TouchingPanel3Crystal)
        {
            Destroy(newCrystalIcon.gameObject);
            //panel3.GetComponent<Image>().sprite = crystalSprite;
            //panel3.GetComponent<Image>().color = Color.white;
        }
    }


}
