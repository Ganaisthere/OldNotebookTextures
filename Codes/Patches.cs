using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

namespace OldNotebookTextures
{

    [HarmonyPatch(typeof(Notebook))]
    public class NotebookPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void StartPostfix(Notebook __instance)
        {
            __instance.StartCoroutine(Wait(__instance));
        }

        public static IEnumerator Wait(Notebook notebook)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject gameObject = new GameObject("ON_CustomNotebookDisplay");
            gameObject.transform.SetParent(notebook.transform);
            gameObject.AddComponent<ON_CustomNotebookDisplay>();
            yield break;
        }

        [HarmonyPatch("Hide", new Type[] { typeof(bool) })]
        [HarmonyPostfix]
        public static void HidePostfix(Notebook __instance, bool hide)
        {
            ON_CustomNotebookDisplay custom = __instance.GetComponentInChildren<ON_CustomNotebookDisplay>();
            if (custom != null)
            {
                __instance.transform.Find("ItemSprite").gameObject.SetActive(false);
                custom.gameObject.SetActive(!hide);
            }
        }
    }
}
