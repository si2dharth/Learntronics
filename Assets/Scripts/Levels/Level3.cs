using UnityEngine;
using System.Collections;

public class Level3 : Challenge
{

    public Level3()
    {
        nPiles = 3;
        ElementPile.LampPile.setup(3, false, this);
        ElementPile.SwitchPile.setup(1, false, this);
        ElementPile.BattPile.setup(1, false, this);

        showInfo();
    }

    public override void showInfo()
    {
        CanvasScript.canvas.ShowInfo("Connect three bulbs, a switch and a battery so that two bulbs are always on.\r\nThe third bulb is controlled by a switch");
    }

    public override bool checkComplete()
    {
        if (nPiles > 0) return false;

        ArrayList lamps = new ArrayList();
        Switch sw = null;
        int nLampsOn = 0;
        foreach (Transform t in elements)
        {
            Element e = t.GetComponent(typeof(Element)) as Element;
            if (!e.connected()) return false;
            if (t.tag == "Lamp")
            {
                Lamp l = e as Lamp;
                lamps.Add(l);
                if (l.current != 0) nLampsOn++;
            }
            else if (t.tag == "Switch") sw = e as Switch;

        }
        if (nLampsOn < 2) return false;
       // MonoBehaviour.print("Olfd " + nLampsOn);
        int newNLampsOn = 0;
        //return false;
        //MonoBehaviour.print("Toggling");
        sw.toggle();
        run();
        foreach (Lamp l in lamps)
        {
            if (l.current != 0) newNLampsOn++;
        }
        //MonoBehaviour.print(" Change " + Mathf.Abs(newNLampsOn - nLampsOn));
        if (Mathf.Abs(newNLampsOn - nLampsOn) != 1 || newNLampsOn < 2) return false;        

        return true;
    }
}
