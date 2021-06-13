using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) /*|| (Input.GetMouseButtonDown(0))*/)
        {
            Swing();
        }
    }

    public void Swing()
    {

        anim.Play("Swing", -1, 0);
        audioSrc.PlayOneShot(swing);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
