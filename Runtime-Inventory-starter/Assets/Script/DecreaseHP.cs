using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DecreaseHP : MonoBehaviour
{
    private int hp = 100;
    private VisualElement m_Root;
    private VisualElement m_HPBar;

    private void Awake()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_HPBar = m_Root.Q<VisualElement>("HPBar");
        m_HPBar.Add(new HP());
    }
    // Start is called before the first frame update
    void Start()
    {
        //hpDisplay.setHP(50);
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(hp > 0) { MainUIController.hpValue.setHP(--hp); }
        
    }
}
