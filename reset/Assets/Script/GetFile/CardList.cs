using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    List<Item> past = new List<Item>();    //���� ����Ʈ
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //����Ʈ�� �������� ��ü �����ϵ��� �ϴ� �뵵

    public CardManager cardManager;
    [SerializeField]
    private Transform slotParent;
    [SerializeField] GameObject cardPrefab;
    public RectTransform content;

#if UNITY_EDITOR
    private void OnValidate()
    {
        
    }
#endif

    void Awake()
    {
        FreshSlot();
    }

    private void Start()
    {
    }

    public void FreshSlot()
    {
        //items = cardManager.GetItemBuffer();
        
        /*for(; i <slots.Length;i++)
        {
            slots[i].item = null;
        }  */
    }
    public void AddCard(Item item)  //���ſ� ������ �̹��� ���� �뵵
    {
        var cardObject = Instantiate(cardPrefab, content);
        var card = cardObject.GetComponent<Slot>();
        card.Setup(item);
        past.Add(item);

        cardnamePrefabslist.Add(cardObject);
    }
    
    public void GetCardList(Item cards)
    {
        print("ī�� ������ ����"+cardManager.GetItemBuffer().Count);
        for(int i=0; i<cardManager.GetItemBuffer().Count;i++)
        {
            past[i] = cardManager.GetItemBuffer()[i];
        }
        FreshSlot();
    }

    public List<Item> GetPast()
    {
        return past;
    }
    public void ClearItems()
    {
        foreach(var prefab in cardnamePrefabslist)  //��� ������ ����
        {
            Destroy(prefab);
        }
        past.Clear();   //���� ����
        FreshSlot();
    }
}
