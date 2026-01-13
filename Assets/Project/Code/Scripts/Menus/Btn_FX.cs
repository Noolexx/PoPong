using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using random = UnityEngine.Random;

[RequireComponent(typeof(EventTrigger), typeof(AudioSource))]
public class Btn_FX : MonoBehaviour
{
    [Header("Hover")]
    [SerializeField] private AudioClip hover_SFX;
    [SerializeField] private List<AudioClip> hoverSFX_List;
    public Action onHoverComplete;

    [Header("Click")]
    [SerializeField] private AudioClip click_SFX;
    [SerializeField] private List<AudioClip> clickSFX_List;
    public Action onClickComplete;

    private AudioSource audioSource;


    private void Awake()
    {
        audioSource ??= GetComponent<AudioSource>();
    }


    public void Clicked(Action onComplete)
    {
        if(clickSFX_List.Count > 1)
        {
            PlayRandomSound(clickSFX_List);
            return;
        }

        StartCoroutine(PlaySound(click_SFX, onComplete));
        //audioSource?.PlayOneShot(click_SFX);
    }


    public void Hover()
    {
        if (hoverSFX_List.Count > 1)
        {
            PlayRandomSound(hoverSFX_List);
            return;
        }

        audioSource?.PlayOneShot(hover_SFX);
    }


    private IEnumerator PlaySound(AudioClip clip, Action onComplete)
    {
        audioSource?.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        onComplete?.Invoke();
    }


    private void PlayRandomSound(List<AudioClip> clips)
    {
        AudioClip randomSFX = clips[random.Range(0, clips.Count)];
        StartCoroutine(PlaySound(randomSFX, onHoverComplete));
        //audioSource.PlayOneShot(randomSFX);
    }
}
