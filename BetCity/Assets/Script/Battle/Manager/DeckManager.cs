using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Transform deckPanel;
    public Transform libraryPanel;

    private Dictionary<int, GameObject> libraryDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> deckDic = new Dictionary<int, GameObject>();

    public GameObject cardPrefab;
    public GameObject deckPrefab;

    public GameObject DataManager;

    private PlayerData PlayerData;
    private CardStore CardStore;

    // 检查单张卡牌数量是否超过上限
    private bool IsSingleCardOverLimit(int cardId)
    {
        return PlayerData.playerDeck[cardId] >= 4;
    }

    // 检查卡组总数量是否超过上限
    private bool IsTotalCardsOverLimit()
    {
        int total = 0;
        foreach (int count in PlayerData.playerDeck)
        {
            total += count;
        }
        return total >= 25;
    }
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
        for (int i = 0; i < PlayerData.playerCard.Length; i++)
        {
            if (PlayerData.playerCard[i] > 0)
            {
                CreatCard(i, CardState.Library);
            }
        }
    }

    public void UpdateDeck()
    {
        for (int i = 0; i < PlayerData.playerDeck.Length; i++)
        {
            if (PlayerData.playerDeck[i] > 0)
            {
                CreatCard(i, CardState.Deck);
            }
        }
    }

    public void UpdateCard(CardState _state, int _id)
{
    if (_state == CardState.Deck)
    {
        if (deckDic.ContainsKey(_id))
        {
            PlayerData.playerCard[_id]++;
            PlayerData.playerDeck[_id]--;

            // 减少卡组中的卡牌数量
            if (!deckDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                deckDic.Remove(_id);
            }

            // 增加图书馆中的卡牌数量
            if (libraryDic.ContainsKey(_id))
            {
                libraryDic[_id].GetComponent<CardCounter>().SetCounter(1);
            }
            else
            {
                // 如果图书馆中没有这张卡，创建它
                CreatCard(_id, CardState.Library);
            }
        }
    }
    else if (_state == CardState.Library)
    {
        if (libraryDic.ContainsKey(_id))
        {
            if (IsSingleCardOverLimit(_id))
            {
                    Debug.LogWarning("单张卡牌数量不能超过4张");
                    return;
            }

            if (IsTotalCardsOverLimit())
            {
                    Debug.LogWarning("卡组总数量不能超过25张");
                    return;
            }
            PlayerData.playerCard[_id]--;
            PlayerData.playerDeck[_id]++;

            // 增加卡组中的卡牌数量
            if (deckDic.ContainsKey(_id))
            {
                deckDic[_id].GetComponent<CardCounter>().SetCounter(1);
            }
            else
            {
                // 如果卡组中没有这张卡，创建它
                CreatCard(_id, CardState.Deck);
            }

            // 减少图书馆中的卡牌数量
            if (!libraryDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                libraryDic.Remove(_id);
            }
        }
    }
}
    public void CreatCard(int _id, CardState _state)
    {
        Transform targetPanel;
        GameObject targetPrefab;
        var refData = PlayerData.playerCard;
        Dictionary<int, GameObject> targetDic = libraryDic;
        if (_state == CardState.Library)
        {
            targetPanel = libraryPanel;
            targetPrefab = cardPrefab;
        }
        else
        {
            targetPanel = deckPanel;
            targetPrefab = deckPrefab;
            refData = PlayerData.playerDeck;
            targetDic = deckDic;
        }
        GameObject newCard = Instantiate(targetPrefab, targetPanel);
        newCard.GetComponent<CardCounter>().SetCounter(refData[_id]);
        newCard.GetComponent<CardDisplay>().card = CardStore.cardList[_id];
        targetDic.Add(_id, newCard);
    }
}
