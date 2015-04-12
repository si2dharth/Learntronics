using UnityEngine;
using System.Collections;

public class Level2 : Challenge{

    public Level2() {
        nPiles = 3;
        ElementPile.LampPile.setup(2, false, this);
        ElementPile.SwitchPile.setup(1, false, this);
        ElementPile.BattPile.setup(1, false, this);
        showInfo();
    }

    public override void showInfo()
    {
        CanvasScript.canvas.ShowInfo("Create a circuit with two light bulbs, one switch and two batteries.\r\nConnect the light bulbs in parallel and then connect the switch in series to the bulbs. Complete the circuit to make the bulbs light up.");
    }

    public override void PileEmpty(ElementPile pile)
    {
        base.PileEmpty(pile);
        nPiles--;
    }

    public override bool checkComplete()
    {
        if (nPiles > 0) return false;
        if (WireGrid.GetNumberOfGrids() != 3) return false;
        foreach (Transform t in elements)
        {
            if (t.tag != "Lamp") continue;
            Element e = t.GetComponent(typeof(Element)) as Element;
            if (e.current == 0) return false;
        }
        return true;
    }
}
