using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    InputChannel inputChannel;
    bool isJumping = false;
    bool isOnPlatform = true;
    [SerializeField] float sideDrag = 0.8f;
    public float maxDropVelocity { get; set; }
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        addListeners();
    }

    private void addListeners()
    {
        var bacon = FindObjectOfType<Beacon>();
        inputChannel = bacon.inputChannel;
        inputChannel.MoveEvent += HandleMovement;
    }

    private void Update()
    {

    }



    // for new input
    private void HandleMovement(Vector2 direction)
    {
        if(direction == Vector2.left)
        {
            HandleMovement(Direction.LEFT);
        }
        else if(direction == Vector2.right)
        {
            HandleMovement(Direction.RIGHT);
        }
    }

    private void HandleMovement(Direction direction)
    {
        if (isOnPlatform && direction == Direction.LEFT)
            return;
        if(isOnPlatform && direction == Direction.RIGHT)
        {
            // jump
            GameManager gm = FindObjectOfType<GameManager>();
            float sideForce = gm.playerSideJumpForce;
            float jumpForce = gm.playerJumpForce;
            ForceJump(sideForce, jumpForce, direction);
            isJumping = true;
            isOnPlatform = false;
            var bacon = FindObjectOfType<Beacon>();
            bacon.UIChannel.HideInstructionEvent();
            bacon.UIChannel.ShowSpeedCounter();
            StartCoroutine(JumpTimer());
        }
        if (!isJumping)
        {
            float playerLinearSideForce = FindObjectOfType<GameManager>().playerLinearSideForce;
            isJumping = true;
            ForceSide(playerLinearSideForce, direction);
            StartCoroutine(JumpTimer());
        }
    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
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
       // Debug.Log(rb.velocity + " " + maxDropVelocity);
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
       // Debug.Log("Force added: " + forceVector);
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
        float horizontalDrag = -rb.velocity.x * sideDrag;
        rb.velocity = new Vector2(rb.velocity.x + horizontalDrag * Time.fixedDeltaTime, rb.velocity.y);
    }

    public float GetCurrentFallingVelocity()
    {
        return rb.velocity.y;
    }

    private void OnDestroy()
    {
        inputChannel.MoveEvent -= HandleMovement;
    }

}
