using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Player")]
    [SerializeField] List<AudioSource> walkCycle;
    [SerializeField] AudioSource jump;

    [Header("UI")]
    [SerializeField] AudioSource select;
    [SerializeField] AudioSource back;

    [Header("Game State")]
    [SerializeField] AudioSource startLevel;
    [SerializeField] AudioSource finishLevel;

    [Header("Music")]
    [SerializeField] AudioSource gameMusic;

    [Header("Sound Properties")]
    [SerializeField] ShuffleType stepShuffleType;
    [SerializeField] float timeBetweenSteps;
    float stepTimer;

    public enum ShuffleType { Random, Sequential }

    int stepIndex;

    enum PlayerSounds { Step, Jump }
    enum UISounds { Select, Back }
    enum GameState { StartLevel, FinishLevel }

    private void OnEnable()
    {
        Player.OnPlayerActionEventHandler += HandlePlayerAction;
    }

    private void OnDisable()
    {
        Player.OnPlayerActionEventHandler -= HandlePlayerAction;
    }

    private void Start()
    {
        startLevel.Play();
        gameMusic.Play();
    }

    private void Update()
    {
        stepTimer += Time.deltaTime;
    }

    void HandlePlayerAction(object sender, Player.PlayerActionEventArgs e)
    {
        switch (e.action)
        {
            case Player.PlayerActionEventArgs.ActionType.Step:
                PlaySound(PlayerSounds.Step);
            break;

            case Player.PlayerActionEventArgs.ActionType.Jump:
                PlaySound(PlayerSounds.Jump);
            break;
        }
    }

    void HandleUIAction(object sender)
    {

    }

    void HandleGameAction(object sender)
    {

    }

    void PlaySound(UISounds uiSounds)
    {
        switch (uiSounds)
        {
            case UISounds.Select:
                select.Play();
            break;

            case UISounds.Back:
                back.Play();
            break;
        }
    }

    void PlaySound(GameState gameSound)
    {
        switch (gameSound)
        {
            case GameState.StartLevel:
                startLevel.Play();
            break;

            case GameState.FinishLevel:
                finishLevel.Play();
            break;
        }
    }

    void PlaySound(PlayerSounds playerSound)
    {
        switch (playerSound)
        {
            case PlayerSounds.Step:
                if (stepTimer < timeBetweenSteps)
                    return;

                stepTimer = 0;
                stepIndex = stepShuffleType switch
                {
                    ShuffleType.Random => Random.Range(0, walkCycle.Count),
                    ShuffleType.Sequential => ((stepIndex + 1) % walkCycle.Count), // 0 / 2 = 0 | 1 / 2 = 1 | 2 / 2 = 0 | 3 / 2 = 1
                    _ => throw new System.NotImplementedException(),
                };
                Debug.Log($"{(stepIndex % walkCycle.Count)}");
                walkCycle[stepIndex].Play();
            break;

            case PlayerSounds.Jump:
                jump.Play();
            break;
        }
    }
}