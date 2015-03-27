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
    public DisplayScript infoPanel;
    public static int lampBrightness;

	void Start ()
	{
		Load (0);
        infoPanel.showLevelInfo(Levels.LevelInfo[0]);
        Common.moralPanel = infoPanel;
	}


    public void LoadNextLevel()
    {
        if (curLevel == Levels.maxLevels()) return;
        curLevel++;
        print(curLevel);    
        Common.removeAllObjects();
        Load(curLevel-1);
        Common.levelStarted = false;
        infoPanel.showLevelInfo(Levels.LevelInfo[curLevel-1]);
    }


    private void Load(int levelNumber)
    {
        Common.removeAllWires();
        Common.readyToConnect = false;
        Common.tutorialLevel = Levels.tutorials[levelNumber];
        var wires = GameObject.FindGameObjectsWithTag("WireSymbol");
        foreach (GameObject wire in wires)
        {
            (wire.GetComponent(typeof(Animator)) as Animator).SetTrigger("Show");
        }
        ArrayList components = Load(Levels.Level[levelNumber]);
        string[][] connectionList = readFile(Levels.LevelConnections[levelNumber]);
        if (Common.tutorialLevel)
        {
            for (int i = 0; i < connectionList.Length; i++)
            {
                Transform source = components[i] as Transform;
                for (int j = 0; j < connectionList[i].Length; j++)
                {
                    Terminal s = (source.FindChild("Terminals/Terminal" + (j + 1))).GetComponent("Terminal") as Terminal;
                    string[] connections = Regex.Split(connectionList[i][j], ",");
                    for (int k = 0; k < connections.Length; k++)
                    {
                        Common.numConnections++;
                        Transform destination = components[int.Parse((connections[k][0]).ToString())] as Transform;
                        Terminal t = (destination.FindChild("Terminals/Terminal" + connections[k][1])).GetComponent("Terminal") as Terminal;
                        s.addConnection(t, false);
                    }
                }
            }
        }
        else Common.connections = connectionList;
        Common.numConnections /= 2;
        //print(Common.numConnections);
    }

	// Use this for initialization
	ArrayList Load (string fileName)
	{
        ArrayList components = new ArrayList();
        bool addToList = false;
		string[][] jagged = readFile (fileName);
		for (int y = 0; y < jagged.Length; y++) {
			for (int x = 0; x < jagged[y].Length; x++) {
				Transform t = null;
                addToList = false;
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
                    addToList = true;
                    resPile.AddObject();
					break;
				case batteryC:
					t = battery;
                    addToList = true;
                    batPile.AddObject();
					break;
				case lampC:
					t = lamp;
                    addToList = true;
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
                    if (addToList) components.Add(o);
                    o.eulerAngles = new Vector3(0, 0, angle);
                    Symbol.register(o);
                    Common.addObject(o.gameObject);
					t.renderer.enabled = true;
				}
			}
		}
        return components;
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
