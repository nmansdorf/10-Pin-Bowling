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
	
	[SetUp]
	public void Setup()
	{
		var obj = new GameObject();
		actionMaster = new ActionMaster();
	}

	[Test]
	public void PassingTest () {
		Assert.AreEqual (1, 1);
	}

	[Test]
	public void T01OneStrikeReturnsEndTurn()
	{
		List<Bowl> bowlList = new List<Bowl>();
		bowlList.Add(new Bowl(10));
		Assert.AreEqual(endTurn, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T02Bowl8ReturnsTidy()
	{
		List<Bowl> bowlList = new List<Bowl>();
		bowlList.Add(new Bowl(8));
		Assert.AreEqual(tidy, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T03Bowl2Then8ReturnEndTurn()
	{
		List<Bowl> bowlList = new List<Bowl>();
		bowlList.Add(new Bowl(2));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(8));
		Assert.AreEqual(endTurn, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T04EndGameAfter10ThFrame()
	{
		List<Bowl> bowlList = new List<Bowl>();
		for (int i = 0; i < 20; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		Assert.AreEqual(endGame, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T05AdditionalBowlIfStrikeOnLastFrame()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		for (int i = 0; i < 18; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
			Debug.Log("Frame: " + bowlList.Last().GetFrame());
		}
		bowlList.Add(new Bowl(10)); //10th Frame
		Assert.AreEqual(reset, actionMaster.Bowl(bowlList)); 
	}

	[Test]
	public void T06EndGameAfter21Bowls()
	{
		List<Bowl> bowlList = new List<Bowl>();
		for (int i = 0; i < 19; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		bowlList.Add(new Bowl(9)); //10th Frame
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(1));
		Assert.AreEqual(endGame, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T07AdditionalBowlIfSpareOnLastFrame()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		for (int i = 0; i < 18; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		bowlList.Add(new Bowl(8)); //10th Frame
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(2));
		Assert.AreEqual(reset, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T08TidyOnFrame20AfterStrike()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		for (int i = 0; i < 18; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		bowlList.Add(new Bowl(10)); //10th Frame
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(5));
		Assert.AreEqual(tidy, actionMaster.Bowl(bowlList)); 
	}

	[Test]
	public void T09ResetAfter2StrikesOnFrame21()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		for (int i = 0; i < 18; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		bowlList.Add(new Bowl(10)); //10th Frame
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		Assert.AreEqual(reset,actionMaster.Bowl(bowlList));
		
	}
	
	[Test]
	public void T10PinsOnSecondBowlTidyAfterNextRoll()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		bowlList.Add(new Bowl(0));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(3));
		Assert.AreEqual(tidy, actionMaster.Bowl(bowlList));
	}
	
	[Test]
	public void T1110PinsOnSecondBowlEndTun()
	{	
		List<Bowl> bowlList = new List<Bowl>();
		bowlList.Add(new Bowl(0));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		Assert.AreEqual(endTurn, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T12EndTurnAfterSpareAndTwoRolls()
	{
		List<Bowl> bowlList = new List<Bowl>();
		bowlList.Add(new Bowl(0));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(1));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(3));
		Assert.AreEqual(endTurn, actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T13LastFrameTurkey()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		for (int i = 0; i < 18; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		bowlList.Add(new Bowl(10)); //10th Frame
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		Assert.AreEqual(endGame,actionMaster.Bowl(bowlList));
	}

	[Test]
	public void T14LLastFrameSpare()
	{
		List<Bowl> bowlList = new List<Bowl>();	
		for (int i = 0; i < 18; i++)
		{
			bowlList.Add(new Bowl(1));
			actionMaster.Bowl(bowlList);
		}
		bowlList.Add(new Bowl(8)); //10th Frame
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(2));
		actionMaster.Bowl(bowlList);
		bowlList.Add(new Bowl(10));
		Assert.AreEqual(endGame,actionMaster.Bowl(bowlList));
	}
}