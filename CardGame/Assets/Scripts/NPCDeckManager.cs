using UnityEngine;

public class NPCDeckManager : DeckManager
{
    public static NPCDeckManager instanceNPC;

    protected override void Awake()
    {
        instanceNPC = this;

        btnStart.onClick.AddListener(RandomAddCard);
    }

    protected override void Update()
    {

    }

    private void RandomAddCard()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 1; j <= 15; j++)
            {
                AddCard(j);
            }
        }

        Invoke("Shuffle", 3);
    }
}
