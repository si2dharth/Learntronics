using UnityEngine;
using System.Collections;

public class Level1 : Challenge {

    public Level1() {
        nPiles = 3;
        ElementPile.LampPile.setup(2, false, this);
        ElementPile.BattPile.setup(1, false, this);
        ElementPile.SwitchPile.setup(1, false, this);

        showInfo();
    }

    public override void showInfo()
    {
        CanvasScript.canvas.ShowInfo("Create a circuit with two light bulbs, one switch and two batteries.\r\nConnect the circuit elements in series and complete the circuit to make the bulbs light up.");
    }



    public override bool checkComplete()
    {
        if (nPiles > 0) return false;
        if ( WireGrid.GetNumberOfGrids() != 4) return false;
        foreach (Transform t in elements) {
            if (t.tag != "Lamp") continue;
            Element e = t.GetComponent(typeof(Element)) as Element;
            if (e.current == 0) return false;
        }
        return true;
    }
}
