using UnityEngine;

namespace Maca
{
    public class KeyListener : Singleton<KeyListener>
    {
        private void Awake()
        {
            instance = this;
        }

        void OnGUI()
        {
            Event e = Event.current;

            if (e.isKey)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //Example usage
                    Debug.Log("pressed right arrow");
                }
            }
                
        }
    }
}