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
        /// <summary>
        /// The currently playing music.
        /// </summary>
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

        /// <summary>
        /// Will play the given sound.
        /// </summary>
        /// <param name="sound"></param>
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
