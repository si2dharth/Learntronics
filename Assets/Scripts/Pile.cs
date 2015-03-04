﻿using UnityEngine;
using System.Collections;

public class Pile : MonoBehaviour {

	public Transform TypeOfPile;
	private int numberOfObjects = 0;
    private Transform draggedObject;


	public void AddObject() {
		numberOfObjects++;
        if (numberOfObjects == 1)
        {
            renderer.enabled = true;
            Common.addPile();
            (GetComponent("Animator") as Animator).Play("MouseLeave_Anim");
            collider.enabled = true;
        }
	}

	void RemoveObject() {
		numberOfObjects--;
        if (numberOfObjects == 0)
        {
            (GetComponent("Animator") as Animator).SetTrigger("Hide");
            collider.enabled = false;
            Common.removePile();
        }
	}

    public void HidePile()
    {
        renderer.enabled = false;
    }

	void OnMouseDown() {
        draggedObject = Instantiate(TypeOfPile, transform.position, new Quaternion(0, 0, 0, 0)) as Transform;
        Common.startDrag(draggedObject.tag,0);
        draggedObject.renderer.material.color = new Color(1, 1, 1, 0.7f);
    }

	void OnMouseUp() {
        Vector2 returnVal = Common.stopDrag();
        if (returnVal.x == -0.5f)
        {
            Destroy(draggedObject.gameObject);
        }
        else
        {
            draggedObject.renderer.material.color = new Color(1, 1, 1, 1);
            draggedObject.position = Common.getMousePositionInUnits();
            draggedObject.position = returnVal;
            ShowStars(draggedObject);
            Common.addObject(draggedObject.gameObject);
            RemoveObject();
        }
        draggedObject = null;

    }

    void ShowStars(Transform t)
    {
        GameObject stars = GameObject.Find("Stars");
        //Instantiate(stars, t.position, t.rotation);
        stars.transform.position = t.position;
        stars.transform.rotation = t.rotation;
        (stars.GetComponent("Animator") as Animator).SetTrigger("Play");
    }

    void OnMouseDrag()
    {
        draggedObject.position = Common.getMousePositionInUnits();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            draggedObject.eulerAngles += new Vector3(0, 0, 90);
            Common.rotateToAngle((int)draggedObject.eulerAngles.z);
        }
    }
}