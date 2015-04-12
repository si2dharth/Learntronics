using UnityEngine;
using System.Collections;

public class Level4 : Challenge{

    public Level4() {
        nPiles = 4;
        ElementPile.BattPile.setup(1, false, this);
        ElementPile.SwitchPile.setup(1, false, this);
        ElementPile.LampPile.setup(1, false, this);
        ElementPile.NotPile.setup(1, false, this);
    }

    public override void showInfo()
    {
        CanvasScript.canvas.ShowInfo("Create a circuit using a not gate, switch, battery and a lamp so that the lamp is on when switch is off and vice versa");
    }

    public override bool checkComplete()
    {
        if (nPiles > 0) return false;
        if (WireGrid.GetNumberOfGrids() < 4) return false;
        Switch sw = null;
        Lamp l = null;
        bool flow = false;
        foreach (Transform t in elements) {
            if (t.tag == "Switch") sw = t.GetComponent(typeof(Switch)) as Switch;
            if (t.tag == "Lamp") l = t.GetComponent(typeof(Lamp)) as Lamp;
        }
        flow = (l.current != 0);
        sw.toggle();
        run();
        if (flow != (l.current != 0)) return true;
        return false;
    }

}
