# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import Functor

class Account(KBEngine.Entity):
	def __init__(self):
		KBEngine.Entity.__init__(self)


	def setPlayernameCell(self, playername):
		self.playernameCell = playername


	def setIndex(self, index):
		self.index = index


	def setIsReady(self, isReady):
		self.isReady = isReady

		
	def onGetCell(self):
		"""
		获取到Cell
		"""
		KBEngine.globalData["Room_%i" % self.spaceID].sitDown(self, self.playernameCell)