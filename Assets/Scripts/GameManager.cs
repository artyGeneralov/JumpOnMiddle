using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject scissorsPrefab, baloonObsticlePrefab, powerUpPrefab, slowerObsticlePrefab;
    [SerializeField] GameObject splashPrefab;
    [SerializeField] GameObject leftWall, rightWall;
    [SerializeField] GameObject player;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject ground;
    [SerializeField] float playerJumpForce, playerSideJumpForce, playerLinearSideForce, playerForceChangeAmount, playerVelocityPerBaloon, playerBaseMaxDropVelocity, powerUpIncrease, slowerObsticleDecrease;
    [SerializeField] int numberOfScissors, numberOfPowerups, numberOfSlowerObsticles, numberOfBaloonObsticles;
    [SerializeField] float maxVelocityForSplatter, maxScaleForSplatter;
    [SerializeField] GameObject UIManagerObject;
    PlayerController playerController;
    BaloonsManager baloonsManager;
    UIManager uiManager;


    bool isJumping;
    bool isOnPlatform;

    float leftBound;
    float rightBound;

    void Start()
    {

        isJumping = false;
        isOnPlatform = true;
        playerController = player.GetComponent<PlayerController>();
        baloonsManager = FindObjectOfType<BaloonsManager>();
        playerController.maxDropVelocity = playerBaseMaxDropVelocity - (playerVelocityPerBaloon * baloonsManager.GetCurrentBaloonCount());

        uiManager = UIManagerObject.GetComponent<UIManager>();
        ObsticleEventController groundEvent = ground.GetComponent<ObsticleEventController>();
        if (groundEvent)
        {
            groundEvent.touchedPlayer += HandleGroundCollision;
        }

        leftBound = leftWall.transform.position.x + 5;
        rightBound = rightWall.transform.position.x - 5;

        GenerateScissors();
        GeneratePowerUps();
        GenerateObsticles();
    }




    void Update()
    {



        // listen for keys
        KeyListeners();


        // update current speed UI.
        uiManager.updateSpeed((int)Mathf.Abs(playerController.GetCurrentFallingVelocity()));


    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(0.1f);
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
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerController.ForceJump(playerSideJumpForce, playerJumpForce, PlayerController.Direction.RIGHT);
                isOnPlatform = false;
                uiManager.HideInstructions();
                uiManager.SpawnSpeedCounter();
            }
        }    
    }

    void HandleScissors()
    {
        baloonsManager.RemoveBaloon();
        playerController.maxDropVelocity = playerBaseMaxDropVelocity - (playerVelocityPerBaloon * baloonsManager.GetCurrentBaloonCount());
    }

    void HandlePowerUp()
    {
        // add current speed
        if (baloonsManager.GetCurrentBaloonCount() == 0)
        {
            playerController.maxDropVelocity += powerUpIncrease;
        }
        playerController.AddFlatVelocity(powerUpIncrease);
    }

    void HandleSlowerObsticle()
    {
        playerController.DropVelocity();
    }

    void HandleBaloonPickup()
    {
        baloonsManager.AddBaloon();
        playerController.maxDropVelocity = playerBaseMaxDropVelocity - (playerVelocityPerBaloon * baloonsManager.GetCurrentBaloonCount());
    }

    void HandleGroundCollision()
    {
        // destroy player object
        GameObject splash = Instantiate(splashPrefab, player.transform.position, Quaternion.identity);
        float currentFallingVelocity = playerController.GetCurrentFallingVelocity();
        Renderer rend = player.GetComponent<Renderer>();
        if (rend != null)
        {
            Color color = rend.material.color;
            color.a = 0f;
            rend.material.color = color;
        }
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        baloonsManager.removeAllBaloons();
        // spawn splatter with radius proportional to final speed

        float proportion = Mathf.Clamp(Mathf.Abs(currentFallingVelocity) / maxVelocityForSplatter, 0f, 1f);
        float targetScale = proportion * maxScaleForSplatter;

        splash.transform.localScale = new Vector3(targetScale, 0.5f, 0);

        uiManager.HideSpeedCounter();
        uiManager.SpawnEndGameSummary((int)Mathf.Abs(currentFallingVelocity));
        //Debug.Log($"targetscale {targetScale}, proportion {proportion}, currentFallingVelocity, {currentFallingVelocity}");


        // 

    }


    void GenerateScissors()
    {
        for (int i = 0; i < numberOfScissors; i++)
        {
            // generate scissors in upper half 

            // height bounds:
            float upperBound = platform.transform.position.y - 50;
            float lowerBound = platform.transform.position.y / 2;



            float randPosY = Random.Range(lowerBound, upperBound);
            float randPosX = Random.Range(leftBound, rightBound);

            GameObject newScissors = Instantiate(scissorsPrefab, new Vector3(randPosX, randPosY, 0), Quaternion.identity);
            ObsticleEventController eventController = newScissors.GetComponent<ObsticleEventController>();
            if (eventController)
            {
                eventController.touchedPlayer += HandleScissors;
            }

        }
    }

    void GeneratePowerUps()
    {
        // generate powerups in lower half
        for (int i = 0; i < numberOfPowerups; i++)
        {

            // height bounds:
            float upperBound = platform.transform.position.y / 2;
            float lowerBound = 50;


            float randPosY = Random.Range(lowerBound, upperBound);
            float randPosX = Random.Range(leftBound, rightBound);

            GameObject newPowerUp = Instantiate(powerUpPrefab, new Vector3(randPosX, randPosY, 0), Quaternion.identity);
            ObsticleEventController eventController = newPowerUp.GetComponent<ObsticleEventController>();
            newPowerUp.transform.localScale = new Vector3(-1f, -1f, 1f);
            if (eventController)
            {
                eventController.touchedPlayer += HandlePowerUp;
            }

        }
    }

    void GenerateObsticles()
    {
        // generate baloons obsticles in lower half
        for (int i = 0; i < numberOfBaloonObsticles; i++)
        {

            // height bounds:
            float upperBound = platform.transform.position.y / 2;
            float lowerBound = 50;


            float randPosY = Random.Range(lowerBound, upperBound);
            float randPosX = Random.Range(leftBound, rightBound);

            GameObject newBaloonObsticle = Instantiate(baloonObsticlePrefab, new Vector3(randPosX, randPosY, 0), Quaternion.identity);
            ObsticleEventController eventController = newBaloonObsticle.GetComponent<ObsticleEventController>();
            if (eventController)
            {
                eventController.touchedPlayer += HandleBaloonPickup;
            }

        }

        // generate slowdown obsticles everywhere
        for (int i = 0; i < numberOfSlowerObsticles; i++)
        {

            // height bounds:
            float upperBound = platform.transform.position.y - 50;
            float lowerBound = 50;


            float randPosY = Random.Range(lowerBound, upperBound);
            float randPosX = Random.Range(leftBound, rightBound);

            GameObject newSlowerObsticle = Instantiate(slowerObsticlePrefab, new Vector3(randPosX, randPosY, 0), Quaternion.identity);
            ObsticleEventController eventController = newSlowerObsticle.GetComponent<ObsticleEventController>();
            if (eventController)
            {
                eventController.touchedPlayer += HandleSlowerObsticle;
            }

        }
    }


}
