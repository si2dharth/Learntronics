using UnityEngine;
using System.Collections;

public class Lamp : Element {
    public GameObject LightedLamp;
    public GameObject Light;
    SpriteRenderer LightSR;

    public float brightness = 0;

    void Start() {
        LightedLamp.SetActive(false);
        Light.SetActive(false);
        LightSR = Light.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    }

    public override float getResistance()
    {
        return 5;
    }

    public override void passCurrent(float current) {
        base.passCurrent(current);
        if (current == 0) {
            LightedLamp.SetActive(false);
            Light.SetActive(false);
        }
        else {
            current = Mathf.Abs(current);
            LightedLamp.SetActive(true);
            Light.SetActive(true);
            brightness = current;
            LightSR.color = new Color(LightSR.color.r, LightSR.color.g, LightSR.color.b,current);
        }
    }
}
