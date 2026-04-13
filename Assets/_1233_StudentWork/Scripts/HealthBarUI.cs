using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private Health _health;
    [SerializeField] private Image _fillImage;

    private Camera _cameraReference;
    private Camera CameraToRotateWith => _cameraReference ??= Camera.main;

    private void Awake()
    {
        if (_health == null)
            _health = GetComponentInParent<Health>();
    }

    private void OnEnable()
    {
        if (_health == null)
        {
            Debug.LogError($"HealthBarUI: No health assigned on {name}.");
            Refresh(null);
            return;
        }

        _health.OnHealthChanged += Refresh;
        _health.OnDied += HandleDied;
        Refresh(_health);
    }

    private void OnDisable()
    {
        if (_health == null) return;

        _health.OnHealthChanged -= Refresh;
        _health.OnDied -= HandleDied;
    }

    private void HandleDied()
    {
        _fillImage.fillAmount = 0;
    }

    private void Refresh(Health health)
    {
        _fillImage.fillAmount = health != null ? health.NormalizedHealth : 0f;
    }

    private void Update()
    {
        if (CameraToRotateWith == null)
            return;

        _canvasTransform.rotation = CameraToRotateWith.transform.rotation;
    }
}