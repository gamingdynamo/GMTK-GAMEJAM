using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAudio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private AudioClip[] m_Clip;
    private AudioSource m_Source;
    void Start()
    {
        m_Source = GetComponent<AudioSource>(); 
        m_Source.clip = m_Clip[Random.Range(0, m_Clip.Length)];
        m_Source.pitch = Random.Range(m_Source.pitch - 0.4f, m_Source.pitch + 0.2f);
        m_Source.volume = Random.Range(m_Source.volume - 0.02f, m_Source.volume + 0.02f);
        m_Source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        m_Source.Stop();
    }
}
