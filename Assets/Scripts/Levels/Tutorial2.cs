using UnityEngine;
using System.Collections;

public class Tutorial2 : Tutorial
{

    Symbol res1, res2, batt, lamp;


    public Tutorial2()
    {
        nPiles = 3;
        numGrids = 3;
        res1 = Symbol.create(Symbol.SymbolType.Resistor);
        res2 = Symbol.create(Symbol.SymbolType.Resistor);
        batt = Symbol.create(Symbol.SymbolType.Battery);
        lamp = Symbol.create(Symbol.SymbolType.Lamp);
        addSymbol(res1);
        addSymbol(res2);
        addSymbol(batt);
        addSymbol(lamp);
        res1.transform.position = new Vector2(0, 3);
        res2.transform.position = new Vector2(0, 2);
        batt.transform.position = new Vector2(-3, 0);
        lamp.transform.position = new Vector2(3, 0);
        lamp.transform.eulerAngles = new Vector3(0, 0, 90);

        ElementPile.BattPile.setup(1, false, this);
        ElementPile.ResPile.setup(2, false, this);
        ElementPile.LampPile.setup(1, false, this);

        batt.connectTo(1, res1);
        batt.connectTo(1, res2);
        batt.connectTo(2, lamp);

        res1.connectTo(2, batt);
        res1.connectTo(1, lamp);

        res2.connectTo(2, batt);
        res2.connectTo(1, lamp);

        lamp.connectTo(2, res2);
        lamp.connectTo(2, res1);
        lamp.connectTo(1, batt);

        showInfo();
    }

    public override void showInfo()
    {
        CanvasScript.canvas.ShowInfo("Drag the components onto corresponding symbols to create a parallel circuit.");
    }
}
