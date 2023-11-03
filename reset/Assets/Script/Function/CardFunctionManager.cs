using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFunctionManager : MonoBehaviour
{
    private Dictionary<string, Action> cardEffects = new Dictionary<string, Action>();

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

    GameObject target;
    GameObject[] monster;
    GameObject findplayer;
    Entity player;
    bool isRushUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        // ī�� �̸��� ����� ��Ī�Ͽ� Dictionary�� ����
        cardEffects["Counter"] = Counter;
        cardEffects["Vaccination"] = Vaccination;
        cardEffects["StageAccident"] = StageAccident;
        cardEffects["PenOfTruepenny"] = PenOfTruepenny;
        cardEffects["Sortie"] = Sortie;
        cardEffects["Boomerang"] = Boomerang;
        cardEffects["Rush"] = Rush;
        cardEffects["Moon"] = Moon;
        cardEffects["TheHangedMan"] = TheHangedMan;
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // ���ٽ� ��� ����
        cardEffects["Encore"] = Encore;
        cardEffects["TestBuffAttackUp"] = TestBuffAttackUp;
        cardEffects["TestBuffAttackDown"] = TestBuffAttackDown;
        cardEffects["TestAttack"] = TestAttack;
        cardEffects["TestBuffShield"] = TestBuffShield;
        cardEffects["TestSleep"] = TestSleep;
    }
    //���� �� gameobj �������� �Լ�
    public void GetEnemy(GameObject targetObj)
    {
        target = targetObj;
        Debug.Log("Ȯ��");
    }

    // ī�� �׼�,����,���⺰�� region�ٽ� ���� �� ��
    #region CardEffects

    private void Counter() //�ݰ�
    {
        CardManager.Inst.SetIntrusionCounter();
    }

    private void Vaccination() //���� �ֻ�
    {
        Debug.Log("���� �ֻ�!");
        TMP_Text playerHealthTMP = GameObject.Find("MyPlayer").GetComponentInChildren<TMP_Text>();
        if (playerHealthTMP != null)
        {
            int currentPlayerHealth = int.Parse(playerHealthTMP.text);
            currentPlayerHealth -= 2;
            playerHealthTMP.text = currentPlayerHealth.ToString();
        }
        else
            Debug.LogWarning("HealthTMP not found in the Player GameObject.");

        //���� ������ �鿪 �Լ� ��� ���� ��ũ��Ʈ �����


    }
    private void StageAccident() //������� ���� ���!!
    {
        Debug.Log("������� ���� ���!!");
        GameObject monsterObject = GameObject.FindGameObjectWithTag("Monster");
        TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
            int currentHealth = int.Parse(healthTMP.text);


            currentHealth = Mathf.RoundToInt(currentHealth / 2); //�������� ������ �ݿø�

            //���л��� �ο� �Լ� �� ��

            healthTMP.text = currentHealth.ToString();
        }
    }
    private void PenOfTruepenny() //������ ���� ���� �������� TrueDamage�� ���� ���� �Լ��� ���� �������� ���� ��������� ����
    {
        Debug.Log("������ ���� ����!!");
        GameObject monsterObject = target;
        TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
            int currentHealth = int.Parse(healthTMP.text);

            // -12�� �ϰ� �� ���� ���⿡ �������� �Լ� ��
            currentHealth -= 12;
            healthTMP.text = currentHealth.ToString();
        }
    }
    private void Sortie() //����
    {
        Debug.Log("����!!");
        GameObject monsterObject = target;
        TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
            int currentHealth = int.Parse(healthTMP.text);

            // -8�� �ϰ� �� ����
            currentHealth -= 5;
            healthTMP.text = currentHealth.ToString();
        }
        ResetTarget();
    }
    private void Rush() // �⼼����
    {
        Debug.Log("�⼼����!!");
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        GameObject targetMonsterObject = monsterObjects[0];
        foreach (GameObject monsterObject in monsterObjects)
        {
            if (int.Parse(targetMonsterObject.GetComponentInChildren<TMP_Text>().text) >
                int.Parse(monsterObject.GetComponentInChildren<TMP_Text>().text))
            {
                targetMonsterObject = monsterObject;
            }
        }

        TMP_Text healthTMP = targetMonsterObject.GetComponentInChildren<TMP_Text>();
        if (healthTMP != null)
        {
            // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
            int currentHealth = int.Parse(healthTMP.text);

            // -8�� �ϰ� �� ����
            currentHealth -= 8;
            healthTMP.text = currentHealth.ToString();

            if (isRushUsed)
            {
                isRushUsed = false;
            }
            else if (currentHealth < 0)
            {
                if (!isRushUsed)
                    Debug.Log("�ѹ���!");
                //Rush(); ���⼭ ���װ� �ɷ��� �ϴ� ����׷θ� �س���
            }
        }
        else
        {
            Debug.LogWarning("HealthTMP not found in the Monster GameObject.");
        }


    }
    private void Boomerang() //�θ޶�
    {
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");

        Debug.Log("�θ޶�!!");

        foreach (GameObject monsterObject in monsterObjects)
        {
            // �� Monster ���� ������Ʈ���� TMP_Text ������Ʈ�� ã���ϴ�.
            TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();

            if (healthTMP != null)
            {
                // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
                int currentHealth = int.Parse(healthTMP.text);

                // -8�� �ϰ� �� ����
                currentHealth -= 8;
                healthTMP.text = currentHealth.ToString();
            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Monster GameObject.");
            }
        }
        //�÷��̾� ü�� ȸ�� ����� ^^����^^ ����
        TMP_Text playerHealthTMP = GameObject.Find("MyPlayer").GetComponentInChildren<TMP_Text>();
        if (playerHealthTMP != null)
        {
            int currentPlayerHealth = int.Parse(playerHealthTMP.text);
            currentPlayerHealth += 4;
            playerHealthTMP.text = currentPlayerHealth.ToString();
        }
        else
            Debug.LogWarning("HealthTMP not found in the Player GameObject.");

    }
    // ���÷� ���� �޼����
    private void Moon()
    {
        //��, TheMoon, �ڽ�Ʈ�� 1 ����ϴ�
        int cost = int.Parse(costTMP.text);
        cost++;
        costManager.CostSetNewCost(cost);
    }

    private void TheHangedMan()
    {
        // Monster �±׸� ���� ��� ���� ������Ʈ�� ã���ϴ�.
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        
        foreach (GameObject monsterObject in monsterObjects)
        {
            // �� Monster ���� ������Ʈ���� TMP_Text ������Ʈ�� ã���ϴ�.
            TMP_Text healthTMP = monsterObject.GetComponentInChildren<TMP_Text>();

            if (healthTMP != null)
            {
                // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
                int currentHealth = int.Parse(healthTMP.text);

                // -5�� �ϰ� �� ����
                currentHealth -= 5;
                healthTMP.text = currentHealth.ToString();
            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Monster GameObject.");
            }
        }
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
        Attack("anything", 5, "1");
    }

    private void TestSleep()
    {
        FindPlayer();
        player.MakeSleep(2);
    }

    private void Encore()
    {
        CardManager.Inst.SetIntrusionEncore();
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
    
    public void Attack(string targetcount, int damage, string type)   //��󿡰� ���ظ� n �ݴϴ�
    {                                   //targetcount(anything: ���� �ƹ���, enemy:��, player: �÷��̾�, all:��ü, enemyall: �� ��ü 
                                        //damage(���ط�)
        switch (targetcount)            //type �������� ���� ����
        {
            case "anything":
                //������ ��� ����
                if(target == null)  //�ӽÿ� Ÿ���� ���� ���� CardManager���� �����ؾ���
                {
                    break;
                }
                Debug.Log(target);
                FindPlayer();
                damage += player.GetAllAttackUpEffect();    //��� ���ݷ� ���� ȿ�� �����ͼ� ����
                damage -= player.GetAllAttackDownEffect();
                Debug.Log(damage);
                TMP_Text healthTMP = target.GetComponentInChildren<TMP_Text>();
                Entity mytarget = target.GetComponent<Entity>();
                // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
                int currentHealth = int.Parse(healthTMP.text);
                currentHealth -= damage;
                mytarget.health -= damage;
                healthTMP.text = currentHealth.ToString();
                ResetTarget();
        //target.GetComponents<Entity>();
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
    #endregion

    #region methodUtils
    public void SetTarget(GameObject gameobject)
    {
        target = gameobject;
    }

    private void ResetTarget()
    {
        target = null;
    }

    private void FindAllMonster()   //Monster �±׸� ���� ��� ���� ������Ʈ ã��
    {
        monster = GameObject.FindGameObjectsWithTag("Monster");
    }

    private void FindPlayer()   //�÷��̾� �±׸� ���� ���� ������Ʈ ã��
    {
        findplayer = GameObject.FindGameObjectWithTag("Player");
        player = findplayer.GetComponent<Entity>();
    }
    #endregion
}