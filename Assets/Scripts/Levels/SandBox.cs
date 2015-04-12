using UnityEngine;
using System.Collections;

public class SandBox : Challenge
{
    public SandBox()
    {
        ElementPile.BattPile.setup(1, false, this);
        ElementPile.ResPile.setup(1, true, this);
        ElementPile.LampPile.setup(1, true, this);
        ElementPile.SwitchPile.setup(1, true, this);
        ElementPile.AndPile.setup(1, true, this);
        ElementPile.OrPile.setup(1, true, this);
        ElementPile.NotPile.setup(1, true, this);

        
        CanvasScript.canvas.ShowInfo("Welcome to Free Play!\r\nEverything except  the battery is available in unlimited quantity");
    }
}
