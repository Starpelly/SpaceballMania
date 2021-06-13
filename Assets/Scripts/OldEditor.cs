/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using CodeMonkey.Utils;
using System.IO;


// DONT TRUST ME TO MAKE LEVEL EDITORS CAUSE I AM NOT GOOD AT IT
public class OldEditor : MonoBehaviour
{
    [Header("Assignables")]
    float bpm = 120f;

    [Header("Game")]
    public GameManager gameManager;

    string songPath;
    string levelPath;

    private List<AudioClip> audioClips;

    public Conductor conductorScript;
    public AudioSource musicSource;

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

    bool isPlaying = false;

    private Grid grid;

    public RectTransform timelineLayer;

    [HideInInspector]
    public string editorJSON;

    public int numOfBeats = 17;


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

    private bool loadedSong = false;
    private bool playedSong = false;

    public Transform gridHolder;
    GameObject actualHolder;
    public Camera cam;


    void Start()
    {
        gameData = new GameData();
        CreateData();
        LoadData();
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

    void LoadData()
    {
        gameData.bpm = bpm;
        bpmField.text = conductorScript.bpm.ToString();
        levelCreatorField.text = gameManager.levelcreator;
        songCreatorField.text = gameManager.songcreator;

        gameData.entities = new float[numOfBeats];
        gameData.riceballs = new float[numOfBeats];
        gameData.highballs = new float[numOfBeats];
        gameData.highriceballs = new float[numOfBeats];
        gameData.costume = new CostumeJSON[numOfBeats];
        gameData.camera = new CameraJSON[numOfBeats];

        editorJSON = JsonUtility.ToJson(gameData);
        gameManager.json = editorJSON;
        gameManager.LoadData(editorJSON, true);
    }

    int dataAdded = 0;

    Vector3 Vec;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //cam.transform.position -= new Vector3(0.65f, cam.transform.position.y);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //cam.transform.position += new Vector3(0.65f, cam.transform.position.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }

        if (conductorScript.musicSource.isPlaying)
        {
            timelineSlider.value = conductorScript.songPosition;
        }
        else
        {
        }

        timelineText.text = conductorScript.songPosBeat.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            //I need to do this for some reason god I have no idea why.
            if (playedSong == false)
                return;


            int x = grid.GetValueX(UtilsClass.GetMouseWorldPosition());
            int y = grid.GetValueY(UtilsClass.GetMouseWorldPosition());

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

    public void LoadSong()
    {
        numOfBeats = Mathf.RoundToInt(conductorScript.songLength / conductorScript.secPerRealBeat);

        // LoadData();
    }

    public void ReloadBPM()
    {
        numOfBeats = Mathf.RoundToInt(conductorScript.songLength / conductorScript.secPerRealBeat);
        //LoadData();
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

    public void OpenSong(bool LoadLevel = false)
    {
        songPath = EditorUtility.OpenFilePanel("Open song file", "", "ogg");

        if (songPath.Length != 0)
        {
            StartCoroutine(LoadAudio(LoadLevel));
        } else
        {
            return;
        }
    }

    int initialLoad = 0;
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

        LoadSong();

        grid = new Grid(numOfBeats, 4, 0.75f, new Vector3(26.2232f, -4.85f), actualHolder.transform, null);

        if (LoadLevel == false)
        {
            gameData.entities = new float[numOfBeats];
            gameData.riceballs = new float[numOfBeats];
            gameData.highballs = new float[numOfBeats];
            gameData.highriceballs = new float[numOfBeats];
            gameData.costume = new CostumeJSON[numOfBeats];
            gameData.camera = new CameraJSON[numOfBeats];
        }

        if (LoadLevel)
        {
            for (int i = 0; i < gameData.entities.Length; i++)
            {
                grid.SetValueXY((int)gameData.entities[i], 3, (int)gameData.entities[i], ball);
            }
            for (int i = 0; i < gameData.riceballs.Length; i++)
            {
                grid.SetValueXY((int)gameData.riceballs[i], 3, (int)gameData.riceballs[i], riceball);
            }
            for (int i = 0; i < gameData.highballs.Length; i++)
            {
                grid.SetValueXY((int)gameData.highballs[i], 2, (int)gameData.highballs[i], ball);
            }
            for (int i = 0; i < gameData.highriceballs.Length; i++)
            {
                grid.SetValueXY((int)gameData.highriceballs[i], 2, (int)gameData.highriceballs[i], riceball);
            }
            for (int i = 0; i < gameData.costume.Length; i++)
            {
                Sprite costumeType = costume_normal;
                if (gameData.costume[i].type == 1) costumeType = costume_normal;
                if (gameData.costume[i].type == 2) costumeType = costume_Bunny;
                if (gameData.costume[i].type == 3) costumeType = costume_circle;
                grid.SetValueXY((int)gameData.costume[i].beat, 1, (int)gameData.costume[i].beat, costumeType);
            }
            for (int i = 0; i < gameData.camera.Length; i++)
            {
                grid.SetValueXY((int)gameData.camera[i].beat, 0, (int)gameData.camera[i].beat, cameraSpr);
            }
        }
    }

    public void SaveLevel()
    {
        levelPath = EditorUtility.SaveFilePanel("Save Level", "", "level", "json");
        File.WriteAllText(levelPath, editorJSON);
    }

    string loadedData;
    public void OpenLevel()
    {
        levelPath = EditorUtility.OpenFilePanel("Open Level", "", "json");
        gameManager.LoadData(levelPath, false);
        loadedData = File.ReadAllText(levelPath);
        gameData = JsonUtility.FromJson<GameData>(loadedData);
        bpm = gameData.bpm;
        gameManager.levelcreator = gameData.levelcreator;
        bpmField.text = conductorScript.bpm.ToString();
        levelCreatorField.text = gameManager.levelcreator;

        // Debug.Log(gameData.entities.Length);
        
        editorJSON = JsonUtility.ToJson(gameData);
        gameManager.json = editorJSON;
        gameManager.LoadData(editorJSON, true);

        //LoadData();
        OpenSong(true);

        // aaCreateData();
        // Destroy(gridHolder.gameObject);
    }

    public void ChangeCreator(string s)
    {
        gameManager.levelcreator = s;
        gameData.levelcreator = gameManager.levelcreator;

        editorJSON = JsonUtility.ToJson(gameData);
        gameManager.json = editorJSON;
        gameManager.LoadData(editorJSON, true);
    }

    int pause = 0;
    int firstPlay = 0;

    // Coroutine timelineMarkerCo;

    public void Pause()
    {
        if (loadedSong && bpm > 0)
        {
            playedSong = true;
            firstPlay += 1;

            if (firstPlay == 1)
            {
                LoadSong();

                playBtn.sprite = pauseBtnSpr;
                Restart();
                timelineSlider.maxValue = conductorScript.songLength;
                return;
            }
            else
                pause += 1;

            if (pause <= 1)
            {
                conductorScript.onBeat = false;
                //AudioListener.pause = true;
                // StopCoroutine(timelineMarkerCo);
                playBtn.sprite = playBtnSpr;
                musicSource.Stop();
            }
            else
            {
                // gameManager.WriteData(editorJSON, gameManager.json);
                playBtn.sprite = pauseBtnSpr;
                Restart();
                // conductorScript.musicSource.time = timelineSlider.value;
                //musicSource.UnPause();

                pause = 0;
            }
        }
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

    public void MoveTimelineMarker()
    {
        timelineMarker.transform.position = new Vector3(timelineMarker.position.x + 0.1875f, timelineMarker.position.y);
    }

    public void AutoPlay()
    {

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
*/