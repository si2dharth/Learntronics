using UnityEngine;
using System.Collections;

public class Terminal : MonoBehaviour {

    bool mouseEnable = false;
    public Symbol parent;
    ArrayList connections;
    public WireGrid wireGrid;

    static Terminal currentTerminal = null;

    void Start() {
        connections = new ArrayList();
    }
     public void Enable() {
         mouseEnable = true;
    }

    public void Disable() {
        mouseEnable = false;
    }

    void OnMouseEnter() {
        if (!mouseEnable) return;
        renderer.enabled = true;
        currentTerminal = this;
    }

    void OnMouseExit()
    {
        renderer.enabled = false;
        if (currentTerminal == this) currentTerminal = null;
    }


    float doubleClickStart = 0;
    void _OnMouseUp() {
        if ((Time.time - doubleClickStart) < 0.3f) {
            this.OnMouseDblClick();
            doubleClickStart = -1;
            return;
        }
        else {
            doubleClickStart = Time.time;
        }
    }

    Wire currentWire = null;

    void OnMouseDblClick() {
        
    }

    void OnMouseDrag() {
        if (!currentWire) currentWire = Wire.create(this);
        if (currentTerminal == null)
            currentWire.Drag(MyCommon.GetMousePosition());
        else
            currentWire.Drag(currentTerminal.transform.position);
    }

    void OnMouseUp() {
        if (currentTerminal == null || currentTerminal == this) {
            Wire.destroyCurrent();
            Levels.currentLevel.connectionFail(1);
        }
        else
        {
            if (parent.CheckConnection(this, currentTerminal) && currentTerminal.parent.CheckConnection(currentTerminal, this))
            {
                WireGrid.addWire(this, currentTerminal);
                Levels.currentLevel.connectionMade();
                Wire.confirmCurrent();
            }
            else
            {
                Wire.destroyCurrent();
                Levels.currentLevel.connectionFail(2);
            }
        }
        currentWire = null;
    }
}
