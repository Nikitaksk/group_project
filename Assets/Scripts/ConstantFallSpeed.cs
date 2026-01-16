using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantFallSpeed : MonoBehaviour
{
    public float fallSpeed = 0.5f;
    private Rigidbody2D rb;

    void Awake()
    {
        // Get the Rigidbody2D component attached to this object
        rb = GetComponent<Rigidbody2D>();
    }

    // OnEnable is called every time this component is set to active
    void OnEnable()
    {
        // Set the velocity to a constant downward speed
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
