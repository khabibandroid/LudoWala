using System.Collections.Generic;
using Toast.Kit.Common.Indicator.Internal;

namespace Toast.Kit.Common.Indicator
{
    public sealed class ToastKitIndicator
    {
        public const string SERVICE_NAME = "Indicator";

        public static void Send(string serviceName, string serviceVersion, Dictionary<string, string> customData, bool ignoreActivation = false)
        {
            IndicatorImplementation.Instance.Send(serviceName, serviceVersion, customData, ignoreActivation);
        }
    }
}