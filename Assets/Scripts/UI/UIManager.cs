using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public MenuUI MenuUI => menuUI;

    [SerializeField] private Canvas canvas;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private QuestUI questUI;
    [SerializeField] private GameObject playerUIManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public PlayerUIManager CreatePlayerUI()
    {
        var playerHUDObject = Instantiate(playerUIManager, transform);
        return playerHUDObject.GetComponent<PlayerUIManager>();
    }
}
