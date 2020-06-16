using UnityEngine;

public class NPCBattlerManager : BattleManager
{
    public static NPCBattlerManager instanceNPC;

    protected override void Start()
    {
        instanceNPC = this;
    }

    private void RandomCard()
    {
        //DeckManager.instance.deck
    }
}
