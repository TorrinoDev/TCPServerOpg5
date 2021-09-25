using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ModelLib;

namespace TCPServer
{
	class Server
	{
		private FootballPlayer player;
		private static List<FootballPlayer> players = new List<FootballPlayer>()
		{
			new FootballPlayer(1, "Brandon", 200000, 3),
			new FootballPlayer(2, "Wayne", 120000, 12),
			new FootballPlayer(3, "Salem", 150000, 19),
			new FootballPlayer(4, "Jason", 220000, 34),
			new FootballPlayer(5, "Lars", 300000, 4),
		};
		public void Start()
		{
			TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
			listener.Start();
			while (true)
			{
				Task.Run(() =>
				{
					TcpClient socket = listener.AcceptTcpClient();
					DoClient(socket);
				});
			}
		}

		private void DoClient(TcpClient socket)
		{
			using (StreamReader sr = new StreamReader(socket.GetStream()))
			using (StreamWriter sw = new StreamWriter(socket.GetStream()))
			{
				string str1 = sr.ReadLine();
				string str2 = sr.ReadLine();
				Console.WriteLine("Serveren har modtaget:\n" + str1 + "\n" + str2 + "\n------***------");
				string strFull = str1 + str2;

				if (str1.ToLower().Equals("hentalle"))
				{
					HentAlle(sw);
				} 
				else if (str1.ToLower().Equals("hent"))
				{
					HentId(sw, Int32.Parse(str2));
				} 
				else if (str1.ToLower().Equals("gem"))
				{
					Gem(str2);
				}

			}
		}

		private void HentAlle(StreamWriter sw)
		{
			sw.WriteLine(JsonSerializer.Serialize(players));
		}

		private void HentId(StreamWriter sw, int id)
		{
			sw.WriteLine(JsonSerializer.Serialize(players.Where(i => i.Id == id)));
		}

		private void Gem(string str)
		{
			player = new FootballPlayer(1, "temp", 1, 1);
			player = JsonSerializer.Deserialize<FootballPlayer>(str);
			players.Add(player);
		}
	}
}
