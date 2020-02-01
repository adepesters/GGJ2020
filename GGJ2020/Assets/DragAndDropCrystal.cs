﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
    CraftingManager craftingManager;

    [SerializeField] GameObject panel1;
    [SerializeField] GameObject panel2;
    [SerializeField] GameObject panel3;

    [SerializeField] GameObject slot1EnergyMask;
    [SerializeField] GameObject slot1ActionMask;
    [SerializeField] GameObject slot1ChanceMask;
    [SerializeField] GameObject slot1SpeedMask;

    [SerializeField] GameObject slot2EnergyMask;
    [SerializeField] GameObject slot2ActionMask;
    [SerializeField] GameObject slot2ChanceMask;
    [SerializeField] GameObject slot2SpeedMask;

    [SerializeField] GameObject slot3EnergyMask;
    [SerializeField] GameObject slot3ActionMask;
    [SerializeField] GameObject slot3ChanceMask;
    [SerializeField] GameObject slot3SpeedMask;

    bool returnToBase;

    // Start is called before the first frame update
    void Start()
    {
        robotSelectionManager = GameObject.FindWithTag("RobotSelectionManager").GetComponent<RobotSelectionManager>();
        craftingManager = GameObject.FindWithTag("CraftingManager").GetComponent<CraftingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!returnToBase)
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

            int crystalIndex = int.Parse(Regex.Replace(this.gameObject.name, "[^0-9]", ""));
            if (craftingManager.CrystalsAmount[crystalIndex - 1] > 0)
            {
                GetComponent<Image>().color = Color.white;
            }
            else
            {
                GetComponent<Image>().color = new Color32(142, 142, 142, 102);
            }
        }

        else
        {
            if (newCrystalIcon != null)
            {
                ReturnToBase();
            }
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
        newCrystalIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
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

            UpdateCrystals(0);
        }

        else if (robotSelectionManager.TouchingPanel2Crystal)
        {
            Destroy(newCrystalIcon.gameObject);
            //panel2.GetComponent<Image>().sprite = crystalSprite;
            //panel2.GetComponent<Image>().color = Color.white;

            UpdateCrystals(1);
        }

        else if (robotSelectionManager.TouchingPanel3Crystal)
        {
            Destroy(newCrystalIcon.gameObject);
            //panel3.GetComponent<Image>().sprite = crystalSprite;
            //panel3.GetComponent<Image>().color = Color.white;

            UpdateCrystals(2);
        }

        else
        {
            returnToBase = true;
            //Destroy(newCrystalIcon.gameObject);
        }
    }

    private void UpdateCrystals(int _robotSlot)
    {
        switch (int.Parse(Regex.Replace(this.gameObject.name, "[^0-9]", "")))
        {
            case 1:
                if (craftingManager.CrystalsAmount[0] > 0)
                {
                    craftingManager.CrystalsAmount[0]--;
                    craftingManager.Energy[_robotSlot] += 1;
                    if (_robotSlot == 0)
                    {
                        slot1EnergyMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 1)
                    {
                        slot2EnergyMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 2)
                    {
                        slot3EnergyMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                }
                break;

            case 2:
                if (craftingManager.CrystalsAmount[1] > 0)
                {
                    craftingManager.CrystalsAmount[1]--;
                    craftingManager.Action[_robotSlot] += 1;
                    if (_robotSlot == 0)
                    {
                        slot1ActionMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 1)
                    {
                        slot2ActionMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 2)
                    {
                        slot3ActionMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                }
                break;

            case 3:
                if (craftingManager.CrystalsAmount[2] > 0)
                {
                    craftingManager.CrystalsAmount[2]--;
                    craftingManager.Chance[_robotSlot] += 1;
                    if (_robotSlot == 0)
                    {
                        slot1ChanceMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 1)
                    {
                        slot2ChanceMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 2)
                    {
                        slot3ChanceMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                }
                break;

            case 4:
                if (craftingManager.CrystalsAmount[3] > 0)
                {
                    craftingManager.CrystalsAmount[3]--;
                    craftingManager.Speed[_robotSlot] += 1;
                    if (_robotSlot == 0)
                    {
                        slot1SpeedMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 1)
                    {
                        slot2SpeedMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                    else if (_robotSlot == 2)
                    {
                        slot3SpeedMask.GetComponent<Image>().fillAmount -= 0.1f;
                    }
                }
                break;
        }
    }

    void ReturnToBase()
    {
        newCrystalIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(
        newCrystalIcon.gameObject.GetComponent<RectTransform>().anchoredPosition,
        GetComponent<RectTransform>().anchoredPosition, 2000f * Time.deltaTime);

        if (Vector2.Distance(newCrystalIcon.gameObject.GetComponent<RectTransform>().anchoredPosition,
        GetComponent<RectTransform>().anchoredPosition) < 30f)
        {
            Destroy(newCrystalIcon.gameObject);
            returnToBase = false;
        }
    }
}
