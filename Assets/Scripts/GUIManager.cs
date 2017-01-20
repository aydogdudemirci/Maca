﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Maca
{
    public class GUIManager : Singleton<GUIManager>
    {
        public GameObject Intro;                     
        public GameObject Selections;                       
        public GameObject Settings;
        public GameObject InGame;

        public void GoSelectionScreen()
        {
            Intro.SetActive(false);
            Selections.SetActive(true);
            //Settings.SetActive(false);
            //InGame.SetActive(false);
        }
    }
}