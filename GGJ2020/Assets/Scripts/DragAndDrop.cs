using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
    , IPointerClickHandler
, IDragHandler
//, IPointerEnterHandler
// , IPointerExitHandler

{
    [SerializeField] Sprite robotSprite1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        Debug.Log("ok");
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("I was clicked");
        Sprite newRobotSprite = Instantiate(robotSprite1, Input.mousePosition, Quaternion.identity);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("I'm being dragged!");
        Sprite newRobotSprite = Instantiate(robotSprite1, Input.mousePosition, Quaternion.identity);
    }

}
