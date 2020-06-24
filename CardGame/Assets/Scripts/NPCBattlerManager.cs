using UnityEngine;

public class NPCBattlerManager : BattleManager
{
    public static NPCBattlerManager instanceNPC;

    protected override void Start()
    {
        instanceNPC = this;

        scenePos = -270;
        player = false;
    }

    protected override void CheckCoin()
    {
        firstAttack = !BattleManager.instance.firstAttack;

        int card = 3;

        if (firstAttack)
        {
            crystalTotal = 1;
            crystal = 1;
            card = 4;
        }

        Crystal();

        StartCoroutine(GetCard(card, 500, 100, 275, NPCDeckManager.instanceNPC));
    }

    public override void EndTurn()
    {
        BattleManager.instance.StartTurn(DeckManager.instance, -100, -275);
    }
}
