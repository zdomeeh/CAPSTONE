using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HoverHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public Color hoverColor = Color.yellow;

    void Start()
    {
        // Prende il componente SpriteRenderer dell'oggetto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Salva il colore originale dello sprite
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter()
    {
        // Se un dialogo e' attivo, non cambia colore
        if (InkManager.Instance != null && InkManager.Instance.IsDialogueActive())
            return;

        // Cambia colore quando il mouse passa sopra
        spriteRenderer.color = hoverColor;
    }

    void OnMouseExit()
    {
        // Ripristina il colore originale quando il mouse esce
        spriteRenderer.color = originalColor;
    }
}