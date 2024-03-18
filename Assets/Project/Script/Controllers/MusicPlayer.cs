using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] AudioDataSO _audioDataSO = null;
        [SerializeField] bool _playOnStart = true;

        private void Start()
        {
            if (_playOnStart)
            {
                Play();
            }
        }

        private void Play()
        {
            _audioDataSO.PlayAsMusic();
        }
    }
}
