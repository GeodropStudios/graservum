﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Toggle soundEffectsToggle;
    [SerializeField]
    private Toggle musicToggle;
#pragma warning restore

    private void Start() {
        if (!PlayerPrefs.HasKey("PlaySoundEffects")) PlayerPrefs.SetInt("PlaySoundEffects", 1);
        if (!PlayerPrefs.HasKey("PlayMusic")) PlayerPrefs.SetInt("PlayMusic", 1);

        if (PlayerPrefs.GetInt("PlaySoundEffects") == 0) soundEffectsToggle.isOn = false;
        else soundEffectsToggle.isOn = true;

        if (PlayerPrefs.GetInt("PlayMusic") == 0) musicToggle.isOn = false;
        else musicToggle.isOn = true;

        // TODO play menu music
    }

    public void StartGame() {
        SceneManager.LoadSceneAsync("MainGameScene");
    }

    public void ShowMainMenu() {
        animator.SetTrigger("ShowMain");
    }

    public void ShowInstructionsMenu() {
        animator.SetTrigger("ShowInstructions");
    }

    public void ShowSettingsMenu() {
        animator.SetTrigger("ShowSettings");
    }

    public void OnSoundEffectsValueChange() {
        if (soundEffectsToggle.isOn) {
            PlayerPrefs.SetInt("PlaySoundEffects", 1);
        } else {
            PlayerPrefs.SetInt("PlaySoundEffects", 0);
            AudioManager.instance.StopSoundEffects();
        }
    }

    public void OnMusicValueChange() {
        if (soundEffectsToggle.isOn) {
            PlayerPrefs.SetInt("PlayMusic", 1);
            // TODO play menu music
        } else {
            PlayerPrefs.SetInt("PlayMusic", 0);
            AudioManager.instance.StopMusic();
        }
    }
}
