using ETModel;
using UnityEngine;
using KBEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class RoomPanelEntityAwakeSystem : AwakeSystem<RoomPanelEntity>
	{
		public override void Awake(RoomPanelEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class RoomPanelEntityStartSystem : StartSystem<RoomPanelEntity>
	{
		public override void Start(RoomPanelEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class RoomPanelEntityUpdateSystem : UpdateSystem<RoomPanelEntity>
	{
		public override void Update(RoomPanelEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class RoomPanelEntityDestroySystem : DestroySystem<RoomPanelEntity>
	{
		public override void Destroy(RoomPanelEntity self)
		{
			self.Deregister();
			self.OnDestroy();
		}
	}

	public class PlayerItem
	{
		public GameObject playerInfo;
		public Text playernameText;
		public Image playerIcon;

		public Image isReadyImage;

		private Account mAccount;
		public Account Account
		{
			get { return mAccount; }
			set
			{
				if (mAccount != value)
				{
					mAccount = value;
					if (mAccount != null)
					{
						playernameText.text = mAccount.playernameCell;
						isReadyImage.enabled = mAccount.isReady.ToBool();
						playerInfo.Enable();
					}
					else
					{
						playerInfo.Disable();
					}
				}
			}
		}

		public PlayerItem(GameObject playerInfo, Text playernameText, Image playerIcon, Image isReadyImage)
		{
			this.playerInfo = playerInfo;
			this.playernameText = playernameText;
			this.playerIcon = playerIcon;
			this.isReadyImage = isReadyImage;

			// 初始化UI
			this.playerInfo.Disable();
		}
	}

	public class RoomPanelEntity : Entity
	{
		private Room mRoom;

		private PlayerItem[] mPlayerItems = new PlayerItem[3];

		private ReferenceCollector mCollector;
		private Button mLeaveRoomButton;
		private Text mRoomKeyText;
		private Text mIsPrivateText;
		private Button mReadyButton;
		private Button mChangeRoomButton;

		public void Awake()
		{
			mCollector = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			mLeaveRoomButton = mCollector.Get<GameObject>("LeaveRoomButton").GetComponent<Button>();
			mRoomKeyText = mCollector.Get<GameObject>("RoomKeyText").GetComponent<Text>();
			mReadyButton = mCollector.Get<GameObject>("ReadyButton").GetComponent<Button>();
			mChangeRoomButton = mCollector.Get<GameObject>("ChangeRoomButton").GetComponent<Button>();
			mIsPrivateText = mCollector.Get<GameObject>("IsPrivateText").GetComponent<Text>();

			// 获取玩家UI
			for (int i = 0; i < 3; i++)
			{
				GameObject playerInfo = mCollector.Get<GameObject>($"PlayerInfo{i}");
				Text playernameText = mCollector.Get<GameObject>($"PlayernameText{i}").GetComponent<Text>();
				Image playerIcon = mCollector.Get<GameObject>($"PlayerIcon{i}").GetComponent<Image>();
				Image isReadyImage = mCollector.Get<GameObject>($"IsReadyImage{i}").GetComponent<Image>();
				mPlayerItems[i] = new PlayerItem(playerInfo, playernameText, playerIcon, isReadyImage);
			}
		}

		public void Start()
		{
			#region Event

			this.Register(EventKey.onEnterWorld, OnEvent);
			this.Register(EventKey.onLeaveWorld, OnEvent);
			this.Register(EventKey.onIsReadyChanged, OnEvent);

			#endregion

			#region UI

			mLeaveRoomButton.onClick.Add(() =>
			{
				// TipPanelEntity.ShowTip($"请求离开房间: {mRoom.roomKey}");
				mRoom.cellEntityCall.leaveRoom();
			});

			mReadyButton.onClick.Add(() =>
			{
				mRoom.cellEntityCall.changeIsReady();
			});

			#endregion

			// 获取缓存的Entity
			KBEngine.Event.fireOut(Constant.LoadCacheEntityList);
		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		public void OnEvent(EventKey key, object[] args)
		{
			if (key == EventKey.onEnterWorld)
			{
				KBEngine.Entity entity = (KBEngine.Entity)args[0];
				if (entity.className == "Room")
				{
					mRoom = entity as Room;

					// 数据初始化
					mRoomKeyText.text = mRoom.roomKey.ToString();

					// 私有房间
					if (mRoom.isPrivate == 1)
					{
						mIsPrivateText.text = "私人房";
						mChangeRoomButton.gameObject.Disable();
					}
					else
					{
						mIsPrivateText.text = "匹配房";
						mChangeRoomButton.gameObject.Enable();
					}
				}
				else if (entity.className == "Account")
				{
					Account account = entity as Account;
					mPlayerItems[ConvertLocalIndex(account.index)].Account = account;
					Debug.Log($"玩家:{account.playernameCell} 进入房间, 位置{ConvertLocalIndex(account.index)}");
				}
			}
			else if (key == EventKey.onLeaveWorld)
			{
				KBEngine.Entity entity = (KBEngine.Entity)args[0];
				if (entity.className == "Account")
				{
					Account account = entity as Account;
					mPlayerItems[ConvertLocalIndex(account.index)].Account = null;
					Debug.Log($"玩家:{account.playernameCell} 离开房间, 位置{ConvertLocalIndex(account.index)}");
				}
			}
			else if (key == EventKey.onIsReadyChanged)
			{
				int id = (int)args[0];
				bool isReady = (bool)args[1];
				foreach (var playerItem in mPlayerItems)
				{
					if (playerItem.Account != null && playerItem.Account.id == id)
					{
						playerItem.isReadyImage.enabled = isReady;
						break;
					}
				}
			}
		}

		public static int ConvertLocalIndex(int serverIndex)
		{
			int sub = serverIndex - Constant.Player.index;
			if (sub >= 0)
			{
				return sub;
			}
			else
			{
				return Constant.ROOM_MAX_PLAYER + sub;
			}
		}
	}
}
