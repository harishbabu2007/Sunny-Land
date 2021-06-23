using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();   
        source.Play();
    }
}
