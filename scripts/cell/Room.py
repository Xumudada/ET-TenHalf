# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import Functor

ROOM_MAX_PLAYER = 2

class Room(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)
		KBEngine.globalData["Room_%i" % self.spaceID] = self
		self.seats = []

		# 创建座位
		for i in range(ROOM_MAX_PLAYER):
			seat = Seat(i)
			self.seats.append(seat)


	def enterRoom(self, account):
		"""
		进入房间
		@param account 玩家
		"""
		account.setSpace(self)


	def leaveRoom(self, callerId):
		"""
		离开房间
		@param account 账户
		"""
		for i in range(len(self.seats)):
			seat = self.seats[i]
			if seat.userId == callerId:
				print("玩家(%s) 离开座位 (%i)" % (seat.playername, seat.index))
				self.base.leaveRoom(callerId)
				seat.user.base.onLeaveRoom()
				seat.leave()
				break


	def changeIsReady(self, callerId):
		"""
		改变准备状态
		"""
		seat = self.getSeatById(callerId)
		if seat.isReady == 0:
			seat.setIsReady(1)
		else:
			seat.setIsReady(0)
		print("%i = %s: 改变准备状态为=%i" % (seat.index, seat.playername, seat.isReady))



	def sitDown(self, account, playername):
		"""
		坐下
		@param account 用户
		"""
		print("玩家(%s)在找座位" % playername)
		for i in range(len(self.seats)):
			seat = self.seats[i]
			if seat.userId == 0:
				seat.sitDown(account, playername)
				print("安排 (%s) 坐在 (%i)" % (playername, i))
				break


	def getSeatById(self, id):
		"""
		获取Seat
		"""
		for seat in self.seats:
			if seat.userId == id:
				return seat
		return None


# ============================================== Seat ==================================

class Seat:
	def __init__(self, index):
		self.index = index
		self.userId = 0
		self.user = None
		self.playername = ""
		self.isReady = 0


	def sitDown(self, account, playername):
		self.userId = account.id
		self.user = account
		self.user.setIndex(self.index)
		self.playername = playername


	def leave(self):
		self.userId = 0
		self.user = None
		self.clear()


	def clear(self):
		self.playername = ""
		self.setIsReady(0)


	def setIsReady(self, isReady):
		self.isReady = isReady
		if self.userId != 0:
			self.user.setIsReady(isReady)