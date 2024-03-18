using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] AudioDataSO _audioDataSO = null;

        public void Play()
        {
            _audioDataSO.PlayAsSfx();
        }
    }
}
