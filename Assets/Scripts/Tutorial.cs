using UnityEngine;
using System.Collections;

public class Tutorial : Level {

    ArrayList symbols;
    protected int numGrids = 0;
    protected int nPiles = 0;

    public void addSymbol(Symbol symbol) {
        if (symbols == null) symbols = new ArrayList();
        symbols.Add(symbol);
    }

    protected override bool isValidDrop(Transform draggedElement) {
        if (Symbol.underMouse == null) return false;

        if (symbols.Contains(Symbol.underMouse))                                //Surity check
            if (Symbol.underMouse.renderer.enabled == true)
                if (Symbol.underMouse.CheckElement(draggedElement))  return true;

        return false;
    }

    void ShowStars(Transform t) {
        GameObject stars = GameObject.Find("Stars");
        stars.transform.position = t.position;
        stars.transform.rotation = t.rotation;
        (stars.GetComponent("Animator") as Animator).SetTrigger("Play");
    }

    public override void unLoadLevel()
    {
        base.unLoadLevel();
        foreach (Symbol sym in symbols)
        {
            MonoBehaviour.Destroy(sym.gameObject);
        }
    }

    public override bool dropElement(Transform draggedElement) {
        if (isValidDrop(draggedElement)) {
            Symbol.underMouse.HideSymbol();
            draggedElement.position = Symbol.underMouse.transform.position;
            ShowStars(draggedElement);
            if (elements == null) elements = new ArrayList();
            elements.Add(draggedElement);
            return true;
        }
        return false;
    }

    public override void connectionFail(int type)
    {
        base.connectionFail(type);
        if (type == 2) CanvasScript.canvas.ShowHelp("Wrong Terminal. Try Again.");
    }

    public override void PileEmpty(ElementPile pile)
    {
        nPiles--;
        if (nPiles == 0) connectWires();
    }

    void connectWires()
    {
        CanvasScript.canvas.ShowHelp("Now connect the wires by dragging and complete the circuit.");
    }

    public override void connectionMade() {
        if (numGrids == WireGrid.GetNumberOfGrids()) levelOver();
    }
}
