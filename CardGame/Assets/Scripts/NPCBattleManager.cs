using UnityEngine;

public class NPCBattleManager : BattleManager
{
    public static NPCBattleManager instanceNPC;

    protected override void Start()
    {
        instanceNPC = this;

        sceneName = "NPC 方場地";
        pos = -260;
    }

    protected override void CheckCoin()
    {
        firstAttack = !instance.firstAttack;

        int card = 3;

        if (firstAttack)
        {
            crystalTotal = 1;
            crystal = 1;
            card = 4;
        }

        Crystal();

        StartCoroutine(GetCard(card, NPCDeckManager.instanceNPC, 200, 275));
    }
}
