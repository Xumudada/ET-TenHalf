# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import Functor

ROOM_MAX_PLAYER = 2

class Room(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)
		self.createCellEntityInNewSpace(None)
		self.roomKey = self.cellData["roomKey"]
		self.isPrivate = self.cellData["isPrivate"]

		print("创建房间: %i, %i" % (self.roomKey, self.isPrivate))


	def enterRoom(self, account):
		"""
		一个玩家进入
		@param account 用户实体
		"""
		if len(self.playerList) == ROOM_MAX_PLAYER:
			print("房间(%i)人数已满" % self.roomKey)
			account.onJoinRoom(2)
			return
		if account not in self.playerList:
			self.playerList.append(account)

		# 加入房间
		if self.cell is not None:
			self.cell.enterRoom(account)


	def leaveRoom(self, callerID):
		"""
		离开房间
		@param account 账户
		"""
		for i in range(len(self.playerList)):
			if self.playerList[i].id == callerID:
				self.playerList.pop(i)
				break

		# 如果没人，私有房间应该销毁
		if len(self.playerList) == 0 and self.isPrivate == 1:
			self.destroyCellEntity()


	def needPlayerCount(self):
		"""
		@return int 需要玩家数量
		"""
		return ROOM_MAX_PLAYER - len(self.playerList)


	def onGetCell(self):
		"""
		KBEngine method.
		entity的cell部分实体被创建成功
		"""
		for account in self.playerList:
			self.enterRoom(account)


	def onLoseCell(self):
		"""
		注销私有房间
		"""
		self.destroy()
		KBEngine.globalData["Lobby"].removePrivateRoom(self.roomKey)