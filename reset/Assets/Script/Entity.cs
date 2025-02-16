using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;  //랜덤 사용을 위함

public class Entity : MonoBehaviour //해당 내용을 통해 별자리 생성 계획 그래서 다른 monsterSO를 만듦
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();  //버프 제거 시 선입 선출 방식으로 예상되어 큐로 설정
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] SpriteRenderer patternUI;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text shieldTMP;
    [SerializeField] GameObject hpline;
    [SerializeField] Sprite AttackUI;
    [SerializeField] Sprite ShieldUI;
    [SerializeField] Sprite EffectUI;
    [SerializeField] Sprite WhatUI;

    List<StatusEffect> myStatusEffect = new List<StatusEffect>();
    public Monster monster;
    public int attack;
    public int maxhealth = 40;
    public int health = 40;
    public int pastHealth;
    float hppercent;
    public int shield = 0;
    public string monsterfunctionname;
    public bool isMine;
    public bool myTurn;
    public bool isDie;
    public bool isDamaged;
    public bool isBossOrEmpty;
    public bool attackable;
    //상태이상 관련
    public bool debuffPoisonBool;
    public int debuffPosionInt = 0;
    public Vector3 originPos;
    public int liveCount = 0;
    public bool canplay = true;
    public bool issleep = false;
    public bool hasmask = false;

    private int pattern;
    private string patternname;
    private bool isfirst = true;   //첫턴에 UI설정을 위해서 만든 위치
    private int addtionpattern = 0;

    void Start()
    {
        monsterPatterns["Snail"] = () => SnailPattern();
        monsterPatterns["Hcoronatus"] = () => HcoronatusPattern();
        pattern = Random.Range(0,10);
        ExecutePattern(monsterfunctionname);    //isfirst를 이용해서 처음에 사용할 패턴을 정하게 해 둠 
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
        }
            
    }
    public void Setup(Monster monster)
    {
        this.monster = monster;
        maxhealth = monster.maxhealth;
        health = monster.health;
        attack = int.Parse(attackTMP.text);
        shield = monster.shield;
        monsterfunctionname = this.monster.monsterfunctionname;

        this.monster = monster;
        charater.sprite = this.monster.sprite;
        healthTMP.text = this.monster.health.ToString();
        shieldTMP.text = this.monster.shield.ToString();
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

    public int GetHealthTMP()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return int.Parse(healthTMP.text);
    }
    public int GetAttackTMP()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return int.Parse(attackTMP.text);
    }
    public int GetShieldTMP()      //SerializeField로 인한 보호수준으로 인해 값을 보내는 기능
    {
        return int.Parse(shieldTMP.text);
    }

    public void SetHealthTMP()  //체력을 health로 설정
    {
        if (health >= maxhealth)
            health = maxhealth;
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

    public void SetPastHealth()
    {
        pastHealth = health;
    }

    #region MonsterPattern
    #region Snail
    private void SnailPattern()
    {
        if (isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = 8;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", "monster");
                    break;
                case "effect":
                    CardFunctionManager.Inst.Poison("player", 4); 
                    break;
                case "shield":
                    MakeShield(4, 1);
                    break;
                default:
                    break;
            }
        }

        pattern = Random.Range(0, 10);   //마지막 패턴 설정
        switch (pattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                patternname = "attack"; //이후 이미지 변경
                patternUI.sprite = AttackUI;
                break;
            case 5:
            case 6:
                patternname = "effect";
                patternUI.sprite = EffectUI;
                break;
            case 7:
            case 8:
            case 9:
                patternname = "shield";
                patternUI.sprite = ShieldUI;
                break;
        }
    }
    #endregion  Hcoronatus  난삼귀가 뭔지 몰라서 난초사마귀로 임시대체

    private void HcoronatusPattern()
    {
        if(isfirst)
        {
            isfirst = false;
        }
        else
        {
            int damage = 5;
            damage += GetAllAttackUpEffect();
            damage -= GetAllAttackDownEffect();
            switch (patternname)
            {
                case "attack":
                    CardFunctionManager.Inst.Attack("player", damage, "normal", "monster");
                    CardFunctionManager.Inst.Attack("player", damage, "normal", "monster");
                    break;
                case "effect":
                    MakeAttackUp(2, 2);
                    break;
                case "shield":
                    MakeShield(10, 1);
                    Debug.Log("This monster make Shield");
                    break;
                default:
                    break;
            }
        }

        pattern = Random.Range(0,10);   //마지막 패턴 설정
        switch(pattern)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                patternname = "attack"; //이후 이미지 변경
                patternUI.sprite = AttackUI;
                break;
            case 5:
            case 6:
                patternname = "effect";
                patternUI.sprite = EffectUI;
                break;
            case 7:
            case 8:
            case 9:
                patternname = "shield";
                patternUI.sprite = ShieldUI;
                break;
        }
        Debug.Log(patternname);
    }
    #endregion

    #region Utils

    // 몬스터 패턴 실행 메서드
    public void ExecutePattern(string patternName)
    {
        if (monsterPatterns.TryGetValue(patternName, out Action monsterPattern))
        {
            // 해당 몬스터의 기능 실행
            monsterPattern();
        }
        else
        {
            Debug.Log("MonsterPattern not found.");
        }
    }

    #endregion

    #region MakeEffect  //작동방식 거의 동일함
    public void MakeAttackUp(int damage, int count)
    {
        Debug.Log("Effect - Attack Up");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetPowerUp(damage, count);
        myStatusEffect.Add(newEffect);
    }

    public void MakeAttackDown(int damage, int count)
    {
        Debug.Log("Effect - Attack Down");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetPowerDown(damage, count);
        myStatusEffect.Add(newEffect);
    }

    public void MakeShield(int amount, int turn)
    {
        Debug.Log("Effect - Shield");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetShield(amount, turn);
        myStatusEffect.Add(newEffect);
        shield += amount;
        SetShieldTMP();
    }

    public void MakeFaint(int turn) //기절 생성
    {
        Debug.Log("Effect - Faint");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetFaint(turn);
        myStatusEffect.Add(newEffect);
    }

    public void MakeSleep(int turn) //수면 생성
    {
        Debug.Log("Effect - Sleep");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetSleep(turn);
        myStatusEffect.Add(newEffect);
        issleep = true;
    }

    public void MakeImmuneSleep(int turn)   //수면 면역 생성
    {
        Debug.Log("Effect - Immnue Sleep");
        StatusEffect newEffect = new StatusEffect();
        newEffect.SetImmuneSleep(turn);
        myStatusEffect.Add(newEffect);
    }

    public void MakePoison(int turn)
    {
        Debug.Log("Effect - Poison");
        StatusEffect neweffect = new StatusEffect();
        neweffect.SetPoison(turn);
        myStatusEffect.Add(neweffect);
    }

    public void MakeBurn(int damage, int turn)
    {
        Debug.Log("Effect - Burn");
        StatusEffect neweffect = new StatusEffect();
        neweffect.SetBurn(damage, turn);
        myStatusEffect.Add(neweffect);
    }

    public void MakeHealTurn(int turn)
    {
        Debug.Log("Effect - HealTurn");
        StatusEffect neweffect = new StatusEffect();
        neweffect.SetHealTurn(turn);
        myStatusEffect.Add(neweffect);
    }
    #endregion
    public int GetAllAttackUpEffect()   //공격력 증가 효과 가져오기
    {
        int result = 0;
        foreach(StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackUp();
        }
        return result;
    }

    public int GetAllAttackDownEffect()   //공격력 감소 효과 가져오기
    {
        int result = 0;
        foreach (StatusEffect obj in myStatusEffect)
        {
            result += obj.GetAllAttackDown();
        }
        return result;
    }
    public bool GetSleep()  //임시용
        {
            int sleep = Random.Range(0, 10);    //0~9의 난수
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
            if (sleep < 7)   //0,1,2,3,4,5,6 = 70% = 실패
            {
                Debug.Log("I sleep");
                return true;
            }
            Debug.Log("I don't sleep");
            return false;
        }
    public void GetAllCC()
    {
        foreach (StatusEffect obj in myStatusEffect)
        {
            if(obj.GetFaint())
            {
                Debug.Log("You are faint");
                canplay = false;
                break;
            }
        }
    }

    public void CheckEffect()
    {
        for(int i = myStatusEffect.Count - 1; i >= 0; i--)  //반드시 역순으로 지울 것 
        {
            Debug.Log(myStatusEffect[i].CheckDamageEffect());
            if (myStatusEffect[i].CheckDamageEffect().Item1)  //지속피해 효과 여부
            {
                health -= myStatusEffect[i].CheckDamageEffect().Item2;
                SetHealthTMP();
            }
            myStatusEffect[i].DecreaseEffectTurn();
            if(myStatusEffect[i].GetEffectTurn() <= 0)
            {
                myStatusEffect.RemoveAt(i);
            }
        }
    }

    public bool CheckBlockHeal()
    {
        foreach(StatusEffect A in myStatusEffect)
        {
            if (A.GetHealBlock())
                return true;
        }
        return false;
    }
}

class StatusEffect  //스택 형식의 효과는 없앤 상태임
{
    bool ispowerUp = false;
    bool ispowerDown = false;
    bool isshield = false;   //쉴드 존재 여부
    bool isfaint = false;    //기절 존재 여부
    bool issleep = false;    //수면 존재 여부
    bool isdamageeffect = false;    //피해를 주는지 여부
    bool isimmunesleep = false;
    bool canheal = true;
    int effectamount = 0;    //효과의 양
    int effectturn = 0;    //지속 턴 수
    string effectname;
    #region PowerUp
    public void SetPowerUp(int amount, int turn)
    {
        effectamount = amount;
        effectturn = turn;
        ispowerUp = true;
        effectname = "powerup";
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
    public void SetPowerDown(int amount, int turn)
    {
        effectamount = amount;
        effectturn = turn;
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
    public void SetFaint(int turn)  //수면 생성
    {
        effectturn = turn;
        isfaint = true;
    }

    public bool GetFaint()  //해당 위치에서 수면 면역 체크
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

    /*public bool GetSleep()  //사용할 때 canplay를 바로 설정함
    {
        int sleep = Random.Range(0, 10);    //0~9의 난수
        Debug.Log(sleep);
        if(sleep < 7)   //0,1,2,3,4,5,6 = 70% = 실패
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

    #region Poison
    public void SetPoison(int turn)
    {
        effectturn = turn;
        isdamageeffect = true;
    }
    #endregion

    #region Burn
    public void SetBurn(int damage, int turn)
    {
        effectturn = turn;
        effectamount = damage;
        canheal = false;    //이게 있어야 회복 불가능 추가 가능함
        effectname = "burn";
    }
    #endregion

    #region HealBlock
    public void SetHealBlock(int turn)
    {
        effectturn = turn;
        canheal = false ;
    }

    public bool GetHealBlock()
    {
        return canheal;
    }
    #endregion

    #region HealTurn
    public void SetHealTurn(int turn)
    {
        effectturn = turn;
        effectname = "healturn";
    }
    #endregion
    public void DecreaseEffectTurn()
    {
        effectturn--;
    }

    public int GetEffectTurn()
    {
        return effectturn;
    }

    public (bool, int) CheckDamageEffect()
    {
        switch(effectname)
        {
            case "poison":
                return (true, effectturn);    
            case "burn":
                return (true, effectamount);
            case "healturn":
                if (!canheal)   //회복 불가라면 0 회복
                    return (true, 0);
                return (true, -effectturn);   //힐이라서 대미지와 반대 
            default:
                return (false, 0);
        }
    }
}