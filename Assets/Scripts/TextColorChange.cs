using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextColorChange : MonoBehaviour {

    public Text text;

    public void PointerEnter()
    {
        text.color = new Color(0, 0, 1);
        print("Enter!");
    }

    public void PointerExit()
    {
        text.color = new Color(0, 0, 0);
    }
}
