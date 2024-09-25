using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        PlayerControls playerControls;
        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;
        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

    }
}