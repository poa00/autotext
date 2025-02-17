﻿/*This file is part of AutoText.

Copyright © 2016 Alexander Litvinov

AutoText is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace AutoText.Helpers
{
	public class Sender
	{
		static Process process;
		static NamedPipeServerStream namedPipeDataServerStream;
		static NamedPipeServerStream namedPipeCommandsServerStream = new NamedPipeServerStream("autotextCommands");

		public static event EventHandler<EventArgs> DataSent;

		public static void StartSender()
		{
			if (process == null)
			{
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					FileName = "Sender.exe",
					Arguments = Process.GetCurrentProcess().Id.ToString(),
					RedirectStandardOutput = true,
					RedirectStandardInput = true,
				};

				process = new Process
				{
					StartInfo = startInfo
				};

				process.Start();
			}
			else
			{
				throw new InvalidOperationException("Sender module is already started");
			}

			Task.Factory.StartNew(DataPipeWaitForConnection);
			Task.Factory.StartNew(StartReceiveData);
		}

		private static void StartReceiveData()
		{
			if (!namedPipeCommandsServerStream.IsConnected)
			{
				namedPipeCommandsServerStream.WaitForConnection();
			}

			while (true)
			{
				byte[] buf = new byte[2000];
				int bytesRead = namedPipeCommandsServerStream.Read(buf, 0, buf.Length);
				string msg = Encoding.Unicode.GetString(buf, 0, bytesRead);
				namedPipeCommandsServerStream.Close();

				switch (msg)
				{
					case "Done":
						OnDataSent();
						break;
				}

				namedPipeCommandsServerStream = new NamedPipeServerStream("autotextCommands");
				namedPipeCommandsServerStream.WaitForConnection();
			}
		}


		private static void DataPipeWaitForConnection()
		{
			namedPipeDataServerStream = new NamedPipeServerStream("autotextData");
			namedPipeDataServerStream.WaitForConnection();
		}

		public static void Send(string stringToSend)
		{
			if (process == null || process.HasExited)
			{
				throw new InvalidOperationException("Sender module is not started or terminated");
			}

			List<byte> res = new List<byte>();
			//res.Add(255);
			//res.Add(254);
			res.AddRange(Encoding.Unicode.GetBytes(stringToSend));

			//			string dataLength = (res.Count / 2).ToString();
			//			dataLength = dataLength.PadLeft(10,'0');
			//			List<byte> resDataLength = new List<byte>();
			//			//resDataLength.Add(255);
			//			//resDataLength.Add(254);
			//			resDataLength.AddRange(Encoding.Unicode.GetBytes(dataLength));


			//			namedPipeDataServerStream.Write(resDataLength.ToArray(), 0, resDataLength.Count);
			//			namedPipeDataServerStream.Flush();

			namedPipeDataServerStream.Write(res.ToArray(), 0, res.Count);
			namedPipeDataServerStream.Flush();
			namedPipeDataServerStream.Close();

			Task.Factory.StartNew(DataPipeWaitForConnection);
		}

		public static void StopSender()
		{
			if (process != null && !process.HasExited)
			{
				process.Kill();
			}
		}

		private static void OnDataSent()
		{
			var handler = DataSent;
			if (handler != null) handler(null, EventArgs.Empty);
		}
	}
}
