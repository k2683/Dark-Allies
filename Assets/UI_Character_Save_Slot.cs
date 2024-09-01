using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace BL
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWritter saveFileWritter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;
        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timeplayed;
        private void OnEnable()
        {
            LoadSaveSlot();
        }
        private void LoadSaveSlot()
        {
            saveFileWritter = new SaveFileDataWritter();
            saveFileWritter.saveDataDirectoryPath = Application.persistentDataPath;
            if(characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWritter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if(saveFileWritter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
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
                    characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
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
                    characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                }
                else
                {

                    gameObject.SetActive(false);
                }
            }



        }
        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }
        public void SelectedCurrentSlot()
        {
            TitleScreenManager.instance.SelectedCharacterSlot(characterSlot);
        }
    }
}