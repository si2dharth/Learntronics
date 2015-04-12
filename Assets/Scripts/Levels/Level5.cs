using UnityEngine;
using System.Collections;

public class Level5 : Challenge {

    public Level5()
    {
        ElementPile.AndPile.setup(1, false, this);
        ElementPile.BattPile.setup(1, false, this);
        ElementPile.NotPile.setup(3, true, this);
        ElementPile.SwitchPile.setup(2, false, this);
        ElementPile.LampPile.setup(1, false, this);
    }

    public override void showInfo()
    {
        CanvasScript.canvas.ShowInfo("Use any number of NOT gates and an AND gate to form an OR gate.\r\nUse two switches to provide input, and lamp to check output.");
    }

    public override bool checkComplete()
    {
        if (ElementPile.SwitchPile.getNumberOfElements() > 0) return false;
        if (ElementPile.LampPile.getNumberOfElements() > 0) return false;
        if (ElementPile.AndPile.getNumberOfElements() > 0) return false;
        if (ElementPile.BattPile.getNumberOfElements() > 0) return false;
        if (ElementPile.NotPile.getNumberOfElements() > 0) return false;

        Switch sw1 = null, sw2 = null;
        Lamp l = null;
        foreach (Transform t in elements) {
            Element e = t.GetComponent(typeof(Element)) as Element;
            if (e.Correspoding_Symbol.terminal1.wireGrid == null) return false;
            if (e.Correspoding_Symbol.terminal2.wireGrid == null) return false;
            if (e.Correspoding_Symbol.terminal3 != null)
                if (e.Correspoding_Symbol.terminal3.wireGrid == null) return false;
            if (t.tag == "Switch") if (sw1 == null) sw1 = e as Switch; else sw2 = e as Switch;
            if (t.tag == "Lamp") l = e as Lamp;
        }

        sw1.set(false);
        sw2.set(false);
        run();
        if (l.current != 0) return false;

        sw1.set(true);
        sw2.set(false);
        run();
        if (l.current == 0) return false;

        sw1.set(false);
        sw2.set(true);
        run();
        if (l.current == 0) return false;

        sw1.set(false);
        sw2.set(false);
        run();
        if (l.current == 0) return false;

        return true;
    }
}
