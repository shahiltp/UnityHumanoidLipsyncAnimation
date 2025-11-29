using UnityEngine;

namespace KutumbAssignment
{
    public class LipSyncController : MonoBehaviour
    {
        [Header("BlendShape Settings")]
        [SerializeField] private SkinnedMeshRenderer faceRenderer;
        [SerializeField] private float blendShapeIntensity = 80f;

        [Header("Timing Settings")]
        [SerializeField] private float minMouthChangeTime = 0.08f;
        [SerializeField] private float maxMouthChangeTime = 0.25f;

        private bool isLipSyncing = false;
        private float nextMouthChangeTime = 0f;
        
        private readonly string[] visemeNames = {
            "viseme_sil", "viseme_PP", "viseme_FF", "viseme_TH", "viseme_DD",
            "viseme_kk", "viseme_CH", "viseme_SS", "viseme_nn", "viseme_RR",
            "viseme_aa", "viseme_E", "viseme_I", "viseme_O", "viseme_U"
        };

        private int[] visemeIndices;

        private void Start()
        {
            if (faceRenderer == null)
            {
                faceRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            }

            if (faceRenderer != null && faceRenderer.sharedMesh != null)
            {
                InitializeVisemes();
            }
        }

        private void InitializeVisemes()
        {
            visemeIndices = new int[visemeNames.Length];

            for (int i = 0; i < visemeNames.Length; i++)
            {
                visemeIndices[i] = faceRenderer.sharedMesh.GetBlendShapeIndex(visemeNames[i]);
            }
        }

        private void Update()
        {
            if (isLipSyncing)
            {
                UpdateLipSync();
            }
        }

        public void StartLipSync(float duration)
        {
            isLipSyncing = true;
            nextMouthChangeTime = 0f;
            ResetAllVisemes();

            Invoke(nameof(StopLipSync), duration);
        }

        public void StopLipSync()
        {
            isLipSyncing = false;
            ResetAllVisemes();
        }

        private void UpdateLipSync()
        {
            if (faceRenderer == null || visemeIndices == null) return;

            if (Time.time >= nextMouthChangeTime)
            {
                ResetAllVisemes();
                
                int randomViseme = Random.Range(1, visemeIndices.Length);
                if (visemeIndices[randomViseme] >= 0)
                {
                    float weight = Random.Range(60f, 100f) * (blendShapeIntensity / 100f);
                    faceRenderer.SetBlendShapeWeight(visemeIndices[randomViseme], weight);
                }

                nextMouthChangeTime = Time.time + Random.Range(minMouthChangeTime, maxMouthChangeTime);
            }
        }

        private void ResetAllVisemes()
        {
            if (faceRenderer == null || visemeIndices == null) return;

            for (int i = 0; i < visemeIndices.Length; i++)
            {
                if (visemeIndices[i] >= 0)
                {
                    faceRenderer.SetBlendShapeWeight(visemeIndices[i], 0f);
                }
            }
        }
    }
}