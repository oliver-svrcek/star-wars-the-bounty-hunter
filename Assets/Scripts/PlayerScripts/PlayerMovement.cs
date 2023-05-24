using Other;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public Jetpack Jetpack { get; private set; }
        private Animator Animator { get; set; }
        private CharacterMovementController CharacterMovementController { get; set; }
        private Rigidbody2D Rigidbody2D { get; set; }
        private float HorizontalMove { get; set; }
        private float WalkSpeed { get; set; }
        private bool Jump { get; set; }
    
        private void Awake()
        {
            Animator = Utils.GetComponentOrThrow<Animator>("Player");
            CharacterMovementController = Utils.GetComponentOrThrow<CharacterMovementController>("Player");
            Rigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>("Player");
            Jetpack = Utils.GetComponentOrThrow<Jetpack>("Player/Jetpack");
        }
        
        private void Start()
        {
            WalkSpeed = 30f;
        }

        private void Update()
        {
            HorizontalMove = Input.GetAxisRaw("Horizontal") * WalkSpeed;

            if (Input.GetButton("Jump"))
            {
                Jump = true;
            }

            if (Input.GetKeyDown("space"))
            {
                Jetpack.Activate();
                HorizontalMove *= 0.9f;
            }
            if (Input.GetKeyUp("space"))
            {
                Jetpack.Deactivate();
            }
        }

        private void FixedUpdate()
        {
            Move();
            Jump = false;
        }

        private void Move()
        {
            CharacterMovementController.Move(HorizontalMove * Time.fixedDeltaTime, Jump);
        }
    
        public void SetFreeze(bool freeze)
        {
            if (freeze)
            {
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
