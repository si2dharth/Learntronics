using UnityEngine;
using System.Collections;

public class Symbol : MonoBehaviour {

    static public Symbols parent;

    //Hashtable connections;
    ArrayList []connections;
    public Terminal terminal1, terminal2, terminal3;
    public Element element;

    public enum SymbolType { 
        Resistor,
        Lamp,
        Battery,
        Switch,
        And,
        Or,
        Not
    }

    public static Symbol create(SymbolType type) {
        Transform t = null;
        switch (type) {
            case SymbolType.Resistor:
                t = Instantiate(parent.resistorSym.transform) as Transform;
                break;
            case SymbolType.Lamp:
                t = Instantiate(parent.lampSym.transform) as Transform;
                break;
            case SymbolType.Switch:
                t = Instantiate(parent.switchSym.transform) as Transform;
                break;
            case SymbolType.Battery:
                t = Instantiate(parent.batterySym.transform) as Transform;
                break;
            case SymbolType.And:
                t = Instantiate(parent.andSym.transform) as Transform;
                break;
            case SymbolType.Or:
                t = Instantiate(parent.orSym.transform) as Transform;
                break;
            case SymbolType.Not:
                t = Instantiate(parent.notSym.transform) as Transform;
                break;
            default:
                return null;
        }
        return t.GetComponent(typeof(Symbol)) as Symbol;
    }

    public static Symbol create(string type) {
        if (type == "Resistor") return create(SymbolType.Resistor);
        if (type == "Lamp") return create(SymbolType.Lamp);
        if (type == "Switch") return create(SymbolType.Switch);
        if (type == "Battery") return create(SymbolType.Battery);
        if (type == "AND") return create(SymbolType.And);
        if (type == "OR") return create(SymbolType.Or);
        if (type == "NOT") return create(SymbolType.Not);
        return null;
    }

    void Start() {
        if (terminal1 != null) terminal1.parent = this;
        if (terminal2 != null) terminal2.parent = this;
        if (terminal3 != null) terminal3.parent = this;
    }

    public void connectTo(int terminal, Symbol sym) {
        if (connections == null) connections = new ArrayList[3];
        if (connections[terminal-1] == null) connections[terminal-1] = new ArrayList();
        connections[terminal-1].Add(sym);
    }

    public static Symbol underMouse = null;
    public int anglePrecision = 180;

    void OnMouseEnter() {
        underMouse = this;
    }

    public bool CheckElement(Transform draggedElement) {
        if (draggedElement == null) return false;
        int angle = (int)draggedElement.eulerAngles.z;
        angle -= (int)transform.eulerAngles.z;
        if (draggedElement.tag == tag)
            if (angle % anglePrecision != 0) 
                CanvasScript.canvas.DisplayRotateHelp();
            else {
                CanvasScript.canvas.HideRotateHelp();
                element = draggedElement.GetComponent(typeof(Element)) as Element;
                element.Correspoding_Symbol = this;
                return true;
            }
        return false;
    }

    void OnMouseExit() {
        if (underMouse == this) underMouse = null;
        CanvasScript.canvas.HideRotateHelp();
    }

    public void HideSymbol() {
        renderer.enabled = false;
        collider.enabled = false;
        if (terminal1 != null) terminal1.Enable();
        if (terminal2 != null) terminal2.Enable();
        if (terminal3 != null) terminal3.Enable();
    }

    public bool CheckConnection(Terminal terminal1, Terminal terminal2) {
        if (connections == null) return true;
        if (this.terminal1 == terminal1)
        {
            return (connections[0].Contains(terminal2.parent));
        }
        else if (this.terminal2 == terminal1)
        {
            return (connections[1].Contains(terminal2.parent));
        }
        else if (this.terminal3 == terminal1)
        {
            return (connections[2].Contains(terminal2.parent));
        }
        else print( "Problem");
        return false;
    }
}
