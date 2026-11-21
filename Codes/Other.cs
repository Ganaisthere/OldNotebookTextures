using BepInEx;
using System.IO;
using UnityEngine;

namespace OldNotebookTextures
{
    public static class Other
    {
        public static string GetModPath(BaseUnityPlugin plugin)
        {
            return Path.Combine(Application.streamingAssetsPath, "Modded", plugin.Info.Metadata.GUID);
        }
    }
}
