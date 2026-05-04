using UnityEngine;

public class ClickManager : MonoBehaviour
{
    void Update()
    {
        // Controlla se il giocatore clicca con il mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Se gli input sono bloccati, non fa nulla
            if (InkManager.IsInputBlocked)
                return;

            // Converte la posizione del mouse nello spazio di gioco
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Lancia un raycast nel punto cliccato
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // Se colpisce qualcosa
            if (hit.collider != null)
            {
                // Stampa il nome dell'oggetto cliccato
                Debug.Log("Ho cliccato: " + hit.collider.name);

                // Controlla se l'oggetto ha un componente Interactable
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                // Se esiste, esegue l'interazione
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}