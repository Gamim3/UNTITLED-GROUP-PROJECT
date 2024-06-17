using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public bool enableDebug;

    public string activeMusic;
    public string activeAmbience;

    [Header("Scene Names")]
    [SerializeField] string _mainMenuScene = "MainMenu";
    [SerializeField] string _guildHallScene = "GuildHall";
    [SerializeField] string _gameScene = "Game";

    [Header("Audio Sources")]
    [SerializeField] AudioSource _masterAudioSource;
    [SerializeField] AudioSource _musicAudioSource;
    [SerializeField] AudioSource _sfxAudioSource;
    [SerializeField] AudioSource _ambienceAudioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip _guildHallAmbience;
    [SerializeField] AudioClip _gameAmbience;

    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _guildhallMusic;
    [SerializeField] AudioClip _gameMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;


    }

    private void OnSceneChanged(Scene idk, Scene newScene)
    {
        if (enableDebug)
            Debug.Log($"AudioManager: Switching Audio To {newScene.name}");

        if (newScene.name == _mainMenuScene && _ambienceAudioSource.clip != _guildHallAmbience)
        {
            if (enableDebug)
                Debug.Log($"AudioManager: Switched Audio To MainMenu Music");
            if (_guildHallAmbience != null)
                _ambienceAudioSource.clip = _guildHallAmbience;
            if (_menuMusic != null)
                _musicAudioSource.clip = _menuMusic;
        }
        else if (newScene.name == _guildHallScene && _ambienceAudioSource.clip != _guildHallAmbience)
        {
            if (enableDebug)
                Debug.Log($"AudioManager: Switched Audio To GuildHall Music");
            if (_guildHallAmbience != null)
                _ambienceAudioSource.clip = _guildHallAmbience;
            if (_guildhallMusic != null)
                _musicAudioSource.clip = _guildhallMusic;
        }
        else if (newScene.name == _gameScene)
        {
            if (enableDebug)
                Debug.Log($"AudioManager: Switched Audio To Game Music");
            if (_gameAmbience != null)
                _ambienceAudioSource.clip = _gameAmbience;
            if (_gameMusic != null)
                _musicAudioSource.clip = _gameMusic;
        }
        if (_ambienceAudioSource.clip != null)
        {
            activeAmbience = _ambienceAudioSource.clip.name;
            _ambienceAudioSource.Play();
        }
        if (_musicAudioSource.clip != null)
        {
            activeMusic = _musicAudioSource.clip.name;
            _musicAudioSource.Play();
        }

    }
}
