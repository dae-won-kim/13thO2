using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster    //���� �������� �ٹ̴°� ���� ��� ������ ���_���� ������ �ۼ��� ��
{                       //�ϱ� - Low / ��� - Senior / ����Ʈ - Elite / ���� - Boss
    public string name;
    public string grade; //���
    public string monsterfunctionname;  //���� ���
    public Sprite sprite;
    public int maxhealth;
    public int health;
    public int attack;
    public float percent;
}

[CreateAssetMenu(fileName = "MonsterSO", menuName = "Scriptable Object/MonsterSO")]   //���� �޴��� �߰� ��������
public class MonsterSO : ScriptableObject
{
    public Monster[] monsters;
}
