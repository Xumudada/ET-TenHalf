using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class LobbyPanelEntityAwakeSystem : AwakeSystem<LobbyPanelEntity>
	{
		public override void Awake(LobbyPanelEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class LobbyPanelEntityStartSystem : StartSystem<LobbyPanelEntity>
	{
		public override void Start(LobbyPanelEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class LobbyPanelEntityUpdateSystem : UpdateSystem<LobbyPanelEntity>
	{
		public override void Update(LobbyPanelEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class LobbyPanelEntityDestroySystem : DestroySystem<LobbyPanelEntity>
	{
		public override void Destroy(LobbyPanelEntity self)
		{
			self.Deregister();
			self.OnDestroy();
		}
	}

	public class LobbyPanelEntity : Entity
	{
		private ReferenceCollector mCollector;
		private GameObject mMainBox;
		private GameObject mSetNameBox;
		private GameObject mJoinRoomBox;
		private Text mPlayernameText;
		private Text mGoldText;
		private InputField mPlayernameInput;
		private InputField mRoomKeyInput;

		// Button
		private Button mSureButton;
		private Button mAddGoldButton;
		private Button mCreateRoomButton;
		private Button mJoinRoomButton;
		private Button mMatchButton;
		private Button mCloseJoinRoomButton;

		// Key
		private Button mKey0;
		private Button mKey1;
		private Button mKey2;
		private Button mKey3;
		private Button mKey4;
		private Button mKey5;
		private Button mKey6;
		private Button mKey7;
		private Button mKey8;
		private Button mKey9;
		private Button mKeyClear;
		private Button mKeyDelete;

		public void Awake()
		{
			mCollector = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

			mMainBox = mCollector.Get<GameObject>("MainBox");
			mSetNameBox = mCollector.Get<GameObject>("SetNameBox");
			mJoinRoomBox = mCollector.Get<GameObject>("JoinRoomBox");

			mPlayernameText = mCollector.Get<GameObject>("PlayernameText").GetComponent<Text>();
			mGoldText = mCollector.Get<GameObject>("GoldText").GetComponent<Text>();

			mPlayernameInput = mCollector.Get<GameObject>("PlayernameInput").GetComponent<InputField>();
			mRoomKeyInput = mCollector.Get<GameObject>("RoomKeyInput").GetComponent<InputField>();

			// Button
			mSureButton = mCollector.Get<GameObject>("SureButton").GetComponent<Button>();
			mAddGoldButton = mCollector.Get<GameObject>("AddGoldButton").GetComponent<Button>();
			mCreateRoomButton = mCollector.Get<GameObject>("CreateRoomButton").GetComponent<Button>();
			mJoinRoomButton = mCollector.Get<GameObject>("JoinRoomButton").GetComponent<Button>();
			mMatchButton = mCollector.Get<GameObject>("MatchButton").GetComponent<Button>();
			mCloseJoinRoomButton = mCollector.Get<GameObject>("CloseJoinRoomButton").GetComponent<Button>();

			// Key
			mKey0 = mCollector.Get<GameObject>("Key0").GetComponent<Button>();
			mKey1 = mCollector.Get<GameObject>("Key1").GetComponent<Button>();
			mKey2 = mCollector.Get<GameObject>("Key2").GetComponent<Button>();
			mKey3 = mCollector.Get<GameObject>("Key3").GetComponent<Button>();
			mKey4 = mCollector.Get<GameObject>("Key4").GetComponent<Button>();
			mKey5 = mCollector.Get<GameObject>("Key5").GetComponent<Button>();
			mKey6 = mCollector.Get<GameObject>("Key6").GetComponent<Button>();
			mKey7 = mCollector.Get<GameObject>("Key7").GetComponent<Button>();
			mKey8 = mCollector.Get<GameObject>("Key8").GetComponent<Button>();
			mKey9 = mCollector.Get<GameObject>("Key9").GetComponent<Button>();
			mKeyClear = mCollector.Get<GameObject>("KeyClear").GetComponent<Button>();
			mKeyDelete = mCollector.Get<GameObject>("KeyDelete").GetComponent<Button>();
		}

		public void Start()
		{
			#region Event

			this.Register(EventKey.onPlayernameBaseChanged, OnEvent);
			this.Register(EventKey.onGoldBaseChanged, OnEvent);
			this.Register(EventKey.onJoinRoom, OnEvent);

			#endregion

			#region UI

			mSureButton.onClick.Add(() =>
			{
				Constant.PlayerBase.reqSetPlayername(mPlayernameInput.text);
			});

			mAddGoldButton.onClick.Add(() =>
			{
				Constant.PlayerBase.addGold();
			});

			mCloseJoinRoomButton.onClick.Add(() =>
			{
				mJoinRoomBox.Disable();
			});

			mCreateRoomButton.onClick.Add(() =>
			{
				Constant.PlayerBase.createRoom();
			});

			mJoinRoomButton.onClick.Add(() =>
			{
				mJoinRoomBox.Enable();
			});

			mMatchButton.onClick.Add(() =>
			{
				Constant.PlayerBase.enterMatch();
			});

			// Key 0 ~ 9
			mKey0.onClick.Add(() =>
			{
				ClickKey(0);
			});

			mKey1.onClick.Add(() =>
			{
				ClickKey(1);
			});

			mKey2.onClick.Add(() =>
			{
				ClickKey(2);
			});

			mKey3.onClick.Add(() =>
			{
				ClickKey(3);
			});

			mKey4.onClick.Add(() =>
			{
				ClickKey(4);
			});

			mKey5.onClick.Add(() =>
			{
				ClickKey(5);
			});

			mKey6.onClick.Add(() =>
			{
				ClickKey(6);
			});

			mKey7.onClick.Add(() =>
			{
				ClickKey(7);
			});

			mKey8.onClick.Add(() =>
			{
				ClickKey(8);
			});

			mKey9.onClick.Add(() =>
			{
				ClickKey(9);
			});

			// Clear
			mKeyClear.onClick.Add(() =>
			{
				mRoomKeyInput.text = "";
			});

			// Delete
			mKeyDelete.onClick.Add(() =>
			{
				string roomKey = mRoomKeyInput.text;
				if (roomKey.Length == 0) return;
				mRoomKeyInput.text = roomKey.Substring(0, roomKey.Length - 1);
			});

			#endregion

			// 更新数据
			mPlayernameText.text = Constant.Player.playernameBase.ToString();
			mGoldText.text = Constant.Player.goldBase.ToString();

			// 显示 UI
			if (Constant.Player.playernameBase.Length == 0)
			{
				mMainBox.Disable();
				mSetNameBox.Enable();
			}
			else
			{
				mMainBox.Enable();
				mSetNameBox.Disable();
			}
			mJoinRoomBox.Disable();
		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		public void OnEvent(EventKey key, object[] args)
		{
			if (key == EventKey.onPlayernameBaseChanged)
			{
				string playername = (string)args[0];
				mPlayernameText.text = playername;

				// 如果是在 MainBox 改的
				if (mSetNameBox.activeSelf == true)
				{
					mMainBox.Enable();
					mSetNameBox.Disable();
				}
			}
			else if (key == EventKey.onGoldBaseChanged)
			{
				uint gold = (uint)args[0];
				mGoldText.text = gold.ToString();
			}
			else if (key == EventKey.onJoinRoom)
			{
				byte retcode = (byte)args[0];
				if (retcode == 0)
				{
					TipPanelEntity.ShowTip("加入房间成功");
				}
				else if (retcode == 1)
				{
					TipPanelEntity.ShowTip("此房间不存在");
				}
				else if (retcode == 2)
				{
					TipPanelEntity.ShowTip("此房间已满, 请选择另外一个房间");
				}
			}
		}

		private void ClickKey(int num)
		{
			string roomKey = mRoomKeyInput.text;
			if (roomKey.Length < 4)
			{
				mRoomKeyInput.text = $"{roomKey}{num}";
			}
			else if (roomKey.Length == 4)
			{
				mRoomKeyInput.text = $"{roomKey.Substring(0, roomKey.Length - 1)}{num}";
			}

			if (mRoomKeyInput.text.Length == 4)
			{
				Constant.PlayerBase.joinRoom(ushort.Parse(mRoomKeyInput.text));
			}
		}
	}
}
