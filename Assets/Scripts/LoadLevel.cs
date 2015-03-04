using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class LoadLevel : MonoBehaviour
{
    public static int curLevel = 1;
    
	string[][] readFile (string fileContents)
	{	
		string text = fileContents;
		
		string[] lines = Regex.Split (text, "\r\n");
		int rows = lines.Length;

		string[][] levelBase = new string[rows][];
		for (int i = 0; i < lines.Length; i++) {
			string[] stringsOfLine = Regex.Split (lines [i], " ");
			levelBase [i] = stringsOfLine;
		}
		return levelBase;
	}

	public const char resistorC = 'R', batteryC = 'B', lampC = 'L', wireC = '-', wireAC = 'A', wireTC = 'T', brightness = '>';
    public Transform resistor, battery, lamp, wire, wireA, wireT;
    public Pile resPile, batPile, lampPile;
    public GameObject UIPanel;
    public DisplayScript infoPanel, moralPanel;

    public static int lampBrightness;

	void Start ()
	{
		Load (Levels.Level[0]);
        infoPanel.showLevelInfo(Levels.LevelInfo[0]);
        Common.moralPanel = moralPanel;
	}


    public void LoadNextLevel()
    {
        (GameObject.Find("Next Level Text").GetComponent("Animator") as Animator).Play("Idle");
        if (curLevel == Levels.maxLevels()) return;
        curLevel++;
        //UIPanel.SetActive(true);
      //  MessageDisplay.displayMessage("Drag the components from the piles to the corresponding symbols");
        print(curLevel);    
        Common.removeAllObjects();
        Load(Levels.Level[curLevel-1]);
        infoPanel.showLevelInfo(Levels.LevelInfo[curLevel-1]);
       // moralPanel.showLevelInfo(Levels.LevelMoral[curLevel]);
    }

	// Use this for initialization
	void Load (string fileName)
	{
		string[][] jagged = readFile (fileName);
		for (int y = 0; y < jagged.Length; y++) {
			for (int x = 0; x < jagged[y].Length; x++) {
				Transform t = null;
				switch (jagged [y] [x][0]) {
				case wireC:
                    t = wire;
					break;
				case wireAC:
					t = wireA;
					break;
				case wireTC:
					t = wireT;
					break;
				case resistorC:
					t = resistor;
                    resPile.AddObject();
					break;
				case batteryC:
					t = battery;
                    batPile.AddObject();
					break;
				case lampC:
					t = lamp;
                    lampPile.AddObject();
					break;
                case brightness:
                    jagged[y][x] = jagged[y][x].Substring(1);
                    print(jagged[y][x]);
                    lampBrightness = int.Parse(jagged[y][x]);
                    break;
				}

				if (t) {
                    int angle = 90 * (int)(jagged[y][x][1] - '0');
					Transform o = Instantiate (t, new Vector3 (x - jagged [0].Length / 2, -y + jagged.Length / 2, 0), Quaternion.identity) as Transform;
                    o.eulerAngles = new Vector3(0, 0, angle);
                    Symbol.register(o);
                    Common.addObject(o.gameObject);
					t.renderer.enabled = true;
				}
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
