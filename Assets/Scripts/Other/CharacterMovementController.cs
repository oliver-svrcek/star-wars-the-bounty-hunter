using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Other
{
	public class CharacterMovementController : MonoBehaviour
	{
		private Animator Animator { get; set; }
		private Rigidbody2D Rigidbody2D { get; set; }
		private CapsuleCollider2D BodyCollider  { get; set; }
		private List<LayerMask> GroundLayerMasks { get; set; }
		private Vector3 velocity = Vector3.zero;
		private bool AirControl { get; set; }
		public bool IsGrounded { get; private set; }
		[field: SerializeField] private float JumpForce { get; set; }
		[field: SerializeField] private float MovementSmoothing { get; set; }
		private bool IsLookingRight { get; set; }

		[Header("Events")]
		[Space]
		public UnityEvent OnLandEvent;
		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		protected void Awake()
		{
			Animator = this.gameObject.GetComponent<Animator>();
			Rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
			BodyCollider = this.gameObject.GetComponent<CapsuleCollider2D>();

			GroundLayerMasks = new List<LayerMask>
			{
				LayerMask.GetMask("Tilemap"),
				LayerMask.GetMask("GameWorldSolid")
			};

			if (OnLandEvent == null)
			{
				OnLandEvent = new UnityEvent();	
			}
		
			AirControl = true;
			JumpForce = 850f;
			MovementSmoothing = 0.01f;
			IsLookingRight = true;
		}

		protected void FixedUpdate()
		{
			var wasGrounded = IsGrounded;
			IsGrounded = false;

			var colliders = new List<Collider2D>();
			foreach (var groundLayerMask in GroundLayerMasks)
			{
				var cols = Physics2D.OverlapCapsuleAll(
					new Vector2(BodyCollider.bounds.center.x, BodyCollider.bounds.center.y - 0.1f), 
					new Vector2(BodyCollider.size.x - 0.4f, BodyCollider.size.y), 
					BodyCollider.direction, 0f, groundLayerMask
				);
			
				colliders.AddRange(cols);
			}

			foreach (var col in colliders)
			{
				if (col.gameObject == gameObject)
				{
					continue;
				}
				
				IsGrounded = true;
				
				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}

		public void Move(float move, bool jump)
		{
			if (IsGrounded)
			{
				Animator.SetBool("IsJumping", false);
				Animator.SetBool("IsFalling", false);
				Animator.SetBool("IsGrounded", true);	
			}
			else
			{
				Animator.SetBool("IsGrounded", false);	
			}
		
			if (IsGrounded && move != 0f)
			{
				Animator.SetBool("IsWalking", true);	
			}
			else
			{
				Animator.SetBool("IsWalking", false);
			}
		
			if (!IsGrounded && Rigidbody2D.velocity.y < 0.001f)
			{
				Animator.SetBool("IsFalling", true);
			}
			else if (IsGrounded || Rigidbody2D.velocity.y >= 0.001f)
			{
				Animator.SetBool("IsFalling", false);
			}
		
			if (IsGrounded || AirControl)
			{

				Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
				Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref velocity, MovementSmoothing);

				if (move > 0 && !IsLookingRight)
				{
					Flip();
				}
				else if (move < 0 && IsLookingRight)
				{
					Flip();
				}
			}

			if (!IsGrounded || !jump)
			{
				return;
			}
			
			IsGrounded = false;
			
			Animator.SetBool("IsJumping", true);
			
			Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
			Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
		}
	
		private void Flip()
		{
			IsLookingRight = !IsLookingRight;
			transform.Rotate(0f, 180f, 0f);
		}
	}
}
