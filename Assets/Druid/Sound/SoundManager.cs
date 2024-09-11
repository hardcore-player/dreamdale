using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace Druid
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        // (1) 声音根节点的物体;
        // (2) 保证这个节点在场景切换的时候不会删除，这样就不用再初始化一次;
        // (3) 所有播放声音的生源节点，都是在这个节点下

        bool is_music_mute = false;//存放当前全局背景音乐是否静音的变量

        bool is_effect_mute = false;//存放当前音效是否静音的变量

        bool is_vibrate_mute = false;//存放当前音效是否静音的变量

        // url --> AudioSource 映射, 区分音乐，音效

        Dictionary<string, AudioSource> musics = null;//音乐表

        Dictionary<string, AudioSource> effects = null;//音效表


        private bool is_setup = false;

        public void Setup()
        {
            this.is_setup = true;
        }


        private void Start()
        {
            init();
        }

        //初始化
        public void init()

        {

            //初始化音乐表和音效表
            musics = new Dictionary<string, AudioSource>();
            effects = new Dictionary<string, AudioSource>();

            // 从本地来加载这个开关
            if (PlayerPrefs.HasKey("music_mute"))//判断is_music_mute有没有保存在本地
            {
                int value = PlayerPrefs.GetInt("music_mute");
                is_music_mute = (value == 1);//int转换bool，如果value==1，返回true，否则就是false

            }

            // 从本地来加载这个开关
            if (PlayerPrefs.HasKey("effect_mute"))//判断is_effect_mute有没有保存在本地
            {
                int value = PlayerPrefs.GetInt("effect_mute");
                is_effect_mute = (value == 1);//int转换bool，如果value==1，返回true，否则就是false
            }

        }

        //播放指定背景音乐的接口

        public async void playMusic(string url, bool is_loop = true)
        {
            if (this.is_setup == false)
            {
                Debug.LogWarning("playMusic is_setup false");
                return;
            }

            AudioSource audio_source = null;

            if (musics.ContainsKey(url))//判断是否已经在背景音乐表里面了
            {
                audio_source = musics[url];//是就直接赋值过去
            }
            else//不是就新建一个空节点，节点下再新建一个AudioSource组件
            {
                GameObject s = new GameObject(url);//创建一个空节点

                s.transform.parent = this.transform;//加入节点到场景中

                audio_source = s.AddComponent<AudioSource>();//空节点添加组件AudioSource

                //AudioClip clip = Resources.Load<AudioClip>(url);//代码加载一个AudioClip资源文件

                AudioClip clip = await Addressables.LoadAssetAsync<AudioClip>(url);

                audio_source.clip = clip;//设置组件的clip属性为clip
                audio_source.loop = is_loop;//设置组件循环播放
                audio_source.playOnAwake = true;//再次唤醒时播放声音
                audio_source.spatialBlend = 0.0f;//设置为2D声音

                musics.Add(url, audio_source);//加入到背景音乐字典中，下次就可以直接赋值了

                audio_source.Play();//开始播放
            }

            audio_source.mute = is_music_mute;
            audio_source.enabled = true;
            audio_source.Play();//开始播放
        }

        //停止播放指定背景音乐的接口

        public void stopMusic(string url)
        {

            AudioSource audio_source = null;
            if (!musics.ContainsKey(url))//判断是否已经在背景音乐表里面了
            {
                return;//没有这个背景音乐就直接返回
            }

            audio_source = musics[url];//有就把audio_source直接赋值过去
            audio_source.Stop();//停止播放
        }

        //停止播放所有背景音乐的接口
        public void stopAllMusic()
        {
            foreach (AudioSource s in musics.Values)
            {
                s.Stop();
            }
        }

        //删除指定背景音乐和它的节点
        public void clearMusic(string url)
        {
            AudioSource audio_source = null;
            if (!musics.ContainsKey(url))//判断是否已经在背景音乐表里面了
            {
                return;//没有这个背景音乐就直接返回
            }

            audio_source = musics[url];//有就把audio_source直接赋值过去
            musics[url] = null;//指定audio_source组件清空
            GameObject.Destroy(audio_source.gameObject);//删除掉挂载指定audio_source组件的节点
        }

        //切换背景音乐静音开关
        public void switchMusic()
        {
            // 切换静音和有声音的状态
            is_music_mute = !is_music_mute;

            //把当前是否静音写入本地
            int value = (is_music_mute) ? 1 : 0;//bool转换int
            PlayerPrefs.SetInt("music_mute", value);

            // 遍历所有背景音乐的AudioSource元素
            foreach (AudioSource s in musics.Values)
            {
                s.mute = is_music_mute;//设置为当前的状态
            }
        }

        //切换背景音乐静音开关
        public void switchVibrate()
        {
            // 切换静音和有声音的状态
            is_vibrate_mute = !is_vibrate_mute;

            //把当前是否静音写入本地
            int value = (is_vibrate_mute) ? 1 : 0;//bool转换int
            PlayerPrefs.SetInt("is_vibrate_mute", value);
        }

        //当我的界面的静音按钮要显示的时候，到底是显示关闭，还是开始状态;
        public bool musicIsOff()
        {
            return is_music_mute;
        }

        public bool vibrateIsOff()
        {
            return is_vibrate_mute;
        }

        //接下来开始是音效的接口

        public void playBtnSound()
        {
            this.playEffect(AddressbalePathEnum.WAV_ui_click);
        }

        //播放指定音效的接口
        public async void playEffect(string url, bool is_loop = false)
        {
            if (this.is_setup == false)
            {
                Debug.LogWarning("playMusic is_setup false");
                return;
            }

            AudioSource audio_source = null;
            if (effects.ContainsKey(url))//判断是否已经在音效表里面了
            {
                audio_source = effects[url];//是就直接赋值过去
            }
            else//不是就新建一个空节点，节点下再新建一个AudioSource组件
            {
                GameObject s = new GameObject(url);//创建一个空节点
                s.transform.parent = this.transform;//加入节点到场景中
                audio_source = s.AddComponent<AudioSource>();//空节点添加组件AudioSource
                //AudioClip clip = Resources.Load<AudioClip>(url);//代码加载一个AudioClip资源文件
                AudioClip clip = await Addressables.LoadAssetAsync<AudioClip>(url);
                audio_source.clip = clip;//设置组件的clip属性为clip
                audio_source.loop = is_loop;//设置组件循环播放
                audio_source.playOnAwake = true;//再次唤醒时播放声音
                audio_source.spatialBlend = 0.0f;//设置为2D声音

                if (!effects.ContainsKey(url))
                {
                    effects.Add(url, audio_source);//加入到音效字典中，下次就可以直接赋值了
                }
            }

            audio_source.mute = is_effect_mute;
            audio_source.enabled = true;
            audio_source.Play();//开始播放

        }

        //停止播放指定音效的接口
        public void stop_effect(string url)
        {
            AudioSource audio_source = null;

            if (!effects.ContainsKey(url))//判断是否已经在音效表里面了
            {
                return;//没有这个背景音乐就直接返回
            }

            audio_source = effects[url];//有就把audio_source直接赋值过去
            audio_source.Stop();//停止播放

        }

        //停止播放所有音效的接口
        public void stopAllEffect()
        {
            foreach (AudioSource s in effects.Values)
            {
                s.Stop();
            }

        }

        //删除指定音效和它的节点
        public void clearEffect(string url)
        {
            AudioSource audio_source = null;
            if (!effects.ContainsKey(url))//判断是否已经在音效表里面了
            {
                return;//没有这个音效就直接返回
            }

            audio_source = effects[url];//有就把audio_source直接赋值过去
            effects[url] = null;//指定audio_source组件清空

            GameObject.Destroy(audio_source.gameObject);//删除掉挂载指定audio_source组件的节点

        }

        //切换音效静音开关
        public void switchEffect()
        {
            // 切换静音和有声音的状态
            is_effect_mute = !is_effect_mute;

            //把当前是否静音写入本地
            int value = (is_effect_mute) ? 1 : 0;//bool转换int

            PlayerPrefs.SetInt("effect_mute", value);

            // 遍历所有音效的AudioSource元素
            foreach (AudioSource s in effects.Values)
            {
                s.mute = is_effect_mute;//设置为当前的状态
            }

        }

        //当我的界面的静音按钮要显示的时候，到底是显示关闭，还是开始状态;
        public bool effectIsOff()
        {
            return is_effect_mute;
        }

        //播放3D的音效
        public void playEffect3D(string url, Vector3 pos, bool is_loop = false)
        {
            AudioSource audio_source = null;
            if (effects.ContainsKey(url))
            {
                audio_source = effects[url];
            }
            else
            {
                GameObject s = new GameObject(url);
                s.transform.parent = this.transform;
                s.transform.position = pos;//3D音效的位置
                audio_source = s.AddComponent<AudioSource>();

                AudioClip clip = Resources.Load<AudioClip>(url);

                audio_source.clip = clip;
                audio_source.loop = is_loop;
                audio_source.playOnAwake = true;
                audio_source.spatialBlend = 1.0f; // 3D音效

                effects.Add(url, audio_source);

            }

            audio_source.mute = is_effect_mute;
            audio_source.enabled = true;
            audio_source.Play();

        }

        //优化策略接口
        public void disableOverAudio()
        {
            //遍历背景音乐表
            foreach (AudioSource s in musics.Values)
            {
                if (!s.isPlaying)//判断是否在播放
                {
                    s.enabled = false;//不在播放就直接隐藏
                }
            }

            //遍历音效表
            foreach (AudioSource s in effects.Values)
            {
                if (!s.isPlaying)//判断是否在播放
                {
                    s.enabled = false;//不在播放就直接隐藏
                }
            }
        }
    }
}