using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace YeahTVApi.Common
{
    public abstract class RC4 : SymmetricAlgorithm
    {
        private static KeySizes[] s_legalBlockSizes = { new KeySizes(64, 64, 0) };

        private static KeySizes[] s_legalKeySizes = { new KeySizes(40, 2048, 8) };

        public RC4()
        {
         
            KeySizeValue = 128;
            BlockSizeValue = 64;
            FeedbackSizeValue = BlockSizeValue;
            LegalBlockSizesValue = s_legalBlockSizes;
            LegalKeySizesValue = s_legalKeySizes;
        }

        // required for compatibility with .NET 2.0
        public override byte[] IV
        {
            get { return new byte[0]; }
            set { ; }
        }

        new static public RC4 Create()
        {
            return Create("RC4");
        }

        new static public RC4 Create(string algName)
        {
            object o = CryptoConfig.CreateFromName(algName);
            // in case machine.config isn't configured to use 
            // any RC4 implementation
            if (o == null)
            {
                o = new ARC4Managed();
            }
            return (RC4)o;
        }
    }

    public class ARC4Managed : RC4, ICryptoTransform
    {

        private byte[] key;
        private byte[] state;
        private byte x;
        private byte y;
        private bool m_disposed;

        public ARC4Managed()
            : base()
        {
            state = new byte[256];
            m_disposed = false;
        }

        ~ARC4Managed()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                x = 0;
                y = 0;
                if (key != null)
                {
                    Array.Clear(key, 0, key.Length);
                    key = null;
                }
                Array.Clear(state, 0, state.Length);
                state = null;
                GC.SuppressFinalize(this);
                m_disposed = true;
                //2014-04-23 防止没有释放父类资源的情况
                base.Dispose(disposing);
            }
            
        }

        public override byte[] Key
        {
            get { return (byte[])key.Clone(); }
            set
            {
                key = (byte[])value.Clone();
                KeySetup(key);
            }
        }

        public bool CanReuseTransform
        {
            get { return false; }
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgvIV)
        {
            Key = rgbKey;
            return (ICryptoTransform)this;
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgvIV)
        {
            Key = rgbKey;
            return CreateEncryptor();
        }

        public override void GenerateIV()
        {
            // not used for a stream cipher
            IV = new byte[0];
        }


        public override void GenerateKey()
        {
            //Key = KeyBuilder.Key (KeySizeValue >> 3);

            byte[] arr = new byte[KeySizeValue >> 3];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(arr);
            Key = arr;

        }

        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        public int InputBlockSize
        {
            get { return 1; }
        }

        public int OutputBlockSize
        {
            get { return 1; }
        }

        private void KeySetup(byte[] key)
        {
            byte index1 = 0;
            byte index2 = 0;

            for (int counter = 0; counter < 256; counter++)
                state[counter] = (byte)counter;
            x = 0;
            y = 0;
            for (int counter = 0; counter < 256; counter++)
            {
                index2 = (byte)(key[index1] + state[counter] + index2);
                // swap byte
                byte tmp = state[counter];
                state[counter] = state[index2];
                state[index2] = tmp;
                index1 = (byte)((index1 + 1) % key.Length);
            }
        }

        private void CheckInput(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer");
            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException("inputOffset", "< 0");
            if (inputCount < 0)
                throw new ArgumentOutOfRangeException("inputCount", "< 0");
            // ordered to avoid possible integer overflow
            if (inputOffset > inputBuffer.Length - inputCount)
                throw new ArgumentException("inputBuffer", "Overflow");
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            CheckInput(inputBuffer, inputOffset, inputCount);
            // check output parameters
            if (outputBuffer == null)
                throw new ArgumentNullException("outputBuffer");
            if (outputOffset < 0)
                throw new ArgumentOutOfRangeException("outputOffset", "< 0");
            // ordered to avoid possible integer overflow
            if (outputOffset > outputBuffer.Length - inputCount)
                throw new ArgumentException("outputBuffer", "Overflow");

            return InternalTransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        }

        private int InternalTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte xorIndex;
            for (int counter = 0; counter < inputCount; counter++)
            {
                x = (byte)(x + 1);
                y = (byte)(state[x] + y);
                // swap byte
                byte tmp = state[x];
                state[x] = state[y];
                state[y] = tmp;

                xorIndex = (byte)(state[x] + state[y]);
                outputBuffer[outputOffset + counter] = (byte)(inputBuffer[inputOffset + counter] ^ state[xorIndex]);
            }
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            CheckInput(inputBuffer, inputOffset, inputCount);

            byte[] output = new byte[inputCount];
            InternalTransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
            return output;
        }

        #region 静态方案
        public static void TransformSelf(byte[] buf, byte[] key)
        {
            if (buf == null) return;

            ARC4Managed rc4 = new ARC4Managed();
            rc4.Key = key;
            rc4.TransformBlock(buf, 0, buf.Length, buf, 0);
        }

        public static byte[] Transform(byte[] bufSource, byte[] key)
        {
            if (bufSource == null) return null;

            byte[] bufDest = new byte[bufSource.Length];
            return Transform(bufSource, bufDest, key);
        }

        public static byte[] Transform(byte[] bufSource, byte[] bufDest, byte[] key)
        {
            if ((bufSource == null) || (bufDest == null)) return null;
            if (bufSource.Length != bufDest.Length) throw new Exception("Buffer Size Error");

            ARC4Managed rc4 = new ARC4Managed();
            rc4.Key = key;
            rc4.TransformBlock(bufSource, 0, bufSource.Length, bufDest, 0);
            return bufDest;
        }
        #endregion
    }
}
