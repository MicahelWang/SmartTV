using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace YeahResourceApi.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IImageUpdate" in both code and config file together.
    [ServiceContract]
    public interface IImageUpdate
    {
        [OperationContract]
        UpLoadRequest UpdateImageByBitmapStream(RemoteFileInfo remoteFileInfo);

        [OperationContract]
        UpLoadRequest UpdateAppByStream(RemoteFileInfo remoteFileInfo);
    }

    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileType;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        [MessageHeader(MustUnderstand = true)]
        public long FileLength;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }

    [MessageContract]
    public class UpLoadRequest
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public bool IsUpLoad;
    }
}
