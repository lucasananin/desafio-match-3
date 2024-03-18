using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource _musicSource = null;
        [SerializeField] AudioSource _sfxSource = null;

        private void OnEnable()
        {
            AudioDataSO.onPlayMusic += PlayMusic;
            AudioDataSO.onPlaySfx += PlaySfx;
        }

        private void PlayMusic(AudioClip _audioClip)
        {
            _musicSource.clip = _audioClip;
            _musicSource.Play();
        }

        private void PlaySfx(AudioClip _audioClip)
        {
            _sfxSource.PlayOneShot(_audioClip);
        }
    }
}
