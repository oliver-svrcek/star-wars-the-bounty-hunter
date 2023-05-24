using Managers;
using UnityEngine;
using Utilities;

namespace PlayerScripts
{
    public class PlayerWeapons : MonoBehaviour
    {
        private AudioManagement AudioManagement { get; set; }
        public Blaster Blaster { get; private set; }
        public Flamethrower Flamethrower { get; private set; }

        private void Awake()
        {
            AudioManagement = Utils.GetComponentOrThrow<AudioManagement>("Interface/MainCamera/Audio/Sounds");
            Blaster = Utils.GetComponentOrThrow<Blaster>("Player/Blaster");
            Flamethrower = Utils.GetComponentOrThrow<Flamethrower>("Player/Flamethrower");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Blaster.Activate();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Flamethrower.Activate();
            }
        }
    }
}
