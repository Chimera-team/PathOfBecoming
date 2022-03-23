using UnityEngine;
using System;
using UnityEngine.UI;
using Cinemachine;
using AnimationUtils.ImageUtils;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public abstract class Engine : MonoBehaviour
{
    public static bool load = false;

    public static Engine current;

    [SerializeField] protected Interface userInterface;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineVirtualCamera examineCamera;
    [SerializeField] private GameObject pausePanel;
    public DialogueSystem dialogueSystem;
    public PuzzleController puzzleController;
    public GameSettings gameSettings;

    [Header("Start Positions")]
    [SerializeField] Transform playerStartPosition;
    [SerializeField] Transform fairyStartPosition;

    [Header("Other")]
    [SerializeField] protected GameObject startDialog;

    protected const float timeToFade = 1f;

    private List<int> _checkpoints = new List<int>();

    public PlayerController playerController { get; private set; }
    public EyeOfHassle eyeOfHassle { get; private set; }
    public Fairy fairyController { get; private set; }

    protected Image curtain;

    protected AudioClip mainTheme;
    protected AudioClip ambient;

    private const float timeToZoom = 2f;

    private bool _playerHandler = true;
    public bool PlayerHandler { get { return _playerHandler; }}

    public IPlayerIndicator Indicator 
    {
        get
        {
            if (_playerHandler)
                return playerController;
            else
                return fairyController;
        }
    }

    public GameObject player { get; protected set; }
    public GameObject fairy { get; protected set; }
    protected abstract void Start_Level();

    protected virtual void Load_Level()
    {
        SaveData data;
        try
        {
            data = SaveSyatem.Load_Data();
        }
        catch (NullReferenceException)
        {
            Start_Level();
            return;
        }
        if (data.playerData.sceneIndex != SceneManager.GetActiveScene().buildIndex)
            Start_Level();
        Spawn_Characters(data.playerData.lastCheckpoint.Convert_to_UnityVector(), data.fairyData.checkPoint.Convert_to_UnityVector());
        playerController.Load_State(data.playerData);
        fairyController.Load_State(data.fairyData);
        Connect_Fairy_to_Player();
        userInterface.inventory.LoadInventoryData(data.inventoryData);
        userInterface.spellBook.Load_State(data.spellBookData);

        _checkpoints.AddRange(data.checkpoints);
        dialogueSystem.Load_State(data.checkpoints);
        Show_Scene(() => {
            userInterface.inventory.Enable_Inventory();
            userInterface.spellBook.Enable_Spellbook();
            userInterface.Enable_Interface(true);
            startDialog.SetActive(true);
        });
    }

    protected void Spawn_Characters()
    {
        Spawn_Characters(playerStartPosition.position, fairyStartPosition.position);
    }

    protected void Spawn_Characters(Vector3 playerPosition, Vector3 fairyPosition)
    {
        GameObject player = Resources.Load<GameObject>("Prefabs/Characters/Player");
        GameObject fairy = Resources.Load<GameObject>("Prefabs/Characters/Fairy");
        this.player = Instantiate(player, playerPosition, Quaternion.identity);
        this.fairy = Instantiate(fairy, fairyPosition, Quaternion.identity);
        Initialise();
    }

    protected void Spawn_Characters(GameObject player, GameObject fairy)
    {
        Spawn_Characters(player, fairy, playerStartPosition.position, fairyStartPosition.position);
    }

    protected void Spawn_Characters(GameObject player, GameObject fairy, Vector3 playerPosition, Vector3 fairyPosition)
    {
        this.player = Instantiate(player, playerPosition, Quaternion.identity);
        this.fairy = Instantiate(fairy, fairyPosition, Quaternion.identity);
        Initialise();
    }

    void Initialise()
    {
        playerCamera.Follow = player.transform;
        playerController = player.GetComponent<PlayerController>();
        fairyController = fairy.GetComponent<Fairy>();
    }

    protected virtual void Awake()
    {
        current = this;
        curtain = userInterface.curtain;
        SoundRecorder.Play_Music(mainTheme);
        SoundRecorder.Play_Ambient(ambient);
        
    }

    private void Start()
    {
        gameSettings = SaveSyatem.gameSettings;
        if (!load)
            Start_Level();
        else
            Load_Level();
    }

    public void Checkpoint(int index)
    {
        if (_checkpoints.Contains(index))
            return;
        _checkpoints.Add(index);

        PlayerData playerData = playerController.Save_State();
        FairyData fairyData = fairyController.Save_State();
        InventoryData inventoryData = userInterface.inventory.SaveInvetnoryData();
        SpellBookData spellBookData = userInterface.spellBook.Save_State();
        new SaveData(_checkpoints.ToArray(), playerData, fairyData, inventoryData, spellBookData).Save();
    }

    public void Connect_Fairy_to_Player()
    {
        fairyController.Connect_Fairy(playerController.fairyAnchor);
    }

    public void Last_Chekpoint()
    {
        Hide_Scene(() =>
        {
            load = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    public void Hide_Scene(Action onComplete = null)
    {
        playerController.Change_Controls<UncontrollableHandler>();
        curtain.gameObject.SetActive(true);
        curtain.Unfade(timeToFade, onComplete);
    }

    public void Show_Scene(Action onComplete = null)
    {      
        playerController.Change_Controls<UncontrollableHandler>();
        onComplete += () => curtain.gameObject.SetActive(false);
        onComplete += () => playerController.Change_Controls<DefaultHandler>();
        curtain.Fade(timeToFade, onComplete);
    }

    public void Collect_Eye_Of_Hassle(EyeOfHassle eye)
    {
        eyeOfHassle = eye;
    }

    public void Remove_Eye_Of_Hassle()
    {
        eyeOfHassle = null;
    }

    public void Awake_Eye()
    {
        if (eyeOfHassle)
            eyeOfHassle.Eye_Awaken();
    }

    public void Ensleep_Eye()
    {
        if (eyeOfHassle)
            eyeOfHassle.Eye_Asleep();
    }

    public void Load_Next_Level()
    {
        load = false;
        Hide_Scene(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void Play_Animation(Animation animation)
    {
        animation.Play();
    }

    public void Switch(bool playerActive)
    {
        playerController.Change_Controls<DefaultHandler>();
        if (playerActive)
        {
            Connect_Fairy_to_Player();
            Indicator.Hide_Indicator();
            _playerHandler = true;

            fairyController.Collider.enabled = false;

            playerController.Collider.enabled = true;
            playerController.rb.bodyType = RigidbodyType2D.Dynamic;

            playerCamera.Follow = player.transform;
        }
        else
        {
            fairyController.Free_Fairy();
            Indicator.Hide_Indicator();
            _playerHandler = false;

            playerController.Collider.enabled = false;
            playerController.rb.bodyType = RigidbodyType2D.Static;

            fairyController.Collider.enabled = true;

            playerCamera.Follow = fairy.transform;
        }
    }

    public void Zoom_Object(Transform focus)
    {
        examineCamera.Follow = focus;
        StartCoroutine(Temp_Switch_Cam());
    }

    IEnumerator Temp_Switch_Cam()
    {
        playerController.Change_Controls<UncontrollableHandler>();
        Indicator.Hide_Indicator();
        examineCamera.Priority = 11;
        yield return new WaitForSeconds(timeToZoom);
        playerController.Change_Controls<DefaultHandler>();
        Indicator.Show_Indicator();
        examineCamera.Priority = 1;
    }

    public void Camera_Shake(float timeToShake)
    {
        StartCoroutine(Vcam_Shaker(timeToShake));
    }

    IEnumerator Vcam_Shaker(float timeToShake)
    {
        CinemachineBasicMultiChannelPerlin noise = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        playerController.Change_Controls<UncontrollableHandler>();
        noise.m_AmplitudeGain = 2;
        yield return new WaitForSeconds(timeToShake);
        noise.m_AmplitudeGain = 0;
        playerController.Change_Controls<DefaultHandler>();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void Restart()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        playerController.Die();
    }

    public void Main_Menu()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Hide_Scene(() => SceneManager.LoadScene(0));
    }
}
