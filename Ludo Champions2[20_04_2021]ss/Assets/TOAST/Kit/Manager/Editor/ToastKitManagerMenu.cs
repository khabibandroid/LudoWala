using Toast.Kit.Manager.Ui;
using UnityEditor;

namespace Toast.Kit.Manager
{
    public static class ToastKitManagerMenu
    {
#if TOAST_KIT_DEBUG
        private const string MENU_OPEN_MANAGER = "Tools/TOAST/Kit/Manager #t";
#else
        private const string MENU_OPEN_MANAGER = "Tools/TOAST/Kit/Manager";
#endif

        [MenuItem(MENU_OPEN_MANAGER)]
        private static void OpenManager()
        {
            ToastKitManagerWindow.OpenWindow();
        }
    }
}