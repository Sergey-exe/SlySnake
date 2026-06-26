using System;
using System.Collections.Generic;
using YG;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems; // Обязательно для отслеживания отжатия
using UnityEngine.UI;

public class AudioSetup : MonoBehaviour
{
    public enum MixerParameter { MasterVolume, MusicVolume, SoundsVolume, UIVolume }

    [Serializable]
    public struct AudioChannel
    {
        public MixerParameter ParameterType; 
        public Slider Slider;        
        public Toggle MuteToggle;    
    }

    [Header("Audio Configuration")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private List<AudioChannel> _channels;

    private bool _isLoading = false;

    private void Start()
    {
        SubscribeToUI();
        LoadSaves();
    }

    private void SubscribeToUI()
    {
        for (int i = 0; i < _channels.Count; i++)
        {
            AudioChannel channel = _channels[i];
            if (channel.Slider == null || channel.MuteToggle == null) continue;

            AudioChannel currentChannel = channel;

            // Громкость в микшере меняем мгновенно при движении ползунка (для приятного фидбека)
            channel.Slider.onValueChanged.AddListener((volume) => UpdateMixer(currentChannel));
            
            // А вот сохраняем ТОЛЬКО когда игрок отпустил ползунок
            EventTrigger trigger = channel.Slider.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) trigger = channel.Slider.gameObject.AddComponent<EventTrigger>();
            
            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((data) => OnSliderReleased(currentChannel));
            trigger.triggers.Add(pointerUpEntry);

            // Для Toggle (галочек) оставляем мгновенное сохранение, так как там один клик
            channel.MuteToggle.onValueChanged.AddListener((isMuted) => OnToggleChanged(currentChannel));
        }
    }

    private void OnToggleChanged(AudioChannel channel)
    {
        if (_isLoading) return;
        UpdateMixer(channel);
        Save(channel.ParameterType, channel.Slider.value, channel.MuteToggle.isOn);
    }

    private void OnSliderReleased(AudioChannel channel)
    {
        if (_isLoading) return;
        // Сохраняем финальное значение, когда палец/курсор отпущен
        Save(channel.ParameterType, channel.Slider.value, channel.MuteToggle.isOn);
    }

    private void UpdateMixer(AudioChannel channel)
    {
        string parameterName = channel.ParameterType.ToString();
        if (channel.MuteToggle.isOn)
        {
            _audioMixer.SetFloat(parameterName, -80f);
        }
        else
        {
            float clampedVolume = Mathf.Clamp(channel.Slider.value, 0.0001f, 1f);
            _audioMixer.SetFloat(parameterName, Mathf.Log10(clampedVolume) * 20f);
        }
    }

    public void LoadSaves()
    {
        _isLoading = true;

        for (int i = 0; i < _channels.Count; i++)
        {
            AudioChannel channel = _channels[i];
            if (channel.Slider == null || channel.MuteToggle == null) continue;

            switch (channel.ParameterType)
            {
                case MixerParameter.MasterVolume:
                    channel.Slider.value = YG2.saves.MasterVolume;
                    channel.MuteToggle.isOn = YG2.saves.MasterMute;
                    break;
                case MixerParameter.MusicVolume:
                    channel.Slider.value = YG2.saves.MusicVolume;
                    channel.MuteToggle.isOn = YG2.saves.MusicMute;
                    break;
                case MixerParameter.SoundsVolume:
                    channel.Slider.value = YG2.saves.SoundsVolume;
                    channel.MuteToggle.isOn = YG2.saves.SoundsMute;
                    break;
                case MixerParameter.UIVolume:
                    channel.Slider.value = YG2.saves.UIVolume;
                    channel.MuteToggle.isOn = YG2.saves.UIVomute;
                    break;
            }

            UpdateMixer(channel);
        }

        _isLoading = false;
    }

    public void Save(MixerParameter parameterType, float volume, bool isMuted)
    {
        switch (parameterType)
        {
            case MixerParameter.MasterVolume:
                YG2.saves.MasterVolume = volume;
                YG2.saves.MasterMute = isMuted;
                break;
            case MixerParameter.MusicVolume:
                YG2.saves.MusicVolume = volume;
                YG2.saves.MusicMute = isMuted;
                break;
            case MixerParameter.SoundsVolume:
                YG2.saves.SoundsVolume = volume;
                YG2.saves.SoundsMute = isMuted;
                break;
            case MixerParameter.UIVolume:
                YG2.saves.UIVolume = volume;
                YG2.saves.UIVomute = isMuted;
                break;
        }

        YG2.SaveProgress();
    }
}
