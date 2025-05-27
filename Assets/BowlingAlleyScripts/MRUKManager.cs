using System.Collections.Generic;
using UnityEngine;

namespace Oculus.MRUtilityKit
{
    public class MRUKManager : MonoBehaviour
    {
        private static MRUKManager _instance;
        public static MRUKManager Instance => _instance;

        private readonly Dictionary<string, MRUKAnchorComponent> anchors = new Dictionary<string, MRUKAnchorComponent>();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadAnchors();
        }

        private void LoadAnchors()
        {
            var allAnchors = FindObjectsOfType<MRUKAnchorComponent>();
            foreach (var anchor in allAnchors)
            {
                if (!string.IsNullOrEmpty(anchor.anchorId))
                {
                    if (!anchors.ContainsKey(anchor.anchorId))
                    {
                        anchors.Add(anchor.anchorId, anchor);
                        anchor.RestoreAnchor();
                    }
                }
            }
        }

        public void RegisterAnchor(MRUKAnchorComponent anchor)
        {
            if (anchors.ContainsKey(anchor.anchorId)) return;
            anchors.Add(anchor.anchorId, anchor);
        }

        public void UnregisterAnchor(MRUKAnchorComponent anchor)
        {
            if (anchors.ContainsKey(anchor.anchorId))
            {
                anchors.Remove(anchor.anchorId);
            }
        }
    }
}
