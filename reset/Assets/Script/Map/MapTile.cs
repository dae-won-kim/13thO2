using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    //�ð��� ���µ� ���� �̹����� �ӽ��� ��, ��� ����ν�� ��ġ�� ������ �����Ƿ� �밡�� ������ ������
    //���� ��ȹ�� ����ȴٸ� �ش� ��ũ��Ʈ�� ��ü������ ��� ���ĵ� ��� ����
    [SerializeField] Sprite[] Tileimg;  //Ÿ�� �̹��� �����
    [SerializeField] Sprite[] Stageimg; //�������� �̹��� �����
    [SerializeField] GameObject TileObject;     //Ÿ���� ������Ʈ
    [SerializeField] GameObject StageObject;    //���������� ������Ʈ
    SpriteRenderer mysprite;
    private void Start()
    {
        TileObject.SetActive(false) ;       //�귿 ������ ���� Ȱ��ȭ�� �ǹǷ�
    }

    public void Setup(char stage)
    {
        switch (stage)
            {
            case '0':   //���� ����
            case '1':
            case '2':
            case '3':
            case '4':
                SetSprite(0);
                break;
            case '5':   //���� ����
            case '6':
                SetSprite(1);
                break;
            case '7':   //�������� ����
                SetSprite(2);
                break;
        }
    }

    void SetSprite(int i)   //���� Ÿ��(����) �̹����� ����� �� �־ ����
    {
        mysprite = StageObject.GetComponent<SpriteRenderer>();
        mysprite.sprite = Stageimg[i];
    }
}
