using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))] // AudioSource타입의 Component가 필요한다.
public class SoundManager : MonoBehaviour
{
    public float soundVolume = 1.0f;
    public bool isSoundMute = false;
    public Slider sl;
    public Toggle tg;
    public GameObject Sound;
    public GameObject PlaySoundBtn;

    private AudioSource audio;

    private void Awake()
    {
        //이 오브젝트는 씬 전환시 사라지지 않음
        DontDestroyOnLoad(this.gameObject);
        audio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        soundVolume = sl.value;
        isSoundMute = tg.isOn;
        PlaySoundBtn.SetActive(true);
        AudioSet();
    }

    public void SetSound()
    {
        soundVolume = sl.value;
        isSoundMute = tg.isOn;
        AudioSet();
    }

    void AudioSet()
    {
        audio.volume = soundVolume; 
        audio.mute = isSoundMute;
    }

    public void SoundUiOpen()
    {
        Sound.SetActive(true);
        PlaySoundBtn.SetActive(false);
    }

    public void SoundUiClose()
    {
        Sound.SetActive(false);
        PlaySoundBtn.SetActive(true);
    }
}
