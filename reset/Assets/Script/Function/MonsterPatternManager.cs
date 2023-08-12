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

       // monsterPatterns["Snail"] = () => SnailPattern();
    }
}
