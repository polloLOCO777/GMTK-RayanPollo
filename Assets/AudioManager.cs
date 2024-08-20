using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioSourceExtensions;

public class AudioManager : Singleton<AudioManager>
{
    enum ListSelectionType { Sequential, Random }

    [Header("Player")]
    [SerializeField] List<AudioSource> walkCycle;
    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource land;

    [Header("Player Sounds Properties")]
    [SerializeField] ListSelectionType stepShuffleType;
    [SerializeField] AlreadyPlayingResponse stepAlreadyPlayingResponse;
    [SerializeField] AlreadyPlayingResponse jumpAlreadyPlayingResponse;
    [SerializeField] float timeBetweenSteps;

    [Header("UI")]
    [SerializeField] AudioSource select;
    [SerializeField] AudioSource back;

    [Header("Game State")]
    [SerializeField] AudioSource startLevel;
    [SerializeField] AudioSource finishLevel;

    [Header("Music")]
    [SerializeField] AudioSource gameMusic;    

    [Header("Black Hole")]
    [SerializeField] AudioSource blackHoleGrow;

    [Header("Black Hole Properties")]
    [SerializeField] AlreadyPlayingResponse growAlreadyPlayingResponse;

    [Header("Tile Proxy")]
    [SerializeField] AudioSource tugTile;
    [SerializeField] AudioSource tileDisappear;

    [Header("Tile Sound Properties")]
    [SerializeField] AlreadyPlayingResponse tugAlreadyPlayingResponse;
    [SerializeField] AlreadyPlayingResponse disappearAlreadyPlayingResponse;

    [field: Header("Default Settings")]
    [field: SerializeField] public AlreadyPlayingResponse DefaultResponseType { get; private set; }

    int stepIndex;
    float stepTimer;

    enum PlayerSounds { Step, Jump }
    enum UISounds { Select, Back }
    enum GameState { StartLevel, FinishLevel }

    private void OnEnable()
    {
        Player.OnPlayerActionEventHandler += HandlePlayerAction;
        BlockGone.OnProxyActionEventHandler += HandleProxyAction;
        BlackHole.OnGrowEventHandler += HandleBlackHoleGrow;
    }

    private void OnDisable()
    {
        Player.OnPlayerActionEventHandler -= HandlePlayerAction;
        BlockGone.OnProxyActionEventHandler -= HandleProxyAction;
        BlackHole.OnGrowEventHandler -= HandleBlackHoleGrow;
    }

    private void Start()
    {
        startLevel.AdvancedPlay();
        gameMusic.AdvancedPlay();
    }

    private void Update()
    {
        stepTimer += Time.deltaTime;
    }

    void HandleProxyAction(object sender, BlockGone.ProxyEventArgs e)
    {
        switch (e.action)
        {
            case BlockGone.ProxyEventArgs.ActionType.ShrinkStart:
                tugTile.AdvancedPlay(tugAlreadyPlayingResponse);
            break;

            case BlockGone.ProxyEventArgs.ActionType.Disappear:
                tileDisappear.AdvancedPlay(disappearAlreadyPlayingResponse);
            break;
        }
    }

    void HandleBlackHoleGrow(object sender, EventArgs e)
    {
        blackHoleGrow.AdvancedPlay(growAlreadyPlayingResponse);
    }

    void HandlePlayerAction(object sender, Player.PlayerActionEventArgs e)
    {
        switch (e.action)
        {
            case Player.PlayerActionEventArgs.ActionType.Step:
                PlaySourceFromList(walkCycle, stepShuffleType, ref stepIndex, stepAlreadyPlayingResponse, canPlay: () => stepTimer > timeBetweenSteps, onPlay: () => stepTimer = 0);
            break;

            case Player.PlayerActionEventArgs.ActionType.Jump:
                jump.AdvancedPlay(jumpAlreadyPlayingResponse);
            break;
        }
    }

    void HandleUIAction(object sender) 
    {
        /* switch (uiSounds)
        {
            case UISounds.Select:
                select.Play();
            break;

            case UISounds.Back:
                back.Play();
            break;
        } */
    }

    void HandleSceneAction(object sender)
    {
        /* switch (gameSound)
        {
            case GameState.StartLevel:
                startLevel.Play();
                break;

            case GameState.FinishLevel:
                finishLevel.Play();
                break;
        } */
    }

    /// <summary>
    /// Slects one of multiple audio sources to play
    /// </summary>
    /// <param name="audioSources"> Sources to slect from. </param>
    /// <param name="selectionType"> Method for selecting a source. </param>
    /// <param name="currentIndex"> Last used index for sequential selection. </param>
    /// <param name="canPlay"> Func used to check if we should play sound. </param>
    /// <param name="onAlreadyPlaying"> Method if source is already playing. </param>
    /// <param name="delay"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    void PlaySourceFromList(List<AudioSource> audioSources, ListSelectionType selectionType, ref int currentIndex, AlreadyPlayingResponse onAlreadyPlaying = AlreadyPlayingResponse.Default, float delay = 0, Func<bool> canPlay = null, Action onPlay = null)
    {
        currentIndex = selectionType switch
        {
            ListSelectionType.Random => UnityEngine.Random.Range(0, audioSources.Count),
            ListSelectionType.Sequential => ((currentIndex + 1) % audioSources.Count),
            _ => throw new NotImplementedException(),
        };

        audioSources[currentIndex].AdvancedPlay(onAlreadyPlaying, delay, canPlay, onPlay);
    }
}