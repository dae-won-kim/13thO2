using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Entity : MonoBehaviour //�ش� ������ ���� ���ڸ� ���� ��ȹ �׷��� �ٸ� monsterSO�� ����
{
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;

    public Monster monster;
    public int attack;
    public int health;
    public string monsterfunctionname;
    public bool isMine;
    public bool myTurn;
    public bool isDie;
    public bool isBossOrEmpty;
    public bool attackable;
    public Vector3 originPos;
    int liveCount = 0;

    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
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
            liveCount++;
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


}
