using DsPair.src.Controllers;
using DsPair.src.Enums;
using DsPair.src.Exceptions;
using DsPair.src.Helpers;

namespace DsPair.src;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            if (args.Length < 1) throw new StatusException(ErrorStatus.MissingMode);

            ProgramMode mode = FlagHelper.GetProgramMode(args[0]);

            if (mode == ProgramMode.PairAllNearbyDs)
            {
                return (int)BtController.pairAllDs();
            }
            else if (mode == ProgramMode.PairFromMac)
            {
                return (int)BtController.pairDsFromMac(args);
            }
            else if (mode == ProgramMode.UnpairAll)
            {
                return (int)BtController.unpairAllDs();
            }
            else if (mode == ProgramMode.UnpairFromMac)
            {
                return (int)BtController.unpairDsFromMac(args);
            }
            else
            {
                return (int)ErrorStatus.FatalError;
            }

        }
        catch (StatusException e)
        {
            Console.WriteLine(Enum.GetName(typeof(ErrorStatus), e.status).ToString());
            return (int)e.status;
        }
    }
}