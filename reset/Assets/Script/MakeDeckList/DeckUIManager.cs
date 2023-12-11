using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    BinarySearchTree test;
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //����Ʈ�� �������� ��ü �����ϵ��� �ϴ� �뵵
    private List<Item> itemBuffer = new List<Item>();
    Queue<Item> testqueue = new Queue<Item>();
    public GameObject deckui;
    public RectTransform content;
    public GameObject cardnameuiprefab;
    Canvas deckuicanvas;
    GameObject cardlistpanel;
    CanvasRenderer panelcanvasrenderer;
    UIDeckButton uideckbutton;
    bool isopen = false;
    SaveData savedata;
    List<GameObject> instantiatedCards = new List<GameObject>();    //������ ī�� �������� ����Ʈ - ���߿� ��׸� ��Ƽ� �����Ϸ��� �뵵

    private void Awake()
    {
    }
    private void Start()
    {
        //savedata = GameObject.Find("SaveData").GetComponent<SaveData>();
        deckuicanvas = deckui.GetComponent<Canvas>();
        cardlistpanel = GameObject.Find("CardListPanel");
        test = new BinarySearchTree();
        deckui.SetActive(false);
    }

    public bool IsUIOpen()
    {
        return isopen;
    }
    public void OpenUI()//ī�� �ż� ���� �� ItemSO�� �����Ͽ� �� �����ϴ� ��
    {
        if (!isopen)    //���� �ִٸ�
        {
            deckui.SetActive(true);
            //itemBuffer = new List<Item>();
            for(int i = 0; i<itemSO.items.Length; i++)  //ItemSO���� ī�� ������ �ҷ���
            {
                Item item = itemSO.items[i];
                var cardObject = Instantiate(cardPrefab, cardlistpanel.transform);
                Transform newparent = GameObject.Find("CardListContent").GetComponent<Transform>();
                cardObject.transform.SetParent(newparent);
                RectTransform cardRectTransform = cardObject.GetComponent<RectTransform>(); //UI�� �־ RectTransform���
                instantiatedCards.Add(cardObject);
                var card = cardObject.GetComponent<UICardButton>();
                card.Setup(item);
            }


            isopen = true;

        }
        else    //�̹��� �����ϴ� �ڵ� ���� ��
        {
            isopen = false;
            for (int i = 0; i < itemSO.items.Length; i++)
            {
                foreach (var cardObject in instantiatedCards)
                {
                    Destroy(cardObject);
                }
                deckui.SetActive(false);
                instantiatedCards.Clear(); // ����Ʈ ����
            }
        }
    }


    public void MakeCardNameUI(Item item)
    {
        test.Insert(item);
        //itemBuffer = new List<Item>();
        testqueue.Clear();
        test.InorderTraversal(testqueue);
        Debug.Log(testqueue.Count);
        Debug.Log(test.Search(item));
        SortCard();

    }

    void SortCard()
    {
        Debug.Log(cardnamePrefabslist.Count);
        foreach (var prefab in cardnamePrefabslist)
        {
            Destroy(prefab);
        }
        cardnamePrefabslist.Clear();
        foreach (Item A in testqueue)      //���� ����
        {
            GameObject content = GameObject.Find("Content");
            var newcardname = Instantiate(cardnameuiprefab, content.transform);  //ī�� �̸� ������ ����
            uideckbutton = newcardname.GetComponent<UIDeckButton>();
            uideckbutton.Setup(A);
            cardnamePrefabslist.Add(newcardname);
            RectTransform rectTransform = newcardname.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void RemoveCard(Item item)
    {
        //itemBuffer = new List<Item>();

        test.ResetTree();
        Debug.Log("FinishDelete");
        testqueue.Clear();
        test.InorderTraversal(testqueue);
        Debug.Log(testqueue.Count);
        SortCard();
    }
    public void Test(Item itemname)
    {
        Debug.Log(test.Search(itemname));
    }
}
//���� ��������
#region binarySearchTree    
//����� ����Ʈ https://gamemakerslab.tistory.com/30
public class BinarySearchTree
{
    public class Node
    {
        public Item item;
        public Node Left;
        public Node Right;

        public Node(Item itemname)
        {
            item = itemname;
            Left = null;
            Right = null;
        }
    }

    public Node Root;

    public BinarySearchTree()
    {
        Root = null;
    }

    public void Insert(Item item)
    {
        Debug.Log(item.name);
        Root = InsertRecursively(Root, item);
    }

    private Node InsertRecursively(Node node, Item itemname)
    {
        if (node == null)
        {
            Debug.Log(itemname.name);
            node = new Node(itemname);
            return node;
        }

        if (itemname.identifier <= node.item.identifier)
        {
            node.Left = InsertRecursively(node.Left, itemname);
        }
        else if (itemname.identifier > node.item.identifier)
        {
            node.Right = InsertRecursively(node.Right, itemname);
        }

        return node;
    }

    public bool Search(Item itemname)
    {
        return SearchRecursively(Root, itemname) != null;
    }

    private Node SearchRecursively(Node node, Item itemname)
    {
        if (node == null || node.item.identifier == itemname.identifier)
        {
            return node;
        }

        if (itemname.identifier < node.item.identifier)
        {
            return SearchRecursively(node.Left, itemname);
        }

        return SearchRecursively(node.Right, itemname);
    }

    // ���� ����
    public void Delete(Item itemname)
    {
        Root = DeleteRecursively(Root, itemname);
    }

    private Node DeleteRecursively(Node node, Item itemname)
    {
        Debug.Log(node);
        if(node == null)
        {
            return node;
        }
        else if (itemname.identifier == node.item.identifier)
        {
            Debug.Log(node.item.name);
            node = DeleteNode(node);
            return node;
        }
        else if(itemname.identifier < node.item.identifier)
        {
            Debug.Log(node.item.name);
            node.Left = DeleteRecursively(node.Left, itemname);
            return node;
        }
        else
        {
            Debug.Log(node.item.name);
            node.Right = DeleteRecursively(node.Right, itemname);
            return node;
        }
    }

    Node DeleteNode(Node node)
    {
        if(node.Left == null && node.Right == null)     //��ã�� ���
        {
            return null;
        }
        else if(node.Left == null)  
        {
            return node.Right;  //���ڽĸ�
        }
        else if(node.Right == null)
        {
            return node.Left;   //�����ڽĸ�
        }
        else            //�� �ڽ� ��� ����
        {
            (int minItem, Node newRight) = DeleteMinItem(node.Right);
            node.item.identifier = minItem;
            node.Right = newRight;
            return node;
        }
    }

    (int, Node) DeleteMinItem(Node node)
    {
        if (node.Left == null)
            return (node.item.identifier, node.Right);
        else
        {
            (int minItem, Node newLeft) = DeleteMinItem(node.Left);
            node.Left = newLeft;
            return (minItem, node);
        }
    }

    public void ResetTree()
    {
        Root = null;
    }

    // ��ȸ ����
    public void InorderTraversal(Queue<Item> deck)
    {
        InorderTraversalRecursively(Root, deck);
        Debug.Log("finish line");
    }

    private void InorderTraversalRecursively(Node node, Queue<Item> deck)
    {
        if (node != null)
        {
            InorderTraversalRecursively(node.Left, deck);
            Debug.Log(node.item.name);
            deck.Enqueue(node.item);
            InorderTraversalRecursively(node.Right, deck);
        }
    }
}
#endregion