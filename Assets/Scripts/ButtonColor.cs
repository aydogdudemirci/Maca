using UnityEngine;
using UnityEngine.UI;

namespace Maca
{
    public class ButtonColor : Singleton<ButtonColor>
    {
        public GameObject goGameButton;

        public Color black;
        public Color start;
        public Color end;

        void Update ()
        {
            if ( gameObject.activeSelf )
            {
                goGameButton.GetComponent<Image> ().color = Color.Lerp ( black, end, Mathf.PingPong ( Time.time, 1 ) );
            }
        }
    }
}
