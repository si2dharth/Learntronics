using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MessageDisplay : MonoBehaviour {
    public static GameObject rotateBox;
    private static Animator rotateBoxAnim;
	public static Text textBox;
    private static bool rotateHidden = true;
    private static GameObject rotateSymbol;

    private static GameObject levelInfo, levelMoral;
    private static Text levelInfoText, levelMoralText;
    private static Button levelInfoBtn, levelMoralBtn;
    

    public Text GUIText;
    public GameObject LevelInfoPanel,LevelMoralPanel;
    public GameObject RotatePanel;
    public GameObject RotateSymbol;
	// Use this for initialization

    static string[] levelInfoStrs;
    static string[] levelMoralStrs;
    static int curInfo, curMoral;
	void Start () {
		textBox = GUIText;
        rotateBox = RotatePanel;
        rotateBoxAnim = rotateBox.GetComponent("Animator") as Animator;
        rotateSymbol = RotateSymbol;
        levelInfo = LevelInfoPanel;
        levelMoral = LevelMoralPanel;
        levelInfoText = GameObject.Find("LevelInfo Text").GetComponent("Text") as Text;
        levelMoralText = GameObject.Find("LevelMoral Text").GetComponent("Text") as Text;
        levelInfoBtn = GameObject.Find("LevelInfo Next Button").GetComponent("Button") as Button;
        levelMoralBtn = GameObject.Find("LevelMoral Next Button").GetComponent("Button") as Button;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void displayError(string str) {
		textBox.text = str;
		textBox.color = new Color (1, 0, 0);
	}

	public static void displayMessage(string str) {
		textBox.text = str;
		textBox.color = new Color (1, 1, 1);
	}

	public static void displayReinforcement (string str) {
		textBox.text = str;
		textBox.color = new Color (0, 1, 0);
	}

    public static void showRotateHelp()
    {
        rotateSymbol.transform.position = Input.mousePosition;
        if (rotateHidden)
        {
            rotateBoxAnim.SetTrigger("Show");
            rotateBox.transform.position = new Vector2(Screen.width, 100);
        }
        rotateHidden = false;
    }

    public static void hideRotateHelp()
    {
        if (!rotateHidden) rotateBoxAnim.SetTrigger("Hide");
        rotateHidden = true;
    }
}
