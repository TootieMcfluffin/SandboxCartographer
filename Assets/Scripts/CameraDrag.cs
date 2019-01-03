using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDrag : MonoBehaviour
{

    public Camera cam;
    public GameObject modelHolder;
    public Dropdown mode;
    public GameObject collapsed;
    public GameObject collapsedTwo;
    public InputField NameBox;
    public InputField NotesBox;
    public InputField PlaceableNum;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;
    public int speed = 2;
    private int fastSpeed = 4;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 20;

    Quaternion originalRotation;

    private bool rightClicked = false;

    private Ray ray;
    private TerrainGenerator tg;
    //private List<GameObject> names = new List<GameObject>();
    public Dictionary<GameObject, GameObject> modelToName = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, string> nameToNotes = new Dictionary<GameObject, string>();

    void Update()
    {
        foreach (KeyValuePair<GameObject, GameObject> kvp in modelToName)
        {
            if (DataHolder.ShowNames)
            {
                kvp.Value.SetActive(true);
                //t.transform.LookAt(cam.transform.position, -Vector3.up);
                kvp.Value.transform.forward = cam.transform.forward;
            }
            else
            {
                kvp.Value.SetActive(false);
            }
        }
        if (collapsed.GetComponent<Collapse>().IsCollapsed == true)
        {
            if (Input.GetMouseButtonUp(1))
            {
                rightClicked = !rightClicked;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (tg == null)
                {
                    tg = (TerrainGenerator)FindObjectOfType(typeof(TerrainGenerator));
                }
                if (!(Input.mousePosition.x < Screen.width / 3 && collapsedTwo.GetComponent<Collapse>().IsCollapsed == false))
                {
                    var hitInfo = new RaycastHit();
                    ray = cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hitInfo, 3000))
                    {
                        if (mode.value == 0)
                        {
                            if (hitInfo.collider.gameObject.name.ToString() == "Chunk(Clone)")
                            {

                                if (DataHolder.PlaceableName == "Fir")
                                {
                                    GameObject clone = Instantiate(GameObject.Find("Fir_Tree"));
                                    tg.TreeList.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, true);
                                }
                                else if (DataHolder.PlaceableName == "Oak")
                                {
                                    GameObject clone = Instantiate(GameObject.Find("Oak_Tree"));
                                    tg.TreeList.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, true);
                                }
                                else if (DataHolder.PlaceableName == "Palm")
                                {
                                    GameObject clone = Instantiate(GameObject.Find("Palm_Tree"));
                                    tg.TreeList.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, true);
                                }
                                else if (DataHolder.PlaceableName == "Poplar")
                                {
                                    GameObject clone = Instantiate(GameObject.Find("Poplar_Tree"));
                                    tg.TreeList.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, true);
                                }
                                else if (DataHolder.PlaceableName == "House")
                                {
                                    GameObject clone = Instantiate(GameObject.Find("ME_House"));
                                    tg.Houses.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, false);

                                    //TEXT MESH
                                    GameObject textMesh = new GameObject("Text Mesh");
                                    textMesh.SetActive(false);
                                    modelToName.Add(clone, textMesh);
                                    nameToNotes.Add(textMesh, "");
                                    textMesh.transform.parent = clone.transform;
                                    textMesh.transform.localPosition = new Vector3(0, 30, 0);
                                    textMesh.AddComponent<TextMesh>();
                                    //textMesh.AddComponent<MeshRenderer>();
                                    TextMesh text = textMesh.GetComponent<TextMesh>();
                                    text.anchor = TextAnchor.MiddleCenter;
                                    text.fontSize = 16;
                                    text.alignment = TextAlignment.Center;
                                    text.text = "Village";
                                    textMesh.transform.LookAt(cam.transform);
                                    text.color = Color.black;

                                    Collapse coll = collapsed.GetComponent<Collapse>();
                                    if (modelToName.ContainsKey(clone))
                                    {
                                        GameObject name = modelToName[clone];
                                        NameBox.text = name.GetComponent<TextMesh>().text;
                                        NotesBox.text = nameToNotes[name];
                                        if (coll.IsCollapsed == true)
                                        {
                                            coll.CollapseRight();
                                        }
                                    }
                                    DataHolder.SelectedObject = clone;
                                    ////CANVAS
                                    //GameObject canvas = new GameObject("Canvas");
                                    //canvas.transform.parent = clone.transform;
                                    //canvas.AddComponent<Canvas>();
                                    //canvas.AddComponent<CanvasScaler>();
                                    //canvas.AddComponent<GraphicRaycaster>();
                                    //RectTransform rect = canvas.GetComponent<RectTransform>();
                                    //canvas.transform.localPosition = new Vector3(0, 10, 0);
                                    //rect.sizeDelta = new Vector2(1, 1);

                                    ////TEXTBOX
                                    //GameObject textBox = new GameObject("Text");
                                    //textBox.transform.parent = canvas.transform;
                                    //textBox.transform.localPosition = new Vector3(0, 0, 0);
                                    //textBox.AddComponent<Text>();
                                    //Text text = textBox.GetComponent<Text>();
                                    //text.text = "Test";
                                    //text.font = new Font("Arial");
                                    //text.fontSize = 16;

                                }
                                else if (DataHolder.PlaceableName == "Castle")
                                {
                                    GameObject clone = Instantiate(GameObject.Find("WatchTowerFull"));
                                    tg.Houses.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, false);

                                    //TEXT MESH
                                    GameObject textMesh = new GameObject("Text Mesh");
                                    textMesh.SetActive(false);
                                    modelToName.Add(clone, textMesh);
                                    nameToNotes.Add(textMesh, "");
                                    textMesh.transform.parent = clone.transform;
                                    textMesh.transform.localPosition = new Vector3(0, 50, 0);
                                    textMesh.AddComponent<TextMesh>();
                                    //textMesh.AddComponent<MeshRenderer>();
                                    TextMesh text = textMesh.GetComponent<TextMesh>();
                                    text.anchor = TextAnchor.MiddleCenter;
                                    text.fontSize = 22;
                                    text.alignment = TextAlignment.Center;
                                    text.text = "Castle";
                                    textMesh.transform.LookAt(cam.transform);
                                    text.color = Color.black;

                                    Collapse coll = collapsed.GetComponent<Collapse>();
                                    if (modelToName.ContainsKey(clone))
                                    {
                                        GameObject name = modelToName[clone];
                                        NameBox.text = name.GetComponent<TextMesh>().text;
                                        NotesBox.text = nameToNotes[name];
                                        if (coll.IsCollapsed == true)
                                        {
                                            coll.CollapseRight();
                                        }
                                    }
                                    DataHolder.SelectedObject = clone;
                                }
                                else if (DataHolder.PlaceableName == "Rock")
                                {
                                    Debug.Log("GOT HERE ONE");
                                    GameObject clone = Instantiate(GameObject.Find("Rock"));
                                    tg.TreeList.Add(clone);
                                    clone.AddComponent<MeshCollider>();
                                    Place(clone, hitInfo, false);
                                    Debug.Log("GOT HERE DONE");
                                }
                                ////HOUSE STUFFs
                                //GameObject houseClone = Instantiate(house);//, highEnough[randIndex], transform.rotation);
                                //tg.Houses.Add(houseClone);

                                ////Make canvas
                                //GameObject canvas = new GameObject("Canvas");
                                //canvas.transform.parent = houseClone.transform;
                                //canvas.AddComponent<Canvas>();
                                //canvas.AddComponent<CanvasScaler>();
                                //canvas.AddComponent<GraphicRaycaster>();
                                //canvas.transform.localPosition = new Vector3(0, 10, 0);

                                ////Make text box
                                //GameObject textBox = new GameObject("Text");
                                //textBox.transform.parent = canvas.transform;
                                //textBox.AddComponent<Text>();
                                //Text text = textBox.GetComponent<Text>();
                                //text.text = "Test";
                                //text.font = new Font("Arial");
                                //text.fontSize = 16;

                                //Canvas houseCanvas = canvas.GetComponent<Canvas>();
                                //houseCanvas.renderMode = RenderMode.WorldSpace;
                                //houseClone.transform.localPosition = new Vector3(hitInfo.point.x, hitInfo.point.y + 1.5f, hitInfo.point.z);
                                //houseClone.transform.localScale = new Vector3(.3f, .3f, .3f);
                                ////Debug.Log(hitInfo.point.ToString());
                            }
                        }
                        else if (mode.value == 1)
                        {
                            GameObject collided = hitInfo.collider.gameObject;
                            if (collided.name.ToString() != "Chunk(Clone)" && collided.name.ToString() != "Water(Clone)")
                            {
                                if (collided.transform.parent.gameObject != null && modelToName.ContainsKey(collided.transform.parent.gameObject))
                                {
                                    nameToNotes.Remove(modelToName[collided.transform.parent.gameObject]);
                                    modelToName.Remove(collided.transform.parent.gameObject);
                                    tg.Houses.Remove(collided.transform.parent.gameObject);
                                    GameObject.Destroy(collided.transform.parent.gameObject);
                                }
                                else if (collided.transform.parent.gameObject != null && collided.transform.parent.parent != null && modelToName.ContainsKey(collided.transform.parent.parent.gameObject))
                                {
                                    nameToNotes.Remove(modelToName[collided.transform.parent.parent.gameObject]);
                                    modelToName.Remove(collided.transform.parent.parent.gameObject);
                                    tg.Houses.Remove(collided.transform.parent.parent.gameObject);
                                    GameObject.Destroy(collided.transform.parent.parent.gameObject);
                                }
                                else
                                {
                                    tg.TreeList.Remove(collided);
                                    GameObject.Destroy(collided);
                                }
                            }
                        }
                        else if (mode.value == 2)
                        {
                            GameObject collided = hitInfo.collider.gameObject;
                            Collapse coll = collapsed.GetComponent<Collapse>();
                            if (modelToName.ContainsKey(collided.transform.parent.gameObject))
                            {
                                DataHolder.SelectedObject = collided.transform.parent.gameObject;
                                GameObject name = modelToName[collided.transform.parent.gameObject];
                                NameBox.text = name.GetComponent<TextMesh>().text;
                                NotesBox.text = nameToNotes[name];
                                if (coll.IsCollapsed == true)
                                {
                                    coll.CollapseRight();
                                }
                            }
                            else if (modelToName.ContainsKey(collided.transform.parent.parent.gameObject))
                            {
                                DataHolder.SelectedObject = collided.transform.parent.parent.gameObject;
                                GameObject name = modelToName[collided.transform.parent.parent.gameObject];
                                NameBox.text = name.GetComponent<TextMesh>().text;
                                NotesBox.text = nameToNotes[name];
                                if (coll.IsCollapsed == true)
                                {
                                    coll.CollapseRight();
                                }
                            }
                        }
                        else if (mode.value == 3)
                        {
                            Vector3 center = hitInfo.point;
                            center.y += tg.MaxHeight;
                            int placeableNumber = 100;
                            int.TryParse(PlaceableNum.text, out placeableNumber);
                            if(placeableNumber < 1)
                            {
                                placeableNumber = 100;
                            }
                            for (int i = 0; i < placeableNumber; i++)
                            {
                                Vector3 pos = RandomCircle(center, DataHolder.BrushRadius);
                                var hitInfoTwo = new RaycastHit();
                                Ray ray = new Ray(pos, Vector3.down);
                                if (Physics.Raycast(ray, out hitInfoTwo, tg.MaxHeight + (tg.MaxHeight - tg.MinHeight) + 1))
                                {
                                    if (hitInfoTwo.collider.gameObject.name == "Chunk(Clone)")
                                    {
                                        if (DataHolder.PlaceableName == "Fir")
                                        {
                                            GameObject clone = Instantiate(GameObject.Find("Fir_Tree"));
                                            tg.TreeList.Add(clone);
                                            clone.AddComponent<MeshCollider>();
                                            Place(clone, hitInfoTwo, true);
                                        }
                                        else if (DataHolder.PlaceableName == "Oak")
                                        {
                                            GameObject clone = Instantiate(GameObject.Find("Oak_Tree"));
                                            tg.TreeList.Add(clone);
                                            clone.AddComponent<MeshCollider>();
                                            Place(clone, hitInfoTwo, true);
                                        }
                                        else if (DataHolder.PlaceableName == "Palm")
                                        {
                                            GameObject clone = Instantiate(GameObject.Find("Palm_Tree"));
                                            tg.TreeList.Add(clone);
                                            clone.AddComponent<MeshCollider>();
                                            Place(clone, hitInfoTwo, true);
                                        }
                                        else if (DataHolder.PlaceableName == "Poplar")
                                        {
                                            GameObject clone = Instantiate(GameObject.Find("Poplar_Tree"));
                                            tg.TreeList.Add(clone);
                                            clone.AddComponent<MeshCollider>();
                                            Place(clone, hitInfoTwo, true);
                                        }
                                        else if (DataHolder.PlaceableName == "Rock")
                                        {
                                            GameObject clone = Instantiate(GameObject.Find("Rock"));
                                            tg.TreeList.Add(clone);
                                            clone.AddComponent<MeshCollider>();
                                            Place(clone, hitInfoTwo, false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!rightClicked)
            {
                if (axes == RotationAxes.MouseXAndY)
                {
                    rotAverageY = 0f;
                    rotAverageX = 0f;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                    rotArrayY.Add(rotationY);
                    rotArrayX.Add(rotationX);

                    if (rotArrayY.Count >= frameCounter)
                    {
                        rotArrayY.RemoveAt(0);
                    }
                    if (rotArrayX.Count >= frameCounter)
                    {
                        rotArrayX.RemoveAt(0);
                    }

                    for (int j = 0; j < rotArrayY.Count; j++)
                    {
                        rotAverageY += rotArrayY[j];
                    }
                    for (int i = 0; i < rotArrayX.Count; i++)
                    {
                        rotAverageX += rotArrayX[i];
                    }

                    rotAverageY /= rotArrayY.Count;
                    rotAverageX /= rotArrayX.Count;

                    rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
                    rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                    Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                }
                else if (axes == RotationAxes.MouseX)
                {
                    rotAverageX = 0f;

                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                    rotArrayX.Add(rotationX);

                    if (rotArrayX.Count >= frameCounter)
                    {
                        rotArrayX.RemoveAt(0);
                    }
                    for (int i = 0; i < rotArrayX.Count; i++)
                    {
                        rotAverageX += rotArrayX[i];
                    }
                    rotAverageX /= rotArrayX.Count;

                    rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                    transform.localRotation = originalRotation * xQuaternion;
                }
                else
                {
                    rotAverageY = 0f;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

                    rotArrayY.Add(rotationY);

                    if (rotArrayY.Count >= frameCounter)
                    {
                        rotArrayY.RemoveAt(0);
                    }
                    for (int j = 0; j < rotArrayY.Count; j++)
                    {
                        rotAverageY += rotArrayY[j];
                    }
                    rotAverageY /= rotArrayY.Count;

                    rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);

                    Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                    transform.localRotation = originalRotation * yQuaternion;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        //} 
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        //Random angle
        float theta = UnityEngine.Random.Range(0, 2 * 3.14159f);
        //Random distance within circle
        float distance = UnityEngine.Random.Range(0.0f, 1.0f) * radius;

        //Follow the angle the distance set vector points to point within circle
        float px = (float)(distance * Math.Cos(theta) + center.x);
        float py = center.y;
        float pz = (float)(distance * Math.Sin(theta) + center.z);

        Vector3 pos = new Vector3(px, py, pz);
        return pos;
    }

    private void Place(GameObject clone, RaycastHit hitInfo, bool isTree, float scale = 0.3f)
    {
        if (isTree)
        {
            float xZScale = 1 + UnityEngine.Random.Range(-DataHolder.TreeVariance, DataHolder.TreeVariance);
            clone.transform.localScale = new Vector3(xZScale,
                    1 + UnityEngine.Random.Range(-DataHolder.TreeVariance, DataHolder.TreeVariance),
                    xZScale);
        }
        else if (clone.name == "Rock(Clone)")
        {
            float xZScale = UnityEngine.Random.Range(-0.2f, 0.2f);
            clone.transform.localScale = new Vector3(xZScale, xZScale, xZScale);
        }
        else
        {
            clone.transform.localScale = new Vector3(scale, scale, scale);
        }
        clone.transform.localPosition = hitInfo.point;
        if (!isTree && clone.name == "ME_House(Clone)")
        {
            clone.transform.localPosition = new Vector3(hitInfo.point.x, hitInfo.point.y + 1.5f, hitInfo.point.z);
        }
        Vector3 euler = tg.transform.eulerAngles;
        euler.y = UnityEngine.Random.Range(0f, 360f);
        clone.transform.eulerAngles = euler;
        clone.transform.parent = tg.transform;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray);
    }
}
