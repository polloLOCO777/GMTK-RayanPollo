using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions 
{
    public enum ResponseType { DontPlay, PlayOneShot, Overwrite }
    // To Do : Turn into extension method of the AudioSource class
    /// <summary>
    ///     Plays an audio source, or audio source one shot, based on various parameters.
    /// </summary>
    /// <param name="audioSource"> The audio source to play. </param>
    /// <param name="onAlreadyPlaying"> Response if audio source is already playing. </param>
    /// <param name="delay"> Delay to play audio in seconds. </param>
    public static void Play(this AudioSource audioSource, ResponseType onAlreadyPlaying, float delay = 0, Func<bool> canPlay = null, Action onPlay = null)
    {
        if (delay != 0)
        {
            AudioManager.Instance.StartCoroutine(Delay());
            return;
        }

        if (canPlay == null || !canPlay())
            return;

        if (audioSource.isPlaying)
        {
            switch (onAlreadyPlaying)
            {
                // Stop sound from playing
                case ResponseType.DontPlay:
                return;

                // Oneshot
                case ResponseType.PlayOneShot:
                    onPlay?.Invoke();
                    audioSource.PlayOneShot(audioSource.clip);
                return;

                // Let sound overwrite
                case ResponseType.Overwrite:
                break;
            }
        }

        onPlay?.Invoke();
        audioSource.Play();

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);

            Play(audioSource, onAlreadyPlaying, 0, canPlay, onPlay);
        }
    }
}
