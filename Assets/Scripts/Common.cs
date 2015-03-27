using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Common: MonoBehaviour {

	private static string draggedTag;
    private static int draggedAngle;
	private static Symbol currentMouseObject;
    private static int numPiles;
    public static DisplayScript moralPanel;
    public static bool levelStarted = false;
    public static LineRenderer WireLine = GameObject.Find("WireDummy").GetComponent(typeof(LineRenderer)) as LineRenderer;

    public static bool tutorialLevel = false;

    public static ArrayList levelObjects;
    public static int numConnections = 0;

    public static bool readyToConnect = false;
    public static Transform battery = null;

    public static void addObject(GameObject go) {
        if (levelObjects == null) levelObjects = new ArrayList();
        levelObjects.Add(go);
    }

    public static void removeAllObjects() {
        if (levelObjects == null) return;
        while (levelObjects.Count > 0)
            {
                Destroy(levelObjects[0] as GameObject);
                levelObjects.RemoveAt(0);
            }
    }

	public static void startDrag(string tag, int angle) {
        if (!levelStarted) return;
        draggedTag = tag;
        draggedAngle = angle;
	}

	public static Vector2 stopDrag() {
		Vector2 result = new Vector2(-0.5f, -0.5f);
        if (currentMouseObject) 
            result = currentMouseObject.drop(draggedTag, draggedAngle);

        if (!tutorialLevel)
            result = Common.getMousePositionInUnits();

		currentMouseObject = null;
        draggedTag = "";
		return result;
	}

	public static string startHover(Symbol obj) {
		currentMouseObject = obj;
        return draggedTag;
	}
	
	public static void stopHover() {
        currentMouseObject = null;
	}

    public static Vector2 getMousePositionInUnits()
    {
        return new Vector2((Input.mousePosition.x / Screen.width) * 16 - 8, (Input.mousePosition.y / Screen.height) * 10 - 5);
    }

    public static void rotateToAngle(int newAngle)
    {
        draggedAngle = newAngle;
    }

    public static int getAngle()
    {
        return draggedAngle;
    }

    private static ArrayList wires = new ArrayList();
    private static GameObject currentWire;
    private static LineRenderer currentLR;
    private static Terminal currentSource;
    private static GameObject empty = GameObject.Find("Empty");
    private static bool firstDrag = true, firstDrop = true;
    private static GameObject wireHelp = GameObject.Find("WireHelp");
    private static Text wireHelpText = GameObject.Find("Wire_Help_Text").GetComponent(typeof(Text)) as Text;

    public static void removeAllWires()
    {
        foreach (GameObject wire in wires)
            Destroy(wire);
    }

    public static void startDraggingWire(Terminal source) {
        if (currentSource != null) return;
        if (firstDrag)
        {
            wireHelpText.text = "Drag the wire to the connected terminal as shown";
        }
        currentSource = source;
        currentWire = Instantiate(empty) as GameObject;
        currentWire.SetActive(true);
        currentLR = currentWire.AddComponent(typeof(LineRenderer)) as LineRenderer;
        currentLR.material = WireLine.material;
        currentLR.SetWidth(0.1f,0.1f);
        currentLR.SetVertexCount(5);
        currentLR.SetPosition(0, source.transform.position + new Vector3(0, 0, -1));
        currentLR.SetPosition(1, source.transform.position + new Vector3(0, 0, -1));
        currentLR.SetPosition(2, source.transform.position + new Vector3(0, 0, -1));
        currentLR.SetPosition(3, source.transform.position + new Vector3(0, 0, -1));
        currentLR.SetPosition(4, source.transform.position + new Vector3(0, 0, -1));
    }

    public static void dragWireTo(Vector2 positionInUnits) {
        float x1 = positionInUnits.x, x2 = currentSource.transform.position.x, y1 = positionInUnits.y, y2 = currentSource.transform.position.y;
        float i1 = x1 * x1 + y2 * y2, i2 = x2 * x2 + y1 * y1;

        if (i1 >= i2)
        {
            currentLR.SetPosition(1, new Vector3(positionInUnits.x, currentSource.transform.position.y, -1));
            currentLR.SetPosition(2, new Vector3(positionInUnits.x, positionInUnits.y, -1));
            currentLR.SetPosition(3, new Vector3(positionInUnits.x, currentSource.transform.position.y, -1));
        }
        else
        {
            currentLR.SetPosition(1, new Vector3(currentSource.transform.position.x, positionInUnits.y, -1));
            currentLR.SetPosition(2, new Vector3(positionInUnits.x, positionInUnits.y, -1));
            currentLR.SetPosition(3, new Vector3(currentSource.transform.position.x, positionInUnits.y, -1));
        }
    }

    public static void cancelDragWire() {
        Destroy(currentWire);
        if (firstDrop)
        {
            wireHelpText.text = "Try again. Start by dragging wire from any terminal";
        }
        currentSource = null;
        currentWire = null;
        currentLR = null;
    }

    public static void confirmDragWire() {
        if (firstDrag || firstDrop)
        {
            wireHelpText.text = "Now, complete the rest of the circuit";
            firstDrag = false;
        }
        numConnections--;
        wires.Add(currentWire);
        currentWire = null;
        currentSource = null;
        currentLR = null;
        bool checkResult = false;
        if (!tutorialLevel) if (battery) checkCircuit(battery);
        if (numConnections == 0 || checkResult)
        {
            Symbol.levelOver();
            //(GameObject.Find("Next Level Text").GetComponent("Animator") as Animator).SetTrigger("NextLevel");
            (wireHelp.GetComponent((typeof(Animator))) as Animator).SetTrigger("Hide");
            firstDrop = false;
            moralPanel.showLevelInfo(Levels.LevelMoral[LoadLevel.curLevel - 1], "Next");
        }
    }

    public static void removePile() {
        numPiles--;
        print(numPiles);
        if (numPiles == 0)
        {
            var wires = GameObject.FindGameObjectsWithTag("WireSymbol");
            foreach (GameObject wire in wires)
            {
                (wire.GetComponent(typeof(Animator)) as Animator).SetTrigger("Hide");
            }
            //numConnections = 0;
           // moralPanel.showLevelInfo("Great Work! Now, connect the wires", "OK");
            readyToConnect = true;
            if (firstDrag)
            {
                (wireHelp.GetComponent((typeof(Animator))) as Animator).SetTrigger("Show");
                wireHelpText.text = "Great Work! Now, connect the wires. \r\nStart by dragging wire from a terminal";
            }

        }
    }

    public static void addPile()
    {
        numPiles++;
    }

    public static string[][] connections;

    public static bool checkCircuit(Transform battery)
    {
        ArrayList transformList = new ArrayList();
        transformList.Add(battery);
        Transform curTransform;
        Terminal curTerm;

        for (int i = 0; i < connections.Length; i++)
        {
            curTransform = transformList[i] as Transform;
            print(curTransform.name);
            for (int j = 0; j < connections[i].Length; j++)
            {
                curTerm = curTransform.FindChild("Terminals/Terminal" + (j + 1).ToString()).GetComponent(typeof(Terminal)) as Terminal;
                string[] individualConnections = Regex.Split(connections[i][j], ",");
                for (int k = 0; k < individualConnections.Length; k++)
                {
                    Transform nextTransform = curTerm.CheckConnectionWith(individualConnections[k]);
                    if (nextTransform) print(nextTransform.name);
                    if (!nextTransform) return false; else transformList.Add(nextTransform);
                }
            }
        }
            return true;
    }
}
