using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostManager : MonoBehaviour
{
    [SerializeField] TMP_Text costTMP;  //��꿡 ���� ���̹Ƿ� num = int.Parse(costTMP); �صѰ�
    [SerializeField] TMP_Text rcostTMP;
    [SerializeField] TMP_Text gcostTMP;
    [SerializeField] TMP_Text bcostTMP;
    public static CostManager Inst { get; private set; }
    public GameObject playermaskprafab;
    public Transform playerposition;
    public GameObject player;

    void Awake() => Inst = this;
    private int mycost = 0;
    private int hasmycost;
    private bool can;
    int rcost = 10;
    int gcost = 10;
    int bcost = 10;

    public void ShowCost()
    {
        costTMP.text = hasmycost.ToString();
        rcostTMP.text = rcost.ToString();
        gcostTMP.text = gcost.ToString();
        bcostTMP.text = bcost.ToString();
    }

    public void CostSet()  //�ڽ�Ʈ �÷��ֱ�
    {
        if(mycost < 5)
            mycost++;

        hasmycost = mycost;
    }

    public void CostSetNewCost(int cost)    //�ڽ�Ʈ�� ���ϴ� �ڽ�Ʈ(int cost)�� ����
    {
        hasmycost = cost;
        ShowCost();
    }

    public bool CompareCost(Card card)  //�ڽ�Ʈ ��
    {
        if (hasmycost < card.item.cost)
        {
            can = false;
        }
        else
        {
            can = true;
        }

        return can;
    }

    public void SubtractCost(Card card)
    {
        hasmycost -= card.item.cost;
        if(card.item.color == 'R')
        {
            rcost++;
        }
        else if(card.item.color == 'G')
        {
            gcost++;
        }
        else if(card.item.color == 'B')
        {
            bcost++;
        }
    }
    public void GetMyStarMask(string name)
    {
        Entity playerentityscript = player.GetComponent<Entity>();
        if(playerentityscript.hasmask)
        {
            Debug.Log("You have mask");
        }
        else
        {
            switch (name)
            {
                case "sheep":
                    if (CompareRGB(name, rcost, gcost, bcost))
                    {
                        SpawnMask(name);
                        playerentityscript.MakeAttackUp(3, 9999);
                        playerentityscript.MakeShield(5, 3);
                        playerentityscript.MakeImmuneSleep(3);
                        playerentityscript.hasmask = true;
                    }
                    break;
                case "bull":
                    if (CompareRGB(name, rcost, gcost, bcost))
                    {
                        SpawnMask(name);
                    }
                    break;
                default:
                    Debug.Log("fail");
                    break;
            }
        }
    }
    private bool CompareRGB(string name, int r, int g, int b)   //RGB �ڽ�Ʈ �񱳿� �޼���
    {
        switch(name)    //�̸��� ���� ����ġ�� �ߵ� - ������ ��� �� �ӵ� ����� ���� if ��� ����ġ�� ���
        {
            case "sheep":
                if (r >= 5 && g >= 2 && b >= 3)
                {
                    rcost = rcost - 5;
                    gcost = gcost - 2;
                    bcost = bcost - 3;
                    ShowCost();
                    return true;
                }
                break;
            case "bull":
                return true;
        }
        return false;
    }

    private void SpawnMask(string name)
    {
        Vector3 spawnposition = new Vector3(playerposition.position.x + 0.25f, playerposition.position.y + 0.25f, playerposition.position.z);    //�÷��̾� ��ġ�� ���� 0.25f 0.25f�� �����ϱ� ����
        GameObject mask = Instantiate(playermaskprafab, spawnposition, Quaternion.identity);    //������ ���� �⺻ ���
        Mask mymask = playermaskprafab.GetComponent<Mask>();    //�����տ��� Mask��ũ��Ʈ�� �����ͼ� 
        mymask.ChangeStarMaskImage(name);                       //�̹����� �����ϱ� ����
        mask.transform.SetParent(playerposition);
    }
}
