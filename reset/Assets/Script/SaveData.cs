using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public ItemSO itemSO;
    List<Item> cardlist = new List<Item>();
    List<Item> deck = new List<Item>();
    List<int> cardcount = new List<int>();
    // Start is called before the first frame update
    private static SaveData instance;   //�̱������� ����
    private int playerhealth = 300;
    private void Awake()
    {
        if (instance == null)   //�� ��ȯ �� ������ ���� �뵵
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InputCardInDeck(Item name)
    {
        cardlist.Add(name);
        //Debug.Log(cardlist);
    }
    
    /*public void Test()  //ī�� �ż� ���� �� ItemSO�� �����Ͽ� �� �����ϴ� ��
    {
        Debug.Log("11111111111111");
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            cardlist.Add(item);
        }

        foreach (Item A in cardlist)
        {
            Debug.Log(A.name);
        }
    }*/ //�� �־����� �𸣰ھ �ϴ� ����

    public void DefaultDeckSetting()    //�⺻ �� �����ϴ� ��
    {
    }
    public List<Item> GetPlayerDeck()
    {
        return cardlist;
    }

    public void SetPlayerHealth(int a)
    {
        playerhealth = a;
    }

    public int GetPlayerHealth()
    {
        return playerhealth;
    }
}
