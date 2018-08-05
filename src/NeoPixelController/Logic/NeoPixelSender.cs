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
        private readonly string url;

        public NeoPixelSender(string url)
        {
            this.url = url;
        }

        public void Connect()
        {
            webSocket = new WebSocket(url);
            webSocket.OnClose += WebSocket_OnClose;
            webSocket.OnMessage += WebSocket_OnMessage;
            webSocket.OnOpen += WebSocket_OnOpen;
            webSocket.OnError += WebSocket_OnError;
            webSocket.Connect();
        }

        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            String errorMsg = "Websocket error: " + e.Message;
            if (e.Exception != null)
                errorMsg += Environment.NewLine + e.Exception.ToString();
            Console.WriteLine(errorMsg);
        }

        private void WebSocket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to NeoPixels");
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Message from Neopixels: " + e.Data);
        }

        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine($"Disconnected from Neopixels: {e.Reason} ({e.Code}) - Was clean: {e.WasClean}");
            webSocket.OnClose -= WebSocket_OnClose;
            webSocket.OnMessage -= WebSocket_OnMessage;
        }

        public void SendSettings(string settings)
        {
            webSocket.Send(settings);
        }

        public void Send(NeoPixelStrip strip)
        {
            webSocket.Send(GetBytes(strip));
        }

        private byte[] GetBytes(NeoPixelStrip strip)
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
