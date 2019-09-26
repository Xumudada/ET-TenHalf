using ETModel;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using KBEngine;
using System;

namespace ETHotfix
{
	[ObjectSystem]
	public class LoginPanelEntityAwakeSystem : AwakeSystem<LoginPanelEntity>
	{
		public override void Awake(LoginPanelEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class LoginPanelEntityStartSystem : StartSystem<LoginPanelEntity>
	{
		public override void Start(LoginPanelEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class LoginPanelEntityUpdateSystem : UpdateSystem<LoginPanelEntity>
	{
		public override void Update(LoginPanelEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class LoginPanelEntityDestroySystem : DestroySystem<LoginPanelEntity>
	{
		public override void Destroy(LoginPanelEntity self)
		{
			self.Deregister();
			self.OnDestroy();
		}
	}

	public class LoginPanelEntity : Entity
	{
		private bool mIsLogin = false;
		public bool IsLogin
		{
			get { return mIsLogin; }
			set
			{
				if (mIsLogin != value)
				{
					mIsLogin = value;

					// 显示Login
					if (mIsLogin == true)
					{
						mLoginBox.DOScale(1f, 0.5f).SetEase(Ease.InBack);
						mRegisterBox.DOScale(0f, 0.5f).SetEase(Ease.OutBack);

						mLoginBox.DOLocalMove(Vector3.zero, 0.5f);
						mRegisterBox.DOLocalMove(new Vector3(Screen.width / 2f, -Screen.height / 2f, 0f), 0.5f);
					}
					else
					{
						mRegisterBox.DOScale(1f, 0.5f).SetEase(Ease.InBack);
						mLoginBox.DOScale(0f, 0.5f).SetEase(Ease.OutBack);

						mRegisterBox.DOLocalMove(Vector3.zero, 0.5f);
						mLoginBox.DOLocalMove(new Vector3(-Screen.width / 2f, -Screen.height / 2f, 0f), 0.5f);
					}
				}
			}
		}

		private ReferenceCollector mCollector;
		private RectTransform mLoginBox;
		private RectTransform mRegisterBox;
		private Button mToLoginButton;
		private Button mToRegisterButton;
		private Button mLoginButton;
		private Button mRegisterButton;
		private InputField mLoginUsernameInput;
		private InputField mLoginPasswordInput;
		private InputField mRegisterUsernameInput;
		private InputField mRegisterPasswordInput;

		public void Awake()
		{
			mCollector = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			mLoginBox = mCollector.Get<GameObject>("LoginBox").GetComponent<RectTransform>();
			mRegisterBox = mCollector.Get<GameObject>("RegisterBox").GetComponent<RectTransform>();
			mToLoginButton = mCollector.Get<GameObject>("ToLoginButton").GetComponent<Button>();
			mToRegisterButton = mCollector.Get<GameObject>("ToRegisterButton").GetComponent<Button>();
			mLoginButton = mCollector.Get<GameObject>("LoginButton").GetComponent<Button>();
			mRegisterButton = mCollector.Get<GameObject>("RegisterButton").GetComponent<Button>();
			mLoginUsernameInput = mCollector.Get<GameObject>("LoginUsernameInput").GetComponent<InputField>();
			mLoginPasswordInput = mCollector.Get<GameObject>("LoginPasswordInput").GetComponent<InputField>();
			mRegisterUsernameInput = mCollector.Get<GameObject>("RegisterUsernameInput").GetComponent<InputField>();
			mRegisterPasswordInput = mCollector.Get<GameObject>("RegisterPasswordInput").GetComponent<InputField>();
		}

		public void Start()
		{
			#region Event

			this.Register(EventKey.OnLoginSuccessfully, OnEvent);
			this.Register(EventKey.onCreateAccountResult, OnEvent);

			#endregion

			#region UI

			mToLoginButton.onClick.Add(() =>
			{
				IsLogin = true;
			});

			mToRegisterButton.onClick.Add(() =>
			{
				IsLogin = false;
			});

			mLoginButton.onClick.Add(() =>
			{
				string username = mLoginUsernameInput.text.Trim();
				string password = mLoginPasswordInput.text.Trim();

				if (username.Length == 0 || password.Length == 0)
				{
					TipPanelEntity.ShowTip("登录失败：用户名或密码为空！");
					return;
				}

				KBEngine.Event.fireIn(EventInTypes.login, username, password, System.Text.Encoding.UTF8.GetBytes(Application.platform.ToString()));
			});

			mRegisterButton.onClick.Add(() =>
			{
				string username = mRegisterUsernameInput.text.Trim();
				string password = mRegisterPasswordInput.text.Trim();

				if (username.Length == 0 || password.Length == 0)
				{
					TipPanelEntity.ShowTip("注册失败：用户名或密码为空！");
					return;
				}

				KBEngine.Event.fireIn(EventInTypes.createAccount, username, password, System.Text.Encoding.UTF8.GetBytes(Application.platform.ToString()));
			});

			#endregion

			IsLogin = true;
		}

		public void Update()
		{

		}




		public void OnDestroy()
		{

		}

		public void OnEvent(EventKey key, object[] args)
		{
			if (key == EventKey.OnLoginSuccessfully)
			{
				Debug.Log($"登录成功");
				Game.EventSystem.Run(EventIdType.LoadSceneLobby);
			}
			else if (key == EventKey.onCreateAccountResult)
			{
				UInt16 retcode = (UInt16)args[0];
				if (retcode == 0)
				{
					TipPanelEntity.ShowTip("注册成功!");
					string username = mRegisterUsernameInput.text.Trim();
					string password = mRegisterPasswordInput.text.Trim();

					if (username.Length == 0 || password.Length == 0)
					{
						TipPanelEntity.ShowTip("注册失败：用户名或密码为空！");
						return;
					}

					KBEngine.Event.fireIn(EventInTypes.login, username, password, System.Text.Encoding.UTF8.GetBytes(Application.platform.ToString()));
				}
				else
				{
					TipPanelEntity.ShowTip("注册失败!");
				}
			}
			else if (key == EventKey.onLoginFailed)
			{
				UInt16 retcode = (UInt16)args[0];
				Debug.Log($"登录失败: {retcode}");
			}
		}
	}
}
