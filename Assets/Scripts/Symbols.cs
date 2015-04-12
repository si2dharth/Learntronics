using UnityEngine;
using System.Collections;

public class Symbols : MonoBehaviour {

    public Symbol resistorSym, lampSym, batterySym, switchSym, andSym, orSym, notSym;
	// Use this for initialization
	void Start () {
        Symbol.parent = this;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
