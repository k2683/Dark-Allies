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
        WorldSaveGameManager worldsaveGameManager;
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
                saveFileWritter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if(saveFileWritter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                else
                {
                    
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWritter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileWritter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                else
                {

                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWritter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileWritter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                }
                else
                {

                    gameObject.SetActive(false);
                }
            }



        }
        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }
        public void SelectedCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
        public void DeleteSaveSlot()
        {
            WorldSaveGameManager.instance.DeleteGame(characterSlot);
        }
    }
}