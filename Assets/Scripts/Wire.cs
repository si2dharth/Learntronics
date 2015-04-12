using UnityEngine;
using System.Collections;

public class Wire : MonoBehaviour {
    Terminal connection1, connection2;
    WireGrid parentGrid;
    static ArrayList wires;

    static public Wire currentWire;
    static Vector3 CircuitCenter = new Vector3(0,0,-1);
    static int numComponents = 0;
    public Wire dummy;
    static Wire Dummy;
    LineRenderer LR;

    void Start() {
        if (Dummy == null) Dummy = this;
        else
        {

        }
        LR = GetComponent(typeof(LineRenderer)) as LineRenderer;
        LR.SetVertexCount(5);
        if (connection1 != null) {
            for (int i = 0; i < 5; i++)
                LR.SetPosition(i, connection1.transform.position);
        }
    }

    static public void PlaceComponent(Vector3 position) {
        CircuitCenter += position;
        numComponents++;
    }

    static public void RemoveComponent(Vector3 position) {
        CircuitCenter -= position;
        numComponents--;
    }

    static public Wire create(Terminal startTerminal) {
        Transform container = Instantiate(Dummy.transform) as Transform;
        currentWire = container.GetComponent(typeof(Wire)) as Wire;
        currentWire.Start();
        currentWire.connection1 = startTerminal;
        return currentWire;
    }

    static public void destroyCurrent() {
        Destroy(currentWire.gameObject);
        currentWire = null;
    }

    static public void confirmCurrent() {
        if (wires == null) wires = new ArrayList();
        wires.Add(currentWire);
    }

    void ComputeWire(Vector3 start, Vector3 end) {
        start.z = -1;
        end.z = -1;
        LR.SetPosition(0, start);
        LR.SetPosition(4, start);
        LR.SetPosition(2, end);

        Vector3 center = CircuitCenter / numComponents;
        center.z = -1;

        float x1 = end.x - center.x, x2 = start.x - center.x, y1 = end.y - center.y, y2 = start.y - center.y;
        float i1 = x1 * x1 + y2 * y2, i2 = x2 * x2 + y1 * y1;

        if (i1 >= i2)
        {
            LR.SetPosition(1, new Vector3(end.x, start.y, -1));
            LR.SetPosition(3, new Vector3(end.x, start.y, -1));
        }
        else
        {
            LR.SetPosition(1, new Vector3(start.x, end.y, -1));
            LR.SetPosition(3, new Vector3(start.x, end.y, -1));
        }
        
    }

    public void Drag(Vector3 position) {
        ComputeWire(connection1.transform.position, position);
    }

    public static void DestroyAll() {
        if (wires == null) return;
        foreach (Wire w in wires) {
            Destroy(w.gameObject);
        }
        currentWire = null;
        wires.Clear();
    }
}
