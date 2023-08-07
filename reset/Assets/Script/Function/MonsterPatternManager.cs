using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class MonsterPatternManager : MonoBehaviour
{
    private Dictionary<string, Action> monsterPatterns = new Dictionary<string, Action>();

    [SerializeField] TMP_Text playerhealthTMP;
    MonsterSO monsterSO;
    Entity entity;
    EntityManager entityManager;
    int monsterhealthTMP;
    int monsterattackTMP;
    int monsterlivecount;

    // Start is called before the first frame update
    void Start()
    {
        // ī�� �̸��� ����� ��Ī�Ͽ� Dictionary�� ����
        //cardEffects["Moon"] = AddCost;
        //cardEffects["TheHangedMan"] = BoostMyBooster;
        //cardEffects["TheDevil"] = () => Combo(myscore); // ���ٽ� ��� ����

        monsterPatterns["Snail"] = () => SnailPattern();
    }

    private void SnailPattern()
    {
        // Player �±׸� ���� ��� ���� ������Ʈ�� ã���ϴ�.
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            // �� Player ���� ������Ʈ���� TMP_Text ������Ʈ�� ã���ϴ�.
            TMP_Text playerhealthTMP = playerObject.GetComponentInChildren<TMP_Text>();
            if (playerhealthTMP != null)
            {
                // ���� HealthTMP�� ���� �����ͼ� int�� ��ȯ
                int currentHealth = int.Parse(playerhealthTMP.text);

                if (monsterhealthTMP >= 5 && monsterlivecount > 3)
                {
                    int random = Random.Range(0, 10);
                    if (random < 5)
                    {
                        currentHealth -= 4;
                    }
                    else
                    {
                        currentHealth -= 8;
                    }
                }
                else if(monsterhealthTMP >= 5)
                {
                    // -5�� �ϰ� �� ����
                    currentHealth -= 5;
                }
                else if(monsterhealthTMP < 5)
                {
                    Debug.Log("�͵�");
                }

                playerhealthTMP.text = currentHealth.ToString();

            }
            else
            {
                Debug.LogWarning("HealthTMP not found in the Player GameObject.");
            }
        }
    }

    // ī�� ���� �޼���
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

    public void GetThisValue(Entity selectEntity)   //���õ� ��ƼƼ���� ���� �������� ���
    {
        monsterhealthTMP = selectEntity.GetHealthTMP();
        monsterattackTMP = selectEntity.GetAttackTMP();
        monsterlivecount = selectEntity.GetLiveCount();
    }
}
