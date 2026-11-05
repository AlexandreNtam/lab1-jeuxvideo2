using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private GroundCheck groundCheck;

        [SerializeField] private float moveSpeed = 2f; // Vélocité de déplacement de 2[cite: 3]
        [SerializeField] private float maxJumpVelocity = 8f; // Vélocité maximale de saut de 8[cite: 3]
        [SerializeField] private float minJumpVelocity = 3f; // Vélocité de départ du saut chargé de 3

        [SerializeField] private float coyoteTimeMax = 0.2f; // Coyote time de 0.2 seconde[cite: 5]
        private float coyoteTimeCounter;
        private float jumpPressTimer;
        private bool isChargingJump;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip jumpSound;

        private bool isBlockedByWall = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
            if (groundCheck == null) groundCheck = GetComponentInChildren<GroundCheck>();
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // Gestion du Coyote Time[cite: 44]
            if (groundCheck != null && groundCheck.IsGrounded)
            {
                coyoteTimeCounter = coyoteTimeMax;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            HandleJumpInput();
        }

        private void FixedUpdate()
        {
            float targetSpeed = isBlockedByWall ? 0f : inputReader.MoveInput.x * moveSpeed;
            rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
        }

        private void HandleJumpInput()
        {
            bool canJump = groundCheck.IsGrounded || coyoteTimeCounter > 0f;

            // Saut de base / Début du saut chargé
            if (inputReader.JumpTriggered && canJump && !isChargingJump)
            {
                isChargingJump = true;
                jumpPressTimer = 0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, minJumpVelocity);
                PlayJumpSound();
            }

            // Saut chargé (sans coroutines tel que demandé)[cite: 5]
            if (isChargingJump)
            {
                if (inputReader.JumpHeld && jumpPressTimer < 0.25f) // 0.25 seconde de maintien[cite: 5]
                {
                    jumpPressTimer += Time.deltaTime;
                    float additionalVelocity = Mathf.Lerp(minJumpVelocity, maxJumpVelocity, jumpPressTimer / 0.25f);
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, additionalVelocity);
                }
                else
                {
                    isChargingJump = false;
                }
            }

            if (!inputReader.JumpHeld)
            {
                isChargingJump = false;
            }

            inputReader.ResetJumpTrigger();
        }

        private void PlayJumpSound()
        {
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        // Rester pris dans les murs (Collision Enter et Stay)
        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckWallCollision(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CheckWallCollision(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isBlockedByWall = false; // Éliminer la condition en exit
        }

        private void CheckWallCollision(Collision2D collision)
        {
            if (groundCheck != null && !groundCheck.IsGrounded)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    isBlockedByWall = true; // Vélocité horizontale passe à 0
                }
            }
        }
    }
}