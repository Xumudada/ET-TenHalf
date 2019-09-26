# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import Functor

class Account(KBEngine.Proxy):
	def __init__(self):
		KBEngine.Proxy.__init__(self)


	def reqSetPlayername(self, playername):
		"""
		请求设置玩家名
		@param playername 昵称
		"""
		self.playernameBase = playername
		self.writeToDB()


	def addGold(self):
		"""
		加金币
		"""
		self.goldBase = self.goldBase + 100
		self.writeToDB()


	def createRoom(self):
		"""
		创建房间
		"""
		KBEngine.globalData["Lobby"].createRoom(self)


	def joinRoom(self, roomKey):
		"""
		加入房间
		"""
		KBEngine.globalData["Lobby"].joinRoom(self, roomKey)


	def leaveRoom(self, roomKey):
		"""
		离开房间
		"""
		KBEngine.globalData["Lobby"].leaveRoom(self, roomKey)


	def enterMatch(self):
		"""
		进入匹配
		"""
		KBEngine.globalData["Lobby"].enterMatch(self)


	def setSpace(self, space):
		"""
		设置空间
		"""
		if self.cell is None:
			self.createCellEntity(space)
		else:
			self.teleport(space)


	def onJoinRoom(self, retcode):
		"""
		加入房间
		"""
		if self.client:
			self.client.onJoinRoom(retcode)


	def onLeaveRoom(self):
		"""
		离开房间
		"""
		self.destroyCellEntity()

		
	# =========================================== 回调 ===================================================


	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		DEBUG_MSG(id, userArg)
		
	def onClientEnabled(self):
		"""
		KBEngine method.
		该entity被正式激活为可使用， 此时entity已经建立了client对应实体， 可以在此创建它的
		cell部分。
		"""
		INFO_MSG("account[%i] entities enable. entityCall:%s" % (self.id, self.client))
			
	def onLogOnAttempt(self, ip, port, password):
		"""
		KBEngine method.
		客户端登陆失败时会回调到这里
		"""
		INFO_MSG(ip, port, password)
		return KBEngine.LOG_ON_ACCEPT
		
	def onClientDeath(self):
		"""
		KBEngine method.
		客户端对应实体已经销毁
		"""
		DEBUG_MSG("Account[%i].onClientDeath:" % self.id)
		#self.destroy()


	def onGetCell(self):
		self.cell.setPlayernameCell(self.playernameBase)
		self.cell.setIsReady(0)
		self.cell.onGetCell()


	def onLoseCell(self):
		if self.client:
			print("玩家(%s)离开房间" % self.playernameBase)
			self.client.onLeaveRoom()
		else:
			print("丢失Cell, 并且没有客户端")