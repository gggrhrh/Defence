using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool_Mgr : MonoBehaviour
{
    public GameObject m_BulletPrefab;
    [HideInInspector] public List<Bullet_Ctrl> m_BulletPool = new List<Bullet_Ctrl>();

    //--- 싱글턴 패턴
    public static BulletPool_Mgr Inst;

    private void Awake()
    {
        Inst = this;
    }
    //--- 싱글턴 패턴

    // Start is called before the first frame update
    void Start()
    {
        //총알을 생성해 오브젝트 풀에 저장
        for(int i = 0; i < 50; i++)
        {
            GameObject a_Bullet = Instantiate(m_BulletPrefab) as GameObject;
            a_Bullet.transform.SetParent(transform, false);
            a_Bullet.SetActive(false);
            m_BulletPool.Add(a_Bullet.GetComponent<Bullet_Ctrl>());
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public Bullet_Ctrl GetBulletPool()
    {   
        //리스트 돌며 비활성화 여부 판단
        foreach (Bullet_Ctrl a_BNode in m_BulletPool)
        {
            if (a_BNode.gameObject.activeSelf == false)
            {
                return a_BNode;
            }
        }

        GameObject a_Bullet = Instantiate(m_BulletPrefab) as GameObject;    
        a_Bullet.transform.SetParent(transform, false);
        a_Bullet.SetActive(false);
        Bullet_Ctrl a_BCtrl = a_Bullet.GetComponent<Bullet_Ctrl>();
        m_BulletPool.Add(a_BCtrl);

        return a_BCtrl;
    }
}
