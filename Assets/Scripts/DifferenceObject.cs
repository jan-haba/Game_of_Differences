using UnityEngine;

public class DifferenceObject : MonoBehaviour, IInteractable
{
    public enum DifferenceType
    {
        MoveAndRotate,
        Disappear,
        Appear,
        Swap
    }

    [Header("Difference")]
    public DifferenceType type = DifferenceType.MoveAndRotate;

    public Transform correctTarget;
    public GameObject visualModelToShow;

    private bool isFixed = false;
    public bool IsFixed => isFixed;

    private Outline outline;

    void Start()
    {
        gameObject.tag = "Selectable";

        if (!TryGetComponent(out outline))
        {
            outline = gameObject.AddComponent<Outline>();
        }
        outline.OutlineColor = Color.white;
        outline.OutlineWidth = 10f;
        outline.enabled = false;

        if ((type == DifferenceType.Appear || type == DifferenceType.Swap) && visualModelToShow != null)
        {
            visualModelToShow.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isFixed) return;
        isFixed = true;

        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));

        switch (type)
        {
            case DifferenceType.MoveAndRotate:
                if (correctTarget != null)
                {
                    transform.position = correctTarget.position;
                    transform.rotation = correctTarget.rotation;
                }
                if (outline != null)
                {
                    outline.OutlineColor = Color.white;
                    outline.enabled = true;
                }
                break;

            case DifferenceType.Disappear:
                if (outline != null) outline.enabled = false;
                gameObject.SetActive(false);
                break;

            case DifferenceType.Appear:
                if (visualModelToShow != null)
                {
                    visualModelToShow.SetActive(true);
                    EnableOutlineOn(visualModelToShow, Color.white);
                }
                if (outline != null) outline.enabled = true;
                break;

            case DifferenceType.Swap:
                if (visualModelToShow != null)
                {
                    visualModelToShow.SetActive(true);
                    EnableOutlineOn(visualModelToShow, Color.white);
                }
                gameObject.SetActive(false);
                break;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore();
    }

    public void ShowHint(bool show)
    {
        if (isFixed) return;

        if (type == DifferenceType.Appear && visualModelToShow != null)
        {
            if (show)
            {
                visualModelToShow.SetActive(true);
                EnableOutlineOn(visualModelToShow, Color.yellow);
            }
            else
            {
                if (visualModelToShow.TryGetComponent(out Outline modelOutline))
                {
                    modelOutline.enabled = false;
                }
                visualModelToShow.SetActive(false);
            }
            return;
        }

        if (outline == null) return;

        if (show)
        {
            outline.OutlineColor = Color.yellow;
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
            outline.OutlineColor = Color.white;
        }
    }

    private void EnableOutlineOn(GameObject targetObj, Color color)
    {
        if (!targetObj.TryGetComponent(out Outline targetOutline))
        {
            targetOutline = targetObj.AddComponent<Outline>();
        }
        targetOutline.OutlineColor = color;
        targetOutline.OutlineWidth = 10f;
        targetOutline.enabled = true;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}