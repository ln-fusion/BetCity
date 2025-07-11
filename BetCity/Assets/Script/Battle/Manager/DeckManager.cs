using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Transform deckPanel;
    public Transform libraryPanel;

    public GameObject cardPrefab;
    public GameObject deckPrefab;

    public GameObject DataManager;

    private PlayerData PlayerData;
    private CardStore CardStore;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = DataManager.GetComponent<PlayerData>();
        CardStore = DataManager.GetComponent<CardStore>();
        UpdateLibrary();
        UpdateDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLibrary()
    {
        for(int i=0;i<PlayerData.playerCard.Length;i++)
        {
            if(PlayerData.playerCard[i]>0)
            {
                GameObject newCard  =Instantiate(cardPrefab,libraryPanel);
                newCard.GetComponent<CardCounter>().counter.text = PlayerData.playerCard[i].ToString();
                newCard.GetComponent<CardDisplay>().card = CardStore.cardList[i];
            }
        }
        
    }
    public void UpdateDeck()
    {
        for (int i = 0; i < PlayerData.playerDeck.Length; i++)
        {
            if (PlayerData.playerDeck[i] > 0)
            {
                GameObject newCard = Instantiate(deckPrefab, deckPanel);
                newCard.GetComponent<CardCounter>().counter.text = PlayerData.playerDeck[i].ToString();
                newCard.GetComponent<CardDisplay>().card = CardStore.cardList[i];
            }
        }

    }
}
