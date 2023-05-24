using System.Collections;
using Enums;
using Managers;
using Other;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class Jetpack : MonoBehaviour
    {
        private Animator Animator { get; set; }
        private CharacterMovementController CharacterMovementController { get; set; }
        private Rigidbody2D Rigidbody2D { get; set; }
        private AudioManagement AudioManagement { get; set; }
        private BarManagement JetpackFuelBar { get; set; }
        private bool Fly { get; set; }
        public Coroutine BurnFuelCoroutine { get; private set; }
        public Coroutine RechargeFuelCoroutine { get; private set; }
        private int MaximumJetpackFuel { get; set; }
        private int CurrentJetpackFuel { get; set; }
        public int JetpackFuelConsumptionInitialPoints { get; set; }
        public int JetpackFuelConsumptionPoints { get; set; }
        public float JetpackFuelConsumptionRate { get; set; }
        public int JetpackFuelRechargePoints { get; set; }
        public float JetpackFuelRechargeStartTime { get; set; }
        public float JetpackFuelRechargeRate { get; set; }

        private void Awake()
        {
            Animator = Utils.GetComponentOrThrow<Animator>("Player"); 
            CharacterMovementController = Utils.GetComponentOrThrow<CharacterMovementController>("Player");
            Rigidbody2D = Utils.GetComponentOrThrow<Rigidbody2D>("Player");
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Player/Audio/Jetpack");
            JetpackFuelBar = Utils.GetComponentOrThrow<BarManagement>("Interface/MainCamera/UICanvas/HUD/BarsWrapper/JetpackBar/Slider");
        }

        private void Start()
        {
            Fly = false;
            BurnFuelCoroutine = null;
            RechargeFuelCoroutine = null;
            MaximumJetpackFuel = 10000;
            CurrentJetpackFuel = MaximumJetpackFuel;
            JetpackFuelConsumptionInitialPoints = 800;
            JetpackFuelConsumptionPoints = 70;
            JetpackFuelConsumptionRate = 0.01f;
            JetpackFuelRechargePoints = 30;
            JetpackFuelRechargeStartTime = 1.2f;
            JetpackFuelRechargeRate = 0.01f;
            
            AudioManagement.Play("FlameSoundLong", true);
            AudioManagement.SetPause(true);
            
            JetpackFuelBar.SetBar(BarType.Decreasing, MaximumJetpackFuel, CurrentJetpackFuel);
        }

        public void Reload()
        {
            if (BurnFuelCoroutine is not null)
            {
                Animator.SetBool("IsUsingJetpack", false);
                StopCoroutine(BurnFuelCoroutine);
                
                BurnFuelCoroutine = null;
            }
        
            if (RechargeFuelCoroutine is not null)
            {
                StopCoroutine(RechargeFuelCoroutine);
                RechargeFuelCoroutine = null;
            }

            CurrentJetpackFuel = MaximumJetpackFuel;
            JetpackFuelBar.SetBar(BarType.Decreasing, MaximumJetpackFuel, CurrentJetpackFuel);
        }
    
        public void Activate()
        {
            Fly = true;
        }

        public void Deactivate()
        {
            Fly = false;
        }
    
        private void FixedUpdate()
        {
            if (Fly && CurrentJetpackFuel > 0)
            {
                if (RechargeFuelCoroutine is not null)
                {
                    StopCoroutine(RechargeFuelCoroutine);
                    RechargeFuelCoroutine = null;
                }
                if (BurnFuelCoroutine is null)
                {
                    BurnFuelCoroutine = StartCoroutine(BurnFuel());
                }
            
                FlyJetpack();
            }
            else
            {
                Animator.SetBool("IsUsingJetpack", false);
                
                if (!CharacterMovementController.IsGrounded && Rigidbody2D.velocity.y < 0.001f)
                {
                    Animator.SetBool("IsFalling", true);
                }

                if (AudioManagement.IsPlaying())
                {
                    AudioManagement.SetPause(true);
                }
            }

            if (Fly)
            {
                return;
            }
            
            if (BurnFuelCoroutine is not null)
            {
                Animator.SetBool("IsUsingJetpack", false);
                
                if (!CharacterMovementController.IsGrounded && Rigidbody2D.velocity.y < 0.001f)
                {
                    Animator.SetBool("IsFalling", true);
                }
                
                StopCoroutine(BurnFuelCoroutine);
                BurnFuelCoroutine = null;
            }
            
            if (CurrentJetpackFuel < MaximumJetpackFuel && RechargeFuelCoroutine is null)
            {
                RechargeFuelCoroutine = StartCoroutine(RechargeFuel());
            }
        }

        private void FlyJetpack()
        {
            if (!AudioManagement.IsPlaying())
            {
                AudioManagement.SetPause(false);
            }
            
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
            Rigidbody2D.AddForce(new Vector2(0f, 500f));
        }

        private IEnumerator BurnFuel()
        {
            Animator.SetBool("IsUsingJetpack", true);
        
            CurrentJetpackFuel -= JetpackFuelConsumptionInitialPoints;
            JetpackFuelBar.SetValue(CurrentJetpackFuel);
        
            while (CurrentJetpackFuel > 0)
            {
                yield return new WaitForSeconds(JetpackFuelConsumptionRate);
                
                CurrentJetpackFuel -= JetpackFuelConsumptionPoints;
                JetpackFuelBar.SetValue(CurrentJetpackFuel);
            }
        
            Animator.SetBool("IsUsingJetpack", false);
            if (!CharacterMovementController.IsGrounded)
            {
                Animator.SetBool("IsFalling", true);
            }

            CurrentJetpackFuel = 0;
            JetpackFuelBar.SetValue(CurrentJetpackFuel);
            
            BurnFuelCoroutine = null;
        }
    
        private IEnumerator RechargeFuel()
        {
            yield return new WaitForSeconds(JetpackFuelRechargeStartTime);
            
            JetpackFuelBar.SetValue(CurrentJetpackFuel);
        
            while (CurrentJetpackFuel < MaximumJetpackFuel)
            {
                yield return new WaitForSeconds(JetpackFuelRechargeRate);
                
                CurrentJetpackFuel += JetpackFuelRechargePoints;
                JetpackFuelBar.SetValue(CurrentJetpackFuel);
            }

            CurrentJetpackFuel = MaximumJetpackFuel;
            JetpackFuelBar.SetValue(CurrentJetpackFuel);
            
            RechargeFuelCoroutine = null;
        }
    }
}
