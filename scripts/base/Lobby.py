# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import Functor
import random

ROOM_MAX_PLAYER = 2
MATCH_TIMER = 1

class Lobby(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)
		KBEngine.globalData["Lobby"] = self
		self.privateRooms = {}						# roomKey : room
		self.publicRooms = {}						# roomKey : room
		self.matchPlayers = []						# 匹配中的玩家
		self.matchTimer = 0							# 匹配计时器ID


	def createRoom(self, account):
		"""
		创建房间
		"""
		# 有没有空房间
		for room in self.privateRooms.values():
			if room.needPlayerCount() == ROOM_MAX_PLAYER:
				room.enterRoom(account)
				return

		# 没有空房间就创建一个
		roomKey = self.generatePrivateKey()
		KBEngine.createEntityAnywhere("Room",
									{
										"playerList" : [account],
										"isPrivate" : 1,
										"roomKey" : roomKey
									}, Functor.Functor(self.onCreateRoom, roomKey, True))


	def joinRoom(self, account, roomKey):
		"""
		加入房间
		"""
		if roomKey in self.privateRooms:
			self.privateRooms[roomKey].enterRoom(account)
		else:
			print("玩家(%s)无法加入不存在的房间(%i)" % (account.playernameBase, roomKey))
			account.onJoinRoom(1)


	def enterMatch(self, account):
		"""
		进入匹配
		"""
		if account in self.matchPlayers:
			return
		self.matchPlayers.append(account)
		if self.matchTimer == 0:
			self.matchTimer = self.addTimer(0, 0.5, MATCH_TIMER)


	def match(self):
		"""
		匹配逻辑
		"""
		# 关闭计时器
		if len(self.matchPlayers) == 0 and self.matchTimer != 0:
			self.delTimer(self.matchTimer)
			self.matchTimer = 0
			return

		playerCount = len(self.matchPlayers)
		for room in self.publicRooms.values():
			need = room.needPlayerCount()
			if need == 0:
				continue

			# 玩家是否充裕
			if playerCount > need:
				for i in range(need):
					room.enterRoom(self.matchPlayers.pop(0))
			else:
				for i in range(playerCount):
					room.enterRoom(self.matchPlayers.pop(0))

			# 是否完成匹配
			playerCount = len(self.matchPlayers)
			if playerCount == 0:
				return

		# 如果还剩玩家，就创建房间给他们
		roomCount = int(playerCount / ROOM_MAX_PLAYER) + 1
		print("Match: 当前玩家人数(%i)需要创建房间(%i)" % (playerCount, roomCount))
		for i in range(roomCount):
			players = []
			#  玩家少
			if playerCount < ROOM_MAX_PLAYER:
				for i in range(playerCount):
					players.append(self.matchPlayers.pop(0))
			else:
				for i in range(ROOM_MAX_PLAYER):
					players.append(self.matchPlayers.pop(0))
			playerCount = len(self.matchPlayers)
			roomKey = self.generatePublicKey()
			print("Match: 创建房间(%i)" % roomKey)
			KBEngine.createEntityAnywhere("Room",
										{
											"playerList" : players,
											"isPrivate" : 0,
											"roomKey" : roomKey
										}, Functor.Functor(self.onCreateRoom, roomKey, False))



	def generatePrivateKey(self):
		"""
		生成不重复的私有Key
		@return int key
		"""
		key = self.generateKey()
		while key in self.privateRooms:
			key = self.generateKey()
		return key


	def generatePublicKey(self):
		"""
		生成不重复的公有Key
		@return int key
		"""
		key = self.generateKey()
		while key in self.publicRooms:
			key = self.generateKey()
		return key


	def generateKey(self):
		"""
		生成四位Key
		@return int key
		"""
		key = str(random.randint(1, 9))
		for i in range(3):
			key += str(random.randint(0, 9))
		return int(key)


	def removePrivateRoom(self, roomKey):
		"""
		移除私有房间
		"""
		del self.privateRooms[roomKey]


	def onCreateRoom(self, roomKey, isPrivate, room):
		"""
		创建房间回调
		"""
		if isPrivate:
			self.privateRooms[roomKey] = room
		else:
			self.publicRooms[roomKey] = room


	def onTimer(self, handle, userData):
		"""
		计时器
		"""
		if userData == MATCH_TIMER:
			self.match()