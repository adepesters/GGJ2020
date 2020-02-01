using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragToPanel1 : MonoBehaviour
{
    RobotSelectionManager robotSelectionManager;
    Sprite icon;

    // Start is called before the first frame update
    void Start()
    {
        robotSelectionManager = GameObject.FindWithTag("RobotSelectionManager").GetComponent<RobotSelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.Contains("Icon Panel"))
        {
            robotSelectionManager.TouchingPanel1 = true;
            icon = collision.gameObject.GetComponent<Image>().sprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Icon Canvas"))
        {
            robotSelectionManager.TouchingPanel1 = false;
        }
    }

}
