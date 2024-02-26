using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아이템 공장의 역할 : 아이템 오브젝트의 생성을 책임진다.
// 팩토리 패턴
// 객체 생성을 공장 클래스를 이용해 캡슐화 처리하여 대신 "생성"하게 하는 디자인 패턴
// 객체 생성에 필요한 과정을 템플릿화 해놓고 외부에서 쉽게 사용한다.
// 장점 :
// 1. 생성과 처리 로직을 분리하여 결합도를 낮출 수 있다. (결합도 : 참조를 통해 상호 의존성이 높아지는 정도)
// 2. 확장 및 유지보수가 편리하다.
// 3. 객체 생성 후 공통으로 할 일을 수행하도록 지정해줄 수 있다. 
// 단점 :
// 1. 상대적으로 조금 더 복잡하다.
// 2. 그래서 공부해야 한다.
// 3. 한마디로 단점이 없다.
public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance { get; private set; }

    // 아이템 프리팹들
    public List<GameObject> ItemPrefabs;
    /*public GameObject ItemHealthPrefabs;
    public GameObject ItemStaminaPrefabs;
    public GameObject ItemBulletPrefabs;*/ // 리스트로 만들어서 필요 없어짐

    public List<ItemObject> ItemPool;
    public int ItemPoolSize = 10;

    private void Awake()
    {
        Instance = this;

        // 오브젝트 풀 초기화
        ItemPool = new List<ItemObject>();
        // 지정된 풀 크기만큼 반복하며 아이템 오브젝트를 생성하고 비활성화 상태로 오브젝트 풀에 추가
        for (int i = 0; i < ItemPoolSize; i++)
        {
            foreach(GameObject prefab in ItemPrefabs) 
            {
                // 아이템 프리팹을 복제하여 아이템 오브젝트 생성
                GameObject item = Instantiate(prefab);

                // 아이템 오브젝트를 오브젝트 풀에 추가
                item.transform.SetParent(this.transform);
                ItemPool.Add(item.GetComponent<ItemObject>());

                // 아이템 오브젝트를 비활성화 상태로 설정
                item.SetActive(false);
            }
        }
    }
    
    // 확률 생성
    public void MakePercent(Vector3 position)
    {
        int randomItem = Random.Range(0, 100);
        Debug.Log(randomItem);

        if (randomItem <= 20)  // 0 1
        {
            Make(ItemType.Health, position);
        }
        else if (randomItem <= 40)  // 2 3
        {
            Make(ItemType.Stamina, position);
        }
        else if (randomItem <= 50) // 4
        {
            Make(ItemType.Bullet, position);
        }
    }

    private ItemObject Get(ItemType itemType)  // 창고 뒤지기 
    {
        foreach (ItemObject itemObject in ItemPool) 
        {
            if (itemObject.gameObject.activeSelf == false && itemObject.ItemType == itemType) 
            {
                return itemObject;
            }
        }
        return null;
    }
    // 기본 생성
    public void Make(ItemType itemType, Vector3 position) 
    {
        ItemObject itemObject = Get(itemType);

        if (itemObject != null)
        {
            itemObject.transform.position = position;
            itemObject.Init();
            itemObject.gameObject.SetActive(true);
        }
    }
}
