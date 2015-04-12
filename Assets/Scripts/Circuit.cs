using UnityEngine;
using System.Collections;

public class Circuit
{

    public class Grid
    {
        public int nComponents;

        static Hashtable gridGrid;
        WireGrid corres_Grid;

        public static void Clear()
        {
            if (gridGrid != null) gridGrid.Clear();
        }

        public float V;
        Grid(WireGrid corres_grid)
        {
            nComponents = 0;
            V = -1;
            corres_Grid = corres_grid;
        }

        public static Grid grid(WireGrid w)
        {
            if (gridGrid == null) gridGrid = new Hashtable();
            if (!gridGrid.ContainsKey(w))
            {
                gridGrid.Add(w, new Grid(w));
                w.setVoltage(-1);
            }
            return gridGrid[w] as Grid;
        }

        public void applyVoltage(float value, bool force = true)
        {

            if (V >= 0 && !force) return;
            V = value;
            corres_Grid.setVoltage(V);
        }

        internal void add() { nComponents++; }
        internal void remove() { nComponents--; }
    };

    public class Resistor
    {
        Grid Pos, Neg;
        float Val;
        bool addedToGrid = false;

        Resistor component1, component2;
        Resistor parent;
        bool parallel = false;


        internal Resistor(Grid pos, Grid neg, float val)
        {
            Pos = pos;
            Neg = neg;
            Val = val;
        }

        internal Resistor(Resistor r1, Resistor r2, bool parallel)
        {
              MonoBehaviour.print(r1.Val + " + " + r2.Val);
            component1 = r1;
            component2 = r2;
            r1.parent = r2.parent = this;
            this.parallel = parallel;
            if (parallel)
            {
                if (r1.Val == r2.Val) Val = r1.Val / 2;
                else              //Takes care of the values being zero.
                    Val = r1.Val * r2.Val / (r1.Val + r2.Val);
            }
            else Val = r1.Val + r2.Val;

            if (parallel)
            {
                Pos = r1.Pos;
                Neg = r1.Neg;
            }
            else
            {
                if (r1.Pos == r2.Neg) { Pos = r2.Pos; Neg = r1.Neg; }
                if (r1.Pos == r2.Pos) { Pos = r1.Neg; Neg = r2.Neg; }
                if (r1.Neg == r2.Pos) { Pos = r1.Pos; Neg = r2.Neg; }
                if (r1.Neg == r2.Neg) { Pos = r1.Pos; Neg = r2.Pos; }
            }
        }

        public float GetCurrent()
        {
            if (parent == null)
            {
                if (Pos.V < 0 || Neg.V < 0) return 0;
                return (Pos.V - Neg.V) / Val;
            }
            else
            {
                return parent.GetCurrentForComponent(this);
            }
        }

        internal void setVoltages()
        {
            float current = GetCurrent();
            if (Pos.V != -1) if (Neg.V == -1) Neg.applyVoltage(Pos.V - current * Val);
            if (Neg.V != -1) if (Pos.V == -1) Pos.applyVoltage(Neg.V + current * Val);
        }

        float GetCurrentForComponent(Resistor comp)
        {
            if (!parallel) return GetCurrent();
            else
            {
                Resistor otherComponent;
                if (component1 == comp) otherComponent = component2; else otherComponent = component1;
                return (GetCurrent() * otherComponent.Val / (component1.Val + component2.Val));
            }
        }

        internal void AddToGrid()
        {
            if (addedToGrid) return;
            Pos.add();
            Neg.add();
            addedToGrid = true;
        }

        internal void RemoveFromGrid()
        {
            if (!addedToGrid) return;
            Pos.remove();
            Neg.remove();
            addedToGrid = false;
        }

        static internal bool inParallel(Resistor r1, Resistor r2)
        {
            if (!r1.addedToGrid || !r2.addedToGrid) return false;
            if (r1.Pos == null || r1.Neg == null || r2.Pos == null || r2.Neg == null) return false;
            if (r1.Pos == r2.Pos && r1.Neg == r2.Neg) return true;
            return (r1.Pos == r2.Neg && r1.Neg == r2.Pos);
        }

        static internal bool inSeries(Resistor r1, Resistor r2)
        {
            //MonoBehaviour.print("Series 0");
            if (!r1.addedToGrid || !r2.addedToGrid) return false;
            if (r1.Pos == null || r1.Neg == null || r2.Pos == null || r2.Neg == null) return false;
            //MonoBehaviour.print("Series 1");
            if (r1.Pos != r2.Pos && r1.Pos != r2.Neg)
            {
                // MonoBehaviour.print("Series 1_2");
                if (r1.Neg == r2.Pos || r1.Neg == r2.Neg)
                {
                    //   MonoBehaviour.print("Series 1_3");
                    //  MonoBehaviour.print(r1.Neg.nComponents);
                    if (r1.Neg.nComponents == 2)
                    {
                        //    MonoBehaviour.print("Series 1_4");
                        return true;
                    }
                }
            }
            if (r1.Neg != r2.Pos && r1.Neg != r2.Neg)
            {
                //MonoBehaviour.print("Series 2_2");
                if (r1.Pos == r2.Pos || r1.Pos == r2.Neg)
                {
                    //  MonoBehaviour.print("Series 2_3");
                    //MonoBehaviour.print(r1.Pos.nComponents);

                    if (r1.Pos.nComponents == 2)
                    {
                        //  MonoBehaviour.print("Series 2_4");
                        return true;
                    }
                }
            }
            return false;
        }

        static internal bool inSelfLoop(Resistor r1)
        {
            if (r1.Pos == null || r1.Neg == null) return false;
            return r1.Pos == r1.Neg;
        }

        static internal Resistor CombineInSeries(Resistor r1, Resistor r2)
        {
            r1.RemoveFromGrid();
            r2.RemoveFromGrid();
            Resistor r = new Resistor(r1, r2, false);
            r.AddToGrid();
            return r;
        }

        static internal Resistor CombineInParallel(Resistor r1, Resistor r2)
        {
            r1.RemoveFromGrid();
            r2.RemoveFromGrid();
            Resistor r = new Resistor(r1, r2, true);
            r.AddToGrid();
            return r;
        }

    }

    ArrayList resistors;

    public Resistor addResistor(WireGrid pos, WireGrid neg, float val)
    {
        if (resistors == null) resistors = new ArrayList();
        Resistor r = new Resistor(Grid.grid(pos), Grid.grid(neg), val);
        resistors.Add(r);
        return r;
    }

    public void removeAllResistors()
    {
        if (resistors != null)
        {
            Grid.Clear();
            resistors.Clear();
        }
    }

    public void simulate()
    {
        ArrayList curList = new ArrayList();

        if (resistors == null) return;
        foreach (Resistor r in resistors)
        {
            r.AddToGrid();
        }

        curList.AddRange(resistors);
        bool found = true;

        while (found)
        {
            found = false;

            for (int i = 0; i < curList.Count; i++)
            {
                Resistor r1 = curList[i] as Resistor;
                if (Resistor.inSelfLoop(r1))
                {
                    MonoBehaviour.print("Self Loop");
                    curList.Remove(r1);
                    i--;
                    found = true;
                }
            }

            for (int i = 0; i < curList.Count; i++)
            {
                Resistor r1 = curList[i] as Resistor;
                for (int j = 0; j < curList.Count; j++)
                {
                    if (i == j) continue;
                    Resistor r2 = curList[j] as Resistor;
                    if (Resistor.inParallel(r1, r2))
                    {
                         MonoBehaviour.print("Parallel");
                        curList.Add(Resistor.CombineInParallel(r1, r2));
                        curList.Remove(r1);
                        curList.Remove(r2);
                        i--;
                        found = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < curList.Count; i++)
            {
                Resistor r1 = curList[i] as Resistor;
                for (int j = 0; j < curList.Count; j++)
                {
                    if (i == j) continue;
                    Resistor r2 = curList[j] as Resistor;
                    if (Resistor.inSeries(r1, r2))
                    {
                         MonoBehaviour.print("Series");
                        curList.Add(Resistor.CombineInSeries(r1, r2));
                        curList.Remove(r1);
                        curList.Remove(r2);
                        i--;
                        found = true;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < resistors.Count; i++)
        {
            foreach (Resistor r in resistors)
                r.setVoltages();
        }
        // if (curList.Count > 1) MonoBehaviour.print("Problem!");
    }

    static Circuit main;

    public static Circuit circuit()
    {
        if (main == null) main = new Circuit();
        return main;
    }
}
