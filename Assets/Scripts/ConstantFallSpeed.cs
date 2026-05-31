using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantFallSpeed : MonoBehaviour
{
    public float fallSpeed = 2f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(0f, -fallSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
