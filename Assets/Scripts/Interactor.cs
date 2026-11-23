using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public interface IInteractable
{
    bool IsFixed { get; }
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Crosshair")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color defaultCrosshairColor = Color.white;

    private void Start()
    {
        if (crosshairImage != null)
            crosshairImage.color = defaultCrosshairColor;
    }

    private void Update()
    {
        if (Time.timeScale == 0f || interactorSource == null) return;

        HandleClick();
    }

    private void HandleClick()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);

            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange, interactableLayer))
            {
                IInteractable foundInteractable = hitInfo.collider.GetComponentInParent<IInteractable>();

                if (foundInteractable != null)
                {
                    if (foundInteractable.IsFixed) return;

                    foundInteractable.Interact();
                    return;
                }
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SubtractScore();
            }
        }
    }
}