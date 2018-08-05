using NeoPixelController.Model;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace NeoPixelController.Logic
{
    public class NeoPixelSender
    {
        private WebSocket webSocket;
        private string url;

        public NeoPixelSender(string url)
        {
            this.url = url;
        }


        public void Connect()
        {
            webSocket = new WebSocket(url);
            webSocket.OnClose += WebSocket_OnClose;
            webSocket.OnMessage += WebSocket_OnMessage;
            webSocket.Connect();
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Message from Neopixels: " + e.Data);
        }

        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine($"Disconnected from Neopixels: {e.Reason} ({e.Code}) - Was clean: {e.WasClean}");
        }

        public void SendSettings(string settings)
        {
            webSocket.Send(settings);
        }

        public byte[] GetBytes(NeoPixelStrip strip)
        {

            byte[] bytes = new byte[1 + 1 + 2 + strip.Pixels.Count * 3];

            bytes[0] = strip.Channel;
            bytes[1] = CommandType.Set8BitPixelColours;
            //bytes[2] = ; - Do not set
            //bytes[3] = ; - Do not set
            for (int i = 0; i < strip.Pixels.Count; i++)
            {
                bytes[4 + i * 3 + 0] = strip.Pixels[i].R;
                bytes[4 + i * 3 + 1] = strip.Pixels[i].G;
                bytes[4 + i * 3 + 2] = strip.Pixels[i].B;
            }
            return bytes;
        }

    }
}
