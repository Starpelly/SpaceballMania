using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public TextAsset textJSON;

    public string json;

    [Header("Objects")]
    public Conductor conductor;
    public Player playerScript;
    public Dispenser dispenserScript;
    public Camera cam;
    public Animator alienAnim;

    [Header("Sounds")]
    public AudioClip shoot;
    public AudioClip highShoot;
    private AudioSource audioSrc;

    public string levelcreator;
    public string songcreator;
    public float[] entities;
    float[] riceballs;
    float[] highriceballs;
    float[] highballs;
    float[] alien;
    float end;
    GameData loadedGameData;

    [Header("Sprites")]
    public Sprite ball;
    public Sprite riceBall;

    [Header("Other")]
    public bool autoPlay = false;

    public new bool camera;
    // public Editor editorScript;


    public CameraList myCameraList = new CameraList();
    public CostumeList myCostumeList = new CostumeList();

    public bool mainGame = false;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        if (mainGame)
            LoadDataNonPath(textJSON.text);
    }

    public void LoadDataNonPath(string text)
    {
        loadedGameData = JsonUtility.FromJson<GameData>(text);
        myCameraList = JsonUtility.FromJson<CameraList>(text);
        myCostumeList = JsonUtility.FromJson<CostumeList>(text);

        conductor.bpm = loadedGameData.bpm;

        /*for (int i = 0; i < loadedGameData.entities.Length; i++)
        {
            Debug.Log(loadedGameData.entities[i]);
        }*/
        levelcreator = loadedGameData.levelcreator;
        songcreator = loadedGameData.songcreator;
        entities = loadedGameData.entities;
        riceballs = loadedGameData.riceballs;
        highriceballs = loadedGameData.highriceballs;
        highballs = loadedGameData.highballs;
        alien = loadedGameData.alien;
        end = loadedGameData.end;

        alienAnim.speed = conductor.bpm / 60;
    }

    public void LoadData(string path, bool normalString)
    {
        if (normalString)
        {
            loadedGameData = JsonUtility.FromJson<GameData>(path);

            myCameraList = JsonUtility.FromJson<CameraList>(path);
            myCostumeList = JsonUtility.FromJson<CostumeList>(path);
        } else
        {
            json = File.ReadAllText(path);
            loadedGameData = JsonUtility.FromJson<GameData>(json);

            myCameraList = JsonUtility.FromJson<CameraList>(json);
            myCostumeList = JsonUtility.FromJson<CostumeList>(json);
        }

        conductor.bpm = loadedGameData.bpm;

        /*for (int i = 0; i < loadedGameData.entities.Length; i++)
        {
            Debug.Log(loadedGameData.entities[i]);
        }*/
        levelcreator = loadedGameData.levelcreator;
        songcreator = loadedGameData.songcreator;
        entities = loadedGameData.entities;
        riceballs = loadedGameData.riceballs;
        highriceballs = loadedGameData.highriceballs;
        highballs = loadedGameData.highballs;
        alien = loadedGameData.alien;
        end = loadedGameData.end;

        alienAnim.speed = conductor.bpm / 60;
    }

    public void WriteData(string path, string contents)
    {

        File.WriteAllText(path, contents);
        loadedGameData = JsonUtility.FromJson<GameData>(json);

        myCameraList = JsonUtility.FromJson<CameraList>(json);
        myCostumeList = JsonUtility.FromJson<CostumeList>(json);

        loadedGameData.entities = new float[133];
        loadedGameData.entities[0] = 1f;

        json = JsonUtility.ToJson(path);
    }

    private void Update()
    {
        if (conductor.onBeat)
        {

            if (entities.Contains(conductor.songPosBeat))
                Shoot(ball);
            else if (riceballs.Contains(conductor.songPosBeat))
                Shoot(riceBall);

            if (highballs.Contains(conductor.songPosBeat))
                HighShoot(ball);
            else if (highriceballs.Contains(conductor.songPosBeat))
                HighShoot(riceBall);

            if (alien.Contains(conductor.songPosBeat))
                alienAnim.Play("Open", 0, 0);

            if (camera)
            {
                for (int i = 0; i < myCameraList.camera.Length; i++)
                {
                    if (myCameraList.camera[i].beat == conductor.songPosBeat)
                    {
                        StartCoroutine(ResizeCamera(new Vector3(-0.01f, -0.01f, -myCameraList.camera[i].size), cam.transform.position, conductor.secPerBeat * myCameraList.camera[i].duration * 4f));
                    }
                }
            }

            for (int i = 0; i < myCostumeList.costume.Length; i++)
            {
                if (myCostumeList.costume[i].beat == conductor.songPosBeat)
                {
                    if (myCostumeList.costume[i].type == 1)
                    {
                        playerScript.spr1.SetActive(true);
                        playerScript.spr2.SetActive(false);
                        playerScript.spr3.SetActive(false);
                    } else if (myCostumeList.costume[i].type == 2)
                    {
                        playerScript.spr1.SetActive(false);
                        playerScript.spr2.SetActive(true);
                        playerScript.spr3.SetActive(false);
                    } else if (myCostumeList.costume[i].type == 3)
                    {
                        playerScript.spr1.SetActive(false);
                        playerScript.spr2.SetActive(false);
                        playerScript.spr3.SetActive(true);
                    }
                }
            }

            if (conductor.songPosBeat == end)
            {
                Application.Quit();
                Debug.Log("Quit");
            }
        }
    }

    private void Shoot(Sprite type)
    {
        audioSrc.PlayOneShot(shoot);
        dispenserScript.Shoot(conductor.bpm / 60, type);
    }

    private void HighShoot(Sprite type)
    {
        audioSrc.PlayOneShot(highShoot);
        dispenserScript.ShootHigh(conductor.bpm / 60, type);
    }

    [System.Serializable]
    private class GameData
    {
        public string levelcreator;
        public string songcreator;
        public float bpm;
        public float end;
        public float[] entities;
        public float[] riceballs;
        public float[] highriceballs;
        public float[] highballs;
        public float[] alien;
    }

    [System.Serializable]
    public class CameraJSON
    {
        public float beat;
        public float duration;
        public float size;
    }

    [System.Serializable]
    public class CameraList
    {
        public CameraJSON[] camera;
    }


    [System.Serializable]
    public class CostumeJSON
    {
        public float beat;
        public int type;
    }

    [System.Serializable]
    public class CostumeList
    {
        public CostumeJSON[] costume;
    }


    public void BeatEvent()
    {
        //editorScript.MoveTimelineMarker();
        playerScript.canHit = true;
    }


    ///----------------------------------------------------------------------------------------------

    IEnumerator ResizeCamera(Vector3 endValue, Vector3 valueToLerp, float lerpDuration = 1.4f)
    {
        float timeElapsed = 0;
        Vector3 originalCameraSize = cam.transform.position;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Vector3.Lerp(originalCameraSize, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            cam.transform.position = valueToLerp;
            yield return null;
        }
        cam.transform.position = endValue;
        valueToLerp = endValue;
    }
}
