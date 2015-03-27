using UnityEngine;
using System.Collections;

public class Levels : MonoBehaviour {

    public static bool[] tutorials = {
        //false,
        true,
        true,
    };

    public static string[] Level = {
        //"A0 -0 -0 -0 A3\r\n-1 N N N -1\r\nB0 N N N L1\r\n-1 N N N -1\r\nA1 -0 -0 -0 A2\r\n>100",
       // "A0 -0 R0 -0 A3\r\n-1 N N N -1\r\nB0 N N N L1\r\n-1 N N N -1\r\nA1 -0 -0 -0 A2\r\n>85",
        "A0 -0 R0 -0 R0 -0 A3\r\n-1 N N N N N -1\r\n-1 N N N N N -1\r\nB0 N N N N N L1\r\n-1 N N N N N -1\r\nA1 -0 -0 -0 -0 -0 A2\r\n>75",
        "A0 -0 T0 R0 T0 -0 A3\r\n-1 N A1 R0 A2 N -1\r\n-1 N N N N N -1\r\nB0 N N N N N L1\r\n-1 N N N N N -1\r\nA1 -0 -0 -0 -0 -0 A2\r\n>90",
    };

    public static string[] LevelConnections = {
        //"Lamp Lamp\r\nBattery Battery",
        "12 21\r\n32 01\r\n02 31\r\n22 11",
        "32 21\r\n32 21\r\n02,12 31\r\n22 01,11",
    };

    public static string[] LevelInfo = {
      //  "This is a very simple circuit: Consists of a battery and a lamp. \r\nDrag a lamp from the lamp pile on the lamp symbol, and a battery from a battery pile on the battery symbol.",
      //  "This is just like the previous circuit, with a resistor added to the loop.\r\nDrag the components onto their corresponding symbols.",
        "Drag the components onto their corresponding symbols to make a series circuit.",
        "Drag the components onto their corresponding symbols to make a parallel circuit.",
        "Create a simple loop using a battery and a lamp",
    };

    public static string[] LevelMoral = {
      //  "As you see, the circuit is said to be complete when a loop is formed.\r\nThis is because current flows from the positive end to the negative end of the battery.\r\nClick on Next to move to the next level.",
      //  "You can see that the brightness of the bulb is lesser than that in the previous level.\r\nThis is because the flow of current decreases when passing through a resistor.\r\nThe factor by which the current is decreased depends on the \"resistance\" of the resistor",
        "Great Work. Press Next to continue",
        "Nicely Done. Press Next to continue",
        "Great Work. Press Next for next challenge",
                                        };

    public static int maxLevels()
    {
        return Level.Length;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
