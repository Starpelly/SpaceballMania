using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float lastReportedBeat = 0f;

    public float dspSongTime;
    public float firstBeatOffset;
    public AudioSource musicSource;
    public AudioSource metronome_audioSrc;
    public float secPerRealBeat;

    public float songLength;

    public bool onBeat;

    public float songPosBeat;

    public float beatTime;

    private int timesQuarterBeat;

    //DELETE THESE COMMENTS

    /*public delegate void Beat();
    public static event Beat BeatEvent;*/

    public bool metronome;

    public bool autoPlay = true;

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

        songLength = musicSource.clip.length;
    }

    void Update()
    {
        lastReportedBeat = songPositionInBeats;
        if (musicSource.isPlaying)
        {
            secPerRealBeat = 60f / bpm;
            secPerBeat = 15f / bpm;

            //determine how many seconds since the song started
            //songPosition = (float)(AudioSettings.dspTime - dspSongTime);
            songPosition = (float)(musicSource.time - dspSongTime - firstBeatOffset);
            //determine how many beats since the song started
            songPositionInBeatsExact = songPosition / secPerBeat;
            songPositionInBeats = (int)songPositionInBeatsExact;
            ReportBeat();
            //GameTimeline();

            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log(songPosBeat);
            }

            songPosBeat = (float)songPositionInBeats / 4;
        }
    }

    void ReportBeat()
    {
        if (lastReportedBeat < songPositionInBeats)
        {
            onBeat = true;
            times += 1;
            beatTime += 0.25f;
            QuarterBeat();
            lastReportedBeat = songPositionInBeats;
        }
        else
        {
            onBeat = false;
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
        //Debug.Log("beat");
        //songPosBeat += 0.25f; //DONT USE THIS IT COULD GO OUTTA SYNC
    }

    public void FullBeat()
    {
        //BeatEvent();
        //Metronome
        if (metronome == true)
        {
            Debug.Log("Beat");
            metronome_audioSrc.Play();
        }
    }
}