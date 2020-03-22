﻿using Shadowsocks.Controller;
using System;

namespace Shadowsocks.Encryption
{
    public enum CipherFamily
    {
        Plain,
        Table,

        AesGcm,

        AesCfb,
        AesCtr,

        Chacha20,
        Chacha20Poly1305,
        XChacha20Poly1305,

        Rc4,
        Rc4Md5,
    }

    public enum CipherStandardState
    {
        InUse,
        Deprecated,
        Hidden,
    }

    public class CipherParameter
    {
        public int KeySize;
    }
    public class StreamCipherParameter : CipherParameter
    {
        public int IvSize;
        public override string ToString() => $"stream (key:{KeySize * 8}, iv:{IvSize * 8})";
    }

    public class AEADCipherParameter : CipherParameter
    {
        public int SaltSize;
        public int TagSize;
        public int NonceSize;
        public override string ToString() => $"aead (key:{KeySize * 8}, salt:{SaltSize * 8}, tag:{TagSize * 8}, nonce:{NonceSize * 8})";
    }

    public class CipherInfo
    {
        public string Name;
        public CipherFamily Type;
        public CipherParameter CipherParameter;

        public CipherStandardState StandardState = CipherStandardState.InUse;

        #region Stream ciphers
        public CipherInfo(string name, int keySize, int ivSize, CipherFamily type)
        {
            Type = type;
            Name = name;
            StandardState = CipherStandardState.Hidden;
            CipherParameter = new StreamCipherParameter
            {
                KeySize = keySize,
                IvSize = ivSize,
            };
        }

        #endregion

        #region AEAD ciphers
        public CipherInfo(string name, int keySize, int saltSize, int nonceSize, int tagSize, CipherFamily type)
        {
            Type = type;
            Name = name;

            CipherParameter = new AEADCipherParameter
            {
                KeySize = keySize,
                SaltSize = saltSize,
                NonceSize = nonceSize,
                TagSize = tagSize,
            };
        }
        #endregion

        public override string ToString()
        {
            return StandardState == CipherStandardState.InUse ? Name : $"{Name} ({I18N.GetString("deprecated")})";
        }
        public string ToString(bool verbose)
        {
            if (!verbose) return ToString();

            return $"{Name} {StandardState} {CipherParameter}";
        }
    }
}