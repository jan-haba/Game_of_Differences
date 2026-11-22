using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public interface IInteractable
{
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
    [SerializeField] private Color hoverCrosshairColor = Color.red;

    private Collider lastHitCollider;
    private IInteractable currentInteractable;
    private Outline currentOutline;

    private void Start()
    {
        if (crosshairImage != null)
            crosshairImage.color = defaultCrosshairColor;
    }

    private void Update()
    {
        if (Time.timeScale == 0f || interactorSource == null) return;

        UpdateHoverTarget();
        HandleClick();
    }

    private void UpdateHoverTarget()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange, interactableLayer))
        {
            if (hitInfo.collider == lastHitCollider) return;

            lastHitCollider = hitInfo.collider;
            IInteractable foundInteractable = hitInfo.collider.GetComponentInParent<IInteractable>();

            if (foundInteractable != null)
            {
                SetNewTarget(foundInteractable);
                return;
            }
        }

        if (lastHitCollider != null || currentInteractable != null)
        {
            ClearTarget();
        }
    }

    private void SetNewTarget(IInteractable newTarget)
    {
        ClearTarget();

        currentInteractable = newTarget;
        MonoBehaviour targetMono = newTarget as MonoBehaviour;

        if (targetMono != null)
        {
            if (!targetMono.TryGetComponent(out currentOutline))
            {
                currentOutline = targetMono.gameObject.AddComponent<Outline>();
                currentOutline.OutlineColor = Color.white;
                currentOutline.OutlineWidth = 5f;
            }
            currentOutline.enabled = true;
        }

        if (crosshairImage != null)
            crosshairImage.color = hoverCrosshairColor;
    }

    private void HandleClick()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
                ClearTarget();
            }
            else
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.SubtractScore();
            }
        }
    }

    private void ClearTarget()
    {
        lastHitCollider = null;
        currentInteractable = null;

        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }

        if (crosshairImage != null)
            crosshairImage.color = defaultCrosshairColor;
    }
}