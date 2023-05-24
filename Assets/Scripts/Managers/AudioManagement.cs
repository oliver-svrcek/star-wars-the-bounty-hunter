using System.Collections;
using System.Collections.Generic;
using Exceptions;
using UnityEngine;
using Utilities;
using Random=UnityEngine.Random;

namespace Managers
{
    public class AudioManagement : MonoBehaviour
    {
        private AudioSource AudioSource { get; set; }
        private Dictionary<string, AudioClip> AudioClips { get; set; }
        private Coroutine PlaySequenceCoroutine { get; set; }
        [field: SerializeField] private bool LoadSounds { get; set; }

        protected void Awake()
        {
            AudioSource = Utils.GetComponentOrThrow<AudioSource>(this.gameObject);
        
            var audioPaths = new List<string>();
            if (LoadSounds)
            {
                audioPaths.Add("Audio/Sounds");
            }

            AudioClips = new Dictionary<string, AudioClip>();
            foreach (var audioPath in audioPaths)
            {
                var audioClips = Resources.LoadAll<AudioClip>(audioPath);
                if (audioClips.Length == 0)
                {
                    throw new MissingResourceException($"No audio clips found in {audioPath}!");
                }

                foreach (var audioClip in audioClips)
                {
                    AudioClips.Add(audioClip.name, audioClip);
                }
            }
        
            AudioSource.loop = false;
        }

        private void Start()
        {
            MainAudioManagement.AddAudioManagement(this);
        }

        public void RemoveFromMainAudioManagement()
        {
            MainAudioManagement.RemoveAudioManagement(this);
        }
    
        private void OnEnable()
        {
            MainAudioManagement.AddAudioManagement(this);
        }
    
        private void OnDestroy()
        {
            MainAudioManagement.RemoveAudioManagement(this);
        }
    
        private void OnDisable()
        {
            MainAudioManagement.RemoveAudioManagement(this);
        }

        public bool IsPlaying()
        {
            return AudioSource.isPlaying;
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            AudioSource.clip = audioClip;
        }
    
        public void Play()
        {
            AudioSource.Play();
        }

        public void Stop()
        {
            if (PlaySequenceCoroutine is not null)
            {
                StopCoroutine(PlaySequenceCoroutine);
                PlaySequenceCoroutine = null;
            }
        
            AudioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            if (volume < 0f || volume > 1f)
            {
                Debug.LogWarning($"Volume {volume} is not in range [0, 1]!", this);
                return;
            }
        
            AudioSource.volume = volume;
        }

        public void SetMute(bool mute)
        {
            AudioSource.mute = mute;
        }
    
        public void SetPause(bool pause)
        {
            if (pause)
            {
                AudioSource.Pause();
            }
            else
            {
                AudioSource.UnPause();
            }
        }

        public void SetLoop(bool loop)
        {
            AudioSource.loop = loop;
        }
    
        public void PlayClipAtPoint(string audioClipName, Vector3 pointInSpace)
        {
            if (!AudioClips.TryGetValue(audioClipName, out var audioClip)) {
                Debug.LogWarning($"{audioClipName} was not found in AudioClipsGroups!", this);
                return;
            }

            var audioGameObject = new GameObject("TemporaryAudioSource", typeof(AudioSource));
            audioGameObject.transform.position = pointInSpace;
        
            var audioSource = audioGameObject.GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.volume = AudioSource.volume;
            audioSource.spatialBlend = 1f;
            audioSource.dopplerLevel = 0f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 12f;
            audioSource.maxDistance = 15f;
        
            audioSource.Play();
            Destroy(audioGameObject, audioClip.length);
        }

        public void Play(string audioClipName, bool loop)
        {
            if (!AudioClips.TryGetValue(audioClipName, out var audioClip)) {
                Debug.LogWarning($"{audioClipName} was not found in AudioClipsGroups!", this);
                return;
            }

            AudioSource.clip = audioClip;
            AudioSource.loop = loop;
            AudioSource.Play();
        }

        public void PlayRandom(string audioClipGroup, bool loop)
        {
            var audioClips = new List<AudioClip>();
            foreach (var audioClip in AudioClips)
            {
                if (audioClip.Key.StartsWith(audioClipGroup))
                {
                    audioClips.Add(audioClip.Value);
                }
            }

            if (audioClips.Count == 0)
            {
                Debug.LogWarning($"No audio clips found in {audioClipGroup}!", this);
                return;
            }

            AudioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
            AudioSource.loop = loop;
            AudioSource.Play();
        }

        public void PlayOneShot(string audioClipName)
        {
            if (!AudioClips.TryGetValue(audioClipName, out var audioClip)) {
                Debug.LogWarning($"{audioClipName} was not found in AudioClipsGroups!", this);
                return;
            }
        
            AudioSource.PlayOneShot(audioClip);
        }

        public void PlayOneShotRandom(string audioClipGroup)
        {
            var audioClips = new List<AudioClip>();
            foreach (var audioClip in AudioClips)
            {
                if (audioClip.Key.StartsWith(audioClipGroup))
                {
                    audioClips.Add(audioClip.Value);
                }
            }

            if (audioClips.Count == 0)
            {
                Debug.LogWarning($"No audio clips found in {audioClipGroup}!", this);
                return;
            }
        
            AudioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
        }

        public void PlaySequence(string[] audioClipsSequence, bool loopLast)
        {
            if (audioClipsSequence.Length == 0)
            {
                Debug.LogWarning($"No audio clips found in {audioClipsSequence}!", this);
                return;
            }
        
        
            if (PlaySequenceCoroutine is not null)
            {
                StopCoroutine(PlaySequenceCoroutine);
                PlaySequenceCoroutine = null;
            }     
            PlaySequenceCoroutine =  StartCoroutine(PlaySequenceCor(audioClipsSequence, loopLast));
        }

        private IEnumerator PlaySequenceCor(string[] audioClipsSequence, bool loopLast)
        {
            for (var index = 0; index < audioClipsSequence.Length; index++)
            {
                var audioClipName = audioClipsSequence[index];

                if (!AudioClips.TryGetValue(audioClipName, out var audioClip)) {
                    Debug.LogWarning($"{audioClipName} was not found in AudioClipsGroups!", this);
                    yield break;
                }
            
                AudioSource.clip = audioClip;
                AudioSource.loop = false;
                AudioSource.Play();
            
                if (index == audioClipsSequence.Length - 1)
                {
                    AudioSource.loop = loopLast;
                }
                else
                {
                    yield return new WaitForSeconds(audioClip.length);
                }
            }
        }
    }
}
