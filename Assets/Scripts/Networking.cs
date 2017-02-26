using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Maca
{
    public class Networking : Singleton<Networking>
    {
        public string puzzleData;

        private void Awake()
        {
            instance = this;
        }

        public void Connect(string preferences)
        {
            String server = "207.154.236.250";
            String message = preferences;

            UdpClient udpClient = new UdpClient(12005);

            try
            {
                udpClient.Connect(server, 12002);

                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                udpClient.Send(sendBytes, sendBytes.Length);

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                puzzleData = Encoding.GetEncoding("iso-8859-9").GetString(receiveBytes);

                udpClient.Close();
            }

            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
