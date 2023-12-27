using Postwomen.Resources.Strings;

namespace Postwomen.Enums;

public enum OperationStates
{
	Fail,
	Success,
    Waiting
}

public class OperationStatesLangConverter
{
    public static string Convert(OperationStates result)
    {
        switch (result)
        {
            case OperationStates.Fail:
                return AppResources.fail;
            case OperationStates.Success:
                return AppResources.success; 
            case OperationStates.Waiting:
                return AppResources.waiting;
            default:
                return "?";
        }
    }
    public static string Convert(int result)
    {
        return Convert((OperationStates)result);
    }
}


