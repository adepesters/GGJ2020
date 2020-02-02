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
        crystalsAmount[0] = 15;
        crystalsAmount[1] = 5;
        crystalsAmount[2] = 3;
        crystalsAmount[3] = 8;

        hasBeenDiscovered[0] = true;
        hasBeenDiscovered[1] = true;
    }

    // Update is called once per frame
    void Update()
    {
        energyAmount.text = crystalsAmount[0].ToString();
        actionAmount.text = crystalsAmount[1].ToString();
        chanceAmount.text = crystalsAmount[2].ToString();
        speedAmount.text = crystalsAmount[3].ToString();

        slot1EnergyMask.GetComponent<Image>().fillAmount = 1f - energy[0] / 10f;
        slot1ActionMask.GetComponent<Image>().fillAmount = 1f - action[0] / 10f;
        slot1ChanceMask.GetComponent<Image>().fillAmount = 1f - chance[0] / 10f;
        slot1SpeedMask.GetComponent<Image>().fillAmount = 1f - speed[0] / 10f;

        slot2EnergyMask.GetComponent<Image>().fillAmount = 1f - energy[1] / 10f;
        slot2ActionMask.GetComponent<Image>().fillAmount = 1f - action[1] / 10f;
        slot2ChanceMask.GetComponent<Image>().fillAmount = 1f - chance[1] / 10f;
        slot2SpeedMask.GetComponent<Image>().fillAmount = 1f - speed[1] / 10f;

        slot3EnergyMask.GetComponent<Image>().fillAmount = 1f - energy[2] / 10f;
        slot3ActionMask.GetComponent<Image>().fillAmount = 1f - action[2] / 10f;
        slot3ChanceMask.GetComponent<Image>().fillAmount = 1f - chance[2] / 10f;
        slot3SpeedMask.GetComponent<Image>().fillAmount = 1f - speed[2] / 10f;

        //DebugText.Text(new Vector2(transform.position.x - 100, -95), robotSlot[0].ToString());
        //DebugText.Text(new Vector2(transform.position.x, -95), robotSlot[1].ToString());
        //DebugText.Text(new Vector2(transform.position.x + 100, -95), robotSlot[2].ToString());
    }
}
