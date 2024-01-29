using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item   //ī�� �������� �ٹ̴� ��
{
    public string name;
    public string functionname;
    public int cost;
    public Sprite colorimg;
    public Sprite costcolor;
    public string cardtype;
    public char color;   //R:R G:G B:B
    public int rarity;   //ī�� ���
    //public Sprite sprite;
    public string reward;   //���� ��ġ ���� - ���躸��, ���� ��
    public int price;
    public string active;
    public float percent;
    public bool selectable;
    public int identifier;  //�׽�Ʈ�� �ĺ���
    private static int calletc = 0;
    /*
    �⺻ ���ڸ� 1 - int�� 00000000�� �ۼ� �ȵ�
    �ڽ�Ʈ 00 
    �� 0                 //RGB 0 1 2 ����
    �׼� ���� ���� 0      //�׼� 0 ���� 1 ���� 2
    ��Ÿ �ĺ��� ī�� �ѹ� 0000   //������ ���Ѱ� SO���� ������ �ӽ÷� ����

    �׽�Ʈ������ ����� �κ��̰� ���� �Ϻκ��� �ڵ����� �ĺ��� �����ϵ��� �� ��

    ex 100000000	0�ڽ�Ʈ R �׼� ī�� 0000
    ���� ��ȹ������ char�� ������ �Ϻκ� ����� �����ϴµ� �ϳ�
    ī�� ������ �켱����, ���� �� ������ �ű������ �����ļ� int�� ������
    */

}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]   //���� �޴��� �߰� ��������
public class ItemSO : ScriptableObject
{
    public Item[] items;
}

