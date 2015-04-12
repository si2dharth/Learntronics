using UnityEngine;
using System.Collections;

public class Switch : Element
{
    bool on = false;
    public GameObject spriteOn, spriteOff;

    public void toggle()
    {
        on = !on;
        spriteOn.SetActive(on);
        spriteOff.SetActive(!on);
    }

    public void set(bool val) {
        on = !val;
        toggle();
    }

    void OnMouseUp()
    {
        toggle();
        Levels.currentLevel.connectionMade();
    }

    public override float getResistance()
    {
        if (on) return 0; else return -1;
    }
}
