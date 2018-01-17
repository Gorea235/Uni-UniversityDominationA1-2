using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        #region Public Properties

        AudioMusic _audioMusic = AudioMusic.None;
        public AudioMusic CurrentMusic
        {
            get { return _audioMusic; }
            set
            {

                _audioMusic = value;
            }
        }

        #endregion

        #region Public Function

        public void PlaySound(AudioSound sound)
        {

        }

        #endregion

        #region MonoBehaviour

        void Start()
        {

        }
        
        void Update()
        {

        }

        #endregion
    }
}
