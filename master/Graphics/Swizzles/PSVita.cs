using System;

namespace TTG_Tools.Graphics.Swizzles
{
    public static class PSVita
    {
        public static byte[] Swizzle(byte[] deswizzledData, int width, int height, int bytesPerPixelSet, int formatBitsPerPixel)
        {
            if (bytesPerPixelSet <= 0 || deswizzledData == null || deswizzledData.Length <= bytesPerPixelSet)
            {
                return deswizzledData;
            }

            int calculatedBufferSize = (formatBitsPerPixel * width * height) / 8;
            byte[] swizzledData = new byte[Math.Max(calculatedBufferSize, bytesPerPixelSet)];

            int maxU = (int)Math.Log(width, 2);
            int maxV = (int)Math.Log(height, 2);

            for (int j = 0; (j < width * height) && (j * bytesPerPixelSet < deswizzledData.Length); j++)
            {
                int u = 0;
                int v = 0;
                int origCoord = j;

                for (int k = 0; k < maxU || k < maxV; k++)
                {
                    if (k < maxV)
                    {
                        v |= (origCoord & 1) << k;
                        origCoord >>= 1;
                    }

                    if (k < maxU)
                    {
                        u |= (origCoord & 1) << k;
                        origCoord >>= 1;
                    }
                }

                if (u < width && v < height)
                {
                    int srcPos = (v * width + u) * bytesPerPixelSet;
                    int dstPos = j * bytesPerPixelSet;

                    if (srcPos + bytesPerPixelSet <= deswizzledData.Length && dstPos + bytesPerPixelSet <= swizzledData.Length)
                    {
                        Array.Copy(deswizzledData, srcPos, swizzledData, dstPos, bytesPerPixelSet);
                    }
                }
            }

            return swizzledData;
        }

        public static byte[] Unswizzle(byte[] swizzledData, int width, int height, int bytesPerPixelSet, int formatBitsPerPixel)
        {
            if (bytesPerPixelSet <= 0 || swizzledData == null || swizzledData.Length <= bytesPerPixelSet)
            {
                return swizzledData;
            }

            int calculatedBufferSize = (formatBitsPerPixel * width * height) / 8;
            byte[] unswizzledData = new byte[Math.Max(calculatedBufferSize, bytesPerPixelSet)];

            int maxU = (int)Math.Log(width, 2);
            int maxV = (int)Math.Log(height, 2);

            for (int j = 0; (j < width * height) && (j * bytesPerPixelSet < swizzledData.Length); j++)
            {
                int u = 0;
                int v = 0;
                int origCoord = j;

                for (int k = 0; k < maxU || k < maxV; k++)
                {
                    if (k < maxV)
                    {
                        v |= (origCoord & 1) << k;
                        origCoord >>= 1;
                    }

                    if (k < maxU)
                    {
                        u |= (origCoord & 1) << k;
                        origCoord >>= 1;
                    }
                }

                if (u < width && v < height)
                {
                    int srcPos = j * bytesPerPixelSet;
                    int dstPos = (v * width + u) * bytesPerPixelSet;

                    if (srcPos + bytesPerPixelSet <= swizzledData.Length && dstPos + bytesPerPixelSet <= unswizzledData.Length)
                    {
                        Array.Copy(swizzledData, srcPos, unswizzledData, dstPos, bytesPerPixelSet);
                    }
                }
            }

            return unswizzledData;
        }
    }
}
