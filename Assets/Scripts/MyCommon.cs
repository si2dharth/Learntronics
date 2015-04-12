using UnityEngine;
using System.Collections;

public class MyCommon : MonoBehaviour {
    public static Vector2 GetMousePosition() {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition.x = mousePosition.x/Screen.width * 16 - 8;
        mousePosition.y = mousePosition.y/Screen.height * 10 - 5;
        return mousePosition;
    }
}
