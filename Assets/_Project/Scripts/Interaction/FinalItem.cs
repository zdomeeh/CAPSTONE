using UnityEngine.SceneManagement;

public class FinalItem : Item
{
    public string endSceneName = "EndDemo";

    public override void PickUp()
    {
        // Esegue il comportamento base della raccolta dell'oggetto
        base.PickUp();

        // Quando finisce il prossimo dialogo, carica la scena finale
        InkManager.Instance.OnStoryEnd += LoadEndScene;
    }

    private void LoadEndScene()
    {
        // Carica la scena finale del gioco
        SceneManager.LoadScene(endSceneName);
    }
}