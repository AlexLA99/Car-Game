using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Networking
{
    class Message
    {
        public int playerId = 0;
        public string message = string.Empty;
        public byte[] payload = null;

        public byte[] Serialize()
        {
            MemoryStream serializedData = new MemoryStream();

            serializedData.Write(BitConverter.GetBytes(playerId), 0, sizeof(int));

            serializedData.Write(BitConverter.GetBytes(message.Length), 0, sizeof(int));
            if (message.Length > 0)
            {
                byte[] strBytes = Encoding.ASCII.GetBytes(message);
                serializedData.Write(strBytes, 0, strBytes.Length);
            }

            serializedData.Write(BitConverter.GetBytes(payload != null ? payload.Length : 0), 0, sizeof(int));
            if (payload != null)
            {
                serializedData.Write(payload, 0, payload.Length);
            }

            return serializedData.GetBuffer();
        }

        public void Deserialize(byte[] data)
        {
            playerId = BitConverter.ToInt32(data, 0);

            int offset = sizeof(int);
            int messageLength = BitConverter.ToInt32(data, offset);
            offset += sizeof(int);

            if (messageLength > 0)
            {
                message = Encoding.ASCII.GetString(data, offset, messageLength);
            }
            else
            {
                message = string.Empty;
            }

            offset += messageLength;

            int payloadLength = BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            if (payloadLength > 0)
            {
                MemoryStream payloadStream = new MemoryStream(payloadLength);
                payloadStream.Write(data, offset, payloadLength);
                payload = payloadStream.GetBuffer();
            }
            else
            {
                payload = null;
            }
        }
    }
}
