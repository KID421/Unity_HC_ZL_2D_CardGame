using UnityEngine;

public class NPCDeckManager : DeckManager
{
    public static NPCDeckManager instanceNPC;

    protected override void Awake()
    {
        instanceNPC = this;

        btnStart.onClick.AddListener(Choose30Card);
    }

    protected override void Update()
    {
        
    }

    protected override void Choose30Card()
    {
        base.Choose30Card();

        Invoke("Shuffle", 1);
    }
}
