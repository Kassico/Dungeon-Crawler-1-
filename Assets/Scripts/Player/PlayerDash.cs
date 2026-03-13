using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    private bool isDashing = false;
    private bool allowedToDash = true;

    private float dashTime;
    private float dashCooldownTimer;
    private bool _allowedToMove = true;

    public float dashSpeedmultiplier = 3f;
    public float dashDuration = 0.2f;
    public float playerMoveSpeed;
    public float dashCooldown = 1f;


    void Start() // hämtar dash cooldown från player data och move speed från player movement
    {
        if (playerData.instance != null && playerData.isInitialized)
        {
            dashCooldown = playerData.instance.dashCooldown;
        }

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMoveSpeed = playerMovement._moveSpeed;
        

    }

    // Update is called once per frame
    void Update() // kollar efter input för dashen och hanterar dash logiken, inklusive cooldown och att återställa hastigheten efter dash är klar
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        PlayerAudioManeger playerAudioManeger = GetComponent<PlayerAudioManeger>();

        PlayerMovement.allowedToMove = _allowedToMove;
        if (InputManager.Dash && allowedToDash )
        {
            
            //Debug.Log("DASH BUTTON PRESSED");
            isDashing = true;
            playerMoveSpeed *= dashSpeedmultiplier;
            playerAudioManeger.PlayDash();
            playerMovement._moveSpeed = playerMoveSpeed;
            _allowedToMove = false;
        }
        if (isDashing)
        {
            allowedToDash = false;

          

            dashTime += Time.deltaTime;
            if (dashTime >= dashDuration)
            {
                isDashing = false;
                dashTime = 0f;
                playerMoveSpeed /= dashSpeedmultiplier;
                playerMovement._moveSpeed = playerMoveSpeed;
                _allowedToMove = true;
            }
            //Debug.Log("Player is dashing!");
        }
        if (!allowedToDash)
        {
            dashCooldownTimer += Time.deltaTime;
            if (dashCooldownTimer >= dashCooldown)
            {
                allowedToDash = true;
                dashCooldownTimer = 0f;

                
            } 
        }


    }
}
