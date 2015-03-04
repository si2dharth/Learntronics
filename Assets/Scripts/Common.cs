using UnityEngine;
using System.Collections;

public class Common: MonoBehaviour {

	private static string draggedTag;
    private static int draggedAngle;
	private static Symbol currentMouseObject;
    private static int numPiles;
    public static DisplayScript moralPanel;

    public static ArrayList levelObjects;


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
        draggedTag = tag;
        draggedAngle = angle;
	}

	public static Vector2 stopDrag() {
		Vector2 result = new Vector2(-0.5f, -0.5f);
        if (currentMouseObject) 
            result = currentMouseObject.drop(draggedTag, draggedAngle);

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

    public static void removePile() {
        numPiles--;
        print(numPiles);
        if (numPiles == 0)
        {
            Symbol.levelOver();
            (GameObject.Find("Next Level Text").GetComponent("Animator") as Animator).SetTrigger("NextLevel");
            moralPanel.showLevelInfo(Levels.LevelMoral[LoadLevel.curLevel-1]);
        }
    }

    public static void addPile()
    {
        numPiles++;
    }
}
