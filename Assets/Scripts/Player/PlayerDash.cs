using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isDashing = false;
    public float dashSpeedmultiplier = 3f;
    public float dashDuration = 0.2f;
    private float dashTime;
    public static float dashCooldown = 1f;
    private bool allowedToDash= true;
    private float dashCooldownTimer;

    private bool _allowedToMove = true;
    


    void Start()
    {
        if (playerData.instance != null && playerData.isInitialized)
        {
            dashCooldown = playerData.instance.dashCooldown;
        }


    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement.allowedToMove = _allowedToMove;
        if (InputManager.Dash && allowedToDash )
        {
            
            Debug.Log("DASH BUTTON PRESSED");
            isDashing = true;
            PlayerMovement._moveSpeed *= dashSpeedmultiplier;
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
                PlayerMovement._moveSpeed /= dashSpeedmultiplier;
                Invoke("ResetDash", dashCooldown);
                _allowedToMove = true;
            }
            Debug.Log("Player is dashing!");
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
