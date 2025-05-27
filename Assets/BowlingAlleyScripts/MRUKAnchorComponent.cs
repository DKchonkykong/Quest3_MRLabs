using UnityEngine;

namespace Oculus.MRUtilityKit
{
    public class MRUKAnchorComponent : MonoBehaviour
    {
        public string anchorId;
        public bool saveAnchor = true;
        public bool restoreAnchorPose = true;

        private void Start()
        {
            MRUKManager.Instance?.RegisterAnchor(this);
        }

        public void RestoreAnchor()
        {
            if (restoreAnchorPose)
            {
                // NOTE: Replace this with actual spatial anchor load logic
                Debug.Log($"[MRUK] Restoring anchor: {anchorId}");
                // Simulated position could be loaded here
            }
        }

        private void OnDestroy()
        {
            MRUKManager.Instance?.UnregisterAnchor(this);
        }
    }
}
