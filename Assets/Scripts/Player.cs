using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;

    [Header("Sprites")]
    public GameObject spr1;
    public GameObject spr2;
    public GameObject spr3;

    public int type;

    public Conductor conductorScript;

    public AudioSource audioSrc;
    public AudioClip hit;
    public AudioClip swing;
    
    public bool canHit;

    public bool canInput;

    public static Player instance;

    public List<float> hitTimes;

    public Collider2D col;

    public bool canHitAgain;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (canInput)
        {
            if (Input.GetKeyDown(KeyCode.Z) /*|| (Input.GetMouseButtonDown(0))*/)
            {
                Swing();
            }

            if (Conductor.instance.onBeat)
            {
                if (hitTimes.Contains(Conductor.instance.songPosBeat))
                {
                    Swing();
                }
            }
        }
    }

    public void Swing()
    {
        canHitAgain = true;
        anim.Play("Swing", -1, 0);
        audioSrc.PlayOneShot(swing);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
