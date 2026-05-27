using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] heroPrefabs;
    public GameObject[] HeroPrefabs { get { return heroPrefabs; } }

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (Setting.isNewGame)
        {
            Setting.isNewGame = false;
            GeneratePlayerHero();
            AudioManager.instance.PlayBGM(1); //Play Village BGM
        }

        if (Setting.isWarping)
        {
            Setting.isWarping = false;
            WarpPlayers();
        }
    }

    private void GeneratePlayerHero()
    {
        int i = Setting.playerPrefabId;

        GameObject heroObj = Instantiate(heroPrefabs[i],
            new Vector3(46f, 10f, 38f), quaternion.identity);

        heroObj.tag = "Player";

        Character hero = heroObj.GetComponent<Character>();
        PartyManager.instance.Members.Add(hero);

        hero.CharInit(UIManager.instance, InventoryManager.instance, PartyManager.instance);

        InventoryManager.instance.AddItem(hero, 0); //health potion
        InventoryManager.instance.AddItem(hero, 2); //Shield A
    }

    private void WarpPlayers()
    {
        PartyManager.instance.LoadAllHeroData();
    }
}
