using UnityEngine;
using System.Collections;

public class Resistance {

    static ArrayList Resistors;

    float resistance;

    ArrayList conn1, conn2;

    public Resistance() {
        if (Resistors == null) Resistors = new ArrayList();
        Resistors.Add(this);
        resistance = 10;
        conn1 = new ArrayList();
        conn2 = new ArrayList();
    }

    public void addConnection(int n, Resistance r) {
        if (n == 1) if (!conn1.Contains(r)) conn1.Add(r);
        if (n == 2) if (!conn2.Contains(r)) conn2.Add(r);
    }

    static bool checkParallels() {
        bool found = false;
        for (int i = 0; i < Resistors.Count; i++) {
            Resistance r = Resistors[i] as Resistance;
            for (int j = 0; j < r.conn1.Count; j++) {
                Resistance r2 = r.conn1[j] as Resistance;
                if (r.conn2.Contains(r2)) {
                    r.conn1.RemoveAt(j);
                    r.conn2.RemoveAt(j);
                    j--;
                    r.resistance = (r.resistance * r2.resistance)/ (r.resistance + r2.resistance);
                    Resistors.Remove(r2);
                    found = true;
                }
            }
        }
        return found;
    }

    static bool checkSeries() {
        bool found = false;
        for (int i = 0; i < Resistors.Count; i++) {
            Resistance r = Resistors[i] as Resistance;

            if (r.conn1.Count == 1) {
                MonoBehaviour.print("T1");
                Resistance r2 = r.conn1[0] as Resistance;
                if (r2.conn1.Count == 1 && r2.conn1[0] == r) {
                    MonoBehaviour.print("::T1");
                    r.conn1.Clear();
                    r.conn1.AddRange(r2.conn2);
                    r.resistance = r.resistance + r2.resistance;
                    Resistors.Remove(r2);
                    found = true;
                } else if (r2.conn2.Count == 1 && r2.conn2[0] == r) {
                    MonoBehaviour.print("::T2");
                    r.conn1.Clear();
                    r.conn1.AddRange(r2.conn1);
                    r.resistance = r.resistance + r2.resistance;
                    Resistors.Remove(r2);
                    found = true;
                }
            } else if (r.conn2.Count == 1)
            {
                Resistance r2 = r.conn2[0] as Resistance;
                MonoBehaviour.print("T2");
                if (r2.conn1.Count == 1 && r2.conn1[0] == r)
                {
                    MonoBehaviour.print("::T1");
                    r.conn2.Clear();
                    r.conn2.AddRange(r2.conn2);
                    r.resistance = r.resistance + r2.resistance;
                    Resistors.Remove(r2);
                    found = true;
                } else if (r2.conn2.Count == 1 && r2.conn2[0] == r)
                {
                    MonoBehaviour.print("::T2");
                    r.conn2.Clear();
                    r.conn2.AddRange(r2.conn1);
                    r.resistance = r.resistance + r2.resistance;
                    Resistors.Remove(r2);
                    found = true;
                }
            }
        }
        return found;
    }

    public static void findEquivalentResistance() {
        if (Resistors == null) return;
        bool found = true;

        while (found) {
            checkParallels();
            found = checkSeries();
        }
    }

    public static void printResistance() {
        if (Resistors == null) return;
        foreach (Resistance r in Resistors) {
            MonoBehaviour.print(r.resistance);
        }
    }
}
