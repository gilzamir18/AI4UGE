using System.Collections;
using System.Collections.Generic;
using Godot;

namespace ai4u 
{
	public class StepSensor : Sensor
	{
		
		[Export]
		public int maxSize=300;
		[Export]
		public bool discrete;
		
		public override void OnSetup(Agent agent)
		{
			this.agent = (BasicAgent) agent;
			perceptionKey = "steps";
			if (discrete)
			{
				type = SensorType.sint;
			}
			else
			{
				type = SensorType.sfloat; 
			}
			shape = new int[0];
		}

		public override int GetIntValue()
		{
			return agent.NSteps;    
		}
		
		public override float GetFloatValue()
		{
			if (normalized)
			{
				return agent.NSteps/maxSize;
			}
			else
			{
				return agent.NSteps;
			}
		}
	}
}

