using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NeoPixelController.Model
{
    /// <summary>
    /// https://github.com/scanlime/fadecandy/blob/master/doc/fc_protocol_websocket.md
    /// http://openpixelcontrol.org/
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct OpenPixelControlPacket
    {
        /// <summary>
        /// Up to 255 separate strands of pixels can be controlled. 
        /// Each strand is given a channel number from 1 to 255 and listens for messages with that channel number. 
        /// Messages with channel 0 are considered broadcast messages; 
        /// all strands should treat a message with channel 0 as if it were sent on all channels. 
        /// </summary>
        public byte Channel;

        /// <summary>
        /// The command code determines the format of the data and the expected behaviour in response to the message. 
        /// Individual commands are defined below. 
        /// </summary>
        public byte Command;

        /// <summary>
        /// Do NOT set this. 
        /// This is the Data Length, but we do not have to set it due to we are sending the package through websockets, the websocket server will set the length.
        /// The message data block can have any length from 0 to 65535, transmitted as an unsigned two-byte number with the high byte first. 
        /// </summary>
        public short Reserved;


        /// <summary>
        /// The data block must contain exactly the number of bytes indicated by the length field, from 0 to 65535
        /// </summary>        
        public byte[] Data;
    }
}
