
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using System.Text.RegularExpressions;
using System.Reflection;

[CustomEditor(typeof(UltimateParallax))]
public class UltimateParallaxInspector : Editor
{
    Color m_Color;
    public Camera cam;
    public SpriteRenderer spriteR;
    public SpriteRenderer spriteToAdd;
    UltimateParallax parallax;
    float lineHeight;
    float lineHeightSpace;
    bool showInfo;
    Texture2D titleLabel;
    double timer;
    double timerHelper;
    public string[] options = new string[] { "Move up", "Move down" };
    float bHeight = 20;
    float bWidth = 20;
    public Vector2 scrollView;
    int previousFocus;

    Texture2D defaultHelpBoxBG;

    //
    static bool simulated;
    static List<float> savedSpeedH;
    static List<float> savedSpeedV;
    static List<float> savedSelfSpeed;
    static List<bool> savedAlwaysMove;

    static List<Texture2D> icons;

    public void OnEnable()
    {
        if (target == null)
        {
            return;
        }
        parallax = (UltimateParallax)target;

        if (parallax.layers == null)
        {
            parallax.layers = new List<Element>();
        }
    }

    private static void ShowNotification(string notification)
    {
        GetMainGameView().ShowNotification(new GUIContent(notification));
    }

    private static EditorWindow GetMainGameView()
    {
        var assembly = typeof(EditorWindow).Assembly;
        var type = assembly.GetType("UnityEditor.GameView");
        var gameView = EditorWindow.GetWindow(type);
        return gameView;
    }
    
    public override void OnInspectorGUI()
    {


        EditorGUILayout.Separator();
        Texture2D possibleBG = GetIcon("Background");

        if (EditorStyles.helpBox.normal.background != possibleBG)
        {
            defaultHelpBoxBG = EditorStyles.helpBox.normal.background;
        }

        EditorStyles.helpBox.normal.background = possibleBG;

        if (Application.isPlaying)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            string lb = parallax.simulate ? "preview" : "playing";
            GUILayout.Label(GetIcon(lb));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();


            if (!parallax.simulate)
            {

                if (!parallax.cam)
                {
                    ShowNotification("Main Camera is not set!");
                }
                return;
            }

        }

        GUIStyle sd = new GUIStyle(EditorStyles.toolbarButton);
        sd.fontStyle = FontStyle.Bold;
        sd.fontSize = 16;

        GUIStyle sF = new GUIStyle(EditorStyles.largeLabel);
        sF.normal.textColor = Color.black;
        sF.fontStyle = FontStyle.Bold;
        GUI.color = Color.white;



        if (parallax.cam == null && Camera.main != null)
        {
            parallax.cam = Camera.main;
            parallax.cam.orthographic = true;
        }
        if (parallax.showSettings && !Application.isPlaying)
        {

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Settings", EditorStyles.boldLabel);

            parallax.cam = (Camera)EditorGUILayout.ObjectField(new GUIContent("Main Camera", "Your Main Camera. Example: The Camera that follows your main character"), parallax.cam, typeof(Camera), true);
            if (parallax.cam == null)
            {
                EditorGUILayout.HelpBox("Set your Main Camera!", MessageType.Warning);
            }

            parallax.layerOffset = EditorGUILayout.IntField(new GUIContent("Layer order offset", "Lower this value if the parallax is in front of anything that is not part of the parallax"), parallax.layerOffset);
            parallax.previewVelocity = EditorGUILayout.Vector2Field(new GUIContent("Preview Speed", "Camera speed used in Preview Mode"), parallax.previewVelocity);

            GUILayout.EndVertical();
        }
        GUILayout.BeginHorizontal();

        if (!Application.isPlaying)
        {
            if (GUILayout.Button(new GUIContent(GetIcon("plus"), "Add New Layer"), EditorStyles.miniButtonLeft, GUILayout.Width(20), GUILayout.Height(20)))
            {
                NewLayer(false);
            }


            GUILayout.Label("Layers", EditorStyles.miniButtonRight, GUILayout.Height(bHeight));

            GUIStyle s = new GUIStyle();
            s.alignment = TextAnchor.MiddleCenter;
            s.wordWrap = true;

            if (GUILayout.Button(new GUIContent("Preview", "Start Preview Mode"), EditorStyles.miniButton, GUILayout.Height(bHeight)))
            {
                ShowNotification("PREVIEW MODE \n \n Changes made will be saved after previewing is finished.");
                parallax.simulate = true;
                EditorApplication.isPlaying = true;


            }
            if (parallax.showSettings)
            {
                GUI.color = Color.gray;
            }
            if (GUILayout.Button(new GUIContent(GetIcon("settings"), "Settings Toggle"), EditorStyles.miniButton, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
            {
                parallax.showSettings = !parallax.showSettings;
            }
            GUI.color = Color.white;
        }
        else
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            parallax.previewVelocity = EditorGUILayout.Vector2Field("Preview Speed", parallax.previewVelocity);
            GUILayout.EndHorizontal();

        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();

        if (parallax.layers != null)
        {
            if (parallax.layers.Count == 0)
            {
                EditorGUILayout.HelpBox("Click on the '+' sign to add a new layer.", MessageType.Info);
            }

            SavePlayModeChanges();

            for (int i = 0; i < parallax.layers.Count; i++)
            {
                if (parallax.layers[i] == null) { return; }

                var curLayer = parallax.layers[i];

                if (Application.isPlaying)
                {
                    savedSpeedH[i] = curLayer.speedH;
                    savedSpeedV[i] = curLayer.speedV;
                    savedSelfSpeed[i] = curLayer.selfSpeed;
                    savedAlwaysMove[i] = curLayer.alwaysMove;
                }

                if (!Application.isPlaying)
                {
                    if (curLayer.sprites != null)
                    {
                        for (int iS = 0; iS < curLayer.sprites.Count; iS++)
                        {
                            SpriteRenderer curIS = curLayer.sprites[iS];
                            if (curIS != null)
                            {
                                Transform curIST = curIS.transform;

                                curIS.sortingOrder = parallax.layerOffset - i;

                                if (curIST.parent == parallax.transform)
                                {
                                    curIST.localPosition = new Vector3(curIST.localPosition.x, curIST.localPosition.y, curIS.sortingOrder * -1 + 1);
                                }
                                curIST.hideFlags = HideFlags.HideInHierarchy;
                            }

                        }
                        for (int iS = curLayer.sprites.Count - 1; iS > 0; iS--)
                        {
                            if (curLayer.sprites[iS] == null)
                            {
                                curLayer.sprites.RemoveAt(iS);
                                curLayer.sprites = curLayer.sprites.Distinct().ToList();
                            }

                        }
                    }
                }

                curLayer.currentColor = Color.Lerp(curLayer.currentColor, parallax.layersColor, 0.01f);

                if (!Application.isPlaying)
                {
                    GUI.color = curLayer.currentColor;
                }
                else
                {
                    if (curLayer.staticLayer)
                    {
                        GUI.color = (Color)parallax.layersColor * 0.5f;
                    }
                    else
                    {
                        GUI.color = curLayer.currentColor;
                    }
                }


                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUI.color = Color.white;

                GUILayout.BeginHorizontal();

                GUIStyle sWhite = new GUIStyle(EditorStyles.textField);
                sWhite.normal.textColor = Color.black;

                GUIStyle sLayer = new GUIStyle(EditorStyles.textField);
                sLayer.normal.textColor = Color.white;
                sLayer.fontStyle = FontStyle.Bold;

                sF.fontSize = 9;

                GUI.color = Color.white;

                GUIContent eyeIcon = new GUIContent(GetIcon("eye_opened"), "Hide Layer");
                if (curLayer.hidden)
                {
                    eyeIcon = new GUIContent(GetIcon("eye_closed"), "Show Layer");
                }

                if (GUILayout.Button(eyeIcon, EditorStyles.label, GUILayout.Width(bWidth)))
                {
                    curLayer.hidden = !curLayer.hidden;
                    if (curLayer.hidden)
                    {
                        curLayer.currentColor = Color.gray;

                    }
                    else
                    {
                        curLayer.currentColor = (Color)parallax.layersColor * 1.5f;
                    }
                }
                GUI.color = Color.white;
                ToggleLayer(curLayer, curLayer.hidden);


                GUIContent bArrow = new GUIContent(GetIcon("arrow_down"), "Collapse");
                if (curLayer.showWindow)
                {
                    if (GUILayout.Button(bArrow, EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
                    {
                        curLayer.showWindow = false;
                        GUI.FocusControl("");
                        curLayer.currentColor = (Color)parallax.layersColor * 1.5f;


                    }
                }
                else
                {
                    bArrow = new GUIContent(GetIcon("arrow_right"), "Show Content");
                    if (GUILayout.Button(bArrow, EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
                    {
                        curLayer.showWindow = true;
                        GUI.FocusControl("");
                        curLayer.currentColor = (Color)parallax.layersColor / 1.5f;
                    }
                }
                if (!Application.isPlaying)
                {
                    curLayer.name = EditorGUILayout.TextField(curLayer.name, sWhite);

                    if (GUILayout.Button(new GUIContent(GetIcon("copy"), "Add a Copy of this Layer"), EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
                    {
                        CopyLayer(curLayer, i);

                    }

                    GUIContent bMoveUp = new GUIContent(GetIcon("arrow1_up"), "Move Layer Up");
                    if (i == 0)
                    {
                        bMoveUp = new GUIContent(GetIcon("arrow1_up_disabled"));
                    }
                    if (GUILayout.Button(bMoveUp, EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
                    {
                        if (i != 0)
                        {
                            OnMove(i, true, true);
                        }
                    }
                    GUIContent bMoveDown = new GUIContent(GetIcon("arrow1_down"), "Move Layer Down");
                    if (i == parallax.layers.Count - 1)
                    {
                        bMoveDown = new GUIContent(GetIcon("arrow1_down_disabled"));
                    }
                    if (GUILayout.Button(bMoveDown, EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
                    {
                        if (i != parallax.layers.Count - 1)
                        {
                            OnMove(i, false, true);
                        }
                    }

                    if (GUILayout.Button(new GUIContent(GetIcon("close"), "Delete Layer"), EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight)))
                    {
                        curLayer.currentColor = Color.yellow;
                        if (EditorUtility.DisplayDialog("Remove '" + curLayer.name + "'?",
                        "Are you sure you want to remove '" + curLayer.name + "' layer?", "Remove", "Cancel"))
                        {
                            RemoveLayer(i);

                            i = 0;
                            return;
                        }
                    }
                }
                else
                {
                    GUI.color = Color.white;
                    GUILayout.Label(curLayer.name, EditorStyles.objectField);
                }
                GUILayout.EndHorizontal();
                GUI.color = Color.white;

                if (curLayer.showWindow)
                {
                    if (!Application.isPlaying)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(12);
                        curLayer.showElements = EditorGUILayout.Foldout(curLayer.showElements, "Elements");
                        //EditorGUILayout.LabelField("Elements", EditorStyles.boldLabel, GUILayout.Width(bWidth * 4));

                        GUILayout.EndHorizontal();


                        if (curLayer.showElements)
                        {
                            if (curLayer.sprites != null)
                            {
                                if (curLayer.sprites.Count > 0 && curLayer.sprites[0] == null)
                                {
                                    curLayer.sprites.RemoveAt(0);
                                }

                                ElementLayout(curLayer);
                            }

                            AddSpriteSpace(curLayer);
                        }


                        GUILayout.EndVertical();
                    }

                    GUIStyle newFoldoutStyle = EditorStyles.foldout;
                    newFoldoutStyle.fontStyle = FontStyle.Bold;

                    if (!Application.isPlaying)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(12);
                        curLayer.showSettings = EditorGUILayout.Foldout(curLayer.showSettings, "Initial Settings", newFoldoutStyle);
                        //EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                        GUILayout.EndHorizontal();


                        if (curLayer.showSettings)
                        {

                            curLayer.startPosition = EditorGUILayout.Vector2Field("Start Position", curLayer.startPosition);
                            curLayer.scale = EditorGUILayout.FloatField("Elements Scale", curLayer.scale);

                            curLayer.tint = EditorGUILayout.ColorField("Elements Tint", curLayer.tint);

                            int rLayer = 0;
                            Material m = null;
                            if (curLayer.sprites.Count > 0)
                            {
                                SpriteRenderer curSR = curLayer.sprites[0];
                                m = (Material)EditorGUILayout.ObjectField("Material", curSR.sharedMaterial, typeof(Material), false);
                                rLayer = EditorGUILayout.LayerField("Rendering Layer", curSR.gameObject.layer);
                                
                            }

                            foreach (SpriteRenderer item in curLayer.sprites)
                            {
                                item.gameObject.layer = rLayer;
                                item.color = curLayer.tint;
                                item.material = m;

                                if (item == curLayer.sprites[0])
                                {
                                    item.transform.localPosition = new Vector3(curLayer.startPosition.x, curLayer.startPosition.y, item.transform.localPosition.z);
                                    item.transform.localScale = Vector3.one * curLayer.scale;
                                }
                                else if (item != null)
                                {
                                    item.transform.localScale = Vector3.one;
                                }
                            }
                        }

                        GUILayout.EndVertical();
                    }
                    GUI.color = Color.white;
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(12);
                    curLayer.showBehaviourSettings = EditorGUILayout.Foldout(curLayer.showBehaviourSettings, "Movement Settings");
                    GUILayout.EndHorizontal();
                    //EditorGUILayout.LabelField("Behaviour Settings", EditorStyles.boldLabel);
                    if (curLayer.showBehaviourSettings)
                    {


                        if (!curLayer.staticLayer)
                        {
                            LayerIsNotStatic(curLayer);

                        }
                        else
                        {
                            if (!Application.isPlaying)
                            {
                                curLayer.staticLayer = GUILayout.Toggle(curLayer.staticLayer, "Static");
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("Layer is Static", MessageType.Info);
                            }
                        }

                    }
                    //GUILayout.EndVertical();
                    GUILayout.EndVertical();

                }
                GUILayout.EndVertical();

                if (!Application.isPlaying)
                {

                    if (curLayer.orientation == Element.Orientation.Horizontal)
                    {
                        OrganizeElementsHorizontal(curLayer);
                    }
                    else
                    {
                        OrganizeElementsVertical(curLayer);
                    }
                }
            }

        }
        EditorStyles.helpBox.normal.background = defaultHelpBoxBG;

        PrefabUtility.RecordPrefabInstancePropertyModifications(parallax);
    }

    //In Layer Functions
    void ToggleLayer(Element curLayer, bool hide)
    {
        if (curLayer == null) { return; }

        curLayer.hidden = hide;

        if (curLayer.sprites != null)
        {
            foreach (SpriteRenderer item in curLayer.sprites)
            {
                if (item == null) { return; }
                item.enabled = !hide;
            }
        }
    }
    void LayerIsNotStatic(Element curLayer)
    {
        GUILayout.BeginHorizontal();

        if (!Application.isPlaying)
        {
            curLayer.staticLayer = GUILayout.Toggle(curLayer.staticLayer, "Static");
            if (curLayer.sprites.Count > 1)
            {
                curLayer.replicable = GUILayout.Toggle(curLayer.replicable, new GUIContent("Infinite", "Uncheck if your Layer has an ending point"));
            }
        }
        curLayer.alwaysMove = GUILayout.Toggle(curLayer.alwaysMove, new GUIContent("Always Move", "Very useful for Layers that should move by itself"));

        GUILayout.EndHorizontal();

        if (!Application.isPlaying)
        {
            curLayer.orientation = (Element.Orientation)EditorGUILayout.EnumPopup(new GUIContent("Orientation", "Determine if Layer is Horizontal or Vertical"), curLayer.orientation);
            if (curLayer.orientation == Element.Orientation.Vertical)
            {
                curLayer.anchorV = (Element.AnchorV)EditorGUILayout.EnumPopup(new GUIContent("Anchor", "Anchor Layer to the Left, Right, Both or Nothing"), curLayer.anchorV);
            }
            else
            {
                curLayer.anchorH = (Element.AnchorH)EditorGUILayout.EnumPopup(new GUIContent("Anchor", "Anchor Layer to the Bottom, Top, Both or Nothing"), curLayer.anchorH);
            }
        }
        curLayer.speedH = EditorGUILayout.FloatField(new GUIContent("Speed Factor (%)", "100% is equals the negative Camera velocity"), curLayer.speedH);
        curLayer.speedH = Mathf.Clamp(curLayer.speedH, 0, float.MaxValue);

        GUIContent _labelText = new GUIContent("Vertical Factor", "Relative to 'Speed Factor'. 1 is -100%");

        if (curLayer.orientation == Element.Orientation.Vertical)
        {
            _labelText.text = "Horizontal Factor";
        }

        curLayer.speedV = EditorGUILayout.FloatField(_labelText, curLayer.speedV);


        if (curLayer.alwaysMove)
        {
            curLayer.selfSpeed = EditorGUILayout.FloatField("Self Velocity", curLayer.selfSpeed);
        }

    }
    void AddSpriteSpace(Element curLayer)
    {

        GUI.color = Color.white;

        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUI.color = Color.white;

        GUILayout.Label(new GUIContent(GetIcon("plus"), "Drag and drop a Texture to add a new element"), EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight - 4));

        Sprite newSprite = (Sprite)EditorGUILayout.ObjectField(null, typeof(Sprite), false);

        GUI.color = Color.white;


        if (newSprite)
        {
            GameObject newEl = new GameObject(newSprite.name, typeof(SpriteRenderer));
            newEl.GetComponent<SpriteRenderer>().sprite = newSprite;
            AddSprite(curLayer, newEl.GetComponent<SpriteRenderer>(), false);
        }

        GUILayout.EndHorizontal();

        GUI.color = Color.white;
    }
    void OrganizeElementsHorizontal(Element curLayer)
    {

        for (int i = 0; i < curLayer.sprites.Count; i++)
        {
            if (i == 0)
            {
                curLayer.sprites[i].transform.parent = parallax.transform;
            }
            if (i > 0)
            {
                Transform _first = curLayer.sprites[i].transform;
                Transform _last = curLayer.sprites[i - 1].transform;

                float firstOffset = _first.GetComponent<SpriteRenderer>().sprite.bounds.size.x * _first.localScale.x;
                float lastOffset = _last.GetComponent<SpriteRenderer>().sprite.bounds.size.x * _first.localScale.x;

                float _xOffset = firstOffset / 2 + lastOffset / 2;

                _first.parent = curLayer.sprites[0].transform;

                if (i > 1)
                {
                    _first.localPosition = new Vector3(_last.localPosition.x + _xOffset, 0, 0);
                }
                else
                {
                    _first.localPosition = new Vector3(_xOffset, 0, 0);
                }



            }
        }
    }

    void OrganizeElementsVertical(Element curLayer)
    {

        for (int i = 0; i < curLayer.sprites.Count; i++)
        {
            if (i == 0)
            {
                curLayer.sprites[i].transform.parent = parallax.transform;
            }
            if (i > 0)
            {
                Transform _first = curLayer.sprites[i].transform;
                Transform _last = curLayer.sprites[i - 1].transform;

                float firstOffset = _first.GetComponent<SpriteRenderer>().sprite.bounds.size.y * _first.localScale.y;
                float lastOffset = _last.GetComponent<SpriteRenderer>().sprite.bounds.size.y * _first.localScale.y;

                float _xOffset = firstOffset / 2 + lastOffset / 2;

                _first.parent = curLayer.sprites[0].transform;

                if (i > 1)
                {
                    _first.localPosition = new Vector3(0, _last.localPosition.y + _xOffset, 0);
                }
                else
                {
                    _first.localPosition = new Vector3(0, _xOffset, 0);
                }



            }
        }
    }
    void CopyLayer(Element curLayer, int index)
    {
        NewLayer(false);

        var addedLayer = parallax.layers[parallax.layers.Count - 1];
        addedLayer.name = curLayer.name + " - (Copy)";
        addedLayer.startPosition = curLayer.startPosition;
        addedLayer.tint = curLayer.tint;
        addedLayer.scale = curLayer.scale;
        addedLayer.orientation = curLayer.orientation;
        addedLayer.staticLayer = curLayer.staticLayer;
        addedLayer.replicable = curLayer.replicable;
        addedLayer.anchorH = curLayer.anchorH;
        addedLayer.anchorV = curLayer.anchorV;
        addedLayer.alwaysMove = curLayer.alwaysMove;

        addedLayer.selfSpeed = curLayer.selfSpeed;

        addedLayer.speedH = curLayer.speedH;
        addedLayer.speedV = curLayer.speedV;

        addedLayer.sprites = new List<SpriteRenderer>();

        if (curLayer.sprites != null)
        {
            if (curLayer.sprites.Count > 0)
            {
                if (curLayer.sprites[0] != null)
                {
                    SpriteRenderer el = Instantiate(curLayer.sprites[0]) as SpriteRenderer;

                    AddSprite(parallax.layers[parallax.layers.Count - 1], el, false);
                }
            }
        }
        int iHelper = 1;
        if (index != parallax.layers.Count - 2)
        {

            for (int i1 = index + 2; i1 < parallax.layers.Count; i1++)
            {
                int iToMove = parallax.layers.Count - iHelper;

                if (iToMove != 0)
                {
                    OnMove(iToMove, true, false);
                }
                iHelper++;

            }

        }

    }
    void ElementLayout(Element curLayer)
    {
        for (int s = 0; s < curLayer.sprites.Count; s++)
        {

            if (curLayer.sprites.Count > 0)
            {
                SpriteRenderer currentSprite = curLayer.sprites[s];

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (GUILayout.Button(new GUIContent(GetIcon("minus"), "Delete Element"), EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight - 4)))
                {
                    RemoveSprite(curLayer, s);
                    s = 0;
                    return;
                }
                EditorGUILayout.ObjectField(curLayer.sprites[s].sprite, typeof(Sprite), false, GUILayout.Height(bHeight - 4));


                if (GUILayout.Button(new GUIContent(GetIcon("edit"), "Edit Element"), EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight - 4)))
                {
                    GenericMenu menu = new GenericMenu();

                    object c = curLayer.sprites[s];
                    menu.AddDisabledItem(new GUIContent("Element"));
                    menu.AddItem(new GUIContent("Duplicate"), false, OnDuplicateSprite, currentSprite);
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Sprite Renderer"));
                    menu.AddItem(new GUIContent("Flip Horizontal"), false, OnInvertRenderX, currentSprite);
                    menu.AddItem(new GUIContent("Flip Vertical"), false, OnInvertRenderY, currentSprite);

                    menu.ShowAsContext();
                    return;
                }
                GUIContent bMoveUp = new GUIContent(GetIcon("arrow1_up_disabled"), "Move Element Up");
                if (s == 0)
                {
                    bMoveUp = new GUIContent(GetIcon("arrow1_up"));
                }
                if (GUILayout.Button(bMoveUp, EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight - 4)))
                {
                    if (s != 0)
                    {
                        OnMoveSprite(curLayer, s, true);
                    }
                }
                GUIContent bMoveDown = new GUIContent(GetIcon("arrow1_down_disabled"), "Move Element Down");
                if (s == curLayer.sprites.Count - 1)
                {
                    bMoveDown = new GUIContent(GetIcon("arrow1_down"));
                }
                if (GUILayout.Button(bMoveDown, EditorStyles.label, GUILayout.Width(bWidth), GUILayout.Height(bHeight - 4)))
                {
                    if (s != curLayer.sprites.Count - 1)
                    {
                        if (s == 0)
                        {
                            OnMoveSprite(curLayer, s + 1, true);
                        }
                        else
                        {
                            OnMoveSprite(curLayer, s, false);
                        }
                    }
                }

                GUILayout.EndVertical();
            }
        }
    }
    void OnDuplicateSprite(object sprite)
    {
        SpriteRenderer realSprite = (SpriteRenderer)sprite;

        Element curLayer = null;
        for (int i = 0; i < parallax.layers.Count; i++)
        {
            curLayer = parallax.layers[i];
            for (int i1 = 0; i1 < curLayer.sprites.Count; i1++)
            {
                if (realSprite == curLayer.sprites[i1])
                {
                    curLayer = parallax.layers[i];
                    i = parallax.layers.Count;
                    i1 = curLayer.sprites.Count;
                }
            }
        }
        if (curLayer != null)
        {
            CopySprite(curLayer, realSprite);
        }
    }
    void OnInvertRenderX(object sprite)
    {
        SpriteRenderer realSprite = (SpriteRenderer)sprite;
        realSprite.flipX = !realSprite.flipX;
    }
    void OnInvertRenderY(object sprite)
    {
        SpriteRenderer realSprite = (SpriteRenderer)sprite;
        realSprite.flipY = !realSprite.flipY;
    }
    void OnMoveSprite(Element curLayer, int i, bool value)
    {
        int _iNext;
        if (value)
        {
            _iNext = i - 1;
        }
        else
        {
            _iNext = i + 1;
        }

        SpriteRenderer _Cur = curLayer.sprites[i];
        SpriteRenderer _Next = curLayer.sprites[_iNext];

        curLayer.sprites[i].transform.position = _Next.transform.position;
        curLayer.sprites[_iNext].transform.position = _Cur.transform.position;

        curLayer.sprites[i] = _Next;
        curLayer.sprites[_iNext] = _Cur;



    }
    void CopySprite(Element curLayer, SpriteRenderer old)
    {
        SpriteRenderer copy = Instantiate(old) as SpriteRenderer;

        copy.name = copy.sprite.name + " (" + curLayer.sprites.Count + ") ";
        for (int i = copy.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(copy.transform.GetChild(i).gameObject);
        }
        AddSprite(curLayer, copy, false);
    }
    void AddSprite(Element curLayer, SpriteRenderer add, bool changeLayerName)
    {
        if (curLayer.sprites == null)
        {
            curLayer.sprites = new List<SpriteRenderer>();

        }

        bool canAdd = true;
        for (int i = 0; i < parallax.layers.Count; i++)
        {
            Element cLayer = parallax.layers[i];
            for (int i1 = 0; i1 < cLayer.sprites.Count; i1++)
            {
                if (cLayer.sprites[i1].transform == add)
                {
                    canAdd = false;
                }
            }
        }

        if (add == null) { return; }

        if (canAdd)
        {
            curLayer.sprites.Add(new SpriteRenderer());


            if (curLayer.sprites.Count > 0)
            {
                curLayer.sprites[curLayer.sprites.Count - 1] = add;
            }
        }


        if (curLayer.sprites[0] == add)
        {
            add.transform.parent = parallax.transform;
        }
        else if (curLayer.sprites[0] != null)
        {
            add.transform.parent = curLayer.sprites[0].transform;
        }
        if (changeLayerName)
        {
            string input = add.name;

            input = Regex.Replace(input, @"[\d-]", string.Empty);

            curLayer.name = input;
        }

        if (add.transform.childCount > 0)
        {
            for (int i = 0; i < add.transform.childCount; i++)
            {
                SpriteRenderer childS = add.transform.GetChild(i).GetComponent<SpriteRenderer>();

                if (childS != null)
                {
                    AddSprite(curLayer, childS, false);
                }

            }
        }
        curLayer.sprites = curLayer.sprites.Distinct().ToList();
    }
    void RemoveSprite(Element curLayer, int index1)
    {

        SpriteRenderer sprT = curLayer.sprites[index1];

        curLayer.sprites.RemoveAt(index1);

        for (int i = 0; i < curLayer.sprites.Count; i++)
        {
            if (curLayer.sprites[i] != null)
            {
                curLayer.sprites[i].transform.parent = parallax.transform;

                for (int i1 = i; i1 < curLayer.sprites.Count; i1++)
                {
                    curLayer.sprites[i1].transform.parent = curLayer.sprites[i].transform;
                }
                i = curLayer.sprites.Count;
            }
        }

        if (sprT != null)
        {
            DestroyImmediate(sprT.gameObject);
        }

    }
    void OnMove(int i, bool value, bool highlight)
    {
        GUI.FocusControl("");
        int _iNext;
        if (value)
        {
            _iNext = i - 1;
        }
        else
        {
            _iNext = i + 1;
        }

        Element _Cur = parallax.layers[i];
        Element _Next = parallax.layers[_iNext];

        parallax.layers[i] = null;
        parallax.layers[_iNext] = null;



        parallax.layers[i] = _Next;
        parallax.layers[_iNext] = _Cur;

        if (highlight)
        {
            _Cur.currentColor = (Color)parallax.layersColor * 1.5f;
        }
    }
    void RemoveLayer(int index)
    {
        GUI.FocusControl("");
        ToggleLayer(parallax.layers[index], false);
        if (parallax.layers[index].sprites != null)
        {
            if (parallax.layers[index].sprites.Count > 0)
            {
                if (parallax.layers[index].sprites[0] != null)
                {
                    DestroyImmediate(parallax.layers[index].sprites[0].gameObject, false);
                }
            }
        }
        parallax.layers.RemoveAt(index);
    }

    void NewLayer(bool renameLayer)
    {
        if (parallax.layers == null)
        {
            parallax.layers = new List<Element>();
        }

        parallax.layers.Add(new Element());


        int lastIndex = parallax.layers.Count - 1;

        string toName = "Layer - " + (lastIndex).ToString();

        parallax.layers[lastIndex].sprites = new List<SpriteRenderer>();
        parallax.layers[lastIndex].name = toName;
        parallax.layers[lastIndex].showWindow = false;
        parallax.layers[lastIndex].replicable = true;
        if (lastIndex > 0)
        {
            float s = parallax.layers[lastIndex].speedH = parallax.layers[lastIndex - 1].speedH;
            parallax.layers[lastIndex].speedH = parallax.layers[lastIndex - 1].speedH - (20 / ((lastIndex + 1) / 2));

            if (s <= 0)
            {
                parallax.layers[lastIndex].speedH = parallax.layers[lastIndex - 1].speedH = 0;
            }
        }
    }
    void MarkAsDirty()
    {
        if (!Application.isPlaying)
        {
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }

    //Util
    Texture2D GetIcon(string name)
    {
        if (icons == null)
        {
            icons = new List<Texture2D>();
        }
        Texture2D possibleIcon = null;
        for (int i = 0; i < icons.Count; i++)
        {
            if (icons[i].name == "UltimateParallax_ico_" + name)
            {
                return possibleIcon = icons[i];
            }
        }

        if (possibleIcon)
        {
            return possibleIcon;
        }

        string[] _path = AssetDatabase.FindAssets("UltimateParallax_ico_" + name);

        if (_path.Length > 0)
        {

            if (_path[0] != "")
            {
                icons.Add((Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(_path[0]), typeof(Texture2D)));
                return icons[icons.Count - 1];

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    void SavePlayModeChanges()
    {
        if (Application.isPlaying)
        {
            if (parallax.simulate)
            {
                if (!simulated)
                {
                    savedSpeedH = new List<float>();
                    savedSpeedV = new List<float>();
                    savedAlwaysMove = new List<bool>();
                    savedSelfSpeed = new List<float>();

                    for (int i = 0; i < parallax.layers.Count; i++)
                    {
                        Element _curL = parallax.layers[i];
                        savedSpeedH.Add(_curL.speedH);
                        savedSpeedV.Add(_curL.speedV);
                        savedAlwaysMove.Add(_curL.alwaysMove);
                        savedSelfSpeed.Add(_curL.selfSpeed);
                    }
                    simulated = true;
                }
            }
        }
        else if (simulated)
        {
            for (int i = 0; i < parallax.layers.Count; i++)
            {
                Element _curL = parallax.layers[i];
                _curL.speedH = savedSpeedH[i];
                _curL.speedV = savedSpeedV[i];
                _curL.alwaysMove = savedAlwaysMove[i];
                _curL.selfSpeed = savedSelfSpeed[i];
            }
            ShowNotification("PREVIEW MODE \n \n Changes successfully saved!");
            parallax.simulate = false;
            simulated = false;
            PrefabUtility.RecordPrefabInstancePropertyModifications(parallax);
        }
    }

    [MenuItem("GameObject/Ultimate Parallax")]
    static void AddParallaxToScene()
    {
        UltimateParallax _parallax = GameObject.FindObjectOfType<UltimateParallax>();
        if (_parallax == null)
        {
            GameObject parallaxGO = new GameObject("Ultimate Parallax", typeof(UltimateParallax));
            ShowNotification(parallaxGO.name + " added!");
            Selection.activeTransform = parallaxGO.transform;
        }
        else
        {
            ShowNotification("Can't add more than one Ultimate Parallax!");
        }

    }

}