using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiltySelect : MonoBehaviour
{
    private List<MiltySelectElement> elemts = new List<MiltySelectElement>();
    void Start()
    {
        bool found = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<MiltySelectElement>(out MiltySelectElement element))
            {
                elemts.Add(element);
                element.SetBoardConroller(this);
                if(element.tick.activeSelf && !found)
                {
                    element.on = true;
                }
                else
                {
                    element.on = false;
                    element.tick.SetActive(false);
                }
            }
        }
    }

    public void Toggled(int id)
    {
        foreach (MiltySelectElement element in elemts)
        {
            element.off(id);
        }
    }
}
