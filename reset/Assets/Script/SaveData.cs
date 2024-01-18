using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public ItemSO itemSO;
    List<Item> cardlist = new List<Item>();
    List<Item> deck = new List<Item>();
    List<int> cardcount = new List<int>();
    string constellation = "Sheep"; //�⺻���� sheep����
    // Start is called before the first frame update
    private static SaveData instance;   //�̱������� ����
    private int playermaxhelath = 100;
    private int playerhealth = 100;
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

    public void ResetCardList() //����� ī�� ����Ʈ �ʱ�ȭ
    {
        Debug.Log("�ʱ�ȭ �Ϸ�");
        cardlist.Clear();
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
        if (a > playermaxhelath)
            playerhealth = playermaxhelath;
    }

    public void SetPlayerMaxHealth(int a)
    {
        playermaxhelath = a;
    }
    
    public int GetPlayerMaxHealth()
    {
        return playermaxhelath;
    }

    public int GetPlayerHealth()
    {
        return playerhealth;
    }

    public float GetPlayerHealthPercent()
    {
        return (float)playerhealth / playermaxhelath;
    }

    public void SetPlayerConstellation(string name)     //������ ���ڸ� ����
    {
        constellation = name;
        Debug.Log(constellation);
    }

    public string GetPlayerConstellation()
    {
        return constellation;
    }
}
