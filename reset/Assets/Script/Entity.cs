using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Entity : MonoBehaviour //�ش� ������ ���� ���ڸ� ���� ��ȹ �׷��� �ٸ� monsterSO�� ����
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;

    public Monster monster;
    public CardManager cardmanager;
    public int attack;
    public int health = 40;
    public string monsterfunctionname;
    public bool isMine;
    public bool myTurn;
    public bool isDie;
    public bool isBossOrEmpty;
    public bool attackable;
    //�����̻� ����
    public bool debuffPoisonBool;
    public int debuffPosionInt = 0;
    public Vector3 originPos;
    public int liveCount = 0;
    public int poisonCount = 0;

    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
        monsterPatterns["Snail"] = () => SnailPattern();
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;   
    }

    void OnTurnStarted(bool myTurn)
    {
        if (isBossOrEmpty)
            return;

        if (isMine == myTurn)
        {
            liveCount++;
            BuffDown(1);
        }
            
    }

    public void BuffDown(int count)  //���� ���ӽð��� ��� ȿ��
    {
        poisonCount -= count;
    }
    public void Setup(Monster monster)
    {
        this.monster = monster;
        health = int.Parse(healthTMP.text); //�Ƹ� �Ȱ��� monsterSO�� ���� ���͸� �����ҵ�
        attack = int.Parse(attackTMP.text);
        monsterfunctionname = this.monster.monsterfunctionname;

        this.monster = monster;
        charater.sprite = this.monster.sprite;
        healthTMP.text = this.monster.health.ToString();
        attackTMP.text = this.monster.attack.ToString();
    }

    private void OnMouseDown()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDown(this);
    }

    private void OnMouseUp()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseUp();
    }

    private void OnMouseDrag()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDrag();
    }

    public bool Damaged(int damage)
    {
        health -= damage;
        healthTMP.text = health.ToString();
        

        if(health <= 0)
        {
            isDie = true;
            return true;
        }
        return false;
    }

    public int GetHealthTMP()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
    {
        return int.Parse(healthTMP.text);
    }
    public int GetAttackTMP()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
    {
        return int.Parse(attackTMP.text);
    }
    public int GetLiveCount()
    {
        return liveCount;
    }

    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }

    #region Buff

    public void DebuffPosion()
    {
        if(poisonCount > 0)    //�������� ȿ���� �߰��ؾ��ҷ���...
        {
            Debug.Log("test");
            health--;
            healthTMP.text = health.ToString();
            return;
        }
    }

    #endregion


    #region MonsterPattern

    private void SnailPattern()
    {
        // Player �±׸� ���� ��� ���� ������Ʈ�� ã���ϴ�.
        Entity playerEntity = GameObject.Find("MyPlayer").GetComponent<Entity>();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            // �� Player ���� ������Ʈ���� TMP_Text ������Ʈ�� ã���ϴ�.
            TMP_Text playerhealthTMP = playerObject.GetComponentInChildren<TMP_Text>();
            if (playerhealthTMP != null)
            {
                // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
                int currentHealth = int.Parse(playerhealthTMP.text);

                if (health >= 5 && liveCount > 3)
                {
                    int random = UnityEngine.Random.Range(0, 10);
                    if (random < 5)
                    {
                        playerEntity.health -= 4;
                        currentHealth = playerEntity.health;
                    }
                    else
                    {
                        playerEntity.health -= 8;
                        currentHealth = playerEntity.health;
                    }
                }
                else if (health >= 5)
                {
                    // -5�� �ϰ� �� ����
                    playerEntity.health -= 5;
                    currentHealth = playerEntity.health;
                }
                else if (health < 5)
                {
                    Debug.Log("�͵�");
                    playerEntity.poisonCount = 1;
                }

                playerhealthTMP.text = currentHealth.ToString();

            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Player GameObject.");
            }
        }
    }

    // ���� ���� ���� �޼���
    public void ExecutePattern(string patternName)
    {
        if (monsterPatterns.TryGetValue(patternName, out Action monsterPattern))
        {
            // �ش� ī���� ��� ����
            monsterPattern();
        }
        else
        {
            Debug.Log("MonsterPattern not found.");
        }
    }
    #endregion MonsterPattern
}
