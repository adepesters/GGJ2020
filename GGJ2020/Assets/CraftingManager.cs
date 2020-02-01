using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    int[] robotSlot = new int[3];
    int[] energy = new int[3];
    int[] action = new int[3];
    int[] chance = new int[3];
    int[] speed = new int[3];

    int[] crystalsAmount = new int[4];

    bool[] hasBeenDiscovered = new bool[10];

    [SerializeField] Text energyAmount;
    [SerializeField] Text actionAmount;
    [SerializeField] Text chanceAmount;
    [SerializeField] Text speedAmount;

    public int[] RobotSlot { get => robotSlot; set => robotSlot = value; }
    public int[] Energy { get => energy; set => energy = value; }
    public int[] Action { get => action; set => action = value; }
    public int[] Chance { get => chance; set => chance = value; }
    public int[] Speed { get => speed; set => speed = value; }
    public int[] CrystalsAmount { get => crystalsAmount; set => crystalsAmount = value; }
    public bool[] HasBeenDiscovered { get => hasBeenDiscovered; set => hasBeenDiscovered = value; }

    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        crystalsAmount[0] = 4;
        crystalsAmount[1] = 5;
        crystalsAmount[2] = 3;
        crystalsAmount[3] = 8;

        hasBeenDiscovered[0] = true;
        hasBeenDiscovered[1] = true;
        hasBeenDiscovered[3] = true;
    }

    // Update is called once per frame
    void Update()
    {
        energyAmount.text = crystalsAmount[0].ToString();
        actionAmount.text = crystalsAmount[1].ToString();
        chanceAmount.text = crystalsAmount[2].ToString();
        speedAmount.text = crystalsAmount[3].ToString();

        Debug.Log("1: " + robotSlot[0]);
        Debug.Log("2: " + robotSlot[1]);
        Debug.Log("3: " + robotSlot[2]);

        DebugText.Text(new Vector2(transform.position.x - 100, -95), robotSlot[0].ToString());
        DebugText.Text(new Vector2(transform.position.x, -95), robotSlot[1].ToString());
        DebugText.Text(new Vector2(transform.position.x + 100, -95), robotSlot[2].ToString());
    }
}
