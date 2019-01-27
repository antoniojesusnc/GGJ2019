using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonGameObject<LevelManager>
{
    // auto check
    [SerializeField]
    float _timeToAutoCheck;

    float _timeStampAutoCheck;

    Vector3 _lastCameraPosition;
    Transform _cameraTransform;

    [SerializeField]
    GameObject _particleTemplate;

    public class BadObjectsInfo
    {
        public BadObjects Obj { get; set; }
        public List<Vector3> Points { get; set; }
    }

    [SerializeField]
    bool _showGizmos;


    public bool IsGameFinished { get; set; }
    public bool IsDoingFinishAnim { get; set; }

    List<BadObjectsInfo> _badObjects;

    BadObjectsInfo _lastObjectDetected;

    public float ElapsedTime { get; private set; }

    void Start()
    {
        FillObjects();
        ElapsedTime = 0;

        // autocheck
        _timeStampAutoCheck = 0;
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
    }

    private void FillObjects()
    {
        _badObjects = new List<BadObjectsInfo>();

        BadObjectsInfo temp;
        var objects = FindObjectsOfType<BadObjects>();
        Bounds bound;
        for (int i = objects.Length - 1; i >= 0; --i)
        {
            temp = new BadObjectsInfo();
            temp.Obj = objects[i];
            temp.Points = objects[i].GetComponent<BadObjects>().GetPointsToCheck();

            _badObjects.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDoingFinishAnim)
            return;

        if (IsGameFinished)
        {
            UpdateGameFinished();
            return;
        }

        ElapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckVictory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        CheckIfDisableOutline();
        CheckIfAutoCheck();



        if (Input.GetKeyDown(KeyCode.Q))
            Victory();
    }

    private void CheckIfDisableOutline()
    {
        bool hitInObject = false;

        int layerToCheck =
            1 << LayerMask.NameToLayer(LayerReference.ActiveObjects) |
            1 << LayerMask.NameToLayer(LayerReference.StaticObjects) |
            1 << LayerMask.NameToLayer(LayerReference.ToHide);

        Vector3 cameraPosition = Camera.main.transform.position;
        Ray ray = new Ray(cameraPosition, Vector3.forward);
        RaycastHit hitInfo;

        for (int i = _badObjects.Count - 1; i >= 0; --i)
        {
            if (!_badObjects[i].Obj.isActiveAndEnabled)
                continue;

            hitInObject = false;
            for (int j = _badObjects[i].Points.Count - 1; !hitInObject && j >= 0; --j)
            {
                ray.direction = _badObjects[i].Points[j] - cameraPosition;
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layerToCheck))
                {
                    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(LayerReference.ToHide))
                    {
                        _badObjects[i].Obj.SetOutline(true);
                        hitInObject = true;
                    }
                }
            }
            if (!hitInObject)
            {
                _badObjects[i].Obj.SetOutline(false);
            }
        }
    }

    private void CheckIfAutoCheck()
    {
        if (_lastCameraPosition == _cameraTransform.position)
        {
            _timeStampAutoCheck += Time.deltaTime;

            if (_timeStampAutoCheck > _timeToAutoCheck)
                CheckVictory();
        }
        else
        {
            _timeStampAutoCheck = 0;
        }

        _lastCameraPosition = _cameraTransform.position;
    }

    private void UpdateGameFinished()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Caret) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0)
            )
        {
            if (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings < GameManager.Instance.CurrentLevel+1) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            else
            {
                ++GameManager.Instance.CurrentLevel;
                UnityEngine.SceneManagement.SceneManager.LoadScene(GameManager.Instance.CurrentLevel);
            }

        }
    }

    public void CheckVictory()
    {
        _timeStampAutoCheck = 0;
        bool allObjectsHidden = CheckIfAllObjectsHidden();
        bool allObjectsInCorrectPosition = CheckIfAllObjectsInCorrectPosition();

        // victory
        if (allObjectsHidden && allObjectsInCorrectPosition)
        {
            Victory();
        }
        else
        {
            if(!allObjectsHidden)
                GUIGameManager.Instance.ShowMessage("You didn't hide properly the " + _lastObjectDetected.Obj.name);
            else
                GUIGameManager.Instance.ShowMessage("Some Objects are not in a comfortable state");
        }
    }

    private void Victory()
    {
        IsGameFinished = true;
        IsDoingFinishAnim = true;
        GameManager.Instance.UpdateBestTime(GameManager.Instance.CurrentLevel, ElapsedTime);
        GameManager.Instance.SetLevelAsUnlock(GameManager.Instance.CurrentLevel + 1);

        StartCoroutine(StartVictoryAnimCo());
    }

    private IEnumerator StartVictoryAnimCo()
    {
        Vector2 speed = new Vector2(0.5f, 0.05f);
        float zoomSpeed = 1.5f;
        float waitTime = 6;


        float waitForDissapear = 1f;
        float timeToDissapear = 2f;

        Camera.main.GetComponent<CameraControllerPolar>().SetFakeMovement(speed, zoomSpeed);

        LTSeq sequence;

        for (int i = _badObjects.Count - 1; i >= 0; --i)
        {
            sequence = LeanTween.sequence();
            sequence.append(waitForDissapear);
            //sequence.append(LeanTween.alpha(_badObjects[i].Obj.gameObject, 0, timeToDissapear));
            sequence.append(LeanTween.color(_badObjects[i].Obj.gameObject, Color.clear, timeToDissapear));
            LeanTween.delayedCall(waitForDissapear + timeToDissapear * 0.5f, CreateParticles);

            _badObjects[i].Obj.SetOutline(false);
        }

        yield return new WaitForSeconds(waitTime);

        IsDoingFinishAnim = false;
        GUIGameManager.Instance.Victory();
    }

    private void CreateParticles()
    {
        float timeToDestroyParticle = 2f;

        GameObject newParticle = null;
        for (int i = _badObjects.Count - 1; i >= 0; --i)
        {
            newParticle = Instantiate(_particleTemplate, _badObjects[i].Obj.transform.position, Quaternion.identity, transform);
            Destroy(newParticle, timeToDestroyParticle);

            Destroy(_badObjects[i].Obj.gameObject, 0.5f);
        }
    }

    private bool CheckIfAllObjectsHidden()
    {
        bool hitAnyObject = false;

        int layerToCheck =
            1 << LayerMask.NameToLayer(LayerReference.ActiveObjects) |
            1 << LayerMask.NameToLayer(LayerReference.StaticObjects) |
            1 << LayerMask.NameToLayer(LayerReference.ToHide);

        Vector3 cameraPosition = Camera.main.transform.position;
        Ray ray = new Ray(cameraPosition, Vector3.forward);
        RaycastHit hitInfo;

        for (int i = _badObjects.Count - 1; !hitAnyObject && i >= 0; --i)
        {
            if (!_badObjects[i].Obj.isActiveAndEnabled)
                continue;

            for (int j = _badObjects[i].Points.Count - 1; !hitAnyObject && j >= 0; --j)
            {
                ray.direction = _badObjects[i].Points[j] - cameraPosition;
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layerToCheck))
                {
                    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(LayerReference.ToHide))
                    {
                        hitAnyObject = true;
                        _lastObjectDetected = _badObjects[i];
                    }
                }
            }
        }

        return !hitAnyObject;
    }

    private bool CheckIfAllObjectsInCorrectPosition()
    {
        bool allCorrect = true;

        var activableObjects = GameObject.FindObjectsOfType<ActivableObjects>();
        for (int i = activableObjects.Length- 1; allCorrect && i >= 0; i--)
        {
            allCorrect = activableObjects[i].IsInSuccessState;
        }

        return allCorrect;
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos)
            return;

        if (_badObjects == null)
            return;

        bool hitAnyObject = false;
        int layerToCheck =
            1 << LayerMask.NameToLayer(LayerReference.ActiveObjects) |
            1 << LayerMask.NameToLayer(LayerReference.StaticObjects) |
            1 << LayerMask.NameToLayer(LayerReference.ToHide);


        for (int i = _badObjects.Count - 1; i >= 0; --i)
        {
            if (!_badObjects[i].Obj.isActiveAndEnabled)
                continue;

            var bound = _badObjects[i].Obj.GetComponent<Collider>().bounds;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bound.center, bound.size);

            Vector3 cameraPosition = Camera.main.transform.position;
            for (int j = _badObjects[i].Points.Count - 1; j >= 0; --j)
            {
                Ray ray = new Ray(cameraPosition, _badObjects[i].Points[j] - cameraPosition);
                RaycastHit hitInfo;
                Gizmos.color = Color.white;
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layerToCheck))
                {
                    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(LayerReference.ToHide))
                    {
                        hitAnyObject = true;
                        //Debug.Log("Hit in: " + hitInfo.collider.name);
                        Gizmos.color = Color.red;
                    }
                }
                Gizmos.DrawLine(cameraPosition, _badObjects[i].Points[j]);
            }
            //Debug.Log("hitAnyObject: " + hitAnyObject);
        }
    }
}
