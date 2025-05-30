using UnityEngine;
using System.Collections;
 
public class SafePassthroughInitializer : MonoBehaviour
{
    public float delayBeforeEnable = 1.5f;
 
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeEnable);
 
        OVRPassthroughLayer passthrough = FindObjectOfType<OVRPassthroughLayer>();
        if (passthrough == null)
        {
            Debug.LogWarning("❌ No OVRPassthroughLayer found in scene.");
            yield break;
        }
 
        // These match the properties your SDK exposes
        passthrough.overlayType = OVROverlay.OverlayType.Overlay;
        passthrough.projectionSurfaceType = OVRPassthroughLayer.ProjectionSurfaceType.Reconstructed;
        passthrough.textureOpacity = 1.0f;
        passthrough.edgeRenderingEnabled = false;
 
        Debug.Log("✅ Passthrough depth initialized successfully (SDK-compatible).");
    }
}