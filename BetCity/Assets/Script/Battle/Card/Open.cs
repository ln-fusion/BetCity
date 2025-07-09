using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject cardPool;
    CardStore CardStore;

    // Start is called before the first frame update
    void Start()
    {
        CardStore = GetComponent<CardStore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickOpen()
    {
        GameObject newCard = GameObject.Instantiate(cardPrefab,cardPool.transform);
        newCard.GetComponent<CardDisplay>().card = CardStore.RandomCard();
    }    
}
