using UnityEngine;
using System.Collections;

public class ElementPile : MonoBehaviour {
    public Element element;

    public static ElementPile ResPile, BattPile, LampPile, SwitchPile, AndPile, OrPile, NotPile;
    public GameObject InfoBox;

    bool unlimited = false;
    int numberOfElements = 0;
    Level currentLevel;
    Animator animator, infoAnimator;

    void Start() {
        animator = GetComponent(typeof(Animator)) as Animator;
        infoAnimator = InfoBox.GetComponent(typeof(Animator)) as Animator;
        if (tag == "Resistor") ResPile = this;
        if (tag == "Battery") BattPile = this;
        if (tag == "Lamp") LampPile = this;
        if (tag == "Switch") SwitchPile = this;
        if (tag == "AND") AndPile = this;
        if (tag == "OR") OrPile = this;
        if (tag == "NOT") NotPile = this;
        collider.enabled = false;
    }

    public void setup(int numberOfElements, bool unlimited, Level level) {
        this.unlimited = unlimited;
        this.numberOfElements = numberOfElements;
        setLevel(level);
        if (numberOfElements > 0) ShowPile();
    }

    void setLevel(Level level) {
        currentLevel = level;
    }

    public int getNumberOfElements() { 
        return numberOfElements;
    }

    void addElement() {
        if (numberOfElements == 0) ShowPile();
        numberOfElements++;
    }

    void removeElement() {
        if (unlimited) return;
        numberOfElements--;
        if (numberOfElements <= 0 && !unlimited) {
            if (currentLevel != null)
                currentLevel.PileEmpty(this);
            HidePile();
        }
    }

    public void ShowPile() {
        if (collider.enabled) return;
        animator.SetTrigger("Show");
        collider.enabled = true;
    }

    public void HidePile() {
        if (!collider.enabled) return;
        collider.enabled = false;
        animator.SetTrigger("Hide");
    }


    public static Transform draggedElement = null;
    void OnMouseDown() {
        if (draggedElement) return;
        draggedElement = Instantiate(element.transform) as Transform;
        draggedElement.position = MyCommon.GetMousePosition();
    }

    void OnMouseUp() {
        bool success = currentLevel.dropElement(draggedElement);
        (draggedElement.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).color = Color.white;
        if (!success) {
            Destroy(draggedElement.gameObject);
        }
        else {
            Wire.PlaceComponent(draggedElement.position);
            removeElement();
        }
        draggedElement = null;
    }

    void OnMouseDrag() {
        draggedElement.position = MyCommon.GetMousePosition();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            draggedElement.eulerAngles += new Vector3(0, 0, 90);
        }
        if (Symbol.underMouse != null) {
            if (Symbol.underMouse.CheckElement(draggedElement))
                (draggedElement.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).color = new Color(0, 1f, 0, 0.75f);
            else
                (draggedElement.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).color = new Color(1f, 0 , 0, 0.75f);
        }
        else
            (draggedElement.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer).color = Color.white;
    }

    void OnMouseEnter() {
        animator.SetTrigger("Mouse Enter");
        infoAnimator.SetTrigger("Show");
    }

    void OnMouseExit() {
        animator.SetTrigger("Mouse Leave");
        infoAnimator.SetTrigger("Hide");
    }
}
