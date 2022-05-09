using System;

namespace GUIDTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine(Guid.NewGuid().ToString());
            }
        }
    }
}
