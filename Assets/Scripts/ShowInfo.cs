using UnityEngine;
using System.Collections;

public class ShowInfo : MonoBehaviour {

	public GameObject InfoBox_To_Display;

	void OnMouseOver() {
        if (!renderer.enabled) return;
		if (InfoBox_To_Display.renderer.material.color.a < 1)
		InfoBox_To_Display.renderer.material.color += new Color(0f,0f,0f,0.05f);
	}

	void OnMouseExit() {
		InfoBox_To_Display.renderer.material.color = new Color(1f,1f,1f,0.0f);
	}

	// Use this for initialization
	void Start () {
		InfoBox_To_Display.renderer.material.color = new Color(1f,1f,1f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
