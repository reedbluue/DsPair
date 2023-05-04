using DsPair.src.Exceptions;
using DsPair.src.Enums;
using System.Text.RegularExpressions;

namespace DsPair.src.Helpers;
abstract class FlagHelper
{
    public static ProgramMode GetProgramMode(string arg)
    {
        if (!isValidString(arg)) throw new StatusException(ErrorStatus.InvalidFlag);
        char modeChar = arg.ToLower().ToCharArray()[1];

        try
        {
            return (ProgramMode)Enum.Parse(typeof(ProgramMode), Enum.GetName(typeof(ProgramMode), modeChar));
        }
        catch
        {
            throw new StatusException(ErrorStatus.InvalidMode);
        }
    }
    private static bool isValidString(string arg)
    {
        Regex regex = new Regex("^-[a-z]$", RegexOptions.IgnoreCase);
        return regex.IsMatch(arg);
    }
}
