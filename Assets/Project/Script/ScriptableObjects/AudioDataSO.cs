using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Audio_", menuName = "Gameplay/Audio Data")]
    public class AudioDataSO : ScriptableObject
    {
        [SerializeField] AudioClip _audioClip = null;

        public static event Action<AudioClip> onPlayMusic = null;
        public static event Action<AudioClip> onPlaySfx = null;

        public void PlayAsMusic()
        {
            onPlayMusic?.Invoke(_audioClip);
        }

        public void PlayAsSfx()
        {
            onPlaySfx?.Invoke(_audioClip);
        }
    }
}
