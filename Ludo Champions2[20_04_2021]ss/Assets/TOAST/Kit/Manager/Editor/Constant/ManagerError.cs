namespace Toast.Kit.Manager.Constant
{
    public enum ManagerErrorCode
    {
        UNKNOWN,
        CDN,
        SERVICE_LIST,
        SERVICE_INFO,
        SERVICE_INFO_UPDATE,
        SERVICE_INFO_NOT_CHANGE,
        INSTALL,
        UNINSTALL,
        SETTING,
        NETWORK,
    }
    
    internal class ManagerError
    {
        public readonly ManagerErrorCode ErrorCode;
        public readonly string Message;
        public readonly string SubMessage;
        public readonly bool IsOpenDialog;

        public ManagerError(ManagerErrorCode errorCode, string message, string subMessage = "", bool isOpenDialog = true)
        {
            ErrorCode = errorCode;
            Message = message;
            SubMessage = subMessage;
            IsOpenDialog = isOpenDialog;
        }
    }
}