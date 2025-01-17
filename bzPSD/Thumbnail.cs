﻿#region Licence
/*
Copyright (c) 2013, Darren Horrocks
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the <organization> nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion

using System.IO;
using SixLabors.ImageSharp.Processing;
using Bitmap = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24>;

namespace bzPSD
{
    /// <summary>
    /// Summary description for Thumbnail.
    /// </summary>
    public sealed class Thumbnail : ImageResource
    {
        public Bitmap Image { get; private set; }

        public Thumbnail(ImageResource imageResource)
            : base(imageResource)
        {
            using (BinaryReverseReader reverseReader = DataReader)
            {
                int format = reverseReader.ReadInt32();
                int width = reverseReader.ReadInt32();
                int height = reverseReader.ReadInt32();

                reverseReader.ReadInt32(); // read widthBytes
                reverseReader.ReadInt32(); // read size
                reverseReader.ReadInt32(); // read compressedSize
                reverseReader.ReadInt16(); // read bitPerPixel
                reverseReader.ReadInt16(); // read planes

                if (format == 1)
                {
                    byte[] imgData = reverseReader.ReadBytes((int)(reverseReader.BaseStream.Length - reverseReader.BaseStream.Position));

                    using (MemoryStream strm = new MemoryStream(imgData))
                    {
                        Image = (Bitmap)SixLabors.ImageSharp.Image.Load(strm).Clone(d => {});
                    }

                    if (ID == 1033)
                    {
                        //// BGR
                        //for(int y=0;y<m_thumbnailImage.Height;y++)
                        //  for (int x = 0; x < m_thumbnailImage.Width; x++)
                        //  {
                        //    Color c=m_thumbnailImage.GetPixel(x,y);
                        //    Color c2=Color.FromArgb(c.B, c.G, c.R);
                        //    m_thumbnailImage.SetPixel(x, y, c);
                        //  }
                    }

                }
                else
                {
                    Image = new Bitmap(width, height);
                }
            }
        }
    }
}