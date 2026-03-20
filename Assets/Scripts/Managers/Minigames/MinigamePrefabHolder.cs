using UnityEngine;

public enum MinigameType { None, Stomping, Gathering, HeadHit, Mixing };

public class MinigamePrefabHolder : MonoBehaviour
{
    public static MinigamePrefabHolder Instance;

    [SerializeField] private GameObject minigameStompingPrefab;
    [SerializeField] private GameObject minigameGatheringPrefab;
    [SerializeField] private GameObject minigameHeadHitPrefab;
    [SerializeField] private GameObject minigameMixingPrefab;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this.gameObject); }
    }

    public GameObject GetMinigame(MinigameType minigame)
    {
        switch (minigame)
        {
            case MinigameType.Stomping:
                return minigameStompingPrefab;
            case MinigameType.Gathering:
                return minigameGatheringPrefab;
            case MinigameType.HeadHit:
                return minigameHeadHitPrefab;
            case MinigameType.Mixing:
                return minigameMixingPrefab;
            default:
                Debug.LogError("couldn't find minigame: " + minigame);
                return null;
        }

    }

}
