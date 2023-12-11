using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour   //������ ����
{
    private int MonsterCount = 0;
    private int PlayerCount = 0;    //�ӽ�
    //Enable() : Start()�� ����� �뵵�� Ȱ��ȭ �� ������ ȣ�� ��

    private void Start()
    {
    }
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
        if (MonsterCount <= 0)
        {
            //���� ���� �� ������ ���� - �þ �ڵ� �з� �����ؼ� �����Ұ�
            GameObject savedata = GameObject.Find("SaveData");
            SaveData playerdata = savedata.GetComponent<SaveData>();
            GameObject player = GameObject.Find("MyPlayer");
            Entity playernow = player.GetComponent<Entity>();

            playerdata.SetPlayerHealth(playernow.health);
            SceneManager.LoadScene("MapScene");
        }
    }
}
