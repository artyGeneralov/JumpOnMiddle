using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject platform;
    [SerializeField] float playerJumpForce, playerSideJumpForce, playerLinearSideForce, playerForceChangeAmount;
    PlayerController playerController;


    bool canUpdate;
    bool isJumping;
    bool isOnPlatform;
    // Start is called before the first frame update
    void Start()
    {
        canUpdate = false;
        isJumping = false;
        isOnPlatform = true;
        playerController = player.GetComponent<PlayerController>();
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        while (playerController == null)
        {
            yield return null;
        }
        canUpdate = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!canUpdate)
            return;


        // listen for keys

        // side push
        if (!isOnPlatform)
        {
            if (Input.GetKeyDown(KeyCode.A) && !isJumping)
            {
                isJumping = true;
                playerController.ForceSide(playerLinearSideForce, PlayerController.Direction.LEFT);
                StartCoroutine(JumpTimer());
            }
            else if (Input.GetKeyDown(KeyCode.D) && !isJumping)
            {
                isJumping = true;
                playerController.ForceSide(playerLinearSideForce, PlayerController.Direction.RIGHT);
                StartCoroutine(JumpTimer());
            }
        }
        else if (isOnPlatform)
        {

            if (Input.GetKeyDown(KeyCode.A) && !isJumping)
            {
                isJumping = true;
                playerController.ForceJump(playerSideJumpForce, playerJumpForce, PlayerController.Direction.LEFT);
                StartCoroutine(JumpTimer());
            }
            else if (Input.GetKeyDown(KeyCode.D) && !isJumping)
            {
                isJumping = true;
                playerController.ForceJump(playerSideJumpForce, playerJumpForce, PlayerController.Direction.RIGHT);
                StartCoroutine(JumpTimer());
            }
        }

        if (player.transform.position.y < platform.transform.position.y)
        {
            isOnPlatform = false;
        }
    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(1);
        isJumping = false;
    }
}
