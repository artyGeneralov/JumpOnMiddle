using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float sideDrag = 0.8f;
    public float maxDropVelocity { get; set; }
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {

        // cap max drop velocity
        if (Mathf.Abs(rb.velocity.y) > maxDropVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxDropVelocity);
        }


        //apply side drag

        ApplySideDrag();
        Debug.Log(rb.velocity + " " + maxDropVelocity);
    }


    public void ForceJump(float sideForce, float jumpForce, Direction direction = Direction.LEFT)
    {
        if (direction == Direction.UP || direction == Direction.DOWN) { return; }
        if (direction == Direction.LEFT) { sideForce = -sideForce; }
        rb.AddForce(new Vector2(sideForce, jumpForce), ForceMode2D.Impulse);
    }

    public void ForceSide(float amount, Direction direction)
    {
        Vector2 forceVector;
        if (direction == Direction.LEFT)
        {
            forceVector = Vector2.left * amount;
        }
        else
        {
            forceVector = Vector2.right * amount;
        }
        rb.AddForce(forceVector, ForceMode2D.Impulse);
    }

    public void ForceChange(float amount, Direction direction)
    {
        Vector2 forceVector = direction switch
        {
            _ when direction == Direction.UP => Vector2.up * amount,
            _ when direction == Direction.DOWN => Vector2.down * amount,
            _ when direction == Direction.LEFT => Vector2.left * amount,
            _ when direction == Direction.RIGHT => Vector2.right * amount,
            _ => new Vector2()
        };
        Debug.Log("Force added: " + forceVector);
        rb.AddForce(forceVector, ForceMode2D.Impulse);
    }

    public void DropVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    public void AddFlatVelocity(float amount)
    {
        rb.velocity += Vector2.down * amount;
    }

    private void ApplySideDrag()
    {
        // Calculate the horizontal drag based on the current horizontal velocity.
        float horizontalDrag = -rb.velocity.x * sideDrag;

        // Apply the horizontal drag, but keep the vertical velocity the same.
        rb.velocity = new Vector2(rb.velocity.x + horizontalDrag * Time.fixedDeltaTime, rb.velocity.y);
    }


}
