using System;
using Toast.Kit.Common.Util;
using UnityEngine;

namespace Toast.Kit.Common.Indicator.Internal
{
    public sealed class InAppIndicator : BaseIndicator
    {
        private MonoBehaviour monoObject;

        public InAppIndicator()
        {
            monoObject = GameObjectContainer.GetGameObject(ToastKitIndicator.SERVICE_NAME).GetComponent<MonoBehaviour>();
            Initialize();
        }

        protected override void GetLaunchingInfo(UnityWebRequestHelper helper, Action<LaunchingInfo> callback)
        {
            monoObject.StartCoroutine(helper.SendWebRequest(result =>
            {
                if (CheckInvalidResult(result) == true)
                {
                    callback(null);
                }
                else
                {
                    var launchingInfo = ToastKitJsonMapper.ToObject<LaunchingInfo>(result.downloadHandler.text);
                    callback(launchingInfo);
                }
            }));
        }

        protected override void ExecuteQueueDelegate()
        {
            monoObject.StartCoroutine(ExecuteQueue());
        }

        protected override void SendLogNCrash(UnityWebRequestHelper helper, byte[] sendData)
        {
            monoObject.StartCoroutine(helper.SendWebRequest((result) =>
            {
                queueItem.isRunning = false;
            }));
        }

        protected override void SetQueueItemStatus()
        {
            queueItem.isRunning = true;
        }

        protected override bool IsWaitingInChild()
        {
            if (queueItem != null && queueItem.isRunning == true)
            {
                return true;
            }

            return false;
        }
    }
}