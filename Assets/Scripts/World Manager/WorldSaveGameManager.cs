using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BL
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;
        public PlayerManager player;
        [Header("Save/Load")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;
        [Header("World Scene Index")] 
        [SerializeField] int worldSceneIndex = 1;
        [Header("Save Data Writer")]
        private SaveFileDataWritter saveFileDataWritter;
        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;
        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }
        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }
        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch(characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                default:
                    break;
            }

            return fileName;
        }
        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

            yield return null;
        }
        public int GetWorldSceneIndex()
            { return worldSceneIndex; }
        public void AttempToCreateNewGame()
        {
            Debug.Log("AttempToCreateNewGame");
            saveFileDataWritter = new SaveFileDataWritter();
            saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
            //Check if we can create a new file
            saveFileDataWritter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!saveFileDataWritter.CheckToSeeIfFileExists())
            {

                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();

                return;
            }
            saveFileDataWritter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02); 

            if (!saveFileDataWritter.CheckToSeeIfFileExists())
            {

                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWritter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            if (!saveFileDataWritter.CheckToSeeIfFileExists())
            {

                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            //use save slot 1 for now if we have no slot
            else
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;

            }
            //TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopup();
        }
        public void LoadGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            saveFileDataWritter = new SaveFileDataWritter();
            //Generally works on multiple machine types (Application.persistentDataPath)
            saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWritter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWritter.LoadSaveFile();
            StartCoroutine(LoadWorldScene());
        }
        public void SaveGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            saveFileDataWritter = new SaveFileDataWritter();
            saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWritter.saveFileName = saveFileName;

            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            if(!player.playerNetworkManager.IsServer)
            {
                Debug.Log("client");
                return; 
            }
            saveFileDataWritter.CreateNewCharacterSaveFile(currentCharacterData);
        }
        public void DeleteGame(CharacterSlot characterSlot)
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            saveFileDataWritter = new SaveFileDataWritter();
            //Generally works on multiple machine types (Application.persistentDataPath)
            saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWritter.saveFileName = saveFileName;
            saveFileDataWritter.DeleteSaveFile();
        }
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWritter = new SaveFileDataWritter();
            saveFileDataWritter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWritter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWritter.LoadSaveFile();
            saveFileDataWritter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWritter.LoadSaveFile();
            saveFileDataWritter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWritter.LoadSaveFile();
        }
        private void NewGame()
        {

            SaveGame();
            StartCoroutine(LoadWorldScene());
        }
    }
}