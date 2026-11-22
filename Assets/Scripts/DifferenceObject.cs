using UnityEngine;

public class DifferenceObject : MonoBehaviour, IInteractable
{
    public enum DifferenceType
    {
        MoveAndRotate, 
        Disappear,     
        Appear        
    }

    [Header("Difference")]
    public DifferenceType type = DifferenceType.MoveAndRotate;

    public Transform correctTarget;

    public GameObject visualModelToShow;

    private bool isFixed = false;

    void Start()
    {
        gameObject.tag = "Selectable";

        if (type == DifferenceType.Appear && visualModelToShow != null)
        {
            visualModelToShow.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isFixed) return; 
        isFixed = true;

        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default")); Outline outline = GetComponent<Outline>();
        if (outline != null) outline.enabled = false;

        switch (type)
        {
            case DifferenceType.MoveAndRotate:
                if (correctTarget != null)
                {
                    transform.position = correctTarget.position;
                    transform.rotation = correctTarget.rotation;
                }
                break;

            case DifferenceType.Disappear:
                gameObject.SetActive(false);
                break;

            case DifferenceType.Appear:
                if (visualModelToShow != null)
                {
                    visualModelToShow.SetActive(true);
                }
                break;
        }

        Debug.Log("Difference fixed: " + gameObject.name);
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore();
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