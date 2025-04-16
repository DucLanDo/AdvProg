using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    private Vector3 direction;

    // Update is called once per frame
    void FixedUpdate()
    {   
        //face the player
        if (PlayerController.Instance.transform.position.x > transform.position.x) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }

        //move towards player
        direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

    }
}
