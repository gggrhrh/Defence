using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject MonsterPrefab;

    int m_MonMax = 20;  //한라운드당 20마리
    int m_BossMax = 1;  //라운드에 1마리
    float delta = 0.0f;
    float Dur = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnMonster();
    }

    void SpawnMonster()
    {
        if (Game_Mgr.Inst.m_GameRound == GameRound.ReadyRound ||
            Game_Mgr.Inst.m_GameRound == GameRound.BossRound)
            return;

        delta -= Time.deltaTime * Game_Mgr.Inst.m_GameSpeed;
        if (delta <= 0.0f)
        {
            Vector3 a_Pos = new Vector3(1.75f, 4.0f, 0.0f);
            GameObject Go = Instantiate(MonsterPrefab) as GameObject;
            Go.transform.position = a_Pos;
            delta = Dur;

            Game_Mgr.Inst.m_MonCount++;
        }
    }

    void BossSpawn()
    {
        if (Game_Mgr.Inst.m_GameRound == GameRound.ReadyRound ||
            Game_Mgr.Inst.m_GameRound == GameRound.MonsterRound)
            return;
    }
}
