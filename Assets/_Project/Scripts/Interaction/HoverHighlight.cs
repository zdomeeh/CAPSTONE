using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HoverHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public Color hoverColor = Color.yellow;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter()
    {
        // Non attivare hover se dialogo aperto
        if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
            return;

        spriteRenderer.color = hoverColor;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }
}