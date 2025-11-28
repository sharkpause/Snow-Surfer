using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        int numberOfScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;

        if (numberOfScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
