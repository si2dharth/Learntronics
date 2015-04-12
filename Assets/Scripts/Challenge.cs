using UnityEngine;
using System.Collections;

public class Challenge : Level {
    protected override bool isValidDrop(Transform draggedElement)
    {
        Symbol sym = Symbol.create(draggedElement.tag);
        sym.transform.position = draggedElement.position;
        sym.transform.eulerAngles = draggedElement.eulerAngles;
        Symbol.underMouse = sym;
        Symbol.underMouse.CheckElement(draggedElement);
        sym.HideSymbol();
        return true;
    }

    public virtual bool checkComplete() {
        return false;
    }

    protected int nPiles = 0;

    public override void PileEmpty(ElementPile pile)
    {
        base.PileEmpty(pile);
        nPiles--;
    }

    public void run() {
        WireGrid.ResetVoltages();
        Circuit.circuit().removeAllResistors();

        Hashtable lamps = new Hashtable();
        foreach (Transform t in elements)
        {
            Element e = t.GetComponent(typeof(Element)) as Element;
            float res = 0;
            res = e.getResistance();
            if (e.tag == "Battery")
            {
                if (e.Correspoding_Symbol.terminal1.wireGrid != null)
                {
                    Circuit.Grid.grid(e.Correspoding_Symbol.terminal1.wireGrid).applyVoltage(5);
                    Circuit.Grid.grid(e.Correspoding_Symbol.terminal1.wireGrid).add();
                }
                if (e.Correspoding_Symbol.terminal2.wireGrid != null)
                {
                    Circuit.Grid.grid(e.Correspoding_Symbol.terminal2.wireGrid).applyVoltage(0);
                    Circuit.Grid.grid(e.Correspoding_Symbol.terminal2.wireGrid).add();
                }
                continue;
            }
            // MonoBehaviour.print(e.tag + " : " + res);
            if (e.Correspoding_Symbol.terminal1.wireGrid != null && (e.Correspoding_Symbol.terminal2.wireGrid != null) && res >= 0)
            {
                Circuit.Resistor Res = Circuit.circuit().addResistor(e.Correspoding_Symbol.terminal1.wireGrid, e.Correspoding_Symbol.terminal2.wireGrid, res);
                if (e.tag == "Lamp") lamps[e] = Res;
            }
           /*
            else if (res >= 0)
            {
                if (e.Correspoding_Symbol.terminal1.wireGrid != null) Circuit.Grid.grid(e.Correspoding_Symbol.terminal1.wireGrid).add();
                if (e.Correspoding_Symbol.terminal2.wireGrid != null) Circuit.Grid.grid(e.Correspoding_Symbol.terminal2.wireGrid).add();
                if (e.Correspoding_Symbol.terminal3 != null)
                    if (e.Correspoding_Symbol.terminal3.wireGrid != null) Circuit.Grid.grid(e.Correspoding_Symbol.terminal3.wireGrid).add();
            }
          */
        }

        Circuit.circuit().simulate();

        foreach (Element l in lamps.Keys)
        {
            l.passCurrent((lamps[l] as Circuit.Resistor).GetCurrent());
        }
    }

    public override void connectionMade() {
        run();
        if (checkComplete()) levelOver();
    }
}
