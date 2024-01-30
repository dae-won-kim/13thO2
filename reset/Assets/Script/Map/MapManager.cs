using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // ��� �̱������� �����ϰ� �Ǵ°� ������ �̷��� �ǳ�?
    private static MapManager Inst = null;
    void Awake()
    {
        if (null == Inst)
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    [SerializeField] GameObject[] Tile; //���� ����� Tile �������� ���� ��, ��� �� �������� �밡�� �������� ����ǹǷ� �̷��� ��
    MapTile maptile;
    string tilecount;  //Ÿ�ϸ� ��
    void Start()
    {
        for(int i = 0; i < Tile.Length; i++)    //�� �����ϰ� ��ġ�ϴ� �뵵
        {
            tilecount+=Random.Range(0, 8);   //���� ���� : ����:����:�������� = 5:2:1 ����, Ȯ�� �����ؼ��� MapeTile���� �����Ұ�
        }                                   //���� �� ������ ������ ���� ���ӵǴ� ���� ������ �� ����� ���ɼ� ���Ƽ�
        for (int i = 0; i < Tile.Length; i++)
        {
            maptile = Tile[i].GetComponent<MapTile>();
            maptile.Setup(tilecount[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
