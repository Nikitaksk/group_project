using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantFallSpeed : MonoBehaviour
{
    public float fallSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component attached to this object
        rb = GetComponent<Rigidbody2D>();

        // Set the velocity to a constant downward speed
        // The x component is 0 (no sideways movement), and the y component is -fallSpeed.
        rb.linearVelocity = new Vector2(0, -fallSpeed);
    }

    // This function is called when the object collides with another 2D collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with has the "Ground" tag
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Destroy this falling object
            Destroy(gameObject);
        }
    }

    // This function is called when the renderer is no longer visible by any camera
    void OnBecameInvisible()
    {
        // Destroy this falling object
        Destroy(gameObject);
    }
}
