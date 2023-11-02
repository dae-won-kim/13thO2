using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Linq;

public class CardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static CardManager Inst { get; private set;} //�̱���


    private void Awake()
    {
        Inst = this; // �̱��� �ν��Ͻ� ����
        cardfuction = GetComponent<CardFunctionManager>(); // CardFunction ������Ʈ ��������
        //cardfuction.SetCardManager(this); // CardFunctionManager Ŭ������ CardManager �ν��Ͻ� ����
    }

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] Transform cardSpawnPoint;
    //[SerializeField] Transform otherCardSpqwnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] ECardState eCardState;
    //[SerializeField] Item item;   //�� �־�������
    List<Item> itemBuffer;
    Card selectCard;    //���õ� ī�� ����
    CardFunctionManager cardfuction;
    public CostManager costManager;
    bool isMyCardDrag;
    bool onMyCardArea;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }
    int myPutCount; //��ƼƼ ����
    bool intrusionencore = false;
    bool intrusioncounter = false;

    private List<string> intrusionList = new List<string>();

    public Item PopItem()   //�Ǿ��� ī�� ���� �뵵
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }
    void SetupItemBuffer()  //ī�� ���� �������� �ٲ�� �ϴ� �뵵
    {
        itemBuffer = new List<Item>();
        for(int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            for (int j = 0; j < item.percent; j++)
                itemBuffer.Add(item);
        }    

        for (int i = 0; i < itemBuffer.Count; i++ )
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }


    void Start()
    {
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
    }

    void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;    
    }

    void Update()
    {
        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
        SetECardState();
    }
    
    void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        if(isMine)
            myCards.Add(card);
            
        SetOriginOrder(isMine);
        CardAlignment();
    }

    void SetOriginOrder(bool isMine)
    {
        int count = myCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            targetCard.GetComponent<Order>().SetOriginOrder(i);
        }
    }

   void CardAlignment()  // ī�� ����
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 0.05f);  //�� �� ������ ���� �� ī�� ������ �����
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);

        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if(objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }

    public bool TryPutCard(bool isMine)    //��ƼƼ ����
    {
        Card card = isMine ? selectCard : selectCard;
        var targetCards = isMine ? myCards : myCards;

        targetCards.Remove(card);
        card.transform.DOKill();
        DestroyImmediate(card.gameObject);
        if(isMine)
        {
            selectCard = null;
            myPutCount++;
        }
        CardAlignment();
        return true;
    }

    public void UseCard()
    {
        if (cardfuction != null)
        {
            GetSelectCardType(selectCard.cardtype, selectCard.functionname);
            cardfuction.UseSelectCard(selectCard.functionname);
        }
    }

    #region MyCard
    public void CardMouseOver(Card card)    //ī�� ���� ���콺�� �÷� ���� ��(��� ���� X) 
    {
        if (eCardState == ECardState.Nothing)
            return;

        selectCard = card;
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)    //ī�� ���� ���콺�� �� ��(��� ���� X) 
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown() //ī�� ��� �� ���콺 ���� ��
    {
        if (eCardState != ECardState.CanMouseDrag)
        {
            return;    
        }
        if(selectCard.cardtype =="Intrusion")   //Ŭ�� �� ���� ���� ����, �ߺ� Ȯ�� �뵵
        {
            if (IsFullList())
            {
                GameManager.Inst.Notification("������ �ִ� 5����.");
                return;
            }
            if (IsIntrusionDuplication(selectCard.functionname))
            {
                GameManager.Inst.Notification("�ߺ��� ������ ������� �ʴ´�.");
                return;
            }
                
        }
        if (costManager.CompareCost(selectCard))    //�ڽ�Ʈ ��
        {   
            isMyCardDrag = true;
            if(onMyCardArea)
            {
                return;
            }

        }
        GameManager.Inst.Notification("�ڽ�Ʈ�� �����ϴ�");
    }

    public void CardMouseUp()   //���콺�� �� �� ī�� ���
    {
        isMyCardDrag = false;

        if (eCardState != ECardState.CanMouseDrag)
            return;
        if (selectCard.cardtype == "Intrusion")
        {
            if (IsFullList())
            {
                return;
            }
            if(IsIntrusionDuplication(selectCard.functionname))
            {
                return;
            }

        }

        if (costManager.CompareCost(selectCard))
        {
            if (onMyCardArea)
            {
            }
            else
            {
                bool isObjectin = false;
                GameObject[] monster = GameObject.FindGameObjectsWithTag("Monster");
                GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                GameObject[] entity = monster.Concat(player).ToArray();
                if(selectCard.selectable)   //���� �������� ����
                {
                    foreach(GameObject obj in entity)
                    {
                        if (IsMouseCollidingWithObject(obj))
                        {
                            cardfuction.SetTarget(obj);
                            isObjectin = true;
                            break;
                        }
                    }
                }
                else
                {
                    //������ ��ü�����̹Ƿ�
                }
                {
                    isObjectin = true;
                }
                if(isObjectin)
                {
                    CostManager.Inst.SubtractCost(selectCard);
                    CostManager.Inst.ShowCost();
                    UseCard();
                    IntrusionConditionCheck();
                    EntityManager.Inst.FindDieEntity();
                    TryPutCard(true);
                    EntityManager.Inst.CheckBuffDebuff();
                }

            }
        }
    }

    void CardDrag() //ī�� �巡�� ���� ��
    {
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
        else if(!onMyCardArea)     //���� �ؾ��ϴ� ���̽��� ���
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            //Debug.Log("select");
        }
        
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool isEnlarge, Card card) //ī�� Ȯ��
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -3.0f, -0.1f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 0.1f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetECardState()
    {
        if (TurnManager.Inst.isLoading)
            eCardState = ECardState.Nothing;

        else if (!TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseOver;

        else if (TurnManager.Inst.myTurn)
            eCardState = ECardState.CanMouseDrag;     
    }

    #endregion
    //����
    #region Intrusion 
    //���ڸ�
    #region Encore
    public void SetIntrusionEncore()
    {
        intrusionencore = true;
    }
    public bool ConditionIntrusionEncore()  //���ڸ� ���� Ȯ��
    {
        if (selectCard.cardtype == "Action" && EntityManager.Inst.IsDieEntity())
            return true;
        return false;
    }
    public void UseIntrusionEncore()    //���ڸ� �ɷ� �ߵ�
    {
        cardfuction.UseSelectCard(selectCard.functionname);
        intrusionList.Remove("Encore"); //����Ʈ �����
    }
    #endregion
    //�ݰ�
    #region Counter
    public void SetIntrusionCounter()
    {
        intrusioncounter = true;
    }
    public bool ConditionIntrusionCounter()  //�ݰ� ���� Ȯ��
    {
        if (selectCard.cardtype == "Action" && EntityManager.Inst.IsDieEntity())
            return true;
        return false;
    }

    public void UseIntrusionCounter()    //�ݰ� �ɷ� �ߵ�
    {
        cardfuction.UseSelectCard(selectCard.functionname);
        intrusionList.Remove("Counter"); //����Ʈ �����
    }

    #endregion

    public void GetSelectCardType(string type, string functionName)
    {
        if (type == "Intrusion")
        {
            intrusionList.Add(functionName);
        }
    }

    public bool IsIntrusionDuplication(string function)
    {
        foreach(string key in intrusionList)
        {
            if(key == function)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFullList()    //��ġ�� ���� ī���� ���� ����
    {
        if (intrusionList.Count >= 5)
        {
            return true;
        }
        return false;
    }

    public void IntrusionConditionCheck()   //���� Ȯ�ο�
    {
        if(intrusionencore == true && ConditionIntrusionEncore() == true)   //���ڸ� ���� Ȯ��
        {
            UseIntrusionEncore();
        }
    }
    #endregion

    bool IsMouseCollidingWithObject(GameObject obj)
    {
        // ���ϴ� ���̾� ����ũ ���� (��: "Entity" ���̾ ���)
        int layerMask = LayerMask.GetMask("Entity");

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, layerMask);

        if (hitCollider != null && hitCollider.gameObject == obj)
        {
            // ���콺�� obj�� �浹�� ���
            return true;
        }

        return false; // �浹���� ���� ���
    }
}
