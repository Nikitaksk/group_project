using UnityEngine;

// This is a simple test script to see if mouse clicks are detected on this object.
public class ClickTester : MonoBehaviour
{
    void OnMouseDown()
    {
        // If this message appears in the console, then the click is being detected.
        Debug.Log(gameObject.name + " was clicked!", this.gameObject);
    }
}
