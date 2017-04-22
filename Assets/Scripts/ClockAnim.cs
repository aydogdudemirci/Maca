using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class ClockAnim : Singleton<ClockAnim>
    {
        public RuntimeAnimatorController bubbleOpen;
        public RuntimeAnimatorController bubbleClose;

        public List<GameObject> disablingObjects;

        public GameObject loadSquare;
        public GameObject vane;
        public GameObject TimeBar;

        public Color start;
        public Color end;

        public Slider timeout;
        public Text second;

        private void Update ()
        {
            loadSquare.GetComponent<Image> ().fillAmount = timeout.value * 0.05f;
            second.text = ( ( int ) timeout.value ).ToString ();
        }

        public void timeoutScreen ()
        {
            foreach ( GameObject obj in disablingObjects )
            {
                obj.SetActive ( false );
            }

            TimeBar.GetComponent<Animator> ().runtimeAnimatorController = bubbleOpen;
            TimeBar.SetActive ( true );
            TimeBar.GetComponent<Animator> ().SetBool ( "SlideOut", true );
        }

        public void timeoutScreenOff ()
        {
            foreach ( GameObject obj in disablingObjects )
            {
                obj.SetActive ( true );
            }

            TimeBar.GetComponent<Animator> ().runtimeAnimatorController = bubbleClose;
            TimeBar.SetActive ( true );
            TimeBar.GetComponent<Animator> ().SetBool ( "SlideOut", true );
        }
    }
}

