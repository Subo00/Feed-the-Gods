using UnityEngine;

public enum MinigameType { None, Stomping, Gathering, HeadHit };

public class MinigamePrefabHolder : MonoBehaviour
{
    public static MinigamePrefabHolder Instance;

    [SerializeField] private GameObject minigameStompingPrefab;
    [SerializeField] private GameObject minigameGatheringPrefab;
    [SerializeField] private GameObject minigameHeadHitPrefab;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this.gameObject); }
    }

    public GameObject GetStomping() {  return minigameStompingPrefab;}
    public GameObject GetGathering() {  return minigameGatheringPrefab;}
    public GameObject GetHeadHit() {  return minigameHeadHitPrefab;}

}
