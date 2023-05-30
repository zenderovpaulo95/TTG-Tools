/***************************************************************
 * Special thanks to daemon1 and tge for PS4 swizzle algorithm *
 ***************************************************************/

using System;
using System.Reflection.Emit;

namespace TTG_Tools.Graphics.Swizzles
{
    public class PS4
    {
        public static byte[] Swizzle(byte[] data, int width, int height, int blockSize)
        {
            return DoSwizzle(data, width, height, blockSize, false);
        }

        public static byte[] Unswizzle(byte[] data, int width, int height, int blockSize)
        {
            return DoSwizzle(data, width, height, blockSize, true);
        }

        private static byte[] DoSwizzle(byte[] data, int width, int height, int blockSize, bool unswizzle)
        {
            var processed = new byte[data.Length];
            var heightTexels = height / 4;
            var widthTexels = width / 4;
            var heightTexelsAligned = (heightTexels + 7) / 8;
            var widthTexelsAligned = (widthTexels + 7) / 8;
            var dataIndex = 0;

            for (int y = 0; y < heightTexelsAligned; ++y)
            {
                for (int x = 0; x < widthTexelsAligned; ++x)
                {
                    for (int t = 0; t < 64; ++t)
                    {
                        int pixelIndex = SwizzleUtilities.Morton(t, 8, 8);
                        int num8 = pixelIndex / 8;
                        int num9 = pixelIndex % 8;
                        var yOffset = (y * 8) + num8;
                        var xOffset = (x * 8) + num9;

                        if ((xOffset < widthTexels) && (yOffset < heightTexels))
                        {
                            var destPixelIndex = yOffset * widthTexels + xOffset;
                            int destIndex = blockSize * destPixelIndex;

                            if (unswizzle) Array.Copy(data, dataIndex, processed, destIndex, blockSize);
                            else Array.Copy(data, destIndex, processed, dataIndex, blockSize);
                        }

                        dataIndex += blockSize;
                    }
                }
            }

            return processed;
        }
    }
}
