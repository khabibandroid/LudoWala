using System.Collections.Generic;

namespace Toast.Kit.Common.Indicator
{
    public class ToastKitIndicatorData
    {
        private const string KEY_ACTION = "ACTION";
        private const string KEY_ACTION_DETAIL = "ACTION_DETAIL_";

        private Dictionary<string, string> dictionary;
        private int index = 1;

        public ToastKitIndicatorData(string action)
        {
            dictionary = new Dictionary<string, string>();
            dictionary.Add(KEY_ACTION, action);
        }

        public int AddActionDetail(string actionDetail)
        {
            dictionary.Add(string.Format("{0}{1}", KEY_ACTION_DETAIL, index), actionDetail);
            return index++;
        }

        public Dictionary<string, string> ToDictionary()
        {
            return dictionary;
        }
    }
}