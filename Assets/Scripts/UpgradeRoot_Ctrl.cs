using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRoot_Ctrl : MonoBehaviour
{
    public Button Num_UpBtn = null;
    public Button B_S_UpBtn = null;
    public Button B_N_UpBtn = null;
    public Text Num_Lv_Text = null;
    public Text B_S_Lv_Text = null;
    public Text B_N_Lv_Text = null;
    public Text NumUpGold_Text = null;
    public Text B_S_UpGold_Text = null;
    public Text B_N_UpGold_Text = null;

    int NumUpGold = 100;
    int B_S_UpGold = 100;
    int B_N_UpGold = 100;
    public static int Num_Lv = 0;
    public static int B_S_Lv = 0;
    public static int B_N_Lv = 0;


    // Start is called before the first frame update
    void Start()
    {
        Num_Lv = 0;
        B_N_Lv = 0;
        B_S_Lv = 0;

        if (Num_UpBtn != null)
            Num_UpBtn.onClick.AddListener(() =>
            {
                BtnClickMethod(1, NumUpGold);
            });

        if (B_N_UpBtn != null)
            B_N_UpBtn.onClick.AddListener(() =>
            {
                BtnClickMethod(2, B_N_UpGold);
            });

        if (B_S_UpBtn != null)
            B_S_UpBtn.onClick.AddListener(() =>
            {
                BtnClickMethod(3, B_S_UpGold);
            });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BtnClickMethod(int idx, int Gold)
    {
        if (Game_Mgr.Inst.m_Gold < Gold)
        {
            Game_Mgr.Inst.HelpPanelSpawn("업그레이드에 필요한 골드가 부족합니다.");    //골드부족 판넬
            return;
        }

        if (idx == 1)
        {
            Num_Lv++;
            NumUpGold += 100;
        }
        else if (idx == 2)
        {
            B_N_Lv++;
            B_N_UpGold += 100;
        }
        else if(idx == 3)
        {
            B_S_Lv++;
            B_S_UpGold += 100;
        }

        //업그레이드 만큼 공격력 증가
        GameObject[] NumGo = GameObject.FindGameObjectsWithTag("Player");

        for (int ii = 0; ii < NumGo.Length; ii++)
        {
            Number_Ctrl NumCtrl = NumGo[ii].GetComponent<Number_Ctrl>();
            if (NumCtrl != null)
            {
                NumCtrl.UpgradeRefresh((NumType)idx);
            }
        }
        //업그레이드 만큼 공격력 증가

        Game_Mgr.Inst.AddGold(-Gold);   //골드차감
        RefreshText();  
    }

    void RefreshText()
    {
        if(Num_Lv_Text != null)
            Num_Lv_Text.text = "Lv " + Num_Lv.ToString();
        if(B_S_Lv_Text != null)
            B_S_Lv_Text.text = "Lv " + B_S_Lv.ToString();
        if (B_N_Lv_Text != null)
            B_N_Lv_Text.text = "Lv " + B_N_Lv.ToString();

        if(NumUpGold_Text !=null)
            NumUpGold_Text.text = NumUpGold.ToString();
        if(B_S_UpGold_Text != null)
            B_S_UpGold_Text.text = B_S_UpGold.ToString();
        if (B_N_UpGold_Text != null)
            B_N_UpGold_Text.text = B_N_UpGold.ToString();
    }
}
