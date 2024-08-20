using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions 
{

    public enum AlreadyPlayingResponse { Default, DontPlay, PlayOneShot, Overwrite }

    // To Do : Turn into extension method of the AudioSource class
    /// <summary>
    ///     Plays an audio source, or audio source one shot, based on various parameters.
    /// </summary>
    /// <param name="audioSource"> The audio source to play. </param>
    /// <param name="onAlreadyPlaying"> Response if audio source is already playing. </param>
    /// <param name="delay"> Time to wait before checking to play audio. </param>
    public static void AdvancedPlay(this AudioSource audioSource, AlreadyPlayingResponse onAlreadyPlaying = AlreadyPlayingResponse.Default, float delay = 0, Func<bool> canPlay = null, Action onPlay = null)
    {
        if (delay != 0)
        {
            AudioManager.Instance.StartCoroutine(Delay(delay));
            return;
        }

        if (canPlay != null && !canPlay())
            return;

        if (audioSource.isPlaying)
        {
            if (onAlreadyPlaying == AlreadyPlayingResponse.Default) 
                onAlreadyPlaying = AudioManager.Instance.DefaultResponseType;

            switch (onAlreadyPlaying)
            {
                // Stop sound from playing
                case AlreadyPlayingResponse.DontPlay:
                return;

                // Oneshot
                case AlreadyPlayingResponse.PlayOneShot:
                    Debug.Log($"Play one shot : {audioSource.clip}");
                    onPlay?.Invoke();
                    audioSource.PlayOneShot(audioSource.clip);
                return;

                // Let sound overwrite
                case AlreadyPlayingResponse.Overwrite:
                break;

                case AlreadyPlayingResponse.Default:
                    throw new Exception("Response type should not be set default. ");
            }
        }

        onPlay?.Invoke();
        audioSource.Play();

        IEnumerator Delay(float wait)
        {
            yield return new WaitForSeconds(wait);

            AdvancedPlay(audioSource, onAlreadyPlaying, 0, canPlay, onPlay);
        }
    }
}
