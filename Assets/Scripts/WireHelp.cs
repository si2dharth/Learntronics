using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WireHelp : MonoBehaviour {

    bool hidden = true;
    public Text Textbox;
    Animator anim;

    void Start() {
        anim = GetComponent(typeof(Animator)) as Animator;
    }
    
    public void displayMessage(string text) {
        if (hidden) {
            anim.SetTrigger("Show");
            hidden = false;
        }
        Textbox.text = text;
    }

    public void hide() {
        if (hidden) return;
        anim.SetTrigger("Hide");
        hidden = true;
    }
}
