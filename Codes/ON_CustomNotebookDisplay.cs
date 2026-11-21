using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace OldNotebookTextures
{
    public class ON_CustomNotebookDisplay : MonoBehaviour
    {
        public SpriteRenderer? Renderer;
        public Notebook? Notebook;
        public float timer = 0f;
        public string key = "ON_NotebookAnim_Sheet_";
        public int notebookColor = 0;

        public void Awake()
        {
            if (transform.parent.gameObject.TryGetComponent(out Notebook a))
            {
                Notebook = a;
            }
            else
            {
                BasePlugin.Log("No Notebook?");
                Destroy(gameObject);
                return;
            }
            gameObject.layer = LayerMask.NameToLayer("Billboard");
            timer = 0f;
            if (gameObject.TryGetComponent(out SpriteRenderer b))
            {
                Renderer = b;
            }
            else
            {
                Renderer = gameObject.AddComponent<SpriteRenderer>();
                Renderer.material = Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(x => x.name == "SpriteStandard_Billboard");
                string notebookName = Notebook.transform.Find("ItemSprite").gameObject.GetComponent<SpriteRenderer>().sprite.name;
                if (notebookName.StartsWith("NotebookAnim_Sheet_"))
                {
                    int m = int.Parse(notebookName[19..]);
                    for (int c = 0; c <= 6; c++)
                    {
                        if (m >= 0 + 8 * c && m <= 7 + 8 * c)
                        {
                            notebookColor = c;
                            break;
                        }
                    }
                }
                else if (notebookName.StartsWith("NotebookSheet_"))
                {
                    int m = int.Parse(notebookName[14..]);
                    for (int c = 0; c <= 6; c++)
                    {
                        if (m == c)
                        {
                            notebookColor = c;
                            break;
                        }
                    }
                }
                else
                {
                    BasePlugin.Log("Parse Failed?");
                    Destroy(gameObject);
                    return;
                }
                Notebook.transform.Find("ItemSprite").gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (BasePlugin.Instance == null || Notebook == null || Renderer == null)
            {
                return;
            }
            if (BasePlugin.Instance.ConfigNotebookStyle == null || BasePlugin.AllSprites == null)
            {
                return;
            }
            NotebookStyle n = BasePlugin.Instance.ConfigNotebookStyle.Value;

            int i;
            if (n == NotebookStyle.Plus)
            {
                if (timer < 1f)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer %= 1f;
                }
                i = (int)(timer * 16f);
            }
            else
            {
                if (timer < 2f)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer %= 2f;
                }
                i = (int)(timer * 8f);
            }
            i = math.min(i, 15);

            if (n == NotebookStyle.Classic)
            {
                transform.localPosition = new Vector3(0f, 0.5f + math.sin(Time.time * math.PI / 2f) * 0.25f, 0f);
                transform.localScale = Vector3.one * 2f;
                key = "ON_NotebookSheet_";
                string m = key + notebookColor.ToString();
                if (BasePlugin.AllSprites.ContainsKey(m))
                {
                    Renderer.sprite = BasePlugin.AllSprites[m];
                }
                else
                {
                    BasePlugin.Log($"No {m} ?", 2);
                }
            }
            else if (n == NotebookStyle.ClassicRemastered)
            {
                transform.localPosition = new Vector3(0f, 0f + math.sin(Time.time * math.PI / 1.5f) * 0.25f, 0f);
                transform.localScale = Vector3.one;
                key = "ON_NotebookSheet_";
                string m = key + notebookColor.ToString();
                if (BasePlugin.AllSprites.ContainsKey(m))
                {
                    Renderer.sprite = BasePlugin.AllSprites[m];
                }
                else
                {
                    BasePlugin.Log($"No {m} ?", 2);
                }
            }
            else if (n == NotebookStyle.OldPlus)
            {
                transform.localPosition = new Vector3(0f, 0f, 0f);
                transform.localScale = Vector3.one;
                key = "ON_NotebookAnimOld_Sheet_";
                i %= 8;
                string m = key + (notebookColor * 8 + i).ToString();
                if (BasePlugin.AllSprites.ContainsKey(m))
                {
                    Renderer.sprite = BasePlugin.AllSprites[m];
                }
                else
                {
                    BasePlugin.Log($"No {m} ?", 2);
                }
            }
            else if (n == NotebookStyle.Plus)
            {
                transform.localPosition = new Vector3(0f, 0f, 0f);
                transform.localScale = Vector3.one;
                key = "ON_NotebookAnim_Sheet_";
                if (i > 7)
                {
                    i = 15 - i;
                }
                string m = key + (notebookColor * 8 + i).ToString();
                if (BasePlugin.AllSprites.ContainsKey(m))
                {
                    Renderer.sprite = BasePlugin.AllSprites[m];
                }
                else
                {
                    BasePlugin.Log($"No {m} ?", 2);
                }
            }
        }
    }
}
