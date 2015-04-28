﻿using System.IO;

namespace Pfim
{
    /// <summary>
    /// A DirectDraw Surface that is not compressed.  
    /// Thus what is in the input stream gets directly translated to the image buffer.
    /// </summary>
    public class UncompressedDds : DdsBase
    {
        private static DdsLoadInfo loadInfoB8G8R8A8 = new DdsLoadInfo(false, false, false, 1, 4 /*PixelFormat.Format32bppArgb*/);
        private static DdsLoadInfo loadInfoB8G8R8 = new DdsLoadInfo(false, false, false, 1, 3 /*PixelFormat.Format24bppRgb*/);
        private static DdsLoadInfo loadInfoB5G5R5A1 = new DdsLoadInfo(false, true, false, 1, 2 /*PixelFormat.Format16bppArgb1555*/);
        private static DdsLoadInfo loadInfoB5G6R5 = new DdsLoadInfo(false, true, false, 1, 2 /*PixelFormat.Format16bppRgb565*/);
        private static DdsLoadInfo loadInfoIndex8 = new DdsLoadInfo(false, false, true, 1, 1 /*PixelFormat.Format8bppIndexed*/);
        private byte[] buffer;

        public UncompressedDds(Stream stream, DdsHeader header)
            : base(header)
        {
            if (IsThirtyTwoBitRGBA)
                LoadInfo = loadInfoB8G8R8A8;
            else if (IsTwentyFourBitRGB)
                LoadInfo = loadInfoB8G8R8;
            else if (IsSixteenBitAlphaOne)
                LoadInfo = loadInfoB5G5R5A1;
            else if (IsSixteenBitAlphaOne)
                LoadInfo = loadInfoB5G6R5;
            else if (header.PixelFormat.RGBBitCount == 8)
                LoadInfo = loadInfoIndex8;

            buffer = new byte[Size];
            Util.Fill(stream, buffer);
        }

        public override byte[] Data { get { return buffer; } }

        public bool IsSixteenBitAlphaZero
        {
            get
            {
                return (Header.PixelFormat.RGBBitCount == 16) &&
                   (Header.PixelFormat.RBitMask == 0x0000f800) &&
                   (Header.PixelFormat.GBitMask == 0x000007e0) &&
                   (Header.PixelFormat.BBitMask == 0x0000001f);
            }
        }
        public bool IsSixteenBitAlphaOne
        {
            get
            {
                return (Header.PixelFormat.RGBBitCount == 16) &&
                   (Header.PixelFormat.RBitMask == 0x00007c00) &&
                   (Header.PixelFormat.GBitMask == 0x000003e0) &&
                   (Header.PixelFormat.BBitMask == 0x0000001f) &&
                   (Header.PixelFormat.ABitMask == 0x00008000);
            }
        }
        public bool IsThirtyTwoBitRGBA
        {
            get
            {
                return (Header.PixelFormat.RGBBitCount == 32) &&
                    (Header.PixelFormat.RBitMask == 0xff0000) &&
                    (Header.PixelFormat.GBitMask == 0xff00) &&
                    (Header.PixelFormat.BBitMask == 0xff) &&
                    (Header.PixelFormat.ABitMask == 0xff000000U);
            }
        }
        public bool IsTwentyFourBitRGB
        {
            get
            {
                return (Header.PixelFormat.RGBBitCount == 24) &&
                    (Header.PixelFormat.RBitMask == 0xff0000) &&
                    (Header.PixelFormat.GBitMask == 0xff00) &&
                    (Header.PixelFormat.BBitMask == 0xff);
            }
        }
    }
}
