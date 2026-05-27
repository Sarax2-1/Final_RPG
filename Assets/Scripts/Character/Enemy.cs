using UnityEngine;

public class Enemy : Character
{

    [SerializeField]
    private int expDrop;
    public int ExpDrop { get { return expDrop; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CharState.Walk:
                WalkUpdate();
                break;
            case CharState.WalkToEnemy:
                WalkToEnemyUpdate();
                break;
        }
    }

    protected override void Die()
    {
        base.Die();
        partyManager.DistributeTotalExp(expDrop);
    }
}
