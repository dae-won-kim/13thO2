using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFunctionManager : MonoBehaviour
{
    private Dictionary<string, Action> cardEffects = new Dictionary<string, Action>();
    public static CardFunctionManager Inst { get; private set; }
    //����Ǵ� ������ ����
    [SerializeField] TMP_Text costTMP;  //��꿡 ���� ���̹Ƿ� num = int.Parse(costTMP); �صѰ�
    [SerializeField] TMP_Text rcostTMP;
    [SerializeField] TMP_Text gcostTMP;
    [SerializeField] TMP_Text bcostTMP;
    [SerializeField] GameObject cardPrefab;  //�� �ڽ�Ʈ X ī�忡 ���� �ڽ�Ʈ O

    ItemSO itemSO;
    Card card;
    public CostManager costManager;
    public EntityManager entityManager;
    GameManager gameManager;
    CardManager cardManager;
    PlayerManager playerManager;

    GameObject findtarget;
    Entity target;
    GameObject[] findmonsters;
    GameObject findplayer;
    Entity player;
    Entity[] monsters;
    Entity targetentity;
    bool isRushUsed = false;

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // ī�� �̸��� ����� ��Ī�Ͽ� Dictionary�� ����
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // ���ٽ� ��� ����
        cardEffects["Encore"] = Encore;
        cardEffects["TestBuffAttackUp"] = TestBuffAttackUp;
        cardEffects["TestBuffAttackDown"] = TestBuffAttackDown;
        cardEffects["TestAttack"] = TestAttack;
        cardEffects["TestBuffShield"] = TestBuffShield;
        cardEffects["TestFaint"] = TestFaint;
        cardEffects["TestSleep"] = TestSleep;
        cardEffects["TestImmuneSleep"] = TestImmuneSleep;

        cardEffects["SharpNib"] = SharpNib;  // ��ī�ο� ����
        cardEffects["Firestick"] = Firestick;  // �Ҳ� ��ƽ
        cardEffects["Mousefire"] = Mousefire;  // ��ҳ���
        cardEffects["CtrlZ"] = CtrlZ; // �ǵ�����
        cardEffects["Gradation"] = Gradation; // �׶��̼�
    }
    //���� �� gameobj �������� �Լ�
    //public void GetEnemy(GameObject targetObj)
    //{
    //    target = targetObj;
    //    Debug.Log("Ȯ��");
    //}

    // ī�� �׼�,����,���⺰�� region�ٽ� ���� �� ��
    #region CardEffects

    private void Moon() //�ڽ�Ʈ ȸ���� �� ���� ������ ���� ��� ���� 
    {
        //��, TheMoon, �ڽ�Ʈ�� 1 ����ϴ�
        int cost = int.Parse(costTMP.text);
        cost++;
        costManager.CostSetNewCost(cost);
    }

    private void TestBuffAttackUp() //���� �׽�Ʈ�� ī��
    {
        FindPlayer();   //�÷��̾� ã��
        player.MakeAttackUp(5, 2);    //��ƼƼ���� ���� ����Ʈ ����
        Debug.Log("���� ����");
    }

    private void TestBuffAttackDown() //���� �׽�Ʈ�� ī��
    {
        FindPlayer();   //�÷��̾� ã��
        player.MakeAttackDown(3, 2);    //��ƼƼ���� ���� ����Ʈ ����
        Debug.Log("���� ����");
    }

    private void TestBuffShield()   
    {
        FindPlayer();
        player.MakeShield(10, 5);
    }

    private void TestAttack()   //������ ��󿡰� ���ظ� 5 �ݴϴ�
    {
        Attack("anything", 5, "normal");
    }

    private void TestFaint()    //�÷��̾� ���� 
    {
        Faint("anything", 3);
    }

    private void TestSleep()    //�÷��̾� ���� 
    {
        Sleep("anything", 3);
    }

    private void TestImmuneSleep()  //�÷��̾� ���� �鿪
    {
        ImmuneSleep("anything", 3);
    }


    private void ImsiCard1()    //ī�� ��ο쿡 ���� ������ �־� �ӽ÷� ����
    {
        Attack("anything", 7, "normal");
        TurnManager.OnAddCard.Invoke(true);
        TurnManager.OnAddCard.Invoke(true);
    }

    private void Encore()       //���� �����̶� �Ȱǵ帮�°� ���� �ǴܵǾ� ����
    {
        CardManager.Inst.SetIntrusionEncore();
    }

    private void SharpNib()  // ��ī�ο� ����
    {
        Attack("anything", 7, "piercing");
        ResetTarget();  //��� ���� ��� ���� �� Ÿ���� �����ؾ� ��� �������ص� ī�尡 ���Ǵ� ��Ȳ ������
    }


    private void Firestick()  // �Ҳ� ����
    {
        int randNum = UnityEngine.Random.Range(1, 3);

        for (int i = 0; i < randNum; i++)
        {
            Attack("anything", 2, "normal");
        }
        ResetTarget();  //��� ���� ��� ���� �� Ÿ���� �����ؾ� ��� �������ص� ī�尡 ���Ǵ� ��Ȳ ������
    }

    private void Mousefire()  // ��� ����
    {
        int randNum = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < randNum; i++)
        {
            Attack("all", 3, "normal");
        }
        ResetTarget();  //��� ���� ��� ���� �� Ÿ���� �����ؾ� ��� �������ص� ī�尡 ���Ǵ� ��Ȳ ������
    }

    private void Gradation()    //�׶��̼�
    {

        costManager.AddRGBCost('R');
        costManager.AddRGBCost('G');
        costManager.AddRGBCost('B');
    }

    private void CtrlZ()
    {
        // ������ �����ϱ� entity���� ���������ʿ��� ������ ����� �� ����ؼ� ���� ü�� �������ߵɵ�
    }
    #endregion

    #region IntrusionEffects
    private void IntrusionEncore(string functionName)
    {
        UseSelectCard(functionName);
    }

    #endregion


    // ī�� ���� �޼���
    public void UseSelectCard(string functionName)
    {
        if (cardEffects.TryGetValue(functionName, out Action cardEffect))
        {
            cardEffect();
        }
        else
        {
            Debug.Log("Card not found.");
        }
    }

    //�޼��� ��ƿ��Ƽ
    #region method
    //������ �ƴ� ��� �޼����� �ѱ� �Ű�������
    //Ÿ��, ��ġ, ���ӽð�(Ȥ�� Ƚ��), ��Ÿ ����� ����
    public void Attack(string targetcount, int damage, string type, string user = "player")   //��󿡰� ���ظ� n �ݴϴ�
    {                                                                  //��� ���ݷ� ���� ȿ�� �����ͼ� ����
        FindPlayer();      //�⺻���� ���                                             //targetcount(anything: ���� �ƹ���, enemy:��, player: �÷��̾�, all:��ü, enemyall: �� ��ü 
        if(user == "player")    //���Ϳ��Ե� ���ǹǷ� ������ �÷��̾�� ���ͳĿ� ���� �޶��� ��
        {
            damage += player.GetAllAttackUpEffect();                        //damage(���ط�)
            damage -= player.GetAllAttackDownEffect();                      //type (normal: ��� ����, piercing: ���� ����(��ȣ���� ������� ü�¿� ���������� ����))
        }
        switch (type)
        {
            case "normal":
                switch (targetcount)
                {
                    case "anything":
                        NormalDamage(target, damage);
                        target.SetHealthTMP();
                        target.SetShieldTMP();
                        break;
                    case "enemy":
                        break;
                    case "player":
                        NormalDamage(player, damage);
                        break;
                    case "all":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            NormalDamage(nowmonster, damage);
                        }
                        NormalDamage(player, damage);
                        break;
                    case "enemyall":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            NormalDamage(nowmonster, damage);
                        }
                        break;
                }
                break;
            case "piercing":
                switch (targetcount)
                {
                    case "anything":
                        target.health -= damage;
                        target.SetHealthTMP();
                        //target.GetComponents<Entity>();
                        break;
                    case "enemy":
                        break;
                    case "player":
                        player.health -= damage;
                        player.SetHealthTMP();
                        break;
                    case "all":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            nowmonster.health -= damage;
                            nowmonster.SetHealthTMP();
                        }
                        player.health -= damage;
                        player.SetHealthTMP();
                        break;
                    case "enemyall":
                        FindAllMonster();
                        foreach (Entity nowmonster in monsters)
                        {
                            nowmonster.health -= damage;
                            nowmonster.SetHealthTMP();
                        }
                        break;
                }
                break;
        }
    }

    public void Faint(string targetcount, int turn)
    {
        switch (targetcount)            
        {
            case "anything":
                target.MakeFaint(turn);
                break;
            case "enemy":
                break;
            case "player":
                break;
            case "all":
                break;
            case "enemyall":
                break;
        }
    }

    public void Sleep(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                target.MakeSleep(turn);
                break;
            case "enemy":
                break;
            case "player":
                break;
            case "all":
                break;
            case "enemyall":
                break;
        }
    }

    public void ImmuneSleep(string targetcount, int turn)
    {
        switch (targetcount)
        {
            case "anything":
                Debug.Log(target);
                target.MakeImmuneSleep(turn);
                break;
            case "enemy":
                break;
            case "player":
                FindPlayer();
                player.MakeImmuneSleep(turn);
                break;
            case "all":
                break;
            case "enemyall":
                break;
        }
    }
    #endregion

    #region methodUtils
    public void SetTarget(GameObject gameobject)    //ī��Ŵ����� ���� ã�� ���� ������Ʈ�� ��ƼƼ ����
    {
        findtarget = gameobject;
        target = findtarget.GetComponent<Entity>();
    }

    private void ResetTarget()
    {
        target = null;
    }

    private void FindAllMonster()   //Monster �±׸� ���� ��� ���� ������Ʈ ã�� ��ƼƼ ����
    {
        findmonsters = GameObject.FindGameObjectsWithTag("Monster");
        monsters = new Entity[findmonsters.Length];
        for(int i = 0; i < findmonsters.Length; i++)
        {
            monsters[i] = findmonsters[i].GetComponent<Entity>();
        }
    }

    private void FindPlayer()   //�÷��̾� �±׸� ���� ���� ������Ʈ ã�� ��ƼƼ ����
    {
        findplayer = GameObject.FindGameObjectWithTag("Player");
        player = findplayer.GetComponent<Entity>();
    }

    private void NormalDamage(Entity entity, int damage)
    {
        if (entity.shield >= damage)
        {
            entity.shield -= damage;
            entity.SetShieldTMP();
        }
        else
        {
            damage = damage - entity.shield;
            entity.health -= damage;
            entity.shield = 0;
            entity.SetHealthTMP();
            entity.SetShieldTMP();
        }
    }
    #endregion
}