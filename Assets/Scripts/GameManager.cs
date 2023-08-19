using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject scissorsPrefab, baloonObsticlePrefab, powerUpPrefab;
    [SerializeField] GameObject player;
    [SerializeField] GameObject platform;
    [SerializeField] float playerJumpForce, playerSideJumpForce, playerLinearSideForce, playerForceChangeAmount, playerVelocityPerBaloon, playerBaseMaxDropVelocity;
    [SerializeField] int numberOfScissors, numberOfPowerups, numberOfObsticles;
    PlayerController playerController;
    BaloonsManager baloonsManager;


    bool isJumping;
    bool isOnPlatform;

    void Start()
    {

        isJumping = false;
        isOnPlatform = true;
        playerController = player.GetComponent<PlayerController>();
        baloonsManager = FindObjectOfType<BaloonsManager>();
        playerController.maxDropVelocity = playerBaseMaxDropVelocity - (playerVelocityPerBaloon * baloonsManager.GetCurrentBaloonCount());

        // generate scissors

        // generate powerups

        // generate obsticles

    }




    void Update()
    {



        // listen for keys
        KeyListeners();

        
        


    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(0.85f);
        isJumping = false;
    }

    void KeyListeners()
    {
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


    void GenerateScissors()
    {

    }

    void GeneratePowerUps()
    {

    }

    void GenerateObsticles()
    {

    }
}
