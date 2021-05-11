using System.Collections.Generic;
using Toast.Kit.Common.Indicator;
using Toast.Kit.Manager.Constant;

namespace Toast.Kit.Manager.Internal
{
    internal static class ToastKitManagerIndicator
    {
        private const string KEY_ACTION = "ACTION";
        private const string KEY_ACTION_DETAIL_1 = "ACTION_DETAIL_1";
        private const string KEY_ACTION_DETAIL_2 = "ACTION_DETAIL_2";
        private const string KEY_ACTION_DETAIL_3 = "ACTION_DETAIL_3";

        private const string ACTION_AD = "Ad";
        private const string ACTION_INSTALL = "Install";
        private const string ACTION_REMOVE  = "Remove";
        private const string ACTION_LINK = "Link";

        public static void SendAd(string name, string linkUrl)
        {
            Send(new Dictionary<string, string>() 
            {
                { KEY_ACTION,             ACTION_AD },
                { KEY_ACTION_DETAIL_1,    name },
                { KEY_ACTION_DETAIL_2,    linkUrl },
            });
        }

        public static void SendLink(string serviceName, string linkName, string linkUrl)
        {
            Send(new Dictionary<string, string>()
            {
                { KEY_ACTION,             ACTION_LINK },
                { KEY_ACTION_DETAIL_1,    serviceName },
                { KEY_ACTION_DETAIL_2,    linkName },
                { KEY_ACTION_DETAIL_3,    linkUrl },
            });
        }

        public static void SendInstall(string serviceName, string serviceVersion)
        {
            Send(new Dictionary<string, string>()
            {
                { KEY_ACTION,             ACTION_INSTALL },
                { KEY_ACTION_DETAIL_1,    serviceName },
                { KEY_ACTION_DETAIL_2,    serviceVersion },
            });
        }

        public static void SendRemove(string serviceName, string serviceVersion)
        {
            Send(new Dictionary<string, string>()
            {
                { KEY_ACTION,             ACTION_REMOVE },
                { KEY_ACTION_DETAIL_1,    serviceName },
                { KEY_ACTION_DETAIL_2,    serviceVersion },
            });
        }

        private static void Send(Dictionary<string, string> data)
        {
            ToastKitIndicator.Send(ManagerInfos.SERVICE_NAME, ToastKitManagerVersion.VERSION, data);
        }
    }
}
