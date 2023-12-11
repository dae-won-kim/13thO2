using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICardButton : MonoBehaviour
{
    public Item item;
    public List<Item> items;
    [SerializeField] Image coloruiimage;
    [SerializeField] Image costuiimage;
    [SerializeField] Image characterui;
    [SerializeField] SpriteRenderer colorimg;
    [SerializeField] SpriteRenderer costcolor;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;  //��꿡 ���� ���̹Ƿ� num = int.Parse(costTMP); �صѰ�
    [SerializeField] TMP_Text acitveTMP;
    [SerializeField] Sprite cardFront;  //�������Ҽ���?
    [SerializeField] Sprite cardBack;   //22
    public string functionname;
    public string cardtype;
    public bool selectable;
    public int identifier;
    SaveData savedata;
    public DeckUIManager deckuimanager;
    private void Start()
    {
        GameObject save = GameObject.Find("SaveData");
        savedata = save.transform.GetComponent<SaveData>();
    }
    public void Setup(Item item)    //Card.cs�� �����ϰ� ������ �ڵ� -���: ī�� ����
    {
        this.item = item;
        colorimg.sprite = this.item.colorimg;
        costcolor.sprite = this.item.costcolor;
        //character.sprite = this.item.sprite;
        coloruiimage.sprite = this.item.colorimg;       //UI���� ���Ƿ��� Image������Ʈ�� �����ؾ���
        costuiimage.sprite = this.item.costcolor;       //SO������ SpriteRender�� �̹����� 2�� �ִ½����� ��
        //characterui.sprite = this.item.sprite;          //�ʹ� ��Ŵٺ��� ���� ��Ȳ�� �׽�Ʈ ���� ���߿� ü�� ���ƿ��� �׽�Ʈ �غ���
        nameTMP.text = this.item.name;
        //costTMP.text = this.item.cost.ToString();
        //acitveTMP.text = this.item.active;
        functionname = this.item.functionname;
        cardtype = this.item.cardtype;
        selectable = this.item.selectable;
        identifier = this.item.identifier;

        if (this.item.color == 'R')  //���� ���� �����ϸ� �۾� �� �ٲ�
        {
            nameTMP.color = new Color32(255, 88, 88, 255);
            //costTMP.color = new Color32(255, 88, 88, 255);
        }
        else if (this.item.color == 'G')
        {
            nameTMP.color = new Color32(88, 255, 88, 255);
            //costTMP.color = new Color32(88, 255, 88, 255);
        }
        if (this.item.color == 'B')
        {
            nameTMP.color = new Color32(88, 88, 255, 255);
            //costTMP.color = new Color32(88, 88, 255, 255);
        }
    }
    public void InputCard()
    {
        savedata.InputCardInDeck(item);
        deckuimanager.MakeCardNameUI(item);
        //Debug.Log(item.name);
    }
}
