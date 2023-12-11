using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;  //���� ����� ����

public class Entity : MonoBehaviour //�ش� ������ ���� ���ڸ� ���� ��ȹ �׷��� �ٸ� monsterSO�� ����
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();  //���� ���� �� ���� ���� ������� ����Ǿ� ť�� ����
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text shieldTMP;
    [SerializeField] GameObject hpline;

    Queue<StatusEffect> myStatusEffect = new Queue<StatusEffect>();
    public Monster monster;
    public CardManager cardmanager;
    public int attack;
    public int maxhealth = 40;
    public int health = 40;
    float hppercent;
    public int shield = 0;
    public string monsterfunctionname;
    public bool isMine;
    public bool myTurn;
    public bool isDie;
    public bool isDamaged;
    public bool isBossOrEmpty;
    public bool attackable;
    //�����̻� ����
    public bool debuffPoisonBool;
    public int debuffPosionInt = 0;
    public Vector3 originPos;
    public int liveCount = 0;
    public int poisonCount = 0;
    public bool canplay = true;
    public bool issleep = false;
    public bool hasmask = false;

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
        maxhealth = monster.maxhealth;
        health = int.Parse(healthTMP.text); //�Ƹ� �Ȱ��� monsterSO�� ���� ���͸� �����ҵ�
        attack = int.Parse(attackTMP.text);
        shield = int.Parse(shieldTMP.text);
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
        healthTMP.text = health.ToString();

        if (health <= 0)
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
    public int GetShieldTMP()      //SerializeField�� ���� ��ȣ�������� ���� ���� ������ ���
    {
        return int.Parse(shieldTMP.text);
    }

    public void SetHealthTMP()  //ü���� health�� ����
    {
        healthTMP.text = health.ToString();
        hpline.transform.localScale = new Vector3(1 - (float)health/maxhealth, 0.65f, 1f);
    }

    public void SetShieldTMP()
    {
        shieldTMP.text = shield.ToString();
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
            TMP_Text playershieldTMP = playerObject.GetComponentInChildren<TMP_Text>();
            if (playerhealthTMP != null && playershieldTMP != null)
            {
                // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
                int currentHealth = int.Parse(playerhealthTMP.text);
                int currentShield = int.Parse(playershieldTMP.text);
                if (health >= 5 && liveCount > 3)   //ü���� 5 �̻�, �ϼ� 3 �ʰ�
                {
                    int random = UnityEngine.Random.Range(0, 10);
                    if (random < 5)
                    {
                        if(playerEntity.shield > 0)
                        {
                            playerEntity.shield -= 4;
                            currentShield = playerEntity.shield;
                            playerEntity.SetShieldTMP();
                        }
                        else
                        {
                            playerEntity.health -= 4;
                            currentHealth = playerEntity.health;
                        }

                    }
                    else
                    {
                        playerEntity.health -= 8;
                        currentHealth = playerEntity.health;
                    }
                }
                else if (health >= 5)   //ü�� 5 �̻� ����
                {
                    if (playerEntity.shield > 0)
                    {
                        Debug.Log("���� �����");
                        playerEntity.shield -= 5;
                        playerEntity.SetShieldTMP();
                    }
                    else
                    {
                        playerEntity.health -= 5;
                        currentHealth = playerEntity.health;
                    }
                }
                else if (health < 5)
                {
                    Debug.Log("�͵�");
                    playerEntity.poisonCount = 1;
                }

                playerhealthTMP.text = currentHealth.ToString();
                playerEntity.SetHealthTMP();

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

    #region MakeEffect
    public void MakeAttackUp(int damage, int count)
    {
        Debug.Log("Effect - Attack Up");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetPowerUp(damage, count);
        myStatusEffect.Enqueue(newEffect);
    }

    public void MakeAttackDown(int damage, int count)
    {
        Debug.Log("Effect - Attack Down");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetPowerDown(damage, count);
        myStatusEffect.Enqueue(newEffect);
    }

    public void MakeShield(int amount, int turn)
    {
        Debug.Log("Effect - Shield");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetShield(amount, turn);
        myStatusEffect.Enqueue(newEffect);
        shield += amount;
        SetShieldTMP();
    }

    public void MakeFaint(int turn) //���� ����
    {
        Debug.Log("Effect - Faint");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetFaint(turn);
        myStatusEffect.Enqueue(newEffect);
    }

    public void MakeSleep(int turn) //���� ����
    {
        Debug.Log("Effect - Sleep");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetSleep(turn);
        myStatusEffect.Enqueue(newEffect);
        issleep = true;
    }

    public void MakeImmuneSleep(int turn)   //���� �鿪 ����
    {
        Debug.Log("Effect - Immnue Sleep");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetImmuneSleep(turn);
        myStatusEffect.Enqueue(newEffect);
    }
    #endregion
    public int GetAllAttackUpEffect()   //���ݷ� ���� ȿ�� ��������
    {
        int result = 0;
        foreach(StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackUp();
        }
        return result;
    }

    public int GetAllAttackDownEffect()   //���ݷ� ���� ȿ�� ��������
    {
        int result = 0;
        foreach (StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackDown();
        }
        return result;
    }

    public void GetAllCC()
    {
        foreach (StatusEffect obj in myStatusEffect)
        {
            if(obj.GetFaint())
            {
                Debug.Log("You are faint");
                canplay = false;
            }
        }
    }

    public bool GetSleep()  //�ӽÿ�
    {
        int sleep = Random.Range(0, 10);    //0~9�� ����
        foreach (StatusEffect obj in myStatusEffect)
        {
            if (obj.GetImmuneSleep())
            {
                Debug.Log("I can't sleep");
                sleep = 100;
                break;
            }
        }
        Debug.Log(sleep);
        if (sleep < 7)   //0,1,2,3,4,5,6 = 70% = ����
        {
            Debug.Log("I sleep");
            return true;
        }
        Debug.Log("I don't sleep");
        return false;
    }
}

class StatusEffect
{
    public bool ispowerUp = false;
    public bool ispowerDown = false;
    public bool isshield = false;   //���� ���� ����
    public bool isfaint = false;    //���� ���� ����
    public bool issleep = false;    //���� ���� ����
    public bool isimmunesleep = false;
    public int effectamount;    //ȿ���� ��
    public int effectcount;   //Ƚ��
    public int effectturn;    //���� �� ��
    #region PowerUp
    public void SetPowerUp(int amount, int count)
    {
        effectamount = amount;
        effectcount = count;
        ispowerUp = true;
    }

    public int GetAllAttackUp()
    {
        if (ispowerUp)
        {
            return effectamount;
        }
        return 0;
    }
    #endregion

    #region PowerDown
    public void SetPowerDown(int amount, int count)
    {
        effectamount = amount;
        effectcount = count;
        ispowerDown = true;
    }

    public int GetAllAttackDown()
    {
        if (ispowerDown)
        {
            return effectamount;
        }
        return 0;
    }
    #endregion

    #region Shield
    public void SetShield(int amount, int turn)
    {
        effectamount = amount;
        effectturn = turn;
        isshield = true;
    }
    #endregion

    #region Faint
    public void SetFaint(int turn)  //���� ����
    {
        effectturn = turn;
        isfaint = true;
    }

    public bool GetFaint()  //�ش� ��ġ���� ���� �鿪 üũ
    {
        if(isfaint)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Sleep
    public void SetSleep(int turn)
    {
        effectturn = turn;
    }

    /*public bool GetSleep()  //����� �� canplay�� �ٷ� ������
    {
        int sleep = Random.Range(0, 10);    //0~9�� ����
        Debug.Log(sleep);
        if(sleep < 7)   //0,1,2,3,4,5,6 = 70% = ����
        {
            return false;
        }
        return true;
    }*/

    public void SetImmuneSleep(int turn)
    {
        effectturn = turn;
        isimmunesleep = true;
    }

    public bool GetImmuneSleep()    
    {
        if (isimmunesleep)
        {
            Debug.Log(isimmunesleep);
            return true;
        }
        return false;
    }
    #endregion
}
