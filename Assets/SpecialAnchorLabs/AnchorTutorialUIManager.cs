using System; // Added to resolve Guid and Action<,>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public static NewBehaviourScript Instance { get; private set; } // Declare the Instance variable

    [SerializeField]
    private GameObject _saveableAnchorPrefab;

    [SerializeField]
    private GameObject _saveablePreview;

    [SerializeField]
    private Transform _saveableTransform;

    [SerializeField]
    private GameObject _nonSaveableAnchorPrefab;

    [SerializeField]
    private GameObject _nonSaveablePreview;

    [SerializeField]
    public Transform _nonSaveableTransform;

    private List<OVRSpatialAnchor> _anchorInstances = new(); // Active instances (red and green)

    private HashSet<Guid> _anchorUuids = new(); // Simulated external location, like PlayerPrefs

    private Action<bool, OVRSpatialAnchor.UnboundAnchor> _onLocalized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _onLocalized = OnLocalized;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Create a green capsule with the left index trigger
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            var go = Instantiate(_saveableAnchorPrefab, _saveableTransform.position, _saveableTransform.rotation);
            SetupAnchorAsync(go.AddComponent<OVRSpatialAnchor>(), saveAnchor: true);
        }
        // Create a red capsule with the right index trigger
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            var go = Instantiate(_nonSaveableAnchorPrefab, _nonSaveableTransform.position, _nonSaveableTransform.rotation);
            _anchorInstances.Add(go.GetComponent<OVRSpatialAnchor>()); // Optional: Track it if needed for cleanup
        }
        // Destroy all capsules with the X button
        else if (OVRInput.GetDown(OVRInput.Button.One)) // X button
        {
            foreach (var anchor in _anchorInstances)
            {
                if (anchor != null)
                {
                    Destroy(anchor.gameObject);
                }
            }
            _anchorInstances.Clear(); // Clear the list of active anchors
        }
        // Load all saved green capsules with the A button
        else if (OVRInput.GetDown(OVRInput.Button.Two)) // A button
        {
            LoadAllAnchors();
        }
        // Erase all saved green anchors with the Y button
        else if (OVRInput.GetDown(OVRInput.Button.Three)) // Y button
        {
            EraseAllAnchors();
        }
    }
    
    public List<OVRSpatialAnchor> GetActiveAnchors()
{
    return _anchorInstances;
}

    private async void SetupAnchorAsync(OVRSpatialAnchor anchor, bool saveAnchor)
{
    if (!await anchor.WhenLocalizedAsync())
    {
        Debug.LogError($"Unable to create anchor.");
        Destroy(anchor.gameObject);
        return;
    }

    _anchorInstances.Add(anchor);

    if (saveAnchor && (await anchor.SaveAnchorAsync()).Success)
    {
        _anchorUuids.Add(anchor.Uuid);

        // Notify MRUKManager about the new anchor
        MRUKManager manager = FindObjectOfType<MRUKManager>();
        if (manager != null)
        {
            manager.SaveAnchor(anchor.Uuid.ToString(), anchor.transform.position);
        }
    }
}

    public async void LoadAllAnchors()
    {
        // Load and localize
        var unboundAnchors = new List<OVRSpatialAnchor.UnboundAnchor>();
        var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(_anchorUuids, unboundAnchors);

        if (result.Success)
        {
            foreach (var anchor in unboundAnchors)
            {
                anchor.LocalizeAsync().ContinueWith(_onLocalized, anchor);
            }
        }
        else
        {
            Debug.LogError($"Load anchors failed with {result.Status}.");
        }
    }

    private void OnLocalized(bool success, OVRSpatialAnchor.UnboundAnchor unboundAnchor)
    {
        var pose = unboundAnchor.Pose;
        var go = Instantiate(_saveableAnchorPrefab, pose.position, pose.rotation);
        var anchor = go.AddComponent<OVRSpatialAnchor>();

        unboundAnchor.BindTo(anchor);

        // Add the anchor to the running total
        _anchorInstances.Add(anchor);
    }

    public async void EraseAllAnchors()
    {
        var result = await OVRSpatialAnchor.EraseAnchorsAsync(anchors: null, uuids: _anchorUuids);
        if (result.Success)
        {
            // Erase our reference lists
            _anchorUuids.Clear();

            Debug.Log($"Anchors erased.");
        }
        else
        {
            Debug.LogError($"Anchors NOT erased {result.Status}");
        }
    }
}
