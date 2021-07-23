using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; set; }

    public GameManager gameManager;

    [Header("Assignables")]
    public float bpm = 120;
    public float secPerBeat;
    public float songPosition;
    private float songPositionInBeatsExact;
    private int songPositionInBeats;
    public float beatPosition;

    public float lastReportedBeat = 0f;

    public float dspSongTime;
    public float songTimeInBeats;
    public float firstBeatOffset;
    public AudioSource musicSource;
    public AudioSource metronome_audioSrc;
    public float secPerRealBeat;

    public float songLength;

    public bool onBeat;

    public float songPosBeat;

    public float beatTime;

    public bool metronome;

    public bool autoPlay = true;

    public TMP_Text currentBeatTime;

    public static Conductor instance;

    public bool inEditor = false;

    void Awake()
    {
        instance = this;
    }

    // if anything bad happens for any reason me changing this from Start to Awake might be the problem but idk lol
    void Start()
    {
        Conductor.Instance = this;
        //Load the AudioSource attached to the Conductor GameObject
        musicSource.GetComponent<AudioSource>();
        //Metronome
        //metronome_audioSrc.GetComponent<AudioSource>();
        //Calculate the number of seconds in each beat
        secPerRealBeat = 60f / bpm;
        secPerBeat = 15f / bpm;
        //Record the time when the music starts
        dspSongTime = (float)musicSource.time;
        //Start the music
        //musicSource.time = 10f;

        if (autoPlay)
            musicSource.Play();

        if (musicSource.clip) songLength = musicSource.clip.length;
        songTimeInBeats = songLength / secPerRealBeat;
    }

    int point25Times = 0;
    void Update()
    {
        lastReportedBeat = songPositionInBeats;
        if (musicSource.isPlaying)
        {
            secPerRealBeat = 60f / bpm;
            secPerBeat = 15f / bpm;

            songPosition = (float)(musicSource.time - dspSongTime - firstBeatOffset);
            beatPosition = (float)Mathf.Round(songPosition / secPerBeat / 4 * 100) / 100f; // the reason i do this is so we can only get the first 2 decimal places in the float. we dont need any more than that.
            if (inEditor) currentBeatTime.text = beatPosition.ToString();

            //determine how many beats since the song started
            songPositionInBeatsExact = songPosition / secPerBeat;
            songPositionInBeats = (int)songPositionInBeatsExact;
            ReportBeat();

            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log(songPosBeat);
            }

            songPosBeat = (float)songPositionInBeats / 4;

            beatTime = (float)Mathf.Round(Mathf.Repeat(beatPosition, 1.0f) * 100) / 100f;
        }

        if (songPosBeat == 0.25f && point25Times < 1) // if i don't do this getting a beat at 0.25 doesn't work. ONLY 0.25. and i don't know why
        {
            onBeat = true;
            point25Times++;
        }
    }


    int firstBeatTime = 0;
    bool pastFirstBeat = false;
    void ReportBeat()
    {
        // Debug.Log($"{lastReportedBeat}, {songPositionInBeats}");

        if (lastReportedBeat != 0)
        {
            if (lastReportedBeat < songPositionInBeats)
            {
                onBeat = true;
                times += 1;
                // beatTime += 0.25f;
                pastFirstBeat = true;
                QuarterBeat();
                lastReportedBeat = songPositionInBeats;
            }
            else
            {
                onBeat = false;
            }
        }
        else
        {
            if (pastFirstBeat)
            {
                pastFirstBeat = false;
                firstBeatTime = 0;
                point25Times = 0;
            }
            if (firstBeatTime < 1)
            {
                onBeat = true;
                times += 1;
                // beatTime += 0.25f;
                QuarterBeat();
                firstBeatTime++;
            }
            else
            {
                firstBeatTime++;
                onBeat = false;
            }
        }
    }

    private int times;
    public void QuarterBeat()
    {
        gameManager.BeatEvent();
        if (times == 4)
        {
            beatTime = 0;
            times = 0;
            FullBeat();
        }
    }

    public void FullBeat()
    {
        if (metronome == true)
        {
            Debug.Log("Beat");
            metronome_audioSrc.Play();
        }
    }
}