using System;

namespace RSA
{
    internal class Program
    {
        private static void Main()
        {
            var cs = new Cryptosystem(517776452420107, 12377, "50527945233429 232174902406749442926770512941299737185022728");
            cs.Run();
            Console.WriteLine(cs.Message);
            Console.ReadKey();
        }
    }
}
