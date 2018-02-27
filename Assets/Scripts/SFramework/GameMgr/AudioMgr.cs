using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// 音乐管理
    /// 控制游戏中播放BGM的GameObject
    /// 控制游戏中的音效大小
    /// 要求BGM都放在Music目录下
    /// </summary>
    public class AudioMgr:IGameMgr
    {
        private SettingData SettingSaveData { get; set; }    // 只用于获取数据，不进行写入
        private List<string> _musicPathList;
        private static GameObject gameObject;
        private AudioSource musicAudioSource;
        private List<AudioSource> soundAudioSources;    // 管理所有音效

        public AudioMgr(GameMainProgram gameMain) : base(gameMain)
        {
            _musicPathList = new List<string>();
            soundAudioSources = new List<AudioSource>();
        }

        public override void Awake()
        {
            SettingSaveData = gameMain.gameDataMgr.SettingSaveData;

            if (gameObject == null)
            {
                gameObject = new GameObject("AudioMgr");
                musicAudioSource = gameObject.AddComponent<AudioSource>();
                musicAudioSource.loop = true;
                musicAudioSource.playOnAwake = false;
                musicAudioSource.volume = SettingSaveData.MusicVolume / 100.0f;    // /100
                Object.DontDestroyOnLoad(gameObject);
            }
            // 添加音乐
            _musicPathList.Add("Opening");
            _musicPathList.Add("FinalBattle");
            _musicPathList.Add("TheLastGuardian");
            _musicPathList.Add("Torch");
            _musicPathList.Add("WhiteLie");
            _musicPathList.Add("Medieval"); 
            _musicPathList.Add("HWComplex5"); 
            _musicPathList.Add("Horror");
            _musicPathList.Add("Night");
        }

        public override void Release()
        {
            soundAudioSources.Clear();
        }

        /// <summary>
        /// 每个拥有AudioSource的对象应该将其组件注册到AudioMgr
        /// </summary>
        /// <param name="sound"></param>
        public void AddSound(AudioSource sound)
        {
            if (sound == null)
                return;
            soundAudioSources.Add(sound);
            sound.volume = SettingSaveData.SoundVolume / 100.0f;    // /100
        }

        /// <summary>
        /// 同样当销毁组件时需要从AudioMgr移除
        /// </summary>
        /// <param name="sound"></param>
        public void RemoveSound(AudioSource sound)
        {
            if (sound == null)
                return;
            if(soundAudioSources.Contains(sound))
                soundAudioSources.Remove(sound);
        }

        public void PlayMusic(int index)
        {
            musicAudioSource.clip = gameMain.resourcesMgr.LoadResource<AudioClip>(@"Music\" + _musicPathList[index], false);
            musicAudioSource.Play();
        }

        public void PlayMusic(string name)
        {
            int index = _musicPathList.FindIndex(path => path == name);
            musicAudioSource.clip = gameMain.resourcesMgr.LoadResource<AudioClip>(@"Music\" + _musicPathList[index], false);
            musicAudioSource.Play();
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }

        public void ChangeMusicVolume(int volume)
        {
            SettingSaveData.MusicVolume = volume;
            musicAudioSource.volume = volume/100.0f;    // /100
        }

        public void ChangeSoundVolume(int volume)
        {
            SettingSaveData.SoundVolume = volume;
            foreach (var sound in soundAudioSources)
            {
                if (sound != null)
                    sound.volume = volume / 100.0f;    // /100
            }
        }
    }
}
