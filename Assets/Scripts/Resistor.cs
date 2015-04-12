using UnityEngine;
using System.Collections;

public class Resistor : Element {

    Resistance r;

    void Start() {
    }

    void Update() {
    }

    public override float getResistance()
    {
        return 10;
    }

}
