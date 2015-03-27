using UnityEngine;
using System.Collections;

public class CircuitComponent : MonoBehaviour {

    public Transform relatedSymbol;

    public void place() {
        if (!Common.tutorialLevel) {
            Transform t = Instantiate(relatedSymbol) as Transform;
            Common.addObject(t.gameObject);
            t.position = transform.position;
            t.eulerAngles = transform.eulerAngles;
            if (t.tag == "Battery") Common.battery = t;
        }
    }
}
