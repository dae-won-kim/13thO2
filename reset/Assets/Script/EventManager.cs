using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour   //������ ����
{
    private int MonsterCount = 0;
    private int PlayerCount = 0;    //�ӽ�
    //Enable() : Start()�� ����� �뵵�� Ȱ��ȭ �� ������ ȣ�� ��
    void OnEnable()
    {
        EntityManager.EventEntitySpawn += CheckMonster;
        EntityManager.EventEntityDestroy += DestroyMonster;
        // ���⿡ �ʱ�ȭ �ڵ峪 �ٸ� �۾��� �߰��� �� �ֽ��ϴ�.
    }

    //Onenable()�� ¦�� 
    void OnDisable()    
    {
        EntityManager.EventEntitySpawn -= CheckMonster;
        EntityManager.EventEntityDestroy -= DestroyMonster;
    }

    void CheckMonster() //���� �� Ȯ�ο�
    {
        MonsterCount++;
        Debug.Log("���� ����");
        Debug.Log(MonsterCount);
    }

    void DestroyMonster()
    {
        MonsterCount--;
        Debug.Log("���� �ı�");
        if (MonsterCount  <= 0)
        {
            Debug.Log("�¸�");
        }
    }
}
