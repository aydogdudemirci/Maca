using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maca
{
    public class IntroScreen : MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        public GameObject hidingObjects;
        public GameObject timeoutObject;
        public Image fillingBar;
        public Text timeoutValue;
        public Text barText;

        public Slider timeout;

        public RuntimeAnimatorController opening;
        public RuntimeAnimatorController closing;

        public void OnBeginDrag ( PointerEventData data )
        {
            StartCoroutine ( start () );
        }

        public void OnValueChanged()
        {
            float value = timeout.value < 5 ? 5 : timeout.value;
            timeout.value = value;

            fillingBar.fillAmount = value * 0.05f;
            timeoutValue.text = ( ( int ) value ).ToString ();
            barText.text = ( ( int ) value ).ToString ();
        }

        public void OnEndDrag ( PointerEventData data )
        {
            StartCoroutine (end());
        }

        IEnumerator end()
        {
            timeoutObject.GetComponent<Animator> ().runtimeAnimatorController = closing;
            timeoutObject.SetActive ( true );
            timeoutObject.GetComponent<Animator> ().SetBool ( "SlideOut", true );
            yield return new WaitForSeconds (0.25f);
            hidingObjects.SetActive ( true );
            timeoutObject.SetActive ( false );
        }

        IEnumerator start ()
        {
            hidingObjects.SetActive ( false );

            timeoutObject.GetComponent<Animator> ().runtimeAnimatorController = opening;
            timeoutObject.SetActive ( true );
            timeoutObject.GetComponent<Animator> ().SetBool ( "SlideOut", true );
            yield return new WaitForSeconds ( 0.25f );
        }

    }
}