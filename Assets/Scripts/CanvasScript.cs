using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

    static public CanvasScript canvas;

    public GameObject rotateHelp;
    public DisplayScript levelInfo;
    public WireHelp wireHelp;

    bool rotateHelpShown, levelInfoShown;
    Animator rotateAnimator, levelInfoAnimator;
	// Use this for initialization
	void Start () {
        canvas = this;
        rotateAnimator = rotateHelp.GetComponent(typeof(Animator)) as Animator;
        rotateHelpShown = false;
	}

    public void DisplayRotateHelp() {
        if (rotateHelpShown) return;
        rotateAnimator.SetTrigger("Show");
        rotateHelpShown = true;
    }

    public void HideRotateHelp() {
        if (!rotateHelpShown) return;
        rotateHelpShown = false;
        rotateAnimator.SetTrigger("Hide");
    }

    public void ShowInfo(string messages, string lastTextToDisplay = "Start!") {
        levelInfo.showLevelInfo(messages, lastTextToDisplay);
    }

    public void ShowHelp(string text) {
        wireHelp.displayMessage(text);
    }

    public void HideHelp() {
        wireHelp.hide();
    }
}
