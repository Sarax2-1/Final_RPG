using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> monsters;
    public List<Enemy> Monsters
    { get { return monsters; } }

    public static EnemyManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Character m in monsters)
        {
            m.CharInit(UIManager.instance, InventoryManager.instance, PartyManager.instance);
        }
        InventoryManager.instance.AddItem(monsters[0], 0);
        InventoryManager.instance.AddItem(monsters[1], 0);
        InventoryManager.instance.AddItem(monsters[2], 0);
        InventoryManager.instance.AddItem(monsters[3], 0);
        InventoryManager.instance.AddItem(monsters[4], 0);
        InventoryManager.instance.AddItem(monsters[5], 0);



    }
}
