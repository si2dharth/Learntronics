using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class DisplayScript : MonoBehaviour
{
    public Text textComponent;
    public Button nxtButton;
    public Text btnText;
    public Levels levelHandler;

    private string[] strs;
    private int curStr = 0;
    private Animator animator;
    private bool hidden = true;
    private string lastText;
    public void showLevelInfo(string levelInfoStr, string lastTextToDisplay = "Start!")
    {
        if (hidden) animator.SetTrigger("Show");
        hidden = false;
        strs = Regex.Split(levelInfoStr, "\r\n");
        curStr = 0;
        lastText = lastTextToDisplay;
        nextMessage();
    }

    public void nextMessage()
    {
        if (curStr == strs.Length)
        {
            if (lastText == "Next") levelHandler.LoadNextLevel();
            else
            {
                if (!hidden) animator.SetTrigger("Hide");
                hidden = true;
            }
            return;
        }
        textComponent.text = strs[curStr++];
        if (curStr == strs.Length) btnText.text = lastText; else btnText.text = "Continue";
    }

    void Start()
    {
        animator = gameObject.GetComponent("Animator") as Animator;
    }
}
