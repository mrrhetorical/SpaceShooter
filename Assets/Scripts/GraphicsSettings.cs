using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    [Tooltip("Waits for new frames to render before updating previous frames.")]
    [SerializeField] private bool _useVsync = false;

    private void Start()
    {
        QualitySettings.vSyncCount = _useVsync ? 1 : 0;
    }
}