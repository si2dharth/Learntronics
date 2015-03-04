using UnityEngine;
using System.Collections;

public class Symbol : MonoBehaviour {
    public int anglePrecision = 180;

	public Vector2 drop(string tag, int angle) {
        angle -= (int)transform.eulerAngles.z;
        if (tag != this.tag)
            return new Vector2(-0.5f,-0.5f);
        else if (angle % anglePrecision == 0)
        {
            renderer.enabled = false;
            return transform.position;
        }
        return new Vector2(-0.5f, -0.5f);
	}

	void OnMouseOver() {
		string tg = Common.startHover (this);
        int angle = Common.getAngle();
        angle -= (int)transform.eulerAngles.z;
        if (tg == tag)
            if (angle % anglePrecision == 0)
            {
                (GetComponent("SpriteRenderer") as SpriteRenderer).color = new Color(0, 1, 0);
                MessageDisplay.hideRotateHelp();
            } else {
                (GetComponent("SpriteRenderer") as SpriteRenderer).color = new Color(1, 1, 0);
                MessageDisplay.showRotateHelp();
            }
        else if (tg != "")
            (GetComponent("SpriteRenderer") as SpriteRenderer).color = new Color(1, 0, 0);
        else
        {
            (GetComponent("SpriteRenderer") as SpriteRenderer).color = new Color(0, 0, 0);
            MessageDisplay.hideRotateHelp();
        }
    }

	void OnMouseExit() {
        (GetComponent("SpriteRenderer") as SpriteRenderer).color = new Color(0, 0, 0);
		Common.stopHover ();
        MessageDisplay.hideRotateHelp();
	}

    static ArrayList lampList;
    
    public static void register(Transform t)
    {
        if (t.tag == "") return;
        if (t.tag == "Lamp")
        {
            if (lampList == null) lampList= new ArrayList();
            lampList.Add(t);
        }
    }

    public static void levelOver()
    {
        Transform lightedLamp = GameObject.Find("LightedLamp").transform;
        foreach (Transform t in lampList)
        {
            GameObject lLamp = (Instantiate(lightedLamp, t.position, t.rotation) as Transform).gameObject;
            Common.addObject(lLamp);
            float greyLevel = LoadLevel.lampBrightness/100f;
            (lLamp.GetComponent("SpriteRenderer") as SpriteRenderer).color = new Color(greyLevel,greyLevel,greyLevel);
            print(greyLevel);
            Common.levelObjects.Remove(t.gameObject);
            Destroy(t.gameObject);
        }
        lampList.Clear();
    }
}
