using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMgr : MonoBehaviour
{
    public Image SandWatch = null;
    public Image Loading = null;
    public GameObject StartBG = null;

    float SandRotSpeed = 90.0f;
    float LoadTime = 5.0f;
    float LoadFillSpeed = 2.0f;
    float delta = 0.0f;
    float load = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if(delta > LoadTime)
            StartBG.SetActive(true);

        SandWatch.transform.Rotate(new Vector3(0.0f, 0.0f, -SandRotSpeed * Time.deltaTime));

        load += LoadFillSpeed * Time.deltaTime;
        if (load > 1.0f)
            load -= 1.0f;
        Loading.fillAmount = load;

        if (StartBG.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }

}
