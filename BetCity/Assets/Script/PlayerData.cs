using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public CardStore CardStore;

    public int[] playerCard;
    public int[] playerDeck;

    public TextAsset playerData;


    // Start is called before the first frame update
    void Start()
    {
        CardStore.LoadCardData();
        LoadPlayerData();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayerData()
    {
        playerCard = new int[CardStore.cardList.Count];
        playerDeck = new int[CardStore.cardList.Count];
        string[] dataRow = playerData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerCard[id] = num;
            }
            else if (rowArray[0] == "deck")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerDeck[id] = num;
            }
        }
    }

    public void SavePlayerData()
    {
        string path = Application.dataPath + "/Data/playerdata.csv";
        List<string> datas = new List<string>();
        for(int i=0;i<playerCard.Length;i++)
        {
            if (playerCard[i]!=0)
            {
            datas.Add("card," + i.ToString()+ "," + playerCard[i].ToString());
            }

        }
        //保存卡组
        for (int i = 0; i < playerDeck.Length; i++)
        {
            if (playerDeck[i] != 0)
            {
                datas.Add("deck," + i.ToString() + "," + playerDeck[i].ToString());
            }

        }
        //保存
        File.WriteAllLines(path, datas);
        Debug.Log("保存成功");
    }
}
