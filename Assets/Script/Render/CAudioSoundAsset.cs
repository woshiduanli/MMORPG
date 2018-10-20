using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CAudioSoundAsset : ZRender.IRenderObject
{
    //private GameSetSystem SetSystem;
    private AudioSource source;
    //public CAudioSoundAsset(GameSetSystem set)
    //{
    //    this.SetSystem = set;
    //}

    protected override void OnCreate()
    {
        //JobDBModel.Instance.GetList();
        AudioClip clip = GetOwner().GetAsset() as AudioClip;

        this.gameObject = new GameObject(clip.name);
        CategorySettings.Attach(gameObject.transform, "_sound/");
        this.gameObject.tag = "uiSound";

        source = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        source.clip = clip;
        source.loop = false;
        source.playOnAwake = false;
        source.spatialBlend = 0;
        source.dopplerLevel = 0;
        source.maxDistance = 100;
        source.rolloffMode = AudioRolloffMode.Linear;
        //source.volume = this.SetSystem.Volume;
    }

    public void Play()
    {
        //if (source && this.SetSystem.Audio && !source.isPlaying)
        //    source.Play();
    }
    protected override void OnDestroy()
    {
        if (this.gameObject)
        {
            UnityEngine.Object.DestroyImmediate(this.gameObject, true);
            this.gameObject = null;
        }
    }
}