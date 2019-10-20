// Taken from https://github.com/rhargreaves/oauth-dotnetcore

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace OAuth
{
    public static class RsaPrivateKeyParser
    {
        private static readonly string PemBodyPattern = "^-----BEGIN RSA PRIVATE KEY-----(.+)-----END RSA PRIVATE KEY-----$";


        public static RSAParameters ParsePem(string pemContent)
        {
            return ParseRsaParameters(ParseOpenSslPrivateKeyBytes(pemContent));
        }


        private static byte[] ParseOpenSslPrivateKeyBytes(string pemContent)
        {
            return Convert.FromBase64String(ParsePemBody(pemContent));
        }


        private static string ParsePemBody(string pemContent)
        {
            Match match = Regex.Match(pemContent.Trim(), PemBodyPattern, RegexOptions.Compiled | RegexOptions.Singleline);
            if (!match.Success)
                throw new InvalidOperationException("The given key is not pem private key.");
            return match.Groups[1].Value;
        }


        private static RSAParameters ParseRsaParameters(byte[] privateKey)
        {
            using (var memoryStream = new MemoryStream(privateKey)) {
                using (var binaryReader = new BinaryReader(memoryStream)) {
                    ReadPrivateKeyHeader(binaryReader);
                    return CreateRsaParameters(binaryReader);
                }
            }
        }


        private static void ReadPrivateKeyHeader(BinaryReader reader)
        {
            switch (reader.ReadUInt16()) {
                case 33072: reader.ReadByte(); break;
                case 33328: reader.ReadInt16(); break;
                default:
                    throw new InvalidOperationException("Private key has invalid format.");
            }

            if (reader.ReadUInt16() !=  258)
                throw new InvalidOperationException("Private key has invalid format.");
            if (reader.ReadByte() != 0)
                throw new InvalidOperationException("Private key has invalid format.");
        }


        private static RSAParameters CreateRsaParameters(BinaryReader br)
        {
            byte[] numArray1 = ReadRsaParameter(br);
            byte[] numArray2 = ReadRsaParameter(br);
            byte[] numArray3 = ReadRsaParameter(br);
            byte[] numArray4 = ReadRsaParameter(br);
            byte[] numArray5 = ReadRsaParameter(br);
            byte[] numArray6 = ReadRsaParameter(br);
            byte[] numArray7 = ReadRsaParameter(br);
            byte[] numArray8 = ReadRsaParameter(br);
            return new RSAParameters()
            {
                Modulus = numArray1,
                Exponent = numArray2,
                D = numArray3,
                P = numArray4,
                Q = numArray5,
                DP = numArray6,
                DQ = numArray7,
                InverseQ = numArray8
            };
        }


        private static byte[] ReadRsaParameter(BinaryReader reader) => reader.ReadBytes(GetIntegerSize(reader));


        private static int GetIntegerSize(BinaryReader br)
        {
            if (br.ReadByte() != 2)
                throw new InvalidOperationException("Private key has invalid format.");
            byte num1 = br.ReadByte();
            int num2;
            switch (num1) {
                case 129: num2 = br.ReadByte(); break;
                case 130:
                    byte num3 = br.ReadByte();
                    num2 = BitConverter.ToInt32(new byte[4] { br.ReadByte(), num3, (byte) 0, (byte) 0}, 0);
                    break;
                default:
                    num2 = (int) num1; break;
            }

            while (br.ReadByte() == 0)
                --num2;
            br.BaseStream.Seek(-1L, SeekOrigin.Current);
            return num2;
        }
    }
}