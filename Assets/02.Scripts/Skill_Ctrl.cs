using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        a_Pos.z = 0.0f;

        if (a_Pos.x <= -9.0f)
            a_Pos.x = -9.0f;
        if (a_Pos.x >= 9.0f)
            a_Pos.x = 9.0f;
        if (a_Pos.y <= -5.0f)
            a_Pos.y = -5.0f;
        if (a_Pos.y >= 5.0f)
            a_Pos.y = 5.0f;

        this.transform.position = a_Pos;
    }

    public void UseSkill()
    {
        anim.Play();
    }
}
