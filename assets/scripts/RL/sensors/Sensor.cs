using System.Collections;
using System.Collections.Generic;
using Godot;

namespace ai4u
{
	public enum SensorType {
		sfloat,
		sstring,
		sbool,
		sint,
		sbytearray,
		sfloatarray,
		sintarray
	}

	public class Sensor : Node, IAgentResetListener
	{
		
		[Export]
		public string perceptionKey;
		[Export]
		public int stackedObservations = 1;
		[Export]
		public bool isActive = true;
		[Export]
		public bool isInput = true;
		[Export]
		public bool  normalized = true;
		[Export]
		public bool resetable = true;
		[Export]
		public float rangeMin = 0.0f;
		[Export]
		public float rangeMax = 1.0f;

		protected SensorType Type;
		protected bool IsState;
		protected int[] Shape;
		protected BasicAgent agent;

		public int StackedObservations
		{
			get
			{
				return stackedObservations;
			}
		}
		
		public string PerceptionKey
		{
			get
			{
				return perceptionKey;
			}
		}
		
		public bool IsActive
		{
			get
			{
				return isActive;
			}
		}
		
		public bool Normalized 
		{
			get
			{
				return normalized;
			}
		}

		public bool Resetable
		{
			get
			{
				return resetable;	
			}
		}

		public SensorType type
		{
			get 
			{
				return Type;
			}

			set
			{
				Type = value;
			}
		}

		public bool isState
		{
			get
			{
				return IsState;
			}

			set
			{
				IsState = value;
			}

		}

		public int[] shape 
		{
			get
			{
				return Shape;
			}

			set
			{
				Shape = value;
			}
		}

		public void SetAgent(BasicAgent own)
		{
			agent = own;
		}

		public virtual void OnSetup(Agent agent)
		{
		}

		public virtual float GetFloatValue() {
			return 0;
		}

		public virtual string GetStringValue() {
			return string.Empty;
		}

		public virtual bool GetBoolValue() {
			return false;
		}

		public virtual byte[] GetByteArrayValue() {
			return null;
		}

		public virtual int GetIntValue() {
			return 0;
		}

		public virtual int[] GetIntArrayValue() {
			return null;
		}

		public virtual float[] GetFloatArrayValue() {
			return null;
		}

		public virtual void OnReset(Agent agent) {

		}
	}
}
