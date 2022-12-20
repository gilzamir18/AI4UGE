using Godot;
namespace ai4u
{
	public class LocalBrain : Brain
	{
		[Export]
		private NodePath controllerPath;
		private Controller controller;
	
		void Awake()
		{
			if (!enabled)
			{
				return;
			}
			controller = GetNode<Controller>(controllerPath);
			if (agent == null)
			{
				agent = GetParent<Agent>();
			}
			if (controller == null) {
				GD.Print("You must specify a controller for the game object: " + Name);
			}

			if (agentPath == null) {
				GD.Print("You have not defined the agent that the remote brain must control. Game Object: " + Name);
			}
			agent = GetNode<Agent>(agentPath);
			agent.SetBrain(this);
			agent.Setup();
		}

		public string SendMessage(string[] desc, byte[] tipo, string[] valor)
		{
			controller.ReceiveState(desc, tipo, valor);
			return controller.GetAction();
		}

		public override void _Ready()
		{
			Awake();
		}
	}
}
