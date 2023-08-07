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
    CardManager cardManager;
    PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        // ī�� �̸��� ����� ��Ī�Ͽ� Dictionary�� ����
        cardEffects["Moon"] = Moon;
        cardEffects["TheHangedMan"] = TheHangedMan;
        //cardEffects["TheDevil"] = () => TheDevil (myscore); // ���ٽ� ��� ����
        cardEffects["Encore"] = Encore;
    }

    #region CardEffects
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
}