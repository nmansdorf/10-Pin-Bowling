using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ActionMasterTest
{

	private ActionMaster.Action endTurn = ActionMaster.Action.EndTurn;
	private ActionMaster.Action midFrameReset = ActionMaster.Action.MidFrameReset;
	private ActionMaster.Action frameReset = ActionMaster.Action.FrameReset;
	private ActionMaster.Action endGame = ActionMaster.Action.EndGame;
	private ActionMaster actionMaster;
	private Frame currentFrame;
	
	[SetUp]
	public void Setup()
	{
		actionMaster = new ActionMaster();
	}

	[Test]
	public void PassingTest () {
		Assert.AreEqual (1, 1);
	}

	[Test]
	public void T01OneStrikeReturnsEndTurn()
	{
		Frame.ResetFrames();
		int[] rolls = {10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endTurn, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T02Bowl8ReturnsTidy()
	{
		Frame.ResetFrames();
		int[] rolls = {8};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(midFrameReset, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T03Bowl2Then8ReturnEndTurn()
	{
		Frame.ResetFrames();
		int[] rolls = {2,8};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endTurn, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T04EndGameAfter10ThFrame()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endGame, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T05AdditionalBowlIfStrikeOnLastFrame()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(frameReset, actionMaster.NextAction(currentFrame)); 
	}

	[Test]
	public void T06EndGameAfter21Bowls()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 10,1,1};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endGame, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T07AdditionalBowlIfSpareOnLastFrame()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,9};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(frameReset, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T08TidyOnFrame20AfterStrike()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(frameReset, actionMaster.NextAction(currentFrame)); 
	}

	[Test]
	public void T09ResetAfter2StrikesOnFrame21()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 10,10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(frameReset,actionMaster.NextAction(currentFrame));
		
	}
	
	[Test]
	public void T10PinsOnSecondBowlTidyAfterNextRoll()
	{
		Frame.ResetFrames();
		int[] rolls = {0,10,1};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(midFrameReset, actionMaster.NextAction(currentFrame));
	}
	
	[Test]
	public void T1110PinsOnSecondBowlEndTun()
	{	
		Frame.ResetFrames();
		int[] rolls = {0,10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endTurn, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T12EndTurnAfterSpareAndTwoRolls()
	{
		Frame.ResetFrames();
		int[] rolls = {0,10, 1,3};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endTurn, actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T13LastFrameTurkey()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 10,10,10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endGame,actionMaster.NextAction(currentFrame));
	}

	[Test]
	public void T14LLastFrameSpare()
	{
		Frame.ResetFrames();
		int[] rolls = {1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 1,1, 8,2,10};
		for (int i= 0; i < rolls.Length; i++)
		{
			if (Frame.FrameList.Count < 1 || Frame.FrameList.Last().IsFrameComplete())
			{
				currentFrame = new Frame(rolls[i]);
			}
			else
			{
				currentFrame.AddRoll(rolls[i]);
			}	
		}	
		Assert.AreEqual(endGame,actionMaster.NextAction(currentFrame));
	}
}