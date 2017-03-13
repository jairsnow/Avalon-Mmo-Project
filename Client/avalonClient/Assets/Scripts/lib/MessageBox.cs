using UnityEditor;

namespace Assets.Scripts {

    static class MessageBox {

        static public void Show(string message) {
            EditorUtility.DisplayDialog("", message, "ok");
        }

    }


}
