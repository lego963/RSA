using System;
using System.Numerics;

namespace RSA
{
    internal class Cryptosystem
    {
        public ulong N { get; }
        public uint E { get; }
        public ulong P { get; private set; }
        public ulong Q { get; private set; }
        public ulong Phi { get; }
        public ulong D { get; private set; }
        public string CipherText { get; }
        public string Message { get; private set; }

        public Cryptosystem(ulong n, uint e, string cipherText)
        {
            N = n;
            E = e;
            CipherText = cipherText;
            GetPq();
            Phi = (P - 1) * (Q - 1);
            GetD();
        }

        public void Run()
        {
            var codeMessage = string.Empty;
            var blocks = GetBlocks();
            var messageblocks = new ulong[blocks.Length];

            for (var i = 0; i < blocks.Length; i++)
                messageblocks[i] = (ulong)BigInteger.ModPow(blocks[i], D, N);

            for (var i = 0; i < blocks.Length; i++)
                codeMessage += messageblocks[i].ToString();
            GetMessage(codeMessage);
        }

        private void GetMessage(string codedMessage)
        {
            const int blockLength = 2;
            var index = 0;
            var destroyInput = new string[codedMessage.Length / 2 + codedMessage.Length % 2];
            while (codedMessage != string.Empty)
            {
                string tmp;
                if (blockLength < codedMessage.Length)
                {
                    tmp = codedMessage.Substring(0, blockLength);
                    codedMessage = codedMessage.Substring(blockLength);
                }
                else
                {
                    tmp = codedMessage.Substring(0, codedMessage.Length);
                    codedMessage = string.Empty;
                }
                destroyInput[index++] = tmp;
            }
            foreach (var block in destroyInput)
                Message += Convert.ToChar(Convert.ToInt32(block));
        }

        private ulong[] GetBlocks()
        {
            var cipherText = CipherText;
            var nLength = N.ToString().Length;
            var blocks = new ulong[CipherText.Length / nLength + CipherText.Length % nLength];
            var index = 0;
            while (cipherText != string.Empty)
            {
                var blockLength = Convert.ToUInt64(cipherText.Substring(0, nLength < cipherText.Length ? nLength : cipherText.Length));
                var correctLegth = blockLength <= N ? nLength : nLength - 1;
                string tmp;
                if (nLength < cipherText.Length)
                {
                    tmp = cipherText.Substring(0, correctLegth);
                    cipherText = cipherText.Substring(correctLegth);
                }
                else
                {
                    tmp = cipherText.Substring(0, cipherText.Length);
                    cipherText = string.Empty;
                }
                blocks[index++] = Convert.ToUInt64(tmp);
            }
            return blocks;
        }

        private void GetPq()
        {
            var s = (ulong)Math.Round(Math.Sqrt(N));
            ulong k = 1;
            ulong a;
            ulong b;
            while (true)
            {
                var x = (s + k) * (s + k) - N;
                if ((ulong)Math.Sqrt(x) * (ulong)Math.Sqrt(x) - x == 0)
                {
                    a = s + k;
                    b = (ulong)Math.Sqrt(x);
                    break;
                }
                k++;
            }
            P = a + b;
            Q = a - b;
        }

        private void GetD()
        {
            ulong x;
            uint k = 1;
            while (true)
            {
                var xx = 1 + k * Phi;
                if (xx % E == 0)
                {
                    x = xx / E;
                    break;
                }
                k++;
            }
            D = x % Phi;
        }
    }
}