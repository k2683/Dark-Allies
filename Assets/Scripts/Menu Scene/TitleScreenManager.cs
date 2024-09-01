using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace BL
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;
        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [Header("Buttons")]
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;
        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkayButton;
        public GameObject deleteCharacterSlotPopup;
        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;
        
        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.AttempToCreateNewGame();
        }
        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
        }
        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);
            mainMenuLoadGameButton.Select();
        }
        public void DisplayNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopUp.SetActive(true);
        }
        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);

        }
        public void SelectedCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
        public void AttempToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopup.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();

            }
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopup.SetActive(false);
            loadMenuReturnButton.Select();
        }
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopup.SetActive(false);
            loadMenuReturnButton.Select();
            WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);
        }
        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;

        }
    }
}