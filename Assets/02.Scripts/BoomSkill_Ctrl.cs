using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSkill_Ctrl : MonoBehaviour
{
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        a_Pos.z = 0.0f;

        this.transform.position = a_Pos;

    }
}
