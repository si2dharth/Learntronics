using UnityEngine;
using System.Collections;

public class WireGrid {
    ArrayList terminals;
    static ArrayList wireGrids;

    public float voltage = -1;

    WireGrid() {
        terminals = new ArrayList();
        if (wireGrids == null) wireGrids = new ArrayList();
        wireGrids.Add(this);
    }

    public static void ResetVoltages() {
        if (wireGrids == null) return;
        foreach(WireGrid w in wireGrids) {
            w.voltage = -1;
        }
    }

    public void setVoltage(float val) {
        if (voltage == val) return;
        if (voltage != -1 && val < voltage)
        {
            CanvasScript.canvas.ShowHelp("There is a short circuit! Check your solution.");
            return;
        }
        voltage = val;
        foreach (Terminal t in terminals) {
            t.parent.element.process();
        }
    }

    void registerTerminal(Terminal t) {
        terminals.Add(t);
        t.wireGrid = this;
    }

    public ArrayList getResistors() {
        ArrayList list = new ArrayList();
        foreach (Terminal t in terminals) {
            if (t.transform.parent.parent.tag == "Resistor" || t.transform.parent.parent.tag == "Lamp")
                list.Add(t);
        }
        return list;
    }

    static public void PrintGrids() {
        int i = 1;
        foreach (WireGrid wg in wireGrids) {
            MonoBehaviour.print(i++);
            foreach (Terminal t in wg.terminals) {
                MonoBehaviour.print(t.transform.parent.parent.tag); 
            }
        }
    }

    static public WireGrid GetGrid(Terminal t) {
        if (t.wireGrid == null) {
            WireGrid wireGrid = new WireGrid();
            wireGrid.registerTerminal(t);
        }
        return t.wireGrid;
    }

    public static void addWire(Terminal t1, Terminal t2) {
        if (t1.wireGrid == null && t2.wireGrid == null) {
            GetGrid(t1).registerTerminal(t2);
        }

        if (t1.wireGrid == null) {
            t2.wireGrid.registerTerminal(t1);
        }

        if (t2.wireGrid == null) {
            t1.wireGrid.registerTerminal(t2);
        }

        if (t1.wireGrid != t2.wireGrid) {
            CombineGrids(t1.wireGrid, t2.wireGrid);
        }
        //PrintGrids();
    }

    static WireGrid CombineGrids(WireGrid w1, WireGrid w2) {
        if (w1.terminals.Count < w2.terminals.Count) {
            WireGrid tmp = w1;
            w1 = w2;
            w2 = tmp;
        }

        foreach(Terminal t in w2.terminals) {
            w1.registerTerminal(t);
        }
        wireGrids.Remove(w2);

        return w1;
    }

    bool HasTerminal(Terminal t) {
        return terminals.Contains(t);
    }

    internal static int GetNumberOfGrids()
    {
        return wireGrids.Count;
    }

    internal static void RemoveAll() {
        Wire.DestroyAll();
        if (wireGrids == null) return;
        wireGrids.Clear();
    }
}
