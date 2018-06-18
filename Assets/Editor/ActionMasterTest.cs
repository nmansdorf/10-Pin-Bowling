using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ActionMasterTest
{

	private ActionMaster.Action endTurn = ActionMaster.Action.EndTurn;
	private ActionMaster.Action tidy = ActionMaster.Action.MidFrameReset;
	private ActionMaster.Action reset = ActionMaster.Action.FrameReset;
	private ActionMaster.Action endGame = ActionMaster.Action.EndGame;
	private ActionMaster actionMaster;
	private GameManager gameManager;
	
	
	
	[SetUp]
	public void Setup()
	{
		var obj = new GameObject();
		actionMaster = new ActionMaster();
		obj.AddComponent<GameManager>();
		gameManager = obj.GetComponent<GameManager>();

	}

	[Test]
	public void PassingTest () {
		Assert.AreEqual (1, 1);
	}

	[Test]
	public void T01OneStrikeReturnsEndTurn()
	{
		gameManager.BowlList.Clear();
		gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(endTurn, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T02Bowl8ReturnsTidy()
	{
		gameManager.BowlList.Clear();
		gameManager.BowlList.Add(new Bowl(8));
		Assert.AreEqual(tidy, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T03Bowl2Then8ReturnEndTurn()
	{
		gameManager.BowlList.Clear();
		gameManager.BowlList.Add(new Bowl(2));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(8));
		Assert.AreEqual(endTurn, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T04EndGameAfter10ThFrame()
	{
		gameManager.BowlList.Clear();
		for (int i = 0; i < 20; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
		}
		Assert.AreEqual(endGame, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T05AdditionalBowlIfStrikeOnLastFrame()
	{
		gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
			Debug.Log("Frame: " + gameManager.BowlList.Last().GetFrame());
		}
		gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		Assert.AreEqual(reset, actionMaster.Bowl(gameManager.BowlList)); 
	}

	[Test]
	public void T06EndGameAfter21Bowls()
	{
		gameManager.BowlList.Clear();
		for (int i = 0; i < 19; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
		}
		gameManager.BowlList.Add(new Bowl(9)); //10th Frame
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(1));
		Assert.AreEqual(endGame, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T07AdditionalBowlIfSpareOnLastFrame()
	{
		gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
		}
		gameManager.BowlList.Add(new Bowl(8)); //10th Frame
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(2));
		Assert.AreEqual(reset, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T08TidyOnFrame20AfterStrike()
	{
		gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
		}
		gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(5));
		Assert.AreEqual(tidy, actionMaster.Bowl(gameManager.BowlList)); 
	}

	[Test]
	public void T09ResetAfter2StrikesOnFrame21()
	{
		gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
		}
		gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(reset,actionMaster.Bowl(gameManager.BowlList));
		
	}
	
	[Test]
	public void T10PinsOnSecondBowlTidyAfterNextRoll()
	{
		gameManager.BowlList.Clear();	
		gameManager.BowlList.Add(new Bowl(0));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(10));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(3));
		Assert.AreEqual(tidy, actionMaster.Bowl(gameManager.BowlList));
	}
	
	[Test]
	public void T1110PinsOnSecondBowlEndTun()
	{	
		gameManager.BowlList.Clear();
		gameManager.BowlList.Add(new Bowl(0));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(endTurn, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T12EndTurnAfterSpareAndTwoRolls()
	{
		gameManager.BowlList.Clear();
		gameManager.BowlList.Add(new Bowl(0));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(10));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(1));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(3));
		Assert.AreEqual(endTurn, actionMaster.Bowl(gameManager.BowlList));
	}

	[Test]
	public void T13LastFrameTurkey()
	{
		gameManager.BowlList.Clear();	
		for (int i = 0; i < 18; i++)
		{
			gameManager.BowlList.Add(new Bowl(1));
			actionMaster.Bowl(gameManager.BowlList);
		}
		gameManager.BowlList.Add(new Bowl(10)); //10th Frame
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(10));
		actionMaster.Bowl(gameManager.BowlList);
		gameManager.BowlList.Add(new Bowl(10));
		Assert.AreEqual(endGame,actionMaster.Bowl(gameManager.BowlList));
	}
}