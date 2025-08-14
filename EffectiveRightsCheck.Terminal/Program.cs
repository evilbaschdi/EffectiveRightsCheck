using EffectiveRightsCheck.Core;

namespace EffectiveRightsCheck.Terminal;

// ReSharper disable once ArrangeTypeModifiers
// ReSharper disable once ClassNeverInstantiated.Global
class Program
{
    // ReSharper disable once ArrangeTypeMemberModifiers
    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    static void Main(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var userName = string.Empty;
        var path = string.Empty;
        while (string.IsNullOrWhiteSpace(userName))
        {
            Console.Write("userName: ");
            userName = Console.ReadLine();
        }

        while (string.IsNullOrWhiteSpace(path))
        {
            Console.Write("path: ");
            path = Console.ReadLine();
        }

        var rights = FileSystemEffectiveRights.GetRights(userName, path);
        Console.WriteLine();

        Console.WriteLine(rights);
        Console.ReadLine();
    }
}