using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BL
{
    public class UI_Character_Delete_Slot : MonoBehaviour
    {
        SaveFileDataWritter saveFileWritter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;
        public GameObject deleteCharacterSlotPopup;
        public GameObject correspondingSaveSlot;
        public GameObject correspondingDeleteSlot;
        private void OnEnable()
        {
            LoadDeleteSlot();
        }
        private void LoadDeleteSlot()
        {
            saveFileWritter = new SaveFileDataWritter();
            saveFileWritter.saveDataDirectoryPath = Application.persistentDataPath;
            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWritter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileWritter.CheckToSeeIfFileExists())
                {
                    gameObject.SetActive(true);

                }
                else
                {

                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWritter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileWritter.CheckToSeeIfFileExists())
                {
                    gameObject.SetActive(true);

                }
                else
                {

                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWritter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileWritter.CheckToSeeIfFileExists())
                {
                    gameObject.SetActive(true);

                }
                else
                {

                    gameObject.SetActive(false);
                }
            }
        }
        public void AttempToDeleteCharacterSlot()
        {
            if (characterSlot != CharacterSlot.NO_SLOT)
            {
                TitleScreenManager.instance.currentSelectedSlot = characterSlot;
                deleteCharacterSlotPopup.SetActive(true);
                TitleScreenManager.instance.onSelectedDeletedDeletingButton = correspondingDeleteSlot;
                TitleScreenManager.instance.onSelectedDeletedSaveButton = correspondingSaveSlot;

            }
        }
    }
}