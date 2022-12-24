using Godot;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ai4u
{
	///
	/// <summary>This class defines a remote controller for an agent of type Agent of the AI4U.
	/// Estes agentes recebem comandos de um script por meio de uma interface de comunicação em rede.
	/// So, Brain is a generic controller, awhile RemoteBrain implements an agent's network controller.
	/// </summary>
	public class RemoteBrain : Brain
	{   
		///<summary>If true, the remote brain will be 
		///managed manually. Thus, in this case, command 
		///line arguments do not alter the properties of 
		///the remote brain.</summary>
		[Export]
		private bool managed = false;
		private float timeScale = 1.0f; //unity controll of the physical time.
		private bool runFirstTime = false;
        private ControlRequestor controlRequestor;
        ///<summary>The IP of the ai4u2unity training server.</summary>
        [Export]
        public string host = "127.0.0.1";
        
        ///<summary>The server port of the ai4u2unity training server.</summary>
        [Export]
        public int port = 8080;

        [Export]
        public int receiveTimeout = 200;
        private IPAddress serverAddr; //controller address
        private EndPoint endPoint; //controller endpoint
        private Socket sockToSend; //Socket to send async message.

		void Awake() {
			if (!enabled)
			{
				return;
			}
			//one time configuration
			if (!managed && runFirstTime){
				runFirstTime =false;
				string[] args = System.Environment.GetCommandLineArgs ();
				int i = 0;
                while (i < args.Length){
                    switch (args[i]) {
                        case "--ai4u_port":
                            port = int.Parse(args[i+1]);
                            i += 2;
                            break;
                        case "--ai4u_timescale":
                            this.timeScale = float.Parse(args[i+1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                            Engine.TimeScale = this.timeScale;
                            i += 2;
                            break;
                        case "--ai4u_host":
                            host = args[i+1];
                            i += 2;
                            break;
                        default:
                            i+=1;
                            break;
                    }
                }
			}
			if (agentPath == null) {
				GD.Print("You have not defined the agent that the remote brain must control. Game Object: " + Name);
			}
			agent = GetNode<Agent>(agentPath);
			agent.SetBrain(this);
			agent.Setup();
			controlRequestor = agent.ControlRequestor; 
            if (controlRequestor == null)
            {
                GD.Print("No ControlRequestor component added in RemoteBrain component.");
            }
		}

        public Socket TrySocket()
        {
            if (sockToSend == null)
            {
                    serverAddr = IPAddress.Parse(host);
                    endPoint = new IPEndPoint(serverAddr, port);
                    sockToSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            return sockToSend;
        }


		public override void _ExitTree()
		{
			if (sockToSend != null)
			{
				sockToSend.Close();
			}
		}

		public bool sendData(byte[] data, out int total, byte[] received)
        {
            TrySocket().ReceiveTimeout = receiveTimeout;
            sockToSend.SendTo(data, endPoint);
            total = 0;
            try 
            { 
                total = sockToSend.Receive(received);
                return true;
            }
            catch(System.Exception e)
            {
                GD.Print("Script ai4u2unity is not connected! Start the ai4u2unity script! Network error: " + e.Message);
				GetTree().Quit();
				return false;
            }
        }

		public override void _Ready()
		{
			Awake();
		}
	}
}
