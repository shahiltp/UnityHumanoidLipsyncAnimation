using UnityEngine;

namespace KutumbAssignment
{
    public class CharacterAnimator : MonoBehaviour
    {
        [Header("Animation References")]
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private string waveTriggerParameter = "Wave";
        [SerializeField] private float waveAnimationDuration = 2f;
        //[SerializeField] private string waveStateName = "testwave";

        public System.Action OnWaveAnimationComplete;

        private void Start()
        {
            if (characterAnimator == null)
            {
                characterAnimator = GetComponent<Animator>();
                if (characterAnimator == null)
                {
                    characterAnimator = GetComponentInChildren<Animator>();
                }
            }
        }

        public void PlayWaveAnimation()
        {
            if (characterAnimator != null && !string.IsNullOrEmpty(waveTriggerParameter))
            {
                if (HasParameter(waveTriggerParameter))
                {
                    characterAnimator.SetTrigger(waveTriggerParameter);
                    StartCoroutine(WaitForWaveAnimation());
                }
            }
        }

        private System.Collections.IEnumerator WaitForWaveAnimation()
        {
            yield return new WaitForSeconds(waveAnimationDuration);
            OnWaveAnimationComplete?.Invoke();
        }

        private bool HasParameter(string parameterName)
        {
            if (characterAnimator == null) return false;
            
            foreach (AnimatorControllerParameter param in characterAnimator.parameters)
            {
                if (param.name == parameterName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}