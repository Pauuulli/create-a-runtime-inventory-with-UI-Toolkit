using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DecreaseHP : MonoBehaviour
{
    private float hp = 100;
    private float mp = 100;

    // Start is called before the first frame update
    void Start()
    {
        MainUIController.SetHP(hp);
        MainUIController.SetMP(hp);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            MainUIController.SetHP(hp);
            hp -= 0.5f;
        }
        else
            MainUIController.SetHP(0);
        if (mp > 0)
        {
            MainUIController.SetMP(mp);
            mp -= 0.5f;
        }
        else
            MainUIController.SetMP(0);
    }
}
