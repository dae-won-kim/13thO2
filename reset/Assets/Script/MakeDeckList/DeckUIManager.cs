using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    BinarySearchTree test = new BinarySearchTree();
    List<GameObject> cardnamePrefabslist = new List<GameObject>();  //����Ʈ�� �������� ��ü �����ϵ��� �ϴ� �뵵
    private List<Item> itemBuffer;
    public GameObject deckui;
    public RectTransform content;
    public GameObject cardnameuiprefab;
    Canvas deckuicanvas;
    GameObject panel;
    CanvasRenderer panelcanvasrenderer;
    UIDeckButton uideckbutton;
    bool isopen = true;
    List<GameObject> instantiatedCards = new List<GameObject>();    //������ ī�� �������� ����Ʈ - ���߿� ��׸� ��Ƽ� �����Ϸ��� �뵵

    private void Start()
    {
        deckuicanvas = deckui.GetComponent<Canvas>();
        panel = GameObject.Find("Panel");
        deckui.SetActive(false);
    }
    public void OpenUI()//ī�� �ż� ���� �� ItemSO�� �����Ͽ� �� �����ϴ� ��
    {
        if (isopen)
        {
            deckui.SetActive(true);
            itemBuffer = new List<Item>();
            for(int i = 0; i<itemSO.items.Length; i++)  //ItemSO���� ī�� ������ �ҷ���
            {
                Item item = itemSO.items[i];
                var cardObject = Instantiate(cardPrefab, panel.transform);
                RectTransform cardRectTransform = cardObject.GetComponent<RectTransform>(); //UI�� �־ RectTransform���
                instantiatedCards.Add(cardObject);
                var card = cardObject.GetComponent<UICardButton>();
                card.Setup(item);
                //Debug.Log(item.name);
                switch (i%8)    //ī�� ��ǥ
                {
                    case 0:
                        cardRectTransform.anchoredPosition = new Vector2(-450f, 250);
                        break;
                    case 1:
                        cardRectTransform.anchoredPosition = new Vector2(-150f, 250);
                        break;
                    case 2:
                        cardRectTransform.anchoredPosition = new Vector2(150f, 250);
                        break;
                    case 3:
                        cardRectTransform.anchoredPosition = new Vector2(450f, 250);
                        break;
                    case 4:
                        cardRectTransform.anchoredPosition = new Vector2(-450f, -250);
                        break;
                    case 5:
                        cardRectTransform.anchoredPosition = new Vector2(-150f, -250);
                        break;
                    case 6:
                        cardRectTransform.anchoredPosition = new Vector2(150f, -250);
                        break;
                    case 7:
                        cardRectTransform.anchoredPosition = new Vector2(450f, -250);
                        break;
                }
                if(i > 8)
                {
                    break;
                }
            }


            isopen = false;

        }
        else    //�̹��� �����ϴ� �ڵ� ���� ��
        {
            isopen = true;
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
        itemBuffer = new List<Item>();
        test.InOrderTraversal(itemBuffer);
        Debug.Log(itemBuffer.Count);
        foreach (var prefab in cardnamePrefabslist)
        {
            Destroy(prefab);
        }
        cardnamePrefabslist.Clear();
        foreach (Item A in itemBuffer)
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
}

#region binarySearchTree
//����� ����Ʈ https://gamemakerslab.tistory.com/30
public class BinarySearchTree
{
    public class Node
    {
        public Item item;
        public int Value;
        public Node Left;
        public Node Right;

        public Node(Item name)
        {
            item = name;
            Value = name.identifier;
            Left = null;
            Right = null;
        }
    }

    public Node Root;

    public BinarySearchTree()
    {
        Root = null;
    }

    public void Insert(Item name)
    {
        Root = InsertRecursively(Root, name);
    }

    private Node InsertRecursively(Node node, Item name)
    {
        if (node == null)
        {
            node = new Node(name);
            return node;
        }

        if (name.identifier <= node.Value)
        {
            node.Left = InsertRecursively(node.Left, name);
        }
        else if (name.identifier > node.Value)
        {
            node.Right = InsertRecursively(node.Right, name);
        }

        return node;
    }

    public void InOrderTraversal(List<Item> item)
    {
        InOrderTraversalRecursively(Root, item);
    }

    private void InOrderTraversalRecursively(Node node, List<Item> item)
    {
        if (node != null)
        {
            // ���� ���� Ʈ�� ��ȸ
            InOrderTraversalRecursively(node.Left, item);

            // ���� ��� ó��
            item.Add(OutputItem(node.item));

            // ������ ���� Ʈ�� ��ȸ
            InOrderTraversalRecursively(node.Right, item);
        }
    }

    public Item OutputItem(Item item)
    {
        return item;
    }

    public int CountNodes()
    {
        return CountNodesRecursively(Root);
    }

    private int CountNodesRecursively(Node node)
    {
        if (node == null)
        {
            return 0;
        }

        // ���� ��带 �����Ͽ� ���� ���� Ʈ���� ������ ���� Ʈ���� ��� ������ �ջ�
        return 1 + CountNodesRecursively(node.Left) + CountNodesRecursively(node.Right);
    }
}
#endregion