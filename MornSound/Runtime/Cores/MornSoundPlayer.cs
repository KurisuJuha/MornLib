﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace MornSound
{
    [AddComponentMenu("")]
    internal sealed class MornSoundPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private enum FadeType
        {
            None,
            FadeIn,
            FadeOut,
        }

        private FadeType _fadeType;
        private float _elapsedTime;
        private float _fadeSeconds;
        private static readonly Queue<MornSoundPlayer> s_playEndQueue = new();

        public void Init(AudioMixerGroup mixerGroup, AudioClip clip, bool isLoop, float volume, float pitch)
        {
            if (_audioSource == null)
            {
                _audioSource = TryGetComponent<AudioSource>(out var source) ? source : gameObject.AddComponent<AudioSource>();
            }

            _audioSource.outputAudioMixerGroup = mixerGroup;
            _audioSource.volume = volume;
            _audioSource.clip = clip;
            _audioSource.loop = isLoop;
            _audioSource.pitch = pitch;
            _audioSource.Play();
        }

        private void Update()
        {
            if (_fadeType != FadeType.None)
            {
                var start = _fadeType == FadeType.FadeIn ? 0f : 1f;
                var end = _fadeType == FadeType.FadeIn ? 1f : 0f;
                if (_elapsedTime < _fadeSeconds)
                {
                    _elapsedTime += Time.deltaTime;
                    _audioSource.volume = Mathf.Lerp(start, end, Mathf.Clamp01(_elapsedTime / _fadeSeconds));
                }
                else
                {
                    _audioSource.volume = end;
                    if (_fadeType == FadeType.FadeOut)
                    {
                        _audioSource.Stop();
                    }

                    _fadeType = FadeType.None;
                }
            }

            if (_audioSource.isPlaying)
            {
                return;
            }

            _audioSource.Stop();
            gameObject.SetActive(false);
            s_playEndQueue.Enqueue(this);
        }

        public void FadeIn(float seconds)
        {
            _fadeType = FadeType.FadeIn;
            _fadeSeconds = seconds;
            _elapsedTime = 0;
        }

        public void FadeOut(float seconds)
        {
            _fadeType = FadeType.FadeOut;
            _fadeSeconds = seconds;
            _elapsedTime = 0;
        }

        public static MornSoundPlayer GetInstance(Transform parent)
        {
            if (s_playEndQueue.TryDequeue(out var player))
            {
                player.gameObject.SetActive(true);
                player._fadeType = FadeType.None;
                return player;
            }

            player = new GameObject().AddComponent<MornSoundPlayer>();
            player.name = nameof(MornSoundPlayer);
            player.transform.SetParent(parent);
            return player;
        }
    }
}
