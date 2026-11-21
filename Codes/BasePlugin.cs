using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace OldNotebookTextures
{
    [BepInPlugin("ganaisthere.plus.oldnotebooktextures", "Old Notebook Textures", "0.1.0.0")]

    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin? Instance { get; internal set; }
        public static Dictionary<string, Sprite>? AllSprites;

        //----------------------------------------

        public ConfigEntry<NotebookStyle>? ConfigNotebookStyle;

        //----------------------------------------

        public void Awake()
        {
            ConfigNotebookStyle = Config.Bind
            (
                "Debug",
                "Notebook Style",
                NotebookStyle.Classic,
                @"The notebook style.
Classic - A larger notebook without animations;
ClassicRemastered - Only without animation;
OldPlus - Use the hidden rotation animation from v0.14.5;
Plus - Use animations that sway left and right."
            );
            Instance = this;
            new Harmony(Info.Metadata.GUID).PatchAll();
            LoadResources();
        }

        public void LoadResources()
        {
            string bundleName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                bundleName = "assets-win.bundle";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bundleName = "assets-mac.bundle";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                bundleName = "assets-linux.bundle";
            }
            else
            {
                throw new PlatformNotSupportedException($"Unsupported Operating System platform: {RuntimeInformation.OSDescription}");
            }

            string bundlePath = Path.Combine(Other.GetModPath(this), "AssetBundles", bundleName);
            if (!File.Exists(bundlePath))
            {
                throw new FileNotFoundException($"Couldn't get AssetBundles file {bundleName} at: {bundlePath}");
            }
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

            AllSprites = new Dictionary<string, Sprite>();
            foreach (var i in bundle.LoadAllAssets<Sprite>())
            {
                //Log(i.name + "(Sprite)");
                AllSprites.Add(i.name, i);
            }
            bundle.Unload(false);
        }

        internal static void Log(object data, int level = 0)
        {
            if (Instance == null)
            {
                return;
            }
            else if (level == 1)
            {
                Instance.Logger.LogWarning(data);
            }
            else if (level == 2)
            {
                Instance.Logger.LogError(data);
            }
            else
            {
                Instance.Logger.LogInfo(data);
            }
        }
    }

    public enum NotebookStyle
    {
        Classic,
        ClassicRemastered,
        OldPlus,
        Plus
    }
}
