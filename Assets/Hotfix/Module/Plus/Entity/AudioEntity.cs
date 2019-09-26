using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class AudioEntityAwakeSystem : AwakeSystem<AudioEntity>
	{
		public override void Awake(AudioEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class AudioEntityStartSystem : StartSystem<AudioEntity>
	{
		public override void Start(AudioEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class AudioEntityUpdateSystem : UpdateSystem<AudioEntity>
	{
		public override void Update(AudioEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class AudioEntityDestroySystem : DestroySystem<AudioEntity>
	{
		public override void Destroy(AudioEntity self)
		{
			self.OnDestroy();
		}
	}

	public class AudioEntity : Entity
	{
		private string mBGMName = "";
		private float volumn = 1f;

		private AudioSource mAudioSource;

		public void Awake()
		{
			mAudioSource = GameObject.AddComponent<AudioSource>();
			mAudioSource.loop = true;
		}

		public void Start()
		{

		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		/// <summary>
		/// 播放一段声音
		/// </summary>
		/// <param name="soundName">声音名</param>
		public void PlaySound(string soundName, float volumn = 1f)
		{
			mAudioSource.PlayOneShot(ResourceUtil.Load<AudioClip>(soundName), volumn * this.volumn);
			ResourceUtil.Unload(soundName);
		}

		/// <summary>
		/// 设置总音量
		/// </summary>
		/// <param name="volumn">音量</param>
		public void SetVolumn(float volumn)
		{
			this.volumn = volumn;
			mAudioSource.volume = volumn;
		}

		/// <summary>
		/// 设置背景音乐
		/// </summary>
		/// <param name="bgmName">音乐名</param>
		public void SetBGM(string bgmName)
		{
			if (bgmName == mBGMName) return;
			if (string.IsNullOrEmpty(mBGMName) == false)
			{
				ResourceUtil.Unload(mBGMName);
			}

			mAudioSource.clip = ResourceUtil.Load<AudioClip>(bgmName);
			mAudioSource.Play();
		}
	}
}
