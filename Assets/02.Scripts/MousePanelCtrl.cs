using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MousePanelCtrl : MonoBehaviour
{
    Vector3 MousePos = Vector3.zero;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);

        if (MousePos.x < 0.0f)
            MousePos.x = 0.0f;
        if (MousePos.x > Screen.width)
            MousePos.x = Screen.width;
        if (MousePos.y < 0.0f)
            MousePos.y = 0.0f;
        if (MousePos.y > Screen.height)
            MousePos.y = Screen.height;

        transform.position = new Vector3(MousePos.x + 12.0f, MousePos.y - 12.0f, 0.0f);
    }

}
