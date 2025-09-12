using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class HoverTagManager : MonoBehaviour
{
    public static HoverTagManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] Canvas bubbleCanvas;
    [SerializeField] RectTransform bubblePanel;
    [SerializeField] TMP_Text bubbleComment;

    [Header("Settings")]
    [SerializeField] string hoverTag = "Hoverable";
    [SerializeField] Vector2 offset = new Vector2(20f, -20f);
    [SerializeField] float followSpeed = 10f;
    [SerializeField] float scaleSpeed = 10f;

    [Header("Scale Settings")]
    [SerializeField] Vector3 shownScale = Vector3.one;

    [Header("Tilt Settings")]
    [SerializeField] float maxTilt = 10f;
    [SerializeField] float tiltSpeed = 8f; 

    private Camera mainCam;
    private Vector2 targetPos;
    private Coroutine scaleRoutine;

    private Dictionary<string, string> profileComments = new Dictionary<string, string>()
    {
        { "WolfProfile", "Ulala" },
        { "ToniFowlerProfile", "Sarap" },
        { "BrocolliProfile", "Yummerz" },
        { "PoohProfile", "Hnggg" },
        { "CappucinaProfile", "Yummerz" },
        { "CrocodilloProfile", "Ulala" },
        { "CocoProfile", "Hnggg" },
        { "KapProfile", "Ulala" },
        { "DivoredChildProfile", "Oh.." },
        { "GamerBoyProfile", "Ulala" },
        { "MagicTomboyProfile", "Sarap" },
        { "TreeBrainrotProfile", "Yummerz" },
        { "JA_NormalProfile", "Sarap" },
        { "JA_TalikodProfile", "Ulala" },
        { "JA_UpoProfile", "Sarap" },
        { "JA_HandStandProfile", "Ulala" },
        { "JA_PerformativeProfile", "Ulala" },
        { "JA_EmoProfile", "ROCK!" },
        { "JA_DrStrangeProfile", "Yummerz" },
        { "JA_BumberoProfile", "Ulala" },
        { "JA_ZenitsuProfile", "Senpai" },
        { "JA_InosukeProfile", "Senpai" },
        { "JA_AkazaProfile", "Senpai" },
        { "JA_GiyuProfile", "Senpai" },
        { "JA_GojoProfile", "Senpai" },
        { "JA_SzaProfile", "Yummerz" },
        { "MisoProfile", "Ulala" },
        { "BlueProfile", "Hnggg" },
        { "AntProfile", "Oh.." },
        { "BuffLionProfile", "DADDY" },
        { "BadingProfile", "Ma'am?" },
        { "CaloocanProfile", "Ulala" },
        { "CapybaraProfile", "Uwu" },
        { "CatbirdProfile", "Ulala" },
        { "CrocBallerProfile", "Yummerz" },
        { "CryProfile", "Oh.." },
        { "DilaProfile", "Ulala" },
        { "DonkeyProfile", "Hngg" },
        { "GroupProfile", "wtf" },
        { "HmProfile", "Ulala" },
        { "InasalProfile", "Yummerz" },
        { "KalboProfile", "Sarap" },
        { "KimChiuProfile", "Sarap" },
        { "MainitProfile", "taina" },
        { "MinionProfile", "Oh.." },
        { "NinjaRobotProfile", "Ulala" },
        { "PirateProfile", "Hngg" },
        { "SquirrelProfile", "Oh.." },
        { "SarapProfile", "Sarap" },
        { "ShainaProfile", "Yummerz" },
        { "SpiderProfile", "Ulala" },
        { "StarsProfile", "Hngg" },
        { "TralaleloProfile", "Ulala" },
        { "WhiteProfile", "Talk" },
        { "White2Profile", "Dirty" },
        { "YasminProfile", "Sarap" },

    };

    private void Awake()
    {
        Instance = this;
        bubbleCanvas.enabled = false;
        mainCam = Camera.main;

        bubblePanel.localScale = new Vector3(shownScale.x, 0, shownScale.z);
    }

    private void Update()
    {
        DetectHover();
        SmoothFollow();
    }

    private void ProfileComment(GameObject hoveredObj)
    {
        string objName = hoveredObj.name;

        if (profileComments.TryGetValue(objName, out string comment))
        {
            bubbleComment.text = comment;
        }
        else
        {
            bubbleComment.text = "??"; 
        }
    }

    private void DetectHover()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag(hoverTag))
                {
                    if (!bubbleCanvas.enabled)
                    {
                        bubbleCanvas.enabled = true;
                    }

                    Vector2 pos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        bubbleCanvas.transform as RectTransform,
                        Input.mousePosition,
                        bubbleCanvas.worldCamera,
                        out pos
                    );
                    targetPos = pos + offset;

                    ProfileComment(hit.collider.gameObject);

                    if (scaleRoutine != null) StopCoroutine(scaleRoutine);
                    scaleRoutine = StartCoroutine(AnimateScaleY(shownScale.y));

                    return; 
                }
            }
        }

        if (bubbleCanvas.enabled)
        {
            if (scaleRoutine != null) StopCoroutine(scaleRoutine);
            scaleRoutine = StartCoroutine(AnimateScaleY(0));
        }
    }


    private void SmoothFollow()
    {
        if (bubbleCanvas.enabled)
        {
            Vector3 current = bubblePanel.localPosition;
            Vector3 target = new Vector3(targetPos.x, targetPos.y, 0);
            bubblePanel.localPosition = Vector3.Lerp(current, target, Time.deltaTime * followSpeed);

            Vector3 delta = target - current;
            float tilt = Mathf.Clamp(delta.x, -1f, 1f) * maxTilt;
            Quaternion targetRot = Quaternion.Euler(0, 0, -tilt);
            bubblePanel.localRotation = Quaternion.Lerp(bubblePanel.localRotation, targetRot, Time.deltaTime * tiltSpeed);
        }
    }

    private IEnumerator AnimateScaleY(float targetY)
    {
        Vector3 target = new Vector3(shownScale.x, targetY, shownScale.z);

        while (Mathf.Abs(bubblePanel.localScale.y - targetY) > 0.01f)
        {
            bubblePanel.localScale = Vector3.Lerp(bubblePanel.localScale, target, Time.deltaTime * scaleSpeed);
            yield return null;
        }

        bubblePanel.localScale = target;

        if (targetY == 0)
        {
            bubbleCanvas.enabled = false;
        }
    }

}
