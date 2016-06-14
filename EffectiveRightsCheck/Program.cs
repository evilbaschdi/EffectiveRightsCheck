using System;
using EffectiveRightsCheck.Core;

namespace EffectiveRightsCheck.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            System.Console.Write("userName: ");
            var userName = System.Console.ReadLine();
            System.Console.Write("path: ");
            var path = System.Console.ReadLine();

            var rights = FileSystemEffectiveRights.GetRights(userName, path);
            System.Console.WriteLine();


            System.Console.WriteLine(rights);
            System.Console.ReadLine();
        }
    }
}