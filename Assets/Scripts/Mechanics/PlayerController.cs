using UnityEngine;
using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Model;


namespace Platformer.Mechanics
{
   public class PlayerController : KinematicObject
   {
       // reference to the virtual camera dedicated to this player
       [Tooltip("Cinemachine virtual camera that follows this player")]
       public Unity.Cinemachine.CinemachineCamera vcam;

       // allows different instances (or subclasses) to use alternate keys
       public KeyCode jumpKey = KeyCode.Space;

       public AudioClip jumpAudio;
       public AudioClip respawnAudio;
       public AudioClip ouchAudio;


       public float maxSpeed = 3;
       public float jumpTakeOffSpeed = 7;
       public float fallThreshold = -3f;


       private bool stopJump;
       public Collider2D collider2d;
       public AudioSource audioSource;
       public bool controlEnabled = true;


       private bool jump;
       private bool canDoubleJump = true;
       private Vector2 move;
       private SpriteRenderer spriteRenderer;
       internal Animator animator;
       private readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();


       // Input is read from keyboard (space bar). Arduino feature removed.
       private Vector3 lastSafePosition;
       private bool isRespawning = false;


       public bool IsRespawning
       {
           get => isRespawning;
           set => isRespawning = value;
       }


       public bool IsAlive => true;


       public enum JumpState
       {
           Grounded,
           PrepareToJump,
           Jumping,
           InFlight,
           Landed
       }


       public JumpState jumpState = JumpState.Grounded;


       public Bounds Bounds => collider2d.bounds;


       void Awake()
       {
           audioSource = GetComponent<AudioSource>();
           collider2d = GetComponent<Collider2D>();
           spriteRenderer = GetComponent<SpriteRenderer>();
           animator = GetComponent<Animator>();
           lastSafePosition = transform.position;
       }


        protected override void Start()
        {
            base.Start();
        }


       protected override void Update()
       {
           if (controlEnabled)
           {
               move.x = 1;
               if (Input.GetKeyDown(jumpKey))
               {
                   TriggerJump();
               }
           }
           else
           {
               move.x = 0;
           }


           if (transform.position.y < lastSafePosition.y + fallThreshold && !isRespawning)
           {
               isRespawning = true;
               Simulation.Schedule<PlayerSpawn>(0).player = this;
           }

           // continuously update the last safe position while grounded and moving forward
           if (IsGrounded && !isRespawning)
           {
               // only advance the safe point in the direction of travel to avoid regressing
               if (transform.position.x > lastSafePosition.x)
                   lastSafePosition = transform.position;
               // also update vertical position so falling off a higher ledge doesn't send you back down
               else if (transform.position.x == lastSafePosition.x && transform.position.y > lastSafePosition.y)
                   lastSafePosition = transform.position;
           }


           UpdateJumpState();
           base.Update();
       }


       // Reads the serial data from Arduino
       // private void ReadSerialData()
       // {
       //     // Use keyboard input (space) for jump. This replaces serial input.
       //     if (Input.GetKeyDown(KeyCode.Space))
       //     {
       //         TriggerJump();
       //     }
       // }


       private void UpdateJumpState()
       {
           jump = false;
           switch (jumpState)
           {
               case JumpState.PrepareToJump:
                   jumpState = JumpState.Jumping;
                   jump = true;
                   stopJump = false;
                   break;
               case JumpState.Jumping:
                   if (!IsGrounded)
                   {
                       Simulation.Schedule<PlayerJumped>().player = this;
                       jumpState = JumpState.InFlight;
                       canDoubleJump = true;
                   }
                   break;
               case JumpState.InFlight:
                   if (IsGrounded)
                   {
                       Simulation.Schedule<PlayerLanded>().player = this;
                       jumpState = JumpState.Landed;
                       lastSafePosition = transform.position;
                       isRespawning = false;
                       canDoubleJump = false;
                   }
                   break;
               case JumpState.Landed:
                   jumpState = JumpState.Grounded;
                   break;
           }
       }


       protected override void ComputeVelocity()
       {
           if (jump && (IsGrounded || canDoubleJump))
           {
               velocity.y = jumpTakeOffSpeed * model.jumpModifier;
               if (!IsGrounded)
               {
                   canDoubleJump = false;
               }
               jump = false;
               // Track the jump in session stats
               if (GameSessionStats.Instance != null)
               {
                   GameSessionStats.Instance.AddJump(this);
               }
               // Track the rep when a jump is successfully initiated
               ProgressTracker.Instance.AddRep();
           }
           else if (stopJump)
           {
               stopJump = false;
               if (velocity.y > 0)
               {
                   velocity.y *= model.jumpDeceleration;
               }
           }


           if (move.x > 0.01f)
               spriteRenderer.flipX = false;
           else if (move.x < -0.01f)
               spriteRenderer.flipX = true;


           animator.SetBool("grounded", IsGrounded);
           animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);


           targetVelocity = move * maxSpeed;
       }


       public void TriggerJump()
       {
           if (jumpState == JumpState.Grounded || canDoubleJump)
           {
               jumpState = JumpState.PrepareToJump;
           }
       }


       public void KillPlayer()
       {
           Debug.Log("💀 Player Died");
           // Track death in session stats
           if (GameSessionStats.Instance != null)
           {
               GameSessionStats.Instance.AddDeath(this);
           }
           Simulation.Schedule<PlayerSpawn>(0).player = this;
       }


       public void StopJump()
       {
           Debug.Log("🛑 Jump Stopped");
           stopJump = true;
       }


       public Vector3 GetLastSafePosition()
       {
           return lastSafePosition;
       }


       // private void OnApplicationQuit()
       // {
       //     // No serial port to close; nothing to do here.
       // }
   }
}