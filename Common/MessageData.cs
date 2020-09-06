using System;
using System.Drawing;

namespace Common
{
    [Serializable]
    public class MessageData
    {
        public Bitmap bitmap;
        public MessageData(Bitmap image)
        {
            bitmap = image;
        }
    }
}
