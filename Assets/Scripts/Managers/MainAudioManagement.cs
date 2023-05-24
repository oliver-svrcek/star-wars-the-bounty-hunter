using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class MainAudioManagement
    {
        private static List<AudioManagement> AudioManagements { get; set; } = new List<AudioManagement>();
 
        public static void AddAudioManagement(AudioManagement audioManagement)
        {
            if (audioManagement != null && !AudioManagements.Contains(audioManagement))
            {
                AudioManagements.Add(audioManagement);
            }
        }
    
        public static void RemoveAudioManagement(AudioManagement audioManagement)
        {
            if (audioManagement != null && AudioManagements.Contains(audioManagement))
            {
                AudioManagements.Remove(audioManagement);
            }
        }

        public static void PlayAll()
        {
            foreach (var audioManagement in AudioManagements)
            {
                if (audioManagement == null)
                {
                    Debug.LogWarning($"AudioManagement is null!");
                    continue;
                }
            
                audioManagement.Play();
            }
        }


        public static void StopAll()
        {
            foreach (var audioManagement in AudioManagements)
            {
                if (audioManagement == null)
                {
                    Debug.LogWarning($"AudioManagement is null!");
                    continue;
                }

                audioManagement.Stop();
            }
        }

        public static void SetVolumeAll(float volume)
        {
            if (volume < 0f || volume > 1f)
            {
                Debug.LogWarning($"Volume {volume} is not in range [0, 1]!");
                return;
            }
        
            foreach (var audioManagement in AudioManagements)
            {
                if (audioManagement == null)
                {
                    Debug.LogWarning($"AudioManagement is null!");
                    continue;
                }

                audioManagement.SetVolume(volume);
            }
        }
    
        public static void SetMuteAll(bool mute)
        {
            foreach (var audioManagement in AudioManagements)
            {
                if (audioManagement == null)
                {
                    Debug.LogWarning($"AudioManagement is null!");
                    continue;
                }

                audioManagement.SetMute(mute);
            }
        }
    
        public static void SetPauseAll(bool pause)
        {
            foreach (var audioManagement in AudioManagements)
            {
                if (audioManagement == null)
                {
                    Debug.LogWarning($"AudioManagement is null!");
                    continue;
                }

                audioManagement.SetPause(pause);
            }
        }
    
        public static void SetLoopAll(bool loop)
        {
            foreach (var audioManagement in AudioManagements)
            {
                if (audioManagement == null)
                {
                    Debug.LogWarning($"AudioManagement is null!");
                    continue;
                }

                audioManagement.SetLoop(loop);
            }
        }
    }
}
