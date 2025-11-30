using UnityEngine;
using UnityEngine.UI;

namespace KutumbAssignment
{
    public class SmileButtonController : MonoBehaviour
    {
        [Header("Button Reference")]
        [SerializeField] private Button smileButton;

        [Header("Character References")]
        [SerializeField] private CharacterAnimator characterAnimator;
        [SerializeField] private LipSyncController lipSyncController;
        [SerializeField] private AudioSource dialogueAudioSource;

        [Header("Audio Settings")]
        [SerializeField] private AudioClip greetingAudioClip;

        [SerializeField] private float noiseDuration;

        private bool isAnimating = false;

        private void Start()
        {
            if (smileButton != null)
            {
                smileButton.onClick.AddListener(OnSmileButtonPressed);
            }

            if (dialogueAudioSource == null)
            {
                dialogueAudioSource = GetComponent<AudioSource>();
                if (dialogueAudioSource == null)
                {
                    GameObject audioObject = new GameObject("DialogueAudioSource");
                    audioObject.transform.SetParent(transform);
                    dialogueAudioSource = audioObject.AddComponent<AudioSource>();
                }
            }
        }

        private void OnSmileButtonPressed()
        {
            if (isAnimating) return;

            TriggerCharacterInteraction();
        }

        private void TriggerCharacterInteraction()
        {
            isAnimating = true;

            if (smileButton != null)
            {
                smileButton.interactable = false;
            }

            if (characterAnimator != null)
            {
                characterAnimator.OnWaveAnimationComplete += OnWaveAnimationFinished;
                characterAnimator.PlayWaveAnimation();
            }
            else
            {
                StartDialogue();
            }
        }

        private void OnWaveAnimationFinished()
        {
            if (characterAnimator != null)
            {
                characterAnimator.OnWaveAnimationComplete -= OnWaveAnimationFinished;
            }

            StartDialogue();
        }

        private void StartDialogue()
        {
            PlayDialogue();

            float audioDuration = greetingAudioClip != null ? greetingAudioClip.length : 3f;
            
            if (lipSyncController != null && dialogueAudioSource != null && greetingAudioClip != null)
            {
                lipSyncController.StartLipSync(audioDuration-noiseDuration);
            }

            Invoke(nameof(OnInteractionComplete), audioDuration);
        }

        private void PlayDialogue()
        {
            if (dialogueAudioSource != null && greetingAudioClip != null)
            {
                dialogueAudioSource.clip = greetingAudioClip;
                dialogueAudioSource.Play();
            }
        }

        private void OnInteractionComplete()
        {
            isAnimating = false;

            if (smileButton != null)
            {
                smileButton.interactable = true;
            }

            if (lipSyncController != null)
            {
                lipSyncController.StopLipSync();
            }
        }

        private void OnDestroy()
        {
            if (smileButton != null)
            {
                smileButton.onClick.RemoveListener(OnSmileButtonPressed);
            }

            if (characterAnimator != null)
            {
                characterAnimator.OnWaveAnimationComplete -= OnWaveAnimationFinished;
            }
        }
    }
}