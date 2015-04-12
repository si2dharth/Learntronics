using UnityEngine;
using System.Collections;

public class NOT : Element {

    bool state = false;

    public override float getResistance()
    {
        process();
        return -1;
    }

    public override void process()
    {
        if (Correspoding_Symbol.terminal1.wireGrid == null) return;
        if (Correspoding_Symbol.terminal2.wireGrid == null) return;
        state = !(Correspoding_Symbol.terminal1.wireGrid.voltage > 1.5);
        float voltage = 5;
        if (state) voltage = 5; else voltage = 0;
        Circuit.Grid.grid(Correspoding_Symbol.terminal2.wireGrid).applyVoltage(voltage);
    }
}
