using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PellyTools;
using System;

public class Editor : MonoBehaviour
{
    public static Editor instance { get; set; }

    public DiscordManager discordManager;

    [Header("Components")]
    public Conductor conductor;
    public GameManager gameManager;
    public Camera mainCamera;

    [Header("Events")]
    public Transform gridHolder;
    public GameObject normalBallEvent;
    public GameObject riceBallEvent;
    public GameObject highBallEvent;
    public GameObject highRiceBallEvent;
    public GameObject costume1Event;
    public GameObject costume2Event;
    public GameObject costume3Event;
    public GameObject cameraEvent;

    [Header("Menu Shit")]
    public GameObject playBtn;
    public GameObject pauseBtn;
    public GameObject stopBtn;
    public Button metronomeBtn;

    [Header("Timeline Shit")]
    public float beatOffset;
    public GameObject mainHolder;
    public GameObject markersHolder;
    public GameObject tileHolder;
    public GameObject timeMarker;
    public GameObject timelineMarkers;
    public GameObject numberlineText;
    public GameObject timelineCanvas;
    public Slider scrollbar;
    private Vector2 originalTimeMarkerPos;
    public float anotherOffset;
    public float numOfBeats;

    [Header("Game Preview Shit")]
    public ParticleSystem game_stars;
    public GameObject alien;
    public Player player;
    public Camera cam;
    private float originalCamZoom;

    [Header("Grid Shit")]
    public Vector2 gridOffset = new Vector2(1994.57f, -4.8f);
    public Grid grid;
    public List<Vector4> gridPositions;
    public string editorJSON;

    public GameData gameData;

    [System.Serializable]
    public class GameData
    {
        public string levelcreator;
        public string songcreator;
        public float version;
        public float bpm;
        public float end;
        public List<float> entities;
        public List<float> riceballs;
        public List<float> highballs;
        public List<float> highriceballs;
        public List<float> alien;

        public CostumeJSON[] costume;
        public CameraJSON[] camera;
    }

    [System.Serializable]
    public class CostumeJSON
    {
        public float beat;
        public int type;
    }

    [System.Serializable]
    public class CameraJSON
    {
        public float beat;
        public float duration;
        public float size;
    }

    public void CreateData()
    {
        gameData.levelcreator = "levelcreator";
        gameData.songcreator = "songcreator";
        gameData.version = 1.0f;
        gameData.bpm = 105f;

        /*gameData.entities = new float[0];
        gameData.riceballs = new float[(int)numOfBeats];
        gameData.highballs = new float[(int)numOfBeats];
        gameData.highriceballs = new float[(int)numOfBeats];
        gameData.costume = new CostumeJSON[(int)numOfBeats];
        gameData.camera = new CameraJSON[(int)numOfBeats];*/

        gameData.entities = new List<float>();
        gameData.riceballs = new List<float>();
        gameData.highballs = new List<float>();
        gameData.highriceballs = new List<float>();

        UpdateData();
    }

    public void UpdateData()
    {
        editorJSON = JsonUtility.ToJson(gameData);
        gameManager.json = editorJSON;
        gameManager.LoadData(editorJSON, true);
    }


    private void Start()
    {
        gameData = new GameData();
        // CreateData();

        if (conductor.songTimeInBeats > 0)
        {
            numOfBeats = conductor.songTimeInBeats;
        }
        grid.gridSize = new Vector2(numOfBeats, 4);

        // grid = new Grid(Mathf.RoundToInt(numOfBeats), 4, 1, tileHolder.transform, new Vector3(gridOffset.x, gridOffset.y));

        originalTimeMarkerPos = timeMarker.transform.localPosition;

        for (int i = 0; i < Mathf.RoundToInt(numOfBeats) + 1; i++)
        {
            GameObject test = Instantiate(timelineMarkers);
            test.transform.parent = markersHolder.transform;
            test.transform.localPosition = new Vector2(originalTimeMarkerPos.x - 3f + i + beatOffset, test.transform.localPosition.y - 3.48f + 2.72f);
            
            GameObject number = Instantiate(numberlineText, timelineCanvas.transform, false);
            number.GetComponent<RectTransform>().localPosition = new Vector2(originalTimeMarkerPos.x - 3f + i + beatOffset, number.GetComponent<RectTransform>().localPosition.y);
            number.GetComponent<TMP_Text>().text = $"{i}";
        }

        if (conductor.songTimeInBeats - anotherOffset > 0 && conductor.songTimeInBeats > 0)
            scrollbar.maxValue = numOfBeats - anotherOffset;

        pauseBtn.GetComponent<Image>().color = Color.gray;
        stopBtn.GetComponent<Image>().color = Color.gray;

        originalCamZoom = cam.transform.localPosition.z;

        Time.timeScale = 1;

        for (int i = 0; i < gameManager.loadedGameData.entities.Count; i++)
        {
            Instantiate(normalBallEvent, new Vector3(1995 + gameManager.loadedGameData.entities[i], 1.5f), Quaternion.identity, gridHolder);
        }
        for (int i = 0; i < gameManager.loadedGameData.highballs.Count; i++)
        {;
            Instantiate(highBallEvent, new Vector3(1995 + gameManager.loadedGameData.highballs[i], 0.5f), Quaternion.identity, gridHolder);
        }
        for (int i = 0; i < gameManager.loadedGameData.highriceballs.Count; i++)
        {
            Instantiate(highRiceBallEvent, new Vector3(1995 + gameManager.loadedGameData.highriceballs[i], 0.5f), Quaternion.identity, gridHolder);
        }
        for (int i = 0; i < gameManager.myCostumeList.costume.Length; i++)
        {
            if (gameManager.myCostumeList.costume[i].type == 1)
                Instantiate(costume1Event, new Vector3(1995 + gameManager.myCostumeList.costume[i].beat, -0.5f), Quaternion.identity, gridHolder);
            else if (gameManager.myCostumeList.costume[i].type == 2)
                Instantiate(costume2Event, new Vector3(1995 + gameManager.myCostumeList.costume[i].beat, -0.5f), Quaternion.identity, gridHolder);
            else if (gameManager.myCostumeList.costume[i].type == 3)
                Instantiate(costume3Event, new Vector3(1995 + gameManager.myCostumeList.costume[i].beat, -0.5f), Quaternion.identity, gridHolder);
        }
        for (int i = 0; i < gameManager.myCameraList.camera.Length; i++)
        {
            GameObject cameraObj = Instantiate(cameraEvent, new Vector3(1995 + gameManager.myCameraList.camera[i].beat, -1.5f), Quaternion.identity, gridHolder);
            // cameraObj.GetComponentInChildren<DragAndDrop>().transform.localScale = new Vector2(gameManager.myCameraList.camera[i].duration, 1);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(UtilsClass.GetMouseWorldPosition());
            // grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!conductor.musicSource.isPlaying)
                {
                    Play();
                }
                else
                {
                    Pause(false);
                }
            }
            else
            {
                if (!conductor.musicSource.isPlaying)
                {
                    Play();
                }
                else
                {
                    Pause(true);
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift)) scrollbar.value -= 20.10f * Time.deltaTime;
            else scrollbar.value -= 5.03f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift)) scrollbar.value += 20.10f * Time.deltaTime;
            else scrollbar.value += 5.03f * Time.deltaTime;
        }

        mainCamera.transform.position = new Vector3(scrollbar.value + 2000, mainCamera.transform.position.y, -10f);
        timeMarker.transform.localPosition = new Vector2(originalTimeMarkerPos.x + conductor.beatPosition, timeMarker.transform.localPosition.y);
    }

    int metronomeCount = 0;
    public void Metronome()
    {
        metronomeCount++;
        if (metronomeCount <= 1)
            conductor.metronome = true;
        else
        {
            metronomeCount = 0;
            conductor.metronome = false;
        }
    }

    public void PlayButton()
    {
        if (!conductor.musicSource.isPlaying)
        {
            Play();
        }
        else
        {
            Pause(true);
        }
    }

    public void PauseButton()
    {
        if (conductor.musicSource.isPlaying)
        {
            Pause(false);
        }
    }

    public void StopButton()
    {
        Pause(true);
    }

    public void Play()
    {

        // discordManager.UpdateStatus("", "Playtesting");

        // Pause
        pauseBtn.GetComponent<Button>().interactable = true;
        pauseBtn.GetComponent<Image>().color = Color.white;

        // Stop
        stopBtn.GetComponent<Button>().interactable = true;
        stopBtn.GetComponent<Image>().color = Color.white;

        // Play
        playBtn.GetComponent<Button>().interactable = false;
        playBtn.GetComponent<Image>().color = Color.gray;

        player.canInput = true;
        conductor.musicSource.Play();

        // Time.timeScale = 1;
        AudioListener.pause = false;
        gameManager.ResumeCamera();
    }

    public void Pause(bool stop)
    {
        playBtn.GetComponent<Button>().interactable = true;
        conductor.onBeat = false;
        if (stop)
        {
            // Pause
            pauseBtn.GetComponent<Button>().interactable = false;
            pauseBtn.GetComponent<Image>().color = Color.gray;

            // Stop
            stopBtn.GetComponent<Button>().interactable = false;
            stopBtn.GetComponent<Image>().color = Color.gray;

            // Play
            playBtn.GetComponent<Button>().interactable = true;

            conductor.musicSource.Stop();
            conductor.beatPosition = 0;

            gameManager.StopCamera();
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, originalCamZoom);
            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                Destroy(ball);
            }
        }
        else
        {
            // Pause
            pauseBtn.GetComponent<Button>().interactable = false;
            pauseBtn.GetComponent<Image>().color = Color.gray;

            // Stop
            stopBtn.GetComponent<Button>().interactable = true;

            // Play
            playBtn.GetComponent<Button>().interactable = true;

            conductor.musicSource.Pause();

            gameManager.StopCamera();

            /*foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                //ball.GetComponent<Animator>().enabled = false;
            }*/

        }

        playBtn.GetComponent<Image>().color = Color.white;
        player.canInput = false;
        // Time.timeScale = 0;
        AudioListener.pause = true;
    }
}