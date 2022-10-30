using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundParalax : MonoBehaviour
{
    public Transform playerCam;
    public Transform[] backgrounds;
    public Transform overlay;

    public float startSensitivity = 0;
    public float endSensitivity = 0.4f;

    private Vector2 lastPos = Vector2.zero;
    private Vector2 delta = Vector2.zero;

    private List<LinkedList<SpriteRenderer>> backgroundLayers;

    void Start()
    {
        lastPos = playerCam.position;
        backgroundLayers = new List<LinkedList<SpriteRenderer>>(backgrounds.Length);

        for (int i = 0; i < backgrounds.Length; ++i)
        {
            var child = backgrounds[i].GetChild(0);
            var childTransform = child.transform;

            var newObj = Instantiate(child.gameObject, childTransform.position, childTransform.rotation);

            var list = new LinkedList<SpriteRenderer>();
            list.AddFirst(newObj.GetComponent<SpriteRenderer>());
            
            backgroundLayers.Add(list);

            backgrounds[i].gameObject.SetActive(false);
        }

        spawnInView();
    }

    /// <summary>
    /// Return random item from background collection at specific layer
    /// </summary>
    GameObject getRandomSpriteAtLayer(int layer)
    {
        return backgrounds[layer].GetChild(Random.Range(0, backgrounds[layer].childCount)).gameObject;
    }

    void Update()
    {
        updateDelta();

        matchOverlayWithCamera();

       
        moveParalax();

        spawnInView();

        clearInvisible();
    }

    private void clearInvisible()
    {
        foreach (var backgroundLayer in backgroundLayers)
        {
            var node = backgroundLayer?.First;
            while (node != null)
            {
                var next = node.Next;
                if (CameraUtils.outOfView(node.Value, playerCam, 1) && backgroundLayer.Count > 1)
                {
                    Destroy(node.Value.gameObject);
                    backgroundLayer.Remove(node);
                }

                node = next;
            }
        }
    }

    private void spawnInView()
    {
        for (int i = 0; i < backgroundLayers.Count; ++i)
        {
            //doing a little bit of nonsense - adding items while iterating over collection 
            //for (int j = 0; j < backgroundLayers[i].Count; ++j)
            {
                var sprite = backgroundLayers[i].Last;

                //the right edge of right-most sprite is visible - need to spawn to the right
                if (CameraUtils.rightEdgeIn(sprite.Value, playerCam))
                {
                    var newObject = spawnSpriteObj(sprite.Value, getRandomSpriteAtLayer(i), 1, i);
                    backgroundLayers[i].AddLast(newObject.GetComponent<SpriteRenderer>());
                }

                sprite = backgroundLayers[i].First;
                //the left edge of right-most sprite is visible - need to spawn to the left
                if (CameraUtils.leftEdgeIn(sprite.Value, playerCam))
                {
                    var newObject = spawnSpriteObj(sprite.Value, getRandomSpriteAtLayer(i), -1, i);
                    backgroundLayers[i].AddFirst(newObject.GetComponent<SpriteRenderer>());
                }
            }
        }
    }

    private void moveParalax()
    {
        for (int i = 0; i < backgroundLayers.Count; ++i)
        {
            //calculate parallax weight linearly based on how "far" the layer is
            //a simple way to move each layer with different speed
            float shiftWeight = startSensitivity + (endSensitivity - startSensitivity) * i / (backgroundLayers.Count - 1);
            //Debug.Log(delta + " " + shiftWeight);
            foreach (var sprite in backgroundLayers[i])
            {
                //offset the sprite with weight
                sprite.transform.Translate(delta * shiftWeight);
            }
        }
    }

    private void updateDelta()
    {
        delta = (Vector2) playerCam.position - lastPos;

        lastPos = playerCam.position;
    }

    private void matchOverlayWithCamera()
    {
        for (int i = 0; i < overlay.childCount; ++i)
        {
            var child = overlay.GetChild(i);

            var render = child.GetComponent<SpriteRenderer>();
            var size = render.bounds.size;
            var cameraSize = new Vector3(CameraUtils.getCamWidth(playerCam)  * 2, CameraUtils.getCamHeight(playerCam) * 2, 0);
            
            var scaleFactor = new Vector3(cameraSize.x / size.x, cameraSize.y / size.y, 1);

            child.localScale = Vector3.Scale(child.localScale, scaleFactor);
            child.localPosition = new Vector3(0, 0, 0);
            
        }
    }

    private GameObject spawnSpriteObj(SpriteRenderer sample, GameObject sprite, float shift = 1.0f, int layer = 0)
    {
        return spawnSprite(sample, sprite.GetComponent<SpriteRenderer>(), shift, layer);
    }

    private GameObject spawnSprite(SpriteRenderer sample, SpriteRenderer sprite, float shift, int layer)
    {
        var sampleTransform = sample.transform;

        var newObject = Instantiate(sprite.gameObject, sampleTransform.position, sprite.transform.rotation);
        
        var position = newObject.transform.position;
        newObject.transform.position = new Vector3(position.x, sprite.transform.position.y, position.z);

        newObject.SetActive(true);

        newObject.transform.Translate(shift * (CameraUtils.getWidth(sprite) + CameraUtils.getWidth(sample) + getExtraOffset(layer)), 0, 0,
            Space.Self);
        return newObject;
    }

    protected virtual float getExtraOffset(int id)
    {
        return 0;
    }
}