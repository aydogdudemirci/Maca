using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    public GameObject gameModes;
    public List<GameObject> typeOfMode = new List<GameObject>();
    public List<int> mode = new List<int>();
    public Color selected;
    public Color selectedShadow;
    public Color notSelected;
    public Color notSelectedShadow;

    private void Start()
    {
        foreach (Transform child in gameModes.transform)
        {
            typeOfMode.Add(child.gameObject);
        }

        for (int i = 0; i < typeOfMode.Count; i++)
        {
            mode.Add(0);
        }

        foreach (GameObject value in typeOfMode)
        {
            foreach (Transform child in value.transform)
            {
                if (mode[child.parent.GetSiblingIndex()] == child.GetSiblingIndex())
                {
                    child.GetChild(0).GetComponent<Image>().color = selected;
                    child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = selectedShadow;
                }

                else
                {
                    child.GetChild(0).GetComponent<Image>().color = notSelected;
                    child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = notSelectedShadow;
                }
            }
        }
    }

    public void ChangeColor(Button button)
    {
        mode[button.transform.parent.GetSiblingIndex()] = button.transform.GetSiblingIndex();

        foreach (Transform child in button.transform.parent)
        {
            if (mode[child.parent.GetSiblingIndex()] == child.GetSiblingIndex())
            {
                child.GetChild(0).gameObject.GetComponent<Image>().color = selected;
                child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = selectedShadow;
            }

            else
            {
                child.GetChild(0).gameObject.GetComponent<Image>().color = notSelected;
                child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = notSelectedShadow;
            }
        }
    }
}
