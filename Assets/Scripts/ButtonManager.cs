using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class ButtonManager : Singleton<ButtonManager>
    {
        private List<GameObject> typeOfSetting = new List<GameObject>();
        private List<int> selectedMode = new List<int>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (Transform child in GUIManager.Instance.gameModes.transform)
            {
                typeOfSetting.Add(child.gameObject);
            }

            for (int i = 0; i < typeOfSetting.Count; i++)
            {
                selectedMode.Add(0);
            }

            foreach (GameObject value in typeOfSetting)
            {
                foreach (Transform child in value.transform)
                {
                    if (selectedMode[child.parent.GetSiblingIndex()] == child.GetSiblingIndex())
                    {
                        child.GetChild(0).GetComponent<Image>().color = GUIManager.Instance.selectedLed;
                        child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = GUIManager.Instance.selectedLedShadow;
                    }

                    else
                    {
                        child.GetChild(0).GetComponent<Image>().color = GUIManager.Instance.notSelectedLed;
                        child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = GUIManager.Instance.notSelectedLedShadow;
                    }
                }
            }
        }

        public void changeColor(Button button)
        {
            selectedMode[button.transform.parent.GetSiblingIndex()] = button.transform.GetSiblingIndex();

            foreach (Transform child in button.transform.parent)
            {
                if (selectedMode[child.parent.GetSiblingIndex()] == child.GetSiblingIndex())
                {
                    child.GetChild(0).gameObject.GetComponent<Image>().color = GUIManager.Instance.selectedLed;
                    child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = GUIManager.Instance.selectedLedShadow;
                }

                else
                {
                    child.GetChild(0).gameObject.GetComponent<Image>().color = GUIManager.Instance.notSelectedLed;
                    child.GetChild(0).gameObject.GetComponent<Shadow>().effectColor = GUIManager.Instance.notSelectedLedShadow;
                }
            }
        }

        public string getSetting(string request)
        {
            GameObject settingType = GameObject.Find(request);

            return settingType.transform.GetChild(selectedMode[settingType.transform.GetSiblingIndex()]).name;
        }
    }
}
