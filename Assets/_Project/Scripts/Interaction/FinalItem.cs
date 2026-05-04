using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalItem : Item
{
    public string endSceneName = "EndDemo";

    public override void PickUp()
    {
        base.PickUp();

        InkManager.Instance.OnStoryEnd += LoadEndScene;
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene(endSceneName);
    }
}