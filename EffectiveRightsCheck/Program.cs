using System;
using EffectiveRightsCheck.Core;

namespace EffectiveRightsCheck.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Console.Write("userName: ");
            var userName = Console.ReadLine();
            Console.Write("path: ");
            var path = Console.ReadLine();

            var rights = FileSystemEffectiveRights.GetRights(userName, path);
            Console.WriteLine();


            Console.WriteLine(rights);
            Console.ReadLine();
        }
    }
}