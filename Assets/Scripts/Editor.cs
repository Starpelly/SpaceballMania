using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.IO;
using CodeMonkey.Utils;


// DONT TRUST ME TO MAKE LEVEL EDITORS CAUSE I AM NOT GOOD AT IT
public class Editor : MonoBehaviour
{
    [Header("Assignables")]
    float bpm = 120f;

    [Header("Game")]
    public GameManager gameManager;
    public Conductor conductorScript;
    public AudioSource musicSource;

    string songPath;
    string levelPath;

    private List<AudioClip> audioClips;

    [Header("Properties")]
    public TMP_InputField songField;
    public GameObject beatSeperator;
    public TMP_InputField bpmField;
    public TMP_InputField levelCreatorField;
    public TMP_InputField songCreatorField;
    public Slider timelineSlider;
    public RectTransform sliderTransform;
    public TMP_Text timelineText;

    public Transform timelineMarker;

    [Header("Top Buttons")]
    public Image playBtn;
    public Sprite playBtnSpr, pauseBtnSpr;

    private Grid grid;

    // public RectTransform timelineLayer;

    [HideInInspector]
    public string editorJSON;

    public int numOfBeats = 17;
    private bool loadedSong = false;
    private bool playedSong = false;

    GameData gameData;

    [Header("Sprites")]
    public Sprite ball;
    public Sprite riceball;

    public Sprite costume_Bunny;
    public Sprite costume_circle;
    public Sprite costume_normal;
    public Sprite cameraSpr;

    [Header("Background UI")]
    public Image metronomeBG;

    public Transform gridHolder;
    GameObject actualHolder;
    public Camera cam;


    void Start()
    {
        gameData = new GameData();
        CreateData();
    }

    private string levelCreator;
    private string songCreator;

    public void CreateData()
    {
        gameData.levelcreator = levelCreator;
        gameData.songcreator = songCreator;
        gameData.version = 6.9f;
        gameData.bpm = bpm;
        Debug.Log(gameData.bpm);

        gameData.entities = new float[numOfBeats];
        gameData.riceballs = new float[numOfBeats];
        gameData.highballs = new float[numOfBeats];
        gameData.highriceballs = new float[numOfBeats];
        gameData.costume = new CostumeJSON[numOfBeats];
        gameData.camera = new CameraJSON[numOfBeats];

        editorJSON = JsonUtility.ToJson(gameData);
        gameManager.json = editorJSON;
        gameManager.LoadData(editorJSON, true);

        Debug.Log(editorJSON);
    }


    [System.Serializable]
    private class GameData
    {
        public string levelcreator;
        public string songcreator;
        public float version;
        public float bpm;
        public float end;
        public float[] entities;
        public float[] riceballs;
        public float[] highballs;
        public float[] highriceballs;
        public float[] alien;

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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Play();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //I need to do this for some reason god I have no idea why.
            if (playedSong == false)
                return;

            int x = grid.GetValueX(UtilsClass.GetMouseWorldPosition());
            int y = grid.GetValueY(UtilsClass.GetMouseWorldPosition());

            Debug.Log(x);

            //Spaghetti
            if (y == 0)
            {
                int originalValue = grid.GetValue(UtilsClass.GetMouseWorldPosition()) + 1;
                float cameraDuration = grid.GetValue(UtilsClass.GetMouseWorldPosition()) + 0.25f;
                Sprite originalSprite = null;

                originalSprite = cameraSpr;

                gameData.camera[x].beat = x;
                gameData.camera[x].duration = cameraDuration;
                gameData.camera[x].size = originalValue;

                grid.SetValueInt(UtilsClass.GetMouseWorldPosition(), originalValue, originalSprite, false);
            }
            else if (y == 1)
            {
                int originalValue = grid.GetValue(UtilsClass.GetMouseWorldPosition()) + 1;
                Sprite originalSprite = null;

                if (originalValue == 1)
                {
                    originalSprite = costume_normal;
                }
                else if (originalValue == 2)
                {
                    originalSprite = costume_Bunny;
                }
                else if (originalValue == 3)
                {
                    originalSprite = costume_circle;
                }
                else if (originalValue >= 4)
                    originalValue = 0;

                gameData.costume[x].beat = x;
                gameData.costume[x].type = originalValue;
                grid.SetValue(UtilsClass.GetMouseWorldPosition(), originalValue, originalSprite);
            }
            else if (y == 2)
            {
                int originalValue = grid.GetValue(UtilsClass.GetMouseWorldPosition()) + 1;
                Sprite originalSprite = null;

                if (originalValue == 1)
                {
                    gameData.highballs[x] = x;
                    originalSprite = ball;
                }
                else if (originalValue == 2)
                {
                    gameData.highballs[x] = 0;
                    gameData.highriceballs[x] = x;
                    originalSprite = riceball;
                }
                else if (originalValue >= 3)
                {
                    gameData.highballs[x] = 0;
                    gameData.highriceballs[x] = 0;
                    originalValue = 0;
                    originalSprite = null;
                }

                grid.SetValue(UtilsClass.GetMouseWorldPosition(), originalValue, originalSprite);
            }
            else if (y == 3)
            {
                int originalValue = grid.GetValue(UtilsClass.GetMouseWorldPosition()) + 1;
                Sprite originalSprite = null;

                if (originalValue == 1)
                {
                    gameData.entities[x] = x;
                    originalSprite = ball;
                }
                else if (originalValue == 2)
                {
                    gameData.entities[x] = 0;
                    gameData.riceballs[x] = x;
                    originalSprite = riceball;
                }
                else if (originalValue >= 3)
                {
                    gameData.entities[x] = 0;
                    gameData.riceballs[x] = 0;
                    originalValue = 0;
                    originalSprite = null;
                }

                grid.SetValue(UtilsClass.GetMouseWorldPosition(), originalValue, originalSprite);
            }

            editorJSON = JsonUtility.ToJson(gameData);
            gameManager.json = editorJSON;
            gameManager.LoadData(editorJSON, true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (playedSong == false)
                return;

            int x = grid.GetValueX(UtilsClass.GetMouseWorldPosition());
            int y = grid.GetValueY(UtilsClass.GetMouseWorldPosition());

            if (y == 0)
            {
                gameData.camera[x].beat = 0;
                gameData.camera[x].duration = 0;
                gameData.camera[x].size = 0;
                grid.SetValueInt(UtilsClass.GetMouseWorldPosition(), 0, null, true);
            }
            else if (y == 1)
            {
                gameData.costume[x].beat = 0;
                gameData.costume[x].type = 0;
            }
            else if (y == 2)
            {
                gameData.highballs[x] = 0;
                gameData.highriceballs[x] = 0;
                grid.SetValue(UtilsClass.GetMouseWorldPosition(), 0, null);
            }
            else if (y == 3)
            {
                gameData.entities[x] = 0;
                grid.SetValue(UtilsClass.GetMouseWorldPosition(), 0, null);
            }

            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 0, null);
            editorJSON = JsonUtility.ToJson(gameData);
            gameManager.json = editorJSON;
            gameManager.LoadData(editorJSON, true);

            // Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }

    [System.Obsolete]
    public void OpenSong(bool LoadLevel = false)
    {
        //songPath = EditorUtility.OpenFilePanel("Open song file", "", "ogg");

        if (songPath.Length != 0)
        {
            StartCoroutine(LoadAudio(LoadLevel));
        }
        else
        {
            return;
        }
    }

    [System.Obsolete]
    private IEnumerator LoadAudio(bool LoadLevel)
    {
        Destroy(actualHolder);
        WWW www = new WWW("file://" + songPath);
        while (!www.isDone)
            yield return 0;

        loadedSong = true;
        conductorScript.musicSource.clip = www.GetAudioClip(false, true);
        conductorScript.songLength = conductorScript.musicSource.clip.length;
        songField.text = songPath;

        actualHolder = new GameObject("actualHolder");
        actualHolder.transform.parent = gridHolder;

        numOfBeats = Mathf.RoundToInt(conductorScript.songLength / conductorScript.secPerRealBeat);

        grid = new Grid(numOfBeats, 4, 0.75f, new Vector3(26.2232f, -4.85f), actualHolder.transform, null);
    }

    int pause = 0;
    int firstPlay = 0;
    public void Play()
    {
        if (loadedSong && bpm > 0)
        {
            playedSong = true;
            firstPlay += 1;
            if (firstPlay == 1)
            {
                Restart();
                playBtn.sprite = pauseBtnSpr;
                timelineSlider.maxValue = conductorScript.songLength;
                return;
            }
            else
                pause += 1;

            if (pause <= 1)
            {
                conductorScript.onBeat = false;
                playBtn.sprite = playBtnSpr;
                musicSource.Stop();
            }
            else
            {
                Restart();
                playBtn.sprite = pauseBtnSpr;

                pause = 0;
            }
        }
    }

    public void Restart()
    {
        timelineMarker.transform.position = new Vector3(26.2231998f, -3.28800011f, 0);
        //timelineMarkerCo = StartCoroutine(MoveMarker(timelineMarker.position, new Vector3(0.75f * numOfBeats + 26.2232f, timelineMarker.position.y), conductorScript.songLength));
        //Debug.Log(conductorScript.songLength);
        timelineSlider.value = 0;
        playBtn.sprite = pauseBtnSpr;
        conductorScript.musicSource.time = timelineSlider.value;
        musicSource.Play();
    }

    private string inputBpm;
    public void ChangeBPM(string s)
    {
        inputBpm = s;
        float finalBpm = float.Parse(inputBpm);

        if (finalBpm > 300)
        {
            Debug.Log("Greater than 300, might not be ideal");
        }
        bpm = float.Parse(inputBpm);
        gameData.bpm = bpm;
        conductorScript.bpm = float.Parse(inputBpm);

        editorJSON = JsonUtility.ToJson(gameData);
        gameManager.json = editorJSON;
        gameManager.LoadData(editorJSON, true);

        //RectTransformExtensions.SetRight(sliderTransform, conductorScript.bpm * -44.9399f);
    }


    public void SaveLevel()
    {
        // levelPath = EditorUtility.SaveFilePanel("Save Level", "", "level", "json");
        File.WriteAllText(levelPath, editorJSON);
    }

    public void MoveTimelineMarker()
    {
        timelineMarker.transform.position = new Vector3(timelineMarker.position.x + 0.1875f, timelineMarker.position.y);
    }

    IEnumerator MoveMarker(Vector3 valueToLerp, Vector3 endValue, float lerpDuration = 1.4f)
    {
        float timeElapsed = 0;
        Vector3 originalBallPos = this.timelineMarker.position;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Vector3.Lerp(originalBallPos, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            timelineMarker.position = valueToLerp;
            yield return null;
        }
        timelineMarker.position = endValue;
        valueToLerp = endValue;
    }

    int metronome = 0;
    public void Metronome()
    {
        metronome += 1;

        if (metronome <= 1)
        {
            metronomeBG.enabled = true;
            conductorScript.metronome = true;
        }
        else
        {
            metronomeBG.enabled = false;
            conductorScript.metronome = false;
            metronome = 0;
        }
    }

    int music = 0;
    public void MuteMusic()
    {
        music += 1;

        if (music <= 1)
            musicSource.volume = 0;
        else
        {
            musicSource.volume = 1;
            music = 0;
        }
    }
}
