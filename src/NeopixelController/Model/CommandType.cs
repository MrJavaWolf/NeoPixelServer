using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoPixelController.Model
{
    public static class CommandType
    {
        /// <summary>
        /// The data block contains 8-bit RGB values: three bytes in red, green, blue order for each pixel to set. 
        /// If the data block has length 3*n, then the first n pixels of the specified channel are set. 
        /// All other pixels are unaffected and retain their current colour values. 
        /// If the data length is not a multiple of 3, or there is data for more pixels than are present, the extra data is ignored. 
        /// (Because the maximum data length is 65535, this command can control a maximum of 21845 pixels per channel, or a maximum of 5570475 pixels on all 255 channels.)
        /// </summary>
        public const byte Set8BitPixelColours = 0;

        /// <summary>
        /// The data block contains 16-bit RGB values: six bytes for each pixel to set, consisting of three 16-bit words in red, green, blue order with each two-byte word in big-endian order. 
        /// If the data block has length 6*n, then the first n pixels of the specified channel are set.
        /// All other pixels are unaffected and retain their current colour values. 
        /// If the data length is not a multiple of 6, or there is data for more pixels than are present, the extra data is ignored. 
        /// (Because the maximum data length is 65535, this command can control a maximum of 10922 pixels per channel, or a maximum of 2785110 pixels on all 255 channels.) 
        /// </summary>
        public const byte Set16BitPixelColours = 2;
    }
}
