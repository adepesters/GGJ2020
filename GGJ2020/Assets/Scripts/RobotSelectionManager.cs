using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSelectionManager : MonoBehaviour
{
    [SerializeField] GameObject panelsRobots;

    bool touchingPanel1;
    bool touchingPanel2;
    bool touchingPanel3;

    bool touchingPanel1Crystal;
    bool touchingPanel2Crystal;
    bool touchingPanel3Crystal;

    public bool TouchingPanel1 { get => touchingPanel1; set => touchingPanel1 = value; }
    public bool TouchingPanel2 { get => touchingPanel2; set => touchingPanel2 = value; }
    public bool TouchingPanel3 { get => touchingPanel3; set => touchingPanel3 = value; }
    public bool TouchingPanel1Crystal { get => touchingPanel1Crystal; set => touchingPanel1Crystal = value; }
    public bool TouchingPanel2Crystal { get => touchingPanel2Crystal; set => touchingPanel2Crystal = value; }
    public bool TouchingPanel3Crystal { get => touchingPanel3Crystal; set => touchingPanel3Crystal = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
