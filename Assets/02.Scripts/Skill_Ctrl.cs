using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ctrl : MonoBehaviour
{
    public AnimationClip aniClip;
    Animation anim;
    float delta = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if(delta >= aniClip.length)
        {
            Destroy(gameObject);
        }
    }

}
