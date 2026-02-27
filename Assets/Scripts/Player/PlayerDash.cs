using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isDashing = false;
    public float dashSpeedmultiplier = 3f;
    public float dashDuration = 0.2f;
    private float dashTime;
    public float dashCooldown = 1f;
    private bool allowedToDash= true;
    private float dashCooldownTimer;
    public float playerMoveSpeed;

    private bool _allowedToMove = true;
   



    void Start()
    {
        if (playerData.instance != null && playerData.isInitialized)
        {
            dashCooldown = playerData.instance.dashCooldown;
        }

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMoveSpeed = playerMovement._moveSpeed;
        

    }

    // Update is called once per frame
    void Update()
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
                //Invoke("ResetDash", dashCooldown);
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
