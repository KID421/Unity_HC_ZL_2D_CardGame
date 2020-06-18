using System.Collections.Generic;
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
        while (deck.Count < 30)
        {
            int r = Random.Range(1, GetCard.instance.cards.Length + 1);

            CardData card = GetCard.instance.cards[r - 1];

            List<CardData> sameCard = deck.FindAll(c => c.Equals(card));

            if (sameCard.Count < 2)
            {
                AddCard(r, "NPC");
            }
        }

        Invoke("Shuffle", 1);
    }
}
