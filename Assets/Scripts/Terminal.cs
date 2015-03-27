using UnityEngine;
using System.Collections;

public class Terminal : MonoBehaviour {

    private static Terminal terminalUnderMouse = null;

    SpriteRenderer SR;
    LineRenderer LR;

    ArrayList connections = new ArrayList();
    ArrayList actualConnections = new ArrayList();
    public bool dragged;

    void Start()
    {
        SR = GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        LR = null;
    }
    void OnMouseEnter()
    {
        if (!Common.readyToConnect) return;
        terminalUnderMouse = this;
        SR.enabled = true;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    void OnMouseExit()
    {
        if (!Common.readyToConnect) return;
        SR.enabled = false;
        if (terminalUnderMouse == this) terminalUnderMouse = null;
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void OnMouseUp()
    {
        if (!Common.readyToConnect) return;
        hideConnections();
        if (terminalUnderMouse == null || (Common.tutorialLevel && !connections.Contains(terminalUnderMouse)))
            Common.cancelDragWire();
        else {
            Common.dragWireTo(terminalUnderMouse.transform.position);
            Common.confirmDragWire();
            actualConnections.Add(terminalUnderMouse);
            connections.Remove(terminalUnderMouse);
            terminalUnderMouse.connections.Remove(this);
        }
    }

    void showConnections()
    {
        LR = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
        if (LR == null)
        {
            LR = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
            LR.material = Common.WireLine.material;
            int i = 0;
            foreach (Terminal conn in connections)
            {
                i++;
                LR.SetVertexCount(i * 2);
                LR.SetPosition(i * 2 - 2, transform.position + new Vector3(0, 0, -1));
                LR.SetPosition(i * 2 - 1, conn.transform.position + new Vector3(0, 0, -1));
                LR.SetWidth(0.05f, 0.05f);
                conn.SR.enabled = true;
            }
        }
        LR.enabled = true;
        
    }

    void hideConnections()
    {
        LR = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
        if (LR == null) return;
        LR.enabled = false;
        foreach (Terminal conn in connections)
        {
            conn.SR.enabled = false;
        }
    }

    public void addConnection(Terminal term, bool show = true)
    {
        connections.Add(term);
        show = false;
        //if (show)
        //{
        //    LineRenderer LR = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
        //    if (LR == null) LR = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        //    LR.SetVertexCount(connections.Count*2);
        //    LR.SetPosition(connections.Count*2 - 2, transform.position + new Vector3(0, 0, -1));
        //    LR.SetPosition(connections.Count*2 - 1, term.transform.position + new Vector3(0, 0, -1));
        //    LR.SetWidth(0.2f, 0.2f);
        //}
    }

    public void copyConnections(Terminal term)
    {
        foreach (Terminal conn in term.connections)
        {
            addConnection(conn);
        }
    }
    void OnMouseDrag()
    {
        if (!Common.readyToConnect) return;
        dragged = true;
        showConnections();
        foreach (Terminal conn in connections)
        {
            conn.transform.parent.gameObject.SetActive(true);
        }
        Common.startDraggingWire(this);
        Common.dragWireTo(Common.getMousePositionInUnits());
     //   print("Dragging...");
    }

    public Transform CheckConnectionWith(string TypeTag)
    {
        Terminal foundTerminal = null;
        foreach (Terminal conn in actualConnections) {
            if (conn.transform.parent.parent.tag == TypeTag) {
                foundTerminal = conn;
                break;
            }
        }
        if (foundTerminal)
        {
            actualConnections.Remove(foundTerminal);
            return foundTerminal.transform.parent.parent;
        }
        else return null;
        
    }
}
