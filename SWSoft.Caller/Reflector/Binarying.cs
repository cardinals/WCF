using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace SWSoft.Reflector
{
    public class Binarying<T>
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="t">将要序列化的对象</param>
        /// <param name="path">相对路径或着绝对路径</param>
        public static void Serialize(T t, string path)
        {
            new BinaryFormatter().Serialize(File.Open(path.IndexOf(':') > 0 ? path : Application.StartupPath + "\\" + path, FileMode.OpenOrCreate), t);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="t">将要序列化的对象</param>
        /// <param name="path">相对路径或着绝对路径</param>
        public static string Serialize(T t)
        {
            var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, t);
            ms.Position = 0;
            ms.Flush();
            return new StreamReader(ms).ReadToEnd();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="t">将要序列化的对象</param>
        /// <param name="path">相对路径或着绝对路径</param>
        public static T DeserializeString(string seria)
        {
            var buffer = Encoding.UTF8.GetBytes(seria);
            var ms = new MemoryStream(buffer, 0, buffer.Length);
            ms.Position = 0;
            return (T)new BinaryFormatter().Deserialize(ms);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="path">相对路径或着绝对路径</param>
        /// <returns>反序列化的对象</returns>
        public static T Deserialize(string path)
        {
            path = path.IndexOf(':') > 0 ? path : Application.StartupPath + "\\" + path;
            try
            {
                return File.Exists(path) ? (T)new BinaryFormatter().Deserialize(File.Open(path, FileMode.OpenOrCreate)) : default(T);
            }
            catch (SerializationException)
            {
                return default(T);
            }
        }


        #region 序列化/反序列化解压缩

        public static byte[] SerializeCompress(T t)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(mStream, t);
                /*
            byte[] bytes = mStream.ToArray();
            MemoryStream oStream = new MemoryStream();
            DeflateStream zipStream = new DeflateStream(oStream, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Flush();
            zipStream.Close();*/
                return mStream.ToArray();
            }
        }

        /// <summary>  

        /// 反序列化压缩的object  

        /// </summary>  

        /// <param name="_filePath"></param>  

        /// <returns></returns>  

        public static T DeserializeDecompress(byte[] bytes, int count = -1)
        {
            try
            {
                MemoryStream mStream = new MemoryStream(new List<byte>(bytes).GetRange(0, count).ToArray());
                /* mStream.Seek(0, SeekOrigin.Begin);
                 DeflateStream unZipStream = new DeflateStream(mStream, CompressionMode.Decompress, true);*/
                object dsResult = null;
                BinaryFormatter bFormatter = new BinaryFormatter();
                dsResult = (object)bFormatter.Deserialize(mStream);
                return (T)dsResult;
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }
        #endregion
    }
}
