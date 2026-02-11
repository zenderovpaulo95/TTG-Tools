/***************************************************************
 * Special thanks to daemon1 and tge for PS4 swizzle algorithm *
 * Fixed destination array size bug for imports                *
 ***************************************************************/

using System;

namespace TTG_Tools.Graphics.Swizzles
{
    public class PS4
    {
        public static byte[] Swizzle(byte[] data, int width, int height, int blockSize, int blockWidth = 4, int blockHeight = 4)
        {
            return DoSwizzle(data, width, height, blockSize, blockWidth, blockHeight, false);
        }

        public static byte[] Unswizzle(byte[] data, int width, int height, int blockSize, int blockWidth = 4, int blockHeight = 4)
        {
            return DoSwizzle(data, width, height, blockSize, blockWidth, blockHeight, true);
        }

        private static byte[] DoSwizzle(byte[] data, int width, int height, int blockSize, int blockWidth, int blockHeight, bool unswizzle)
        {
            if (blockWidth < 1) blockWidth = 1;
            if (blockHeight < 1) blockHeight = 1;

            // 1. Calcula as dimensões em "Texels" (pixels para formato linear, blocos 4x4 para formatos BC).
            var widthTexels = (width + blockWidth - 1) / blockWidth;
            var heightTexels = (height + blockHeight - 1) / blockHeight;

            // 2. Calcula o alinhamento necessário para o PS4 (Blocos de 8x8 Texels = 32x32 Pixels)
            var heightTexelsAligned = (heightTexels + 7) / 8;
            var widthTexelsAligned = (widthTexels + 7) / 8;

            // 3. Calcula o tamanho total necessário (incluindo o padding do PS4)
            int alignedSize = heightTexelsAligned * widthTexelsAligned * 64 * blockSize;
            int compactSize = widthTexels * heightTexels * blockSize;

            byte[] processed;

            // Alguns arquivos armazenam o swizzle completo com padding, outros armazenam apenas blocos válidos (compacto).
            // Detectamos pelo tamanho disponível.
            bool hasPaddingLayout = data.Length >= alignedSize;

            if (!unswizzle)
            {
                processed = new byte[hasPaddingLayout ? alignedSize : compactSize];
            }
            else
            {
                processed = new byte[compactSize];
            }

            int swizzledIndex = 0;
            int compactIndex = 0;

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

                        // Verifica se estamos dentro da área válida da imagem (não no padding)
                        if ((xOffset < widthTexels) && (yOffset < heightTexels))
                        {
                            var destPixelIndex = yOffset * widthTexels + xOffset;
                            int destIndex = blockSize * destPixelIndex;

                            // Verificações de segurança adicionais para evitar crash se o arquivo DDS estiver corrompido ou incompleto
                            if (unswizzle)
                            {
                                // PS4 (data) -> Linear (processed)
                                if (swizzledIndex + blockSize <= data.Length && destIndex + blockSize <= processed.Length)
                                {
                                    Array.Copy(data, swizzledIndex, processed, destIndex, blockSize);
                                }
                            }
                            else
                            {
                                // Linear (data) -> PS4 (processed)
                                int destinationIndex = hasPaddingLayout ? swizzledIndex : compactIndex;

                                if (destIndex + blockSize <= data.Length && destinationIndex + blockSize <= processed.Length)
                                {
                                    Array.Copy(data, destIndex, processed, destinationIndex, blockSize);
                                }
                            }

                            compactIndex += blockSize;
                        }

                        // Em layout com padding, o índice swizzled avança sempre.
                        // Em layout compacto, ele avança apenas quando o bloco é válido.
                        if (hasPaddingLayout || ((xOffset < widthTexels) && (yOffset < heightTexels)))
                        {
                            swizzledIndex += blockSize;
                        }
                    }
                }
            }

            return processed;
        }
    }
}
