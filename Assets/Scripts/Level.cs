using UnityEngine;
using System.Collections;

public class Level {
    protected ArrayList elements;
    
    public virtual void LoadLevel() {
        
    }

    public virtual void showInfo() { 
    }

    public virtual void unLoadLevel() {
        WireGrid.RemoveAll();
        if (elements == null) return;
        foreach (Transform t in elements) {
            MonoBehaviour.Destroy((t.GetComponent(typeof(Element)) as Element).Correspoding_Symbol.gameObject);
            MonoBehaviour.Destroy(t.gameObject);
        }
        elements.Clear();
    }

    public virtual void PileEmpty(ElementPile pile) {

    }

    protected virtual bool isValidDrop(Transform draggedElement) {
        return true;
    }
 
    public virtual bool dropElement(Transform draggedElement) {
        if (isValidDrop(draggedElement)) {
            if (elements == null) elements = new ArrayList();
            elements.Add(draggedElement);
            Symbol.underMouse = null;
            return true;
        }
        return false;
    }

    public virtual void connectionFail(int type) {
        if (type == 1) CanvasScript.canvas.ShowHelp("Drag the wire to a terminal");
    }

    public virtual void connectionMade() { 
        
    }

    public virtual void levelOver() {
        CanvasScript.canvas.ShowInfo("Great Work. Press Next to Continue", "Next");
    }
}
