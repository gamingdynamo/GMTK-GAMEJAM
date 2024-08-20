using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource m_AudioSource;

    private Transform CamTransform;
    [SerializeField]private AudioClip m_Clip;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CamTransform = Camera.main.transform;
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="point"></param>
    /// <param name="playatCam"> 0 or 1</param>
    public void PlayAudioAtPosition(AudioClip audioClip, Vector3 point, int playatCam ,float pitchdeviation,float VolumeDeviation)
    {
        m_AudioSource.clip = audioClip;
        m_AudioSource.pitch = Random.Range(1 - pitchdeviation, 1 + pitchdeviation);
        m_AudioSource.volume = Random.Range(1 - VolumeDeviation, 1 + VolumeDeviation);

        m_AudioSource.Play();
    }
}
