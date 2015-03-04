using UnityEngine;
using System.Collections;

public class HighLight : MonoBehaviour {

    private Animator pileAnimator;

    void Start()
    {
        pileAnimator = GetComponent("Animator") as Animator;
    }

	void OnMouseEnter() {
	    if (pileAnimator ==null)	transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
        else pileAnimator.SetTrigger("Mouse Enter");
	}

	void OnMouseExit() {
        if (pileAnimator == null) transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        else pileAnimator.SetTrigger("Mouse Leave");
	}
}
