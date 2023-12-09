using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public List<Item> items;


    public CardManager cardManager;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        items = cardManager.GetItemBuffer();
        int i = 0;
        for(;i <items.Count && i< slots.Length;i++) {
            slots[i].item = items[i];
            if (items[i] == null ) {
                slots[i].item = null;
            }
        }
        /*for(; i <slots.Length;i++)
        {
            slots[i].item = null;
        }  */
    }
    public void AddCard(Item _item)
    {
        
        if(items.Count < slots.Length)
        {
            FreshSlot();
            items.Add(_item);
            FreshSlot();
            print("ī���߰�!!!");
        }
        else
        {
            print("���԰�����");
        }
    }
    
    public void GetCardList(Item cards)
    {
        print("ī�� ������ ����"+cardManager.GetItemBuffer().Count);
        for(int i=0; i<cardManager.GetItemBuffer().Count;i++)
        {
            items[i] = cardManager.GetItemBuffer()[i];
        }
        FreshSlot();
    }
}
