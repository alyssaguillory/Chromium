using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UltimateParallax : MonoBehaviour
{
    [HideInInspector]
    public Camera cam;
    public List<Element> layers;
    Vector2 lastPos;
    public Vector2 velocity;
    public Vector2 previewVelocity;
    public int layerOffset = -10;
    public Color32 layersColor = new Color32(68, 71, 93, 255);
    public string path;

    public bool simulate;
    Vector3 curPosition;
    float camWidth;
    public bool showSettings;

    public bool hint0;

    void Awake()
    {
        Simulation();
    }
    void Simulation()
    {

        if (simulate)
        {
            string key = "_DO_NOT_DESTROY";
            foreach (var layer in layers)
            {
                foreach (var sprite in layer.sprites)
                {
                    sprite.name += key;
                }
            }

            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            for (int i = 0; i < allObjects.Length; i++)
            {
                GameObject curGO = allObjects[i];
                if (!curGO.name.EndsWith(key))
                {
                    if (curGO != gameObject)
                    {
                        if (cam)
                        {
                            if (curGO != cam.gameObject)
                            {
                                Destroy(curGO);
                            }
                        }
                        else
                        {
                            Destroy(curGO);
                        }
                    }
                }
            }

            GameObject previewCam = new GameObject("Preview Camera", typeof(Camera));
            Camera previewCamC = previewCam.GetComponent<Camera>();

            previewCamC.orthographic = true;
            if (cam != null)
            {
                previewCamC.orthographicSize = cam.orthographicSize;
                Destroy(cam.gameObject);
            }
            cam = previewCamC;
        }
    }
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        if (cam == null) { return; }


        float height = cam.orthographicSize * 2.0f;
        camWidth = height * cam.aspect;
        Debug.Log(camWidth);
        for (int e = 0; e < layers.Count; e++)
        {
            layers[e].camHeight = height;
            layers[e].camWidth = camWidth;
            layers[e].Initalize(transform, e);

        }
        transform.parent = cam.transform;
        Update();
        lastPos = transform.position;
        transform.localPosition = Vector3.zero + cam.transform.forward * 1;




    }

    void Update()
    {
        if (cam == null) { return; }
        

        if (simulate)
        {
            Previewing();
        }

        CalculateVelocity();
        foreach (Element e in layers)
        {

            if (e.sprites.Count == 0) { return; }
            if (!e.staticLayer)
            {
                e.Move(velocity.x, velocity.y);
            }
            if (simulate)
            {
                e.formula = ((e.speedH / 10) * e.speedV * -1);
            }
        }



    }
    void CalculateVelocity()
    {
        Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
        velocity = (position2D - lastPos) / Time.deltaTime;
        lastPos = position2D;
    }
    void Previewing()
    {

        

        float plusX = 0;
        float plusY = 0;
        Vector2 _pVelocity = previewVelocity;
        if (Input.GetMouseButton(0))
        {
            plusX = Input.GetAxis("Mouse X") * -30;
            plusY = Input.GetAxis("Mouse Y") * -30;
            _pVelocity = Vector2.zero;
        }

        curPosition += new Vector3(_pVelocity.x + plusX, _pVelocity.y + plusY, 0) * Time.deltaTime;
        cam.transform.position = Vector3.Lerp(cam.transform.position, curPosition, 5 * Time.deltaTime);
    }
}
[System.Serializable]
public class Element
{
    public bool showWindow;
    public bool hidden;
    public string name;
    float minY, maxY;
    public Color tint = Color.white;
    public Vector2 startPosition;
    public float selfSpeed = 0;
    public bool alwaysMove;
    public bool staticLayer;
    public bool showElements = true, showSettings = true, showBehaviourSettings = true;
    public float speedH = 100;
    public float speedV = 1f;
    public Color currentColor = Color.green;
    public float blendColorTime = 0.01f;
    public float formula;
    public bool savedChanges;



    public List<SpriteRenderer> sprites;
    public bool replicable;
    public float camHeight;
    public float camWidth;

    Transform layerParent;

    public Transform _el;
    float firstOffset;
    float actualOffset;

    public float scale = 1;
    bool getNext;
    int prevChosen;

    public Orientation orientation = Orientation.Horizontal;
    public AnchorH anchorH = AnchorH.Bottom;
    public AnchorV anchorV = AnchorV.Both;

    public enum AnchorH
    {
        None,
        Top,
        Bottom,
        Both
    }
    public enum AnchorV
    {
        None,
        Left,
        Right,
        Both
    }
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public void Initalize(Transform parent, int layer)
    {
        if (sprites.Count == 0) { return; }
        formula = ((speedH / 10) * speedV * -1);
        prevChosen = -1;
        GameObject layerGO = new GameObject("Layer - " + layer.ToString());
        layerGO.hideFlags = HideFlags.HideInHierarchy;
        layerGO.transform.parent = parent;
        layerGO.transform.localPosition = Vector3.zero;
        layerParent = layerGO.transform;


        float _minY = float.MaxValue;
        float _maxY = float.MinValue;

        if (orientation == Orientation.Horizontal)
        {
            if (anchorH == AnchorH.Bottom || anchorH == AnchorH.Both)
            {
                _minY = sprites[0].transform.localPosition.y;
            }
            if (anchorH == AnchorH.Top || anchorH == AnchorH.Both)
            {
                _maxY = sprites[0].transform.localPosition.y;
            }
        }
        else
        {
            if (anchorV == AnchorV.Left || anchorV == AnchorV.Both)
            {
                _minY = sprites[0].transform.localPosition.x;
            }
            if (anchorV == AnchorV.Right || anchorV == AnchorV.Both)
            {
                _maxY = sprites[0].transform.localPosition.x;
            }
        }

        minY = _minY;
        maxY = _maxY;

        if (sprites != null)
        {
            if (sprites.Count > 0)
            {
                foreach (SpriteRenderer el in sprites)
                {
                    if (el != null)
                    {
                        el.transform.parent = layerParent;
                        if (orientation == Orientation.Horizontal)
                        {
                            el.gameObject.name = (el.sprite.bounds.size.x * el.transform.localScale.x).ToString();
                        }
                        else
                        {
                            el.gameObject.name = (el.sprite.bounds.size.y * el.transform.localScale.y).ToString();
                        }
                    }
                }
            }
        }

        if (sprites.Count == 1)
        {
            replicable = false;
        }



    }

    public void Move(float x, float y)
    {
        float dir = 0;
        if (orientation == Orientation.Horizontal)
        {
            dir = x / 10;
        }
        else
        {
            dir = y / 10;
        }

        if (alwaysMove)
        {
            if (orientation == Orientation.Horizontal)
            {
                dir += selfSpeed / 100;
            }
            else
            {
                dir += selfSpeed / -100;
            }
        }
        dir *= speedH / 10 ;
        foreach (SpriteRenderer el in sprites)
        {
            if (el == null) { return; }

            if (Time.deltaTime == 0){ return; }

            if (orientation == Orientation.Horizontal)
            {
                el.transform.Translate(Vector3.left * dir * Time.deltaTime);

                Vector3 newPos = el.transform.localPosition;
                newPos += Vector3.up * formula * y / 10 * Time.deltaTime;
                newPos.y = Mathf.Clamp(newPos.y, maxY, minY);

                el.transform.localPosition = newPos;
            }
            else
            {

                el.transform.Translate(Vector3.down * dir * Time.deltaTime);

                Vector3 newPos = el.transform.localPosition;
                newPos += Vector3.right * formula * x / 10 * Time.deltaTime;
                newPos.x = Mathf.Clamp(newPos.x, maxY, minY);

                el.transform.localPosition = newPos;
            }
        }


        if (!replicable) { return; }

        if (orientation == Orientation.Horizontal)
        {
            int iHelp = layerParent.childCount - 1;
            if (dir > 0)
            {
                iHelp = 0;
            }
            if (iHelp != prevChosen)
            {
                getNext = true;
            }
            prevChosen = iHelp;

            if (getNext)
            {
                _el = layerParent.GetChild(iHelp);
                firstOffset = float.Parse(_el.name);
                actualOffset = firstOffset / 2 + camWidth / 2;
                getNext = false;
            }
            if (_el == null) { return; }

            if (_el.localPosition.x < -actualOffset && iHelp == 0)
            {
                OrganizeHorizontal(iHelp, _el, firstOffset);
            }
            else if (_el.localPosition.x > actualOffset && iHelp > 0)
            {
                OrganizeHorizontal(iHelp, _el, firstOffset);

            }
        }
        else
        {
            int iHelp = layerParent.childCount - 1;
            if (dir > 0)
            {
                iHelp = 0;
            }
            if (iHelp != prevChosen)
            {
                getNext = true;
            }
            prevChosen = iHelp;

            if (getNext)
            {
                _el = layerParent.GetChild(iHelp);
                firstOffset = float.Parse(_el.name);
                actualOffset = firstOffset / 2 + camHeight / 2;
                getNext = false;
            }
            if (_el == null) { return; }


            if (_el.localPosition.y < -actualOffset && iHelp == 0)
            {
                OrganizeVertical(iHelp, _el, firstOffset);
            }
            else if (_el.localPosition.y > actualOffset && iHelp > 0)
            {
                OrganizeVertical(iHelp, _el, firstOffset);
            }

        }
    }
    public void OrganizeHorizontal(int i, Transform _first, float firstOffset)
    {
        int firstIndex = i;
        int lastIndex = 0;

        if (firstIndex == 0)
        {
            lastIndex = layerParent.childCount - 1;
        }
        Transform _last = layerParent.GetChild(lastIndex);

        float lastOffset = float.Parse(_last.name);

        float _xOffset = firstOffset / 2 + lastOffset / 2;

        if (firstIndex == layerParent.childCount - 1)
        {
            _xOffset *= -1;
        }

        _first.localPosition = new Vector3(_last.localPosition.x + _xOffset, _last.localPosition.y, _first.localPosition.z);

        if (firstIndex == 0)
        {
            _first.SetAsLastSibling();
        }
        else
        {
            _first.SetAsFirstSibling();
        }
        getNext = true;
    }

    public void OrganizeVertical(int i, Transform _first, float firstOffset)
    {

        int firstIndex = i;
        int lastIndex = 0;

        if (firstIndex == 0)
        {
            lastIndex = layerParent.childCount - 1;
        }
        Transform _last = layerParent.GetChild(lastIndex);

        float lastOffset = float.Parse(_last.name);

        float _xOffset = firstOffset / 2 + lastOffset / 2;

        if (firstIndex == layerParent.childCount - 1)
        {
            _xOffset *= -1;
        }

        _first.localPosition = new Vector3(_last.localPosition.x, _last.localPosition.y + _xOffset, _first.localPosition.z);

        if (firstIndex == 0)
        {
            _first.SetAsLastSibling();
        }
        else
        {
            _first.SetAsFirstSibling();
        }
        getNext = true;
    }
}

