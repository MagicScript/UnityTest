using UnityEngine;
using System.Collections.Generic;

public enum LandType
{
	Plain,
	Water
}

public enum Improvement
{
	None
}

public class Tile
{
	public delegate void LandTypeChanged(Tile changed);
	public event LandTypeChanged OnLandTypeChanged;

	public delegate void ImprovementChanged(Tile changed);
	public event ImprovementChanged OnImprovementChanged;

	private LandType type_ = LandType.Plain;
	public LandType Type
	{
		get { return type_; }
		set
		{
			if(type_ != value)
			{
				type_ = value;
				if(OnLandTypeChanged != null)
				{
					OnLandTypeChanged(this);
				}
			}
		}
	}

	private Improvement improvement_ = Improvement.None;
	public Improvement Improvement
	{
		get { return improvement_; }
		set
		{
			if(improvement_ != value)
			{
				improvement_ = value;
				if(OnImprovementChanged != null)
				{
					OnImprovementChanged(this);
				}
			}
		}
	}

	public int X { get; private set; }
	public int Y { get; private set; }

	public Tile(int x, int y)
	{
		X = x;
		Y = y;
	}
}
