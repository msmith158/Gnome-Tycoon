/*
// =======================================================================
// THIS MEGA SCRIPT CONTAINS ALL THE SCRIPTS CUSTOM WRITTEN FOR THIS GAME.
//  THIS IS A SHOWCASE SCRIPT AND IS NOT TO BE USED FOR REAL OPERATIONS.
// =======================================================================
 
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PrototypeCameraShake : MonoBehaviour
{
    public bool isShaking = false;

    public IEnumerator TimedShake(float duration, float xMagnitude, float yMagnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * xMagnitude;
            float y = Random.Range(-1f, 1f) * yMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator Shake(float xMagnitude, float yMagnitude)
    {
        Vector3 originalPos = transform.localPosition;

        while (isShaking)
        {
            float x = Random.Range(-1f, 1f) * xMagnitude;
            float y = Random.Range(-1f, 1f) * yMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void Stop()
    {
        isShaking = false;
    }
}

public class PrototypeConveyor : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2.0f;
    [HideInInspector] public float initialSpeed;
    [HideInInspector] public PrototypeGnomeCoinSystem gnomeCoinSys;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
    }

    private void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position -= transform.forward * (speed + (speed * gnomeCoinSys.permanentSpeed)) * Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * (speed + (speed * gnomeCoinSys.permanentSpeed)) * Time.deltaTime);
    }
}

public class PrototypeDDOLManager : MonoBehaviour
{
    public bool isOneOffComplete = false;
    public int resetTimes = 0;
    public string sceneToLoad;

    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}

public class PrototypeDropper : MonoBehaviour
{
    private PrototypeFactorySystem gameManager;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("proto_gameManager").GetComponent<PrototypeFactorySystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            switch (gameManager.prestigeLvl)
            {
                case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                    value = gameManager.lvl1Value; 
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                    value = gameManager.lvl2Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                    value = gameManager.lvl3Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                    value = gameManager.lvl4Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                    value = gameManager.lvl5Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                    value = gameManager.lvl6Value;
                    break;
            }
            gameManager.AddScore(value);
            Destroy(other.gameObject);
        }
    }
}

public class PrototypeFactorySystem : MonoBehaviour
{
    [Header("Values: General")]
    public float pointScore;
    public PrestigeLevel prestigeLvl = PrestigeLevel.Prestige0;
    public float lvl1Value;
    [HideInInspector] public float lvl1InitialValue;
    public float lvl2Value;
    [HideInInspector] public float lvl2InitialValue;
    public float lvl3Value;
    [HideInInspector] public float lvl3InitialValue;
    public float lvl4Value;
    [HideInInspector] public float lvl4InitialValue;
    public float lvl5Value;
    [HideInInspector] public float lvl5InitialValue;
    public float lvl6Value;
    [HideInInspector] public float lvl6InitialValue;

    [Header("Debug Values")]
    public bool debugMode;
    public float instantPointAddition;
    private bool hasDebugRun;

    [Header("Object References: General")]
    public Collider basketTrigger;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI coinText;
    public List<GameObject> oneOffObjects = new List<GameObject>();
    private PrototypeGnomeCoinSystem ddolManager;

    private void OnEnable()
    {
        SetUpDDOLManager();
        SetUpDDOLManagerOneOff();
        ddolManager.Initialise();

        lvl1InitialValue = lvl1Value;
        Application.targetFrameRate = 60;
        AddScore(0);
    }

    private void SetUpDDOLManager()
    {
        ddolManager = GameObject.Find("proto_ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
        ddolManager.gnomeCoinText = coinText;
    }

    private void SetUpDDOLManagerOneOff()
    {
        switch (ddolManager.gameObject.GetComponent<PrototypeDDOLManager>().isOneOffComplete)
        {
            case true:
                for (int i = 0; i < ddolManager.oneTimeObjects.Count; i++)
                {
                    Destroy(ddolManager.oneTimeObjects[i]);
                }
                Debug.Log("Bingo 2");
                break;
            case false:
                for (int i = 0; i < oneOffObjects.Count; i++)
                {
                    //ddolManager.oneTimeObjects.Add(oneOffObjects[i]);
                    //ddolManager.oneTimeObjectNames.Add(oneOffObjects[i].name);
                }
                ddolManager.gameObject.GetComponent<PrototypeDDOLManager>().isOneOffComplete = true;
                Debug.Log("Bingo 1");
                break;
        }
    }

    public void AddScore(float amount)
    {
        pointScore += amount + (amount * ddolManager.permanentValue);
        if (debugMode && !hasDebugRun) 
        {
            pointScore += instantPointAddition;
            hasDebugRun = true;
        }
        moneyText.text = "Profit: $" + RoundToNearestHundredth(pointScore).ToString("F2");
    }

    public void UpdatePrice(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, float newPrice, string afterText)
    {
        switch (isGnomeCoins)
        {
            case true:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString() + afterText;
                break;
            case false:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                break;
        }
    }

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

    public void PauseGame(bool isPaused)
    {
        switch (isPaused)
        {
            case true:
                Time.timeScale = 0.0f;
                break;
            case false:
                Time.timeScale = 1.0f;
                break;
        }
    }

    public enum PrestigeLevel
    {
        Prestige0,
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5
    }
}

public class PrototypeFinalPrestigeSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float alarmDelayTime;
    [SerializeField] private float delayUntilCountdown;
    [SerializeField] private float countdownTimer;
    [SerializeField] private float delayAfterNuke;
    [SerializeField] private float nukeParticlesDelay;
    [SerializeField] private float delayAfterFadeOut;
    private Color emissiveColor = Color.red;
    private float timeElapsed;

    [Header("Object References: General")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeConveyor conveyor;
    [SerializeField] private PrototypeCameraShake camShake;
    [SerializeField] private Material emerLightMaterial;
    [SerializeField] private Light nukeLight;
    [SerializeField] private Light nukeAmbientLight;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AnimationClip emerLightAnim;
    [SerializeField] private GameObject whiteScreen;
    [SerializeField] private AnimationCurve nukeSequenceShake;
    [SerializeField] private PrototypeDDOLManager ddolSys;
    [Header("Object References: Audio")]
    [SerializeField] private AudioSource switchOffSource;
    [SerializeField] private AudioSource generatorOffSource;
    [SerializeField] private AudioSource alarmSource;
    [SerializeField] private AudioSource clockSource;
    [SerializeField] private AudioSource nukeSource;
    [SerializeField] private AudioSource sirenSource;
    [SerializeField] private AudioClip switchOff;
    [SerializeField] private AudioClip generatorOff;
    [SerializeField] private AudioClip alarm;
    [SerializeField] private AudioClip clock;
    [SerializeField] private AudioClip nuke;
    [SerializeField] private AudioClip siren;
    [SerializeField] private AudioMixerGroup outsideNuke;
    [Header("Object References: Lists")]
    [SerializeField] private List<GameObject> uiToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToEnable = new List<GameObject>();
    [SerializeField] private List<Light> lightsToTurnOff = new List<Light>();
    [SerializeField] private List<Light> emergencyLights = new List<Light>();
    [SerializeField] private List<ParticleSystem> nukeParticles = new List<ParticleSystem>();

    public void OnEnable()
    {
        ddolSys = GameObject.Find("ddolManager").GetComponent<PrototypeDDOLManager>();
    }

    public void StartNukeSequence()
    {
        StartCoroutine(NukeSequence());
    }

    public IEnumerator NukeSequence()
    {
        // The beginning of the sequence, power turns off
        for (int i = 0; i < lightsToTurnOff.Count; i++)
        {
            lightsToTurnOff[i].enabled = false;
        }
        if (!switchOffSource.isPlaying)
        {
            switchOffSource.clip = switchOff;
            switchOffSource.Play();
        }
        if (!generatorOffSource.isPlaying)
        {
            generatorOffSource.clip = generatorOff;
            generatorOffSource.Play();
        }
        for (int i = 0; i < uiToDisable.Count; i++)
        {
            uiToDisable[i].SetActive(false);
        }
        conveyor.enabled = false;

        yield return new WaitForSeconds(alarmDelayTime);

        // The alarm starts blaring
        if (!alarmSource.isPlaying)
        {
            alarmSource.clip = alarm;
            alarmSource.loop = true;
            alarmSource.Play();
        }
        for (int i = 0; i < emergencyLights.Count; i++)
        {
            emergencyLights[i].enabled = true;
            emergencyLights[i].GetComponent<Animator>().enabled = true;
        }
        emerLightMaterial.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(3f);
        if (!sirenSource.isPlaying)
        {
            sirenSource.clip = siren;
            sirenSource.loop = false;
            sirenSource.volume = 0.2f;
            sirenSource.outputAudioMixerGroup = outsideNuke;
            sirenSource.Play();
        }

        yield return new WaitForSeconds(delayUntilCountdown - 3f);

        // The countdown timer begins
        for (int i = 0; i < uiToEnable.Count; i++)
        {
            uiToEnable[i].SetActive(true);
        }
        clockSource.PlayOneShot(clock);

        for (float timer = countdownTimer; timer > 0; timer -= Time.deltaTime)
        {
            timerText.text = sys.RoundToNearestHundredth(timer).ToString("F2");
            if (timer <= 0.5f && sirenSource.isPlaying)
            {
                if (timeElapsed < 0.3f)
                {
                    sirenSource.volume = Mathf.Lerp(0.2f, 0, timeElapsed / 0.3f);
                    timeElapsed += Time.deltaTime;
                }
            }
            yield return null;
        }

        // The nuke goes off
        if (!nukeSource.isPlaying)
        {
            nukeSource.clip = nuke;
            nukeSource.loop = false;
            nukeSource.Play();
        }
        nukeLight.gameObject.SetActive(true);
        nukeAmbientLight.gameObject.SetActive(true);
        nukeLight.GetComponent<Animator>().enabled = true;
        nukeAmbientLight.GetComponent<Animator>().enabled = true;
        alarmSource.Stop();
        StartCoroutine(ProgrammedNukeShake());
        StartCoroutine(DelayedParticles());
        yield return new WaitForSeconds(1);
        timerText.enabled = false;
        sirenSource.Stop();
        sirenSource.outputAudioMixerGroup = null;
        for (int i = 0; i < emergencyLights.Count; i++)
        {
            emergencyLights[i].GetComponent<Animator>().enabled = false;
            emergencyLights[i].enabled = false;
        }
        emerLightMaterial.DisableKeyword("_EMISSION");

        yield return new WaitForSeconds(delayAfterNuke - 1);

        // The nuke finally hits and the screen goes white
        whiteScreen.GetComponent<Image>().enabled = true;
        whiteScreen.GetComponent<Image>().CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(1.5f);
        timeElapsed = 0;
        for (float f = 0f; f < 3f; f += Time.deltaTime)
        {
            if (timeElapsed < 3f)
            {
                whiteScreen.GetComponent<Image>().color = Color.Lerp(Color.white, Color.black, timeElapsed / 3f);
                timeElapsed += Time.deltaTime;
            }
            yield return null;
        }
        camShake.Stop();

        yield return new WaitForSeconds(delayAfterFadeOut);

        // The ending of the nuke sequence, restarting everything.
        ddolSys.resetTimes++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    IEnumerator ProgrammedNukeShake()
    {
        float timeElapsed = 0;
        camShake.isShaking = true;
        float value = 0;

        while (camShake.isShaking == true)
        {
            value = nukeSequenceShake.Evaluate(timeElapsed);
            timeElapsed += Time.deltaTime;
            StartCoroutine(camShake.Shake(value, value));
            yield return null;
        }
    }

    IEnumerator DelayedParticles()
    {
        yield return new WaitForSeconds(nukeParticlesDelay);
        foreach (ParticleSystem nukeParticle in nukeParticles)
        {
            nukeParticle.Play();
        }
    }
}

public class PrototypeGnomeCoinShopSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float spinnerTime = 2f;
    private bool isReadyToDestroy = false;

    [Header("Object References")]
    private PrototypeGnomeCoinSystem gnomeCoinSys;
    public GameObject promptBackground;
    public GameObject spinnerBackground;

    public void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }
    public void BuyGnomeCoins(int amount)
    {
        StartCoroutine(GnomeCoinPurchaseProcess(amount));
    }

    public void DisableListing(GameObject objectToDestroy)
    {
        StartCoroutine(DisableListingDelay(objectToDestroy));
    }

    IEnumerator GnomeCoinPurchaseProcess(int amountToBuy)
    {
        spinnerBackground.SetActive(true);
        yield return new WaitForSeconds(spinnerTime);
        gnomeCoinSys.AddCoins(amountToBuy);
        spinnerBackground.SetActive(false);
        isReadyToDestroy = true;
    }

    IEnumerator DisableListingDelay(GameObject obj)
    {
        while (!isReadyToDestroy)
        {
            yield return null;
        }
        obj.SetActive(false);
        isReadyToDestroy = false;
        gnomeCoinSys.oneTimeObjects.Add(obj);
    }
}

public class PrototypeGnomeCoinSystem : MonoBehaviour
{
    [Header("Object References")]
    public TextMeshProUGUI gnomeCoinText;
    public List<GameObject> oneTimeObjects = new List<GameObject>();
    public List<string> oneTimeObjectNames = new List<string>();

    [Header("Values")]
    public int coinCount;
    public float permanentValue;
    public float permanentSpeed;
    public float permanentTime;
    public float permanentCooldown;

    public void Initialise()
    {
        if (gnomeCoinText != null)
        {
            gnomeCoinText.text = "Coins: �" + coinCount;
        }
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "Coins: �" + coinCount;
    }
}

public class PrototypeManufacturer : MonoBehaviour
{
    [Header("Values")]
    public float manufacturingTime;
    public float manufacturingCooldown;
    [HideInInspector] public float initialManuTime;
    [HideInInspector] public float initialManuCool;
    [Tooltip("Put the max odds in whole numbers. For example, for a 1 in 10,000 chance, put 10000. This number CANNOT be 0.")] [SerializeField] private int chanceOfJimbo;
    private int jimboNumber = 0;
    private bool wasJimboSpawned;

    [Header("Object References")]
    public GameObject timeSlider;
    public GameObject manufacturerSpawnPoint;
    public GameObject objectPrefab;
    [SerializeField] private GameObject jimboPrefab;
    public GameObject objectModel;
    public PrototypeFactorySystem sys;
    [HideInInspector] public PrototypeDDOLManager initSys;
    [HideInInspector] public PrototypeGnomeCoinSystem gnomeCoinSys;
    public List<GameObject> objectsList = new List<GameObject>();
    public List<Material> gnomeMaterialList = new List<Material>();
    private GameObject newObject;
    private GameObject newJimbo;

    // Private variables
    private bool isActivated = false;
    private Scrollbar slider;

    private void Start()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
    }

    private void OnEnable()
    {
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        initSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeDDOLManager>();
        gnomeCoinSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }

    public void SpawnObject()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }

    IEnumerator DelayedSpawn()
    {
        // Work the manufacturing delay timer
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";
        manufacturingTime += (manufacturingTime * gnomeCoinSys.permanentTime);
        manufacturingCooldown += (manufacturingCooldown * gnomeCoinSys.permanentCooldown);

        for (float timer = manufacturingTime; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingTime);
            float progress = Mathf.InverseLerp(0f, manufacturingTime, timer);
            slider.size = progress;
            yield return null;
        }

        // Do a roll between a gnome and Jimbo, and then instantiate
        jimboNumber = Random.Range(0, chanceOfJimbo);
        Debug.Log(jimboNumber);
        if (jimboNumber == 0 && sys.prestigeLvl != PrototypeFactorySystem.PrestigeLevel.Prestige0) // This is to spawn Jimbo
        {
            Debug.Log("Hey buddy");
            newJimbo = Instantiate(jimboPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
            wasJimboSpawned = true;
        }
        else // This is to spawn a gnome
        {
            newObject = Instantiate(objectPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
            wasJimboSpawned = false;
            Debug.Log("Gnome spawned");
        }

        // If not Jimbo, apply the prestige-relevant material to the newly-spawned gnome
        if (initSys.resetTimes == 0)
        {
            switch (sys.prestigeLvl)
            {
                case PrototypeFactorySystem.PrestigeLevel.Prestige0: // Dirty red gnome material
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[0];
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige1: // Red gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[1];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige2: // Yellow gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[2];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige3: // Green gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[3];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige4: // Cyan gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[4];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige5: // Purple gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[5];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
            }
        }
        else if (initSys.resetTimes >= 1) // If the game has been reset at least once, golden gnome material
        {
            for (int i = 0; i < newObject.transform.childCount; i++)
            {
                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[6];
            }
        }

        // Tag and assign objects
        switch (wasJimboSpawned)
        {
            case false:
                newObject.tag = "gnome";
                objectsList.Add(newObject);
                break;
            case true:
                newJimbo.tag = "gnome";
                objectsList.Add(newJimbo);
                break;
        }

        // Work the manufacturer cooldown timer
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Cooling down...";
        for (float timer = 0; timer < manufacturingCooldown; timer += Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingCooldown);
            float progress = Mathf.InverseLerp(0f, manufacturingCooldown, timer);
            slider.size = progress;
            yield return null;
        }

        timeSlider.SetActive(false);
        isActivated = false;
    }
}

public class PrototypePrestige : MonoBehaviour
{
    [Header("General Values")]
    public PrestigeType prestigeType;
    public float price;
    [Header("Reward Values")]
    public float unitMultiplication;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float manufacturerDecreasePercent;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float priceIncreasePercent;
    private int displayablePrestigeLevel = 0;

    [Header("Object References")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeFinalPrestigeSystem finalPrestigeSys;
    [SerializeField] private List<PrototypeUpgrades> upgradeSys = new List<PrototypeUpgrades>();
    [SerializeField] private PrototypeManufacturer manufacturer;
    [SerializeField] private PrototypeConveyor conveyor;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentPrestigeText;
    [SerializeField] private TextMeshProUGUI prestigeBodyText;
    [SerializeField] private GameObject promptParent;
    [SerializeField] private List<GameObject> promptUIToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> PromptUIToEnable = new List<GameObject>();

    void Start()
    {
        sys.UpdatePrice(costText, false, "$", price, "");
        currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
        if (prestigeType != PrestigeType.FinalPrestige)
        {
            // Example: 2x gnome value, 95% manufacturing speed & 110% prices.
            prestigeBodyText.text = unitMultiplication + "x gnome value, " + (100 - (manufacturerDecreasePercent * 100)) + "% manufacturing speed & " + (100 + (priceIncreasePercent * 100)) + "% prices.";
        }
            
    }

    public void UpdatePrestige()
    {
        if (sys.pointScore >= price)
        {
            switch (prestigeType)
            {
                case PrestigeType.Prestige1:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige0 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige2:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige1 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige3:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige2 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige4:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige3 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige5:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige4 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.FinalPrestige: // The nuke sequence
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige5 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    }
                    else if (sys.debugMode)
                    {
                        Debug.Log("Hi");
                        finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    }
                    else PromptWindow();
                    break;
            }
        }
    }

    private void ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel newPrestLvl)
    {
        // Reset money to 0
        sys.prestigeLvl = newPrestLvl;
        switch (sys.debugMode)
        {
            case true:
                break;
            case false:
                sys.pointScore = 0;
                break;
        }
        sys.moneyText.text = "Profit: $" + sys.pointScore;

        // Reset gnome values (and update current prestige text)
        switch (newPrestLvl)
        {
            // Dev note: Yes, I know now that this isn't the best method to do this. I'll change it if I can.
            case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                displayablePrestigeLevel = 0;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                sys.lvl2InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl2Value = sys.lvl2InitialValue;
                Debug.Log("New gnome value: " + sys.lvl2Value);
                displayablePrestigeLevel = 1;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                sys.lvl3InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl3Value = sys.lvl3InitialValue;
                Debug.Log("New gnome value: " + sys.lvl3Value);
                displayablePrestigeLevel = 2;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                sys.lvl4InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl4Value = sys.lvl4InitialValue;
                Debug.Log("New gnome value: " + sys.lvl4Value);
                displayablePrestigeLevel = 3;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                sys.lvl5InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl5Value = sys.lvl5InitialValue;
                Debug.Log("New gnome value: " + sys.lvl5Value);
                displayablePrestigeLevel = 4;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                sys.lvl6InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl6Value = sys.lvl6InitialValue;
                Debug.Log("New gnome value: " + sys.lvl6Value);
                displayablePrestigeLevel = 5;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
        }

        // Reset conveyor values
        conveyor.speed = conveyor.initialSpeed;
        Debug.Log("Conveyor belt speed reset to " + conveyor.speed);

        // Reset manufacturing values
        manufacturer.manufacturingTime = manufacturer.initialManuTime + (manufacturer.initialManuTime * manufacturerDecreasePercent);
        Debug.Log("New manufacturing speed: " + manufacturer.manufacturingTime);

        // Reset and adjust total costs
        for (int i = 0; i < upgradeSys.Count; i++)
        {
            upgradeSys[i].SendMessage("ResetAndAdjustPrices", priceIncreasePercent);
        }

        for (int i = 0; i < manufacturer.objectsList.Count; i++)
        {
            Destroy(manufacturer.objectsList[i]);
        }

        Debug.Log("Upgraded Prestige to " + sys.prestigeLvl);
    }

    private void PromptWindow()
    {
        promptParent.SetActive(true);
        for (int i = 0; i < promptUIToDisable.Count; i++)
        {
            promptUIToDisable[i].SetActive(false);
        }
        for (int i = 0; i < PromptUIToEnable.Count; i++)
        {
            PromptUIToEnable[i].SetActive(true);
        }
    }

    public enum PrestigeType
    {
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5,
        FinalPrestige
    }
}

public class PrototypeUpgrades : MonoBehaviour
{
    [Header("Values")]
    public UpgradeType upgradeType;
    public UpgradeCost upgradeCost;
    public float initialCost;
    [Tooltip("The rate at which the price increases in a curve.")] public float increaseRate;
    [Tooltip("How many of these upgrades the player can buy before reaching the max. Set to 0 for infinity.")] public int upgradeLimit;
    private float currentPrice;
    private float costPercentage;


    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeConveyor conveyor;
    [SerializeField] private PrototypeManufacturer manufacturer;
    [SerializeField] private PrototypeGnomeCoinSystem gnomeCoinSys;

    // Start is called before the first frame update
    void Start()
    {
        currentPrice = initialCost;
        switch (upgradeCost)
        {
            case UpgradeCost.Dollans:
                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                break;
            case UpgradeCost.GnomeCoins:
                sys.UpdatePrice(costText, true, "�", currentPrice, "");
                break;
        }
    }

    public void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }

    public void SetNewValues(float percentage)
    {
        switch (upgradeCost)
        {
            case UpgradeCost.Dollans:
                if (sys.pointScore >= currentPrice)
                {
                    sys.pointScore -= currentPrice;
                    sys.UpdatePrice(sys.moneyText, false, "Profit: $", sys.pointScore, "");

                    switch (upgradeType)
                    {
                        case UpgradeType.GnomeValue:
                            switch (sys.prestigeLvl)
                            {
                                case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                                    sys.lvl1Value += (sys.lvl1InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl1Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                    break;
                                // These will need to be tested as to whether to use each initial value or lvl1InitialValue across the board
                                case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                                    sys.lvl2Value += (sys.lvl2InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl2Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl2Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                                    sys.lvl3Value += (sys.lvl3InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl3Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl3Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                                    sys.lvl4Value += (sys.lvl4InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl4Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl4Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                                    sys.lvl5Value += (sys.lvl5InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl5Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl5Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                                    sys.lvl6Value += (sys.lvl6InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl6Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl6Value, "");
                                    break;
                            }
                            break;
                        case UpgradeType.ConveyorSpeed:
                            conveyor.speed += (conveyor.initialSpeed * percentage);
                            Debug.Log("Conveyor speed: " + conveyor.speed);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, false, "$", currentPrice, "");
                            break;
                        case UpgradeType.ManufactureTime:
                            manufacturer.manufacturingTime -= (manufacturer.initialManuTime * percentage);
                            Debug.Log("Manufacturing time: " + manufacturer.manufacturingTime);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, false, "$", currentPrice, "");
                            break;
                    }
                }
                break;
            case UpgradeCost.GnomeCoins:
                if (gnomeCoinSys.coinCount >= (int)currentPrice)
                {
                    gnomeCoinSys.coinCount -= (int)currentPrice;
                    sys.UpdatePrice(gnomeCoinSys.gnomeCoinText, true, "�", gnomeCoinSys.coinCount, "");

                    switch (upgradeType)
                    {
                        case UpgradeType.GnomeValue:
                            gnomeCoinSys.permanentValue += percentage;
                            Debug.Log("Permanent gnome value: " + gnomeCoinSys.permanentValue);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "�", currentPrice, "");
                            break;
                        case UpgradeType.ConveyorSpeed:
                            conveyor.speed += (conveyor.initialSpeed * percentage);
                            Debug.Log("Conveyor speed: " + conveyor.speed);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "$", currentPrice, "");
                            break;
                        case UpgradeType.ManufactureTime:
                            // Code here
                            break;
                    }
                }
                break;
        }
    }
    

    public void ResetAndAdjustPrices(float costIncrease)
    {
        currentPrice = initialCost + (initialCost * costIncrease);
        sys.UpdatePrice(costText, false, "$", currentPrice, "");
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime
    }

    public enum UpgradeCost
    {
        Dollans,
        GnomeCoins
    }
}

public class AdSystem : MonoBehaviour
{
    [Header("Values")] 
    [SerializeField] private float delayUntilBlackScreen;
    [SerializeField] private float delayUntilAd;
    [SerializeField] private float delayAfterAd;
    [SerializeField] private int chanceOfGnome;
    [SerializeField] private bool allowForAdSkip = true;
    [SerializeField] private int secondsToSkip;
    [SerializeField] private int secondsToSkipBlank;
    [SerializeField] private int coinsToReward;
    [SerializeField] private int blankAdTime;
    private bool endTrigger = false;
    private bool adSkipped = false;

    [Header("Object References")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private List<VideoClip> videos = new List<VideoClip>();
    [SerializeField] private VideoClip secretVideo;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Button adSkipButton;
    [SerializeField] private TextMeshProUGUI ageDenialText;
    [SerializeField] private Image ageDenialQrImage;
    [SerializeField] private List<AudioSource> sourcesToStop = new List<AudioSource>();
    private GnomeCoinSystem coinSys;
    private MainMenuScript menuSys;

    private void OnEnable()
    {
        coinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
        menuSys = GameObject.Find("mainMenuSystemHandler").GetComponent<MainMenuScript>();
    }

    public void PlayFakeAd() { StartCoroutine(PlayAd()); }
    public void SkipAd() { StopCoroutine(PlayAd()); adSkipped = true; StartCoroutine(EndVideo()); }

    private void GiveReward()
    {
        coinSys.AddCoins(coinsToReward, false);
    }

    private IEnumerator PlayAd()
    {
        foreach (AudioSource sources in sourcesToStop)
        {
            sources.Pause();
        }
        yield return new WaitForSeconds(delayUntilBlackScreen);
        blackScreen.enabled = true;
        yield return new WaitForSeconds(delayUntilAd);

        switch (menuSys.isOver13)
        {
            case true:
                // Pick from a random selection of videos
                int chance = Random.Range(0, chanceOfGnome);
                Debug.Log(chance);
                switch (chance)
                {
                    case 0:
                        videoPlayer.clip = secretVideo;
                        break;
                    case >0:
                        int videoSelection = Random.Range(0, videos.Count);
                        videoPlayer.clip = videos[videoSelection];
                        break;
                }
                rawImage.enabled = true;
                videoPlayer.enabled = true;
                videoPlayer.Play();
        
                switch (allowForAdSkip)
                {
                    case true:
                        StartCoroutine(SkipAdFeature());
                        break;
                    case false:
                        break;
                }

                float duration = (float)videoPlayer.clip.length;
                Debug.Log(duration);
                for (int i = 0; i <= duration; duration -= Time.deltaTime)
                {
                    Debug.Log(duration);
                    switch (endTrigger)
                    {
                        case true:
                            duration = 0;
                            yield return null;
                            break;
                        case false:
                            yield return null;
                            break;
                    }
                }
                break;
            case false:
                ageDenialText.enabled = true;
                ageDenialQrImage.enabled = true;
                switch (allowForAdSkip)
                {
                    case true:
                        StartCoroutine(SkipAdFeature());
                        break;
                    case false:
                        break;
                }

                float ageDenialDuration = blankAdTime;
                Debug.Log(ageDenialDuration);
                for (int i = 0; i <= ageDenialDuration; ageDenialDuration -= Time.deltaTime)
                {
                    Debug.Log(ageDenialDuration);
                    switch (endTrigger)
                    {
                        case true:
                            duration = 0;
                            yield return null;
                            break;
                        case false:
                            yield return null;
                            break;
                    }
                }
                break;
        }
        
        switch (adSkipped)
        {
            case true:
                break;
            case false:
                StartCoroutine(EndVideo());
                adSkipped = false;
                break;
        }
    }

    private IEnumerator SkipAdFeature()
    {
        adSkipButton.gameObject.SetActive(true);
        adSkipButton.interactable = false;
        TextMeshProUGUI buttonText = adSkipButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        switch (menuSys.isOver13)
        {
            case true:
                int seconds = secondsToSkip;
                for (int i = 0; i < seconds; seconds--)
                {
                    buttonText.text = seconds.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
            case false:
                int blankSeconds = secondsToSkipBlank;
                for (int i = 0; i < blankSeconds; blankSeconds--)
                {
                    buttonText.text = blankSeconds.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
        }
        buttonText.text = "X";
        adSkipButton.interactable = true;
    }

    // This method is to be invoked via the ad skip button OnClick event
    private IEnumerator EndVideo()
    {
        endTrigger = true;
        switch (menuSys.isOver13)
        {
            case true:
                videoPlayer.Stop();
                rawImage.enabled = false;
                videoPlayer.enabled = false;
                break;
            case false:
                ageDenialText.enabled = false;
                ageDenialQrImage.enabled = false;
                break;
        }
        StopCoroutine(SkipAdFeature());
        adSkipButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(delayAfterAd);
        blackScreen.enabled = false;
        foreach (AudioSource sources in sourcesToStop)
        {
            sources.Play();
        }

        switch (menuSys.isOver13)
        {
            case true:
                GiveReward();
                break;
            case false:
                Debug.Log("No reward was given for the ad due to the player being under 13.");
                break;
        }
        yield return null;
        endTrigger = false;
    }
}

public class CameraMoveMouse : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float dragSpeed = 2.0f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 5.0f;
    [SerializeField] private float maxZoom = 15.0f;
    [SerializeField] private Vector3 minBounds;
    [SerializeField] private Vector3 maxBounds;
    [SerializeField] private List<GameObject> objectsToFollowZ = new List<GameObject>();
    [SerializeField] private float objectFollowZOffset;
    [SerializeField] private List<GameObject> objectsToFollowX = new List<GameObject>();
    [SerializeField] private float objectFollowXOffset;

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Vector3 resetCameraPosition;

    private void Start()
    {
        resetCameraPosition = _camera.transform.position;
    }

    private void LateUpdate()
    {
        HandleDrag();
        HandleReset();
        HandleZoom();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentMousePosition;
            Vector3 newPosition = _camera.transform.position + difference * dragSpeed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

            _camera.transform.position = new Vector3(newPosition.x, _camera.transform.position.y, newPosition.z);

            for (int i = 0; i < objectsToFollowZ.Count; i++)
            {
                objectsToFollowZ[i].transform.position = new Vector3(objectsToFollowZ[i].transform.position.x, objectsToFollowZ[i].transform.position.y, newPosition.z + objectFollowZOffset);
            }
            
            for (int i = 0; i < objectsToFollowX.Count; i++)
            {
                objectsToFollowX[i].transform.position = new Vector3(newPosition.x + objectFollowXOffset, objectsToFollowX[i].transform.position.y, objectsToFollowX[i].transform.position.z);
            }
        }
    }

    private void HandleReset()
    {
        if (Input.GetMouseButton(1))
        {
            _camera.transform.position = resetCameraPosition;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = _camera.orthographicSize - scroll * zoomSpeed;

        _camera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }
}

public class CameraMoveTouch : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float dragSpeed = 2.0f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 5.0f;
    [SerializeField] private float maxZoom = 15.0f;
    [SerializeField] private Vector3 minBounds;
    [SerializeField] private Vector3 maxBounds;
    [SerializeField] private List<GameObject> objectsToFollowZ = new List<GameObject>();
    [SerializeField] private float objectFollowZOffset;
    [SerializeField] private List<GameObject> objectsToFollowX = new List<GameObject>();
    [SerializeField] private float objectFollowXOffset;

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Vector3 resetCameraPosition;

    private void Start()
    {
        resetCameraPosition = _camera.transform.position;
    }

    private void LateUpdate()
    {
        HandleDrag();
        HandleReset();
        HandleZoom();
    }

    private void HandleDrag()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = _camera.ScreenToWorldPoint(touch.position);
                isDragging = true;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                Vector3 currentTouchPosition = _camera.ScreenToWorldPoint(touch.position);
                Vector3 difference = dragOrigin - currentTouchPosition;
                Vector3 newPosition = _camera.transform.position + difference * dragSpeed * Time.deltaTime;

                newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
                newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

                _camera.transform.position = new Vector3(newPosition.x, _camera.transform.position.y, newPosition.z);

                for (int i = 0; i < objectsToFollowZ.Count; i++)
                {
                    objectsToFollowZ[i].transform.position = new Vector3(objectsToFollowZ[i].transform.position.x, objectsToFollowZ[i].transform.position.y, newPosition.z + objectFollowZOffset);
                }
                
                for (int i = 0; i < objectsToFollowX.Count; i++)
                {
                    objectsToFollowX[i].transform.position = new Vector3(newPosition.x + objectFollowXOffset, objectsToFollowX[i].transform.position.y, objectsToFollowX[i].transform.position.z);
                }
            }
        }
    }

    private void HandleReset()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if ((touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began) && touch1.tapCount == 2)
            {
                _camera.transform.position = resetCameraPosition;
            }
        }
    }

    private void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float newSize = _camera.orthographicSize + deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

            _camera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }
}

public class ClearObjects : MonoBehaviour
{
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    [SerializeField] private FinalFactorySystem sys;
    
    // This will temporarily unfreeze time, clear all objects and then freeze it again
    public void ClearAllFrozenConveyors()
    {
        Time.timeScale = 1.0f;
        
        for (int i = 0; i < sys.productionLines.Count; i++)
        {
            for (int j = 0; j < dispensers[i].objectsList.Count; i++)
            {
                Destroy(dispensers[i].objectsList[j]);
            }
        }

        Time.timeScale = 0.0f;
    }

    // This will just clear all objects without attempting to unfreeze time
    public void ClearAllUnfrozenConveyors()
    {
        for (int i = 0; i < sys.productionLines.Count; i++)
        {
            for (int j = 0; j < dispensers[i].objectsList.Count; i++)
            {
                Destroy(dispensers[i].objectsList[j]);
            }
        }
    }
}

public class DDOLManager : MonoBehaviour
{
    public bool isOneOffComplete = false;
    public int resetTimes = 0;
    public string sceneToLoad;
    public bool introSkipped = false;

    // Counter values for the end of the game
    public int totalGnomesMade;
    public int totalUpgradesBought;
    public float totalProfitMade;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(LoadDDOLManager());
    }

    private IEnumerator LoadDDOLManager()
    {
        yield return null;
        DontDestroyOnLoad(this.gameObject);
        yield return null;
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }
}

public class DropDown : MonoBehaviour
{
    [SerializeField] private MainMenuScript menuSys;

    public void HandleInputData(int val)
    {
        switch (val)
        {
            case 0:
                menuSys.adjective = "Amazing";
                break;
            case 1:
                menuSys.adjective = "Awesome";
                break;
            case 2:
                menuSys.adjective = "Charmful";
                break;
            case 3:
                menuSys.adjective = "Cheap";
                break;
            case 4:
                menuSys.adjective = "Elegant";
                break;
            case 5:
                menuSys.adjective = "Excellent";
                break;
            case 6:
                menuSys.adjective = "Fantastic";
                break;
            case 7:
                menuSys.adjective = "Happy";
                break;            
            case 8:
                menuSys.adjective = "Inexpensive";
                break;
            case 9:
                menuSys.adjective = "Jolly";
                break;
            case 10:
                menuSys.adjective = "Jovial";
                break;
            case 11:
                menuSys.adjective = "Magical";
                break;
            case 12:
                menuSys.adjective = "Magnificent";
                break;
            case 13:
                menuSys.adjective = "Majestic";
                break;            
            case 14:
                menuSys.adjective = "Nice";
                break;
            case 15:
                menuSys.adjective = "Outstanding";
                break;
            case 16:
                menuSys.adjective = "Smooth";
                break;
            case 17:
                menuSys.adjective = "Solid";
                break;
            case 18:
                menuSys.adjective = "Sturdy";
                break;
            case 19:
                menuSys.adjective = "Stylish";
                break;
            case 20:
                menuSys.adjective = "Whimsical";
                break;            
            case 21:
                menuSys.adjective = "Wonderful";
                break;
        }

        Debug.Log(menuSys.adjective);
    }  
}

public class FactoryNameChanger : MonoBehaviour
{
    private MainMenuScript menuSys;
    
    void OnEnable()
    {
        menuSys = GameObject.Find("mainMenuSystemHandler").GetComponent<MainMenuScript>();
        if (menuSys.adjective == "")
        {
            GetComponent<TextMeshPro>().text = "Mom & Pop's\n" + "Amazing Gnomes";
        }
        else
        {
            GetComponent<TextMeshPro>().text = "Mom & Pop's\n" + menuSys.adjective + " Gnomes";
        }
    }
}

public class FinalConveyor : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2.0f;
    [HideInInspector] public float initialSpeed;
    [HideInInspector] public GnomeCoinSystem gnomeCoinSys;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
    }

    private void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position -= transform.forward * (speed + (speed * gnomeCoinSys.permanentSpeed)) * Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * (speed + (speed * gnomeCoinSys.permanentSpeed)) * Time.deltaTime);
    }
}

public class FinalDispenser : MonoBehaviour
{
    [SerializeField] private GameObject spawnTrigger;
    [SerializeField] private GameObject newPrefab;
    [HideInInspector] public GnomeCoinSystem gnomeCoinSys;
    [SerializeField] private FinalFactorySystem sys;
    public float manufacturingTime;
    public float manufacturingCooldown;
    [HideInInspector] public float initialManuTime;
    [HideInInspector] public float initialManuCool;
    public GameObject timeSlider;
    public bool isAutomated;
    public List<GameObject> objectsList = new List<GameObject>();
    private bool isActivated = false;
    private Scrollbar slider;
    [SerializeField] private float objectXOffset;
    private DDOLManager ddolManager;

    void OnEnable()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
        
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
        ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
    }
    
    public void SpawnObject() // This function is to be called if trying to use the DelayedSpawn method
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }
    
    private IEnumerator DelayedSpawn()
    {
        // Work the manufacturing delay timer
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";
        sys.manufacturingButtonImage.sprite = sys.manufacturingButtonPressed;
        //manufacturingTime += (manufacturingTime * gnomeCoinSys.permanentTime);
        float manuT = manufacturingTime + gnomeCoinSys.permanentTime;
        manufacturingCooldown += (manufacturingCooldown * gnomeCoinSys.permanentCooldown);

        for (float timer = manuT; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manuT);
            float progress = Mathf.InverseLerp(0f, manuT, timer);
            slider.size = progress;
            yield return null;
        }

        // Instantiate the new raw materials from the dispenser
        Vector3 newPos = new Vector3(spawnTrigger.transform.position.x + objectXOffset, spawnTrigger.transform.position.y,
            spawnTrigger.transform.position.z);
        GameObject newObject = Instantiate(newPrefab, newPos, Quaternion.identity);
        newObject.tag = "gnome";
        objectsList.Add(newObject);
        ddolManager.totalGnomesMade++;

        // Work the manufacturer cooldown timer
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Cooling down...";
        for (float timer = 0; timer < manufacturingCooldown; timer += Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingCooldown);
            float progress = Mathf.InverseLerp(0f, manufacturingCooldown, timer);
            slider.size = progress;
            yield return null;
        }

        timeSlider.SetActive(false);
        sys.manufacturingButtonImage.sprite = sys.manufacturingButtonUnpressed;
        sys.FinishDispensing();
        isActivated = false;
    }

    public IEnumerator AutomatedSpawn()
    {
        // DO THIS FEATURE LATER
        yield return null;
    }
}

public class FinalFactorySystem : MonoBehaviour
{
    [Header("Values: General")]
    public float pointScore;
    public PrestigeLevel prestigeLvl = PrestigeLevel.Prestige0;
    public float lvl1Value;
    [HideInInspector] public float lvl1InitialValue;
    public float lvl2Value;
    [HideInInspector] public float lvl2InitialValue;
    public float lvl3Value;
    [HideInInspector] public float lvl3InitialValue;
    public float lvl4Value;
    [HideInInspector] public float lvl4InitialValue;
    public float lvl5Value;
    [HideInInspector] public float lvl5InitialValue;
    public float lvl6Value;
    [HideInInspector] public float lvl6InitialValue;
    [Range(1, 7)] public int productionLineAmount;
    private float switchPanelTime;
    private bool isProductionLinesSet = false;

    [Header("Debug Values")]
    public bool debugMode;
    public float instantPointAddition;
    private bool hasDebugRun;

    [Header("Object References: General")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI coinText;
    public List<GameObject> productionLines = new List<GameObject>();
    public List<GameObject> oneOffObjects = new List<GameObject>();
    private GnomeCoinSystem ddolManager;
    private DDOLManager ddolManager2;
    private GameObject switchPanelDismissVar;
    private GameObject switchPanelActivateVar;
    public Image gnomeCoinVignetteReference;
    [SerializeField] private TextMeshProUGUI debugMetrics;
    public Image manufacturingButtonImage;
    public Sprite manufacturingButtonUnpressed;
    public Sprite manufacturingButtonPressed;

    [Header("Object References: Audio")]
    [SerializeField] private AudioSource buttonSfxSource;
    [SerializeField] private AudioClip buttonInSfx;
    [SerializeField] private AudioClip buttonOutSfx;

    private void OnEnable()
    {
        SetUpDDOLManager();
        SetUpDDOLManagerOneOff();
        SetProductionLines();
        ddolManager.Initialise();
        
        switch (debugMode)
        {
            case true:
                debugMetrics.gameObject.SetActive(true);
                debugMetrics.GetComponent<NewDebugCanvas>().enabled = true;
                Debug.Log("Booper 1");
                break;
            case false:
                debugMetrics.gameObject.SetActive(false);
                debugMetrics.GetComponent<NewDebugCanvas>().enabled = false;
                Debug.Log("Booper 2");
                break;
        }

        lvl1InitialValue = lvl1Value;
        Application.targetFrameRate = 60;
        // Add code here for amount of production lines from save/load system
        AddScore(0);

        // Start intro sequence
        GetComponent<IntroSequence>().ProgressIntroStates();
    }

    private void SetUpDDOLManager()
    {
        // Set up all the DDOL manager stuff
        ddolManager = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
        ddolManager2 = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
        ddolManager.gnomeCoinText = coinText;
    }

    private void SetUpDDOLManagerOneOff()
    {
        // Functions for the one-off objects, meant for only one interaction
        for (int i = 0; i < oneOffObjects.Count; i++)
        {
            if (ddolManager.oneTimeObjectNames.Count != 0)
            {
                if (oneOffObjects[i].name != ddolManager.oneTimeObjectNames[i])
                {
                    // It's going here once you restart the level after the nuke detonation. See if you can figure out what's going on.
                    ddolManager.oneTimeObjectNames.Add(oneOffObjects[i].name);
                }
                else if (oneOffObjects[i].name == ddolManager.oneTimeObjectNames[i])
                {
                    Destroy(oneOffObjects[i]);
                    oneOffObjects.Remove(oneOffObjects[i]);
                }
            }
            else
            {
                ddolManager.oneTimeObjectNames.Add(oneOffObjects[i].name);
            }
        }
    }

    public void SetProductionLines()
    {
        // This code is just temporary to show off the feature, add code once save/load system is in
        switch (isProductionLinesSet)
        {
            case true:
                break;
            case false:
                foreach (var t in productionLines)
                {
                    t.SetActive(false);
                    isProductionLinesSet = true;
                }
                break;
        }
        for (int i = 0; i < productionLineAmount; i++)
        {
            productionLines[i].SetActive(true);
            float firstManufactureTime = productionLines[0].transform.GetComponentInChildren<FinalDispenser>().manufacturingTime;
            float firstConveyorSpeed = productionLines[0].transform.GetChild(0).GetComponentInChildren<FinalConveyor>().speed;
            productionLines[i].transform.GetComponentInChildren<FinalDispenser>().manufacturingTime = firstManufactureTime;
            productionLines[i].transform.GetChild(0).GetComponentInChildren<FinalConveyor>().speed = firstConveyorSpeed;
        }
    }

    public void AddScore(float amount)
    {
        pointScore += amount + (amount * ddolManager.permanentValue);
        ddolManager2.totalProfitMade += (amount + (amount * ddolManager.permanentValue));
        if (debugMode && !hasDebugRun) 
        {
            pointScore += instantPointAddition;
            hasDebugRun = true;
        }
        moneyText.text = "Profit: $" + RoundToNearestHundredth(pointScore).ToString("F2");
    }

    public void UpdatePrice(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, float newPrice, string afterText)
    {
        switch (isGnomeCoins)
        {
            case true:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString() + afterText;
                break;
            case false:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                break;
        }
    }

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        {
            EditorApplication.isPlaying = false;
        }
#else
        {
            Application.Quit();
        }
#endif
    }

    public void PauseGame(bool isPaused)
    {
        switch (isPaused)
        {
            case true:
                Time.timeScale = 0.0f;
                break;
            case false:
                Time.timeScale = 1.0f;
                break;
        }
    }

    public void ActivateDispensers()
    {
        buttonSfxSource.clip = buttonInSfx;
        buttonSfxSource.Play();
        manufacturingButtonImage.sprite = manufacturingButtonPressed;

        for (int i = 0; i < productionLineAmount; i++)
        {
            string dispenserName = new string("line0" + (i + 1) + "dispenserMachine");
            productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().SpawnObject();
        }
    }

    public void FinishDispensing()
    {
        buttonSfxSource.clip = buttonOutSfx;
        buttonSfxSource.Play();
        manufacturingButtonImage.sprite = manufacturingButtonUnpressed;
    }

    public void ResetProductionLineAmount()
    {
        if (productionLineAmount != 1)
        {
            for (int i = 1; i < productionLines.Count; i++)
            {
                productionLines[i].SetActive(false);
            }

            productionLineAmount = 1;
        }
    }

    public enum PrestigeLevel
    {
        Prestige0,
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5
    }
}

public class FinalMachineDespawnTrigger : MonoBehaviour
{
    [Header("General/Shared Properties")]
    public TriggerType triggerType;

    [Header("Machine Trigger Properties")]
    [SerializeField] private FinalMachineSystems parentToCallBackTo;
    [HideInInspector] public Vector3 oldObjectVelocity;

    [Header("Exit Trigger Properties")]
    public FinalFactorySystem gameManager;
    private float value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gnome")) // Only call the function for the manufactured objects to stop accidentally processing other objects
        {
            switch (triggerType)
            {
                case TriggerType.MachineTrigger:
                    parentToCallBackTo.MachineFunctions(other.gameObject, other.GetComponent<Rigidbody>().velocity);
                    break;
                case TriggerType.ExitTrigger:
                    switch (gameManager.prestigeLvl)
                    {
                        case FinalFactorySystem.PrestigeLevel.Prestige0:
                            value = gameManager.lvl1Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige1:
                            value = gameManager.lvl2Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige2:
                            value = gameManager.lvl3Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige3:
                            value = gameManager.lvl4Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige4:
                            value = gameManager.lvl5Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige5:
                            value = gameManager.lvl6Value;
                            break;
                    }
                    gameManager.AddScore(value);
                    Destroy(other.gameObject);
                    break;
            }
        }
    }

    public enum TriggerType
    {
        MachineTrigger,
        ExitTrigger
    }
}

public class FinalMachineSystems : MonoBehaviour
{
    [Tooltip("It's ideal that you set these regardless of machine type.")][Header("General/Shared Properties")]
    [SerializeField] private MachineType machineType;
    [SerializeField] private GameObject spawnTrigger;
    [SerializeField] private FinalMachineDespawnTrigger despawnTrigger;
    [SerializeField] private GameObject newPrefab;
    [SerializeField] private FinalDispenser dispenser;
    [HideInInspector] public DDOLManager initSys;
    public FinalFactorySystem sys;

    [Header("Painter Properties")]
    public List<Material> gnomeMaterialList = new List<Material>();
    
    [Header("Packager Properties")]
    [SerializeField] private GameObject jimboPrefab;
    [Tooltip("Put the max odds in whole numbers. For example, for a 1 in 10,000 chance, put 10000. This number CANNOT be 0.")] [SerializeField] private int chanceOfJimbo;
    private int jimboNumber = 0;
    private bool wasJimboSpawned;
    
    private void OnEnable()
    {
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        initSys = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
    }

    public void MachineFunctions(GameObject other, Vector3 objectVelocity)
    {
        switch (machineType)
        {
            case MachineType.Sprayer: // The functions for the sprayer machine
                Vector3 newSprayerObjectVelocity = objectVelocity;
                Destroy(other);
                GameObject newSprayedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                newSprayedObject.tag = "gnome";
                dispenser.objectsList.Add(newSprayedObject);
                newSprayedObject.GetComponent<Rigidbody>().velocity = newSprayerObjectVelocity;
                break;
            case MachineType.Moulder: // The functions for the moulder machine
                Vector3 newMoulderObjectVelocity = objectVelocity;
                Destroy(other);
                GameObject newMouldedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                newMouldedObject.tag = "gnome";
                dispenser.objectsList.Add(newMouldedObject);
                newMouldedObject.GetComponent<Rigidbody>().velocity = newMoulderObjectVelocity;
                break;
            case MachineType.Painter: // The functions for the painter machine
                // Apply the prestige-relevant material to the newly-spawned gnome
                if (initSys.resetTimes == 0)
                {
                    switch (sys.prestigeLvl)
                    {
                        case FinalFactorySystem.PrestigeLevel.Prestige0: // Dirty red gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[0];
                            }
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige1: // Red gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[1];
                            }
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige2: // Yellow gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[2];
                            }
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige3: // Green gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[3];
                            }
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige4: // Cyan gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[4];
                            }
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige5: // Purple gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[5];
                            }
                            break;
                    }
                }
                else if (initSys.resetTimes >= 1) // If the game has been reset at least once, golden gnome material
                {
                    for (int i = 0; i < other.transform.childCount; i++)
                    {
                        other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[6];
                    }
                }
                other.transform.position = spawnTrigger.transform.position;
                other.GetComponent<Rigidbody>().velocity = objectVelocity;
                break;
            case MachineType.Packager:
                Destroy(other);
                // Do a roll between a gnome and Jimbo, and then instantiate
                jimboNumber = Random.Range(0, chanceOfJimbo);
                Debug.Log(jimboNumber);
                if (jimboNumber == 0 && sys.prestigeLvl != FinalFactorySystem.PrestigeLevel.Prestige0)
                {
                    GameObject newJimbo = Instantiate(jimboPrefab, spawnTrigger.transform.position, Quaternion.identity);
                    newJimbo.tag = "gnome";
                    dispenser.objectsList.Add(newJimbo);
                    newJimbo.GetComponent<Rigidbody>().velocity = objectVelocity;
                }
                else // This is to spawn the raw materials out of the dispenser
                {
                    GameObject newPackagedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                    newPackagedObject.tag = "gnome";
                    dispenser.objectsList.Add(newPackagedObject);
                    newPackagedObject.GetComponent<Rigidbody>().velocity = objectVelocity;
                }
                break;
        }
    }
    
    

    private enum MachineType // YOU MUST SET YOUR MACHINES TO ONE OF THESE IN ORDER TO MAKE THIS SCRIPT WORK!
    {
        Sprayer,
        Moulder,
        Painter,
        Packager
    }
}

public class FinalStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalText1;
    [SerializeField] private TextMeshProUGUI finalText2;
    [SerializeField] private float endingDelayTime;
    [SerializeField] private string sceneToLoad;
    private DDOLManager ddolManager;

    // Update is called once per frame
    void OnEnable()
    {
        ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();

        finalText1.text = "Total gnomes manufactured:\n" + ddolManager.totalGnomesMade + "\n\nTotal upgrades bought:\n" + ddolManager.totalUpgradesBought + "\n\n\n\n\n ";
        finalText2.text = "\n\n\n\n\n\n\n\nTotal profit made:\n$" + ddolManager.RoundToNearestHundredth(ddolManager.totalProfitMade).ToString("F2");
        StartCoroutine(EndingDelay());
    }

    private IEnumerator EndingDelay()
    {
        yield return new WaitForSeconds(endingDelayTime);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}

public class FinalUpgrades : MonoBehaviour
{
    [Header("Values")]
    public UpgradeType upgradeType;
    public UpgradeCost upgradeCost;
    public float initialCost;
    [Tooltip("The rate at which the price increases in a curve.")] public float increaseRate;
    [Tooltip("How many of these upgrades the player can buy before reaching the max. Set to 0 for infinity.")] public int upgradeLimit;
    [Tooltip("The amount that the upgrade limit increases each prestige.")] [SerializeField] private int upgradeLimitIncrease;
    private float currentPrice;
    private float costPercentage;
    private int currentBuyAmount;
    private Color initialSliderColour;
    private bool hasRanOnce = false;


    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Slider slider;
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private List<FinalConveyor> conveyors = new List<FinalConveyor>();
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    private GnomeCoinSystem gnomeCoinSys;
    private DDOLManager ddolManager;

    public void OnEnable()
    {
        switch (hasRanOnce)
        {
            case false:
                gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
                ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();

                // Set the first prices for upgrades
                currentPrice = initialCost;
                switch (upgradeCost)
                {
                    case UpgradeCost.Dollans:
                        sys.UpdatePrice(costText, false, "$", currentPrice, "");
                        break;
                    case UpgradeCost.GnomeCoins:
                        sys.UpdatePrice(costText, true, "c", currentPrice, "");
                        break;
                }

                initialSliderColour = slider.transform.GetChild(0).GetComponent<Image>().color;

                if (upgradeLimit == 0)
                {
                    slider.transform.GetChild(1).gameObject.SetActive(false);
                }

                hasRanOnce = true;
                break;
            case true:
                break;
        }
    }

    public void SetNewValues(float percentage)
    {
        switch (upgradeCost)
        {
            case UpgradeCost.Dollans:
                if (sys.pointScore >= currentPrice)
                {
                    if (currentBuyAmount != upgradeLimit)
                    {
                        sys.pointScore -= currentPrice;
                        ddolManager.totalUpgradesBought++;
                        sys.UpdatePrice(sys.moneyText, false, "Profit: $", sys.pointScore, "");

                        switch (upgradeType)
                        {
                            case UpgradeType.GnomeValue:
                                switch (sys.prestigeLvl)
                                {
                                    case FinalFactorySystem.PrestigeLevel.Prestige0:
                                        sys.lvl1Value += (sys.lvl1InitialValue * percentage);
                                        Debug.Log("Gnome value: " + sys.lvl1Value);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                        break;
                                    case FinalFactorySystem.PrestigeLevel.Prestige1:
                                        sys.lvl2Value += (sys.lvl2InitialValue * percentage);
                                        Debug.Log("Gnome value: " + sys.lvl2Value);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", sys.lvl2Value, "");
                                        break;
                                    case FinalFactorySystem.PrestigeLevel.Prestige2:
                                        sys.lvl3Value += (sys.lvl3InitialValue * percentage);
                                        Debug.Log("Gnome value: " + sys.lvl3Value);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", sys.lvl3Value, "");
                                        break;
                                    case FinalFactorySystem.PrestigeLevel.Prestige3:
                                        sys.lvl4Value += (sys.lvl4InitialValue * percentage);
                                        Debug.Log("Gnome value: " + sys.lvl4Value);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", sys.lvl4Value, "");
                                        break;
                                    case FinalFactorySystem.PrestigeLevel.Prestige4:
                                        sys.lvl5Value += (sys.lvl5InitialValue * percentage);
                                        Debug.Log("Gnome value: " + sys.lvl5Value);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", sys.lvl5Value, "");
                                        break;
                                    case FinalFactorySystem.PrestigeLevel.Prestige5:
                                        sys.lvl6Value += (sys.lvl6InitialValue * percentage);
                                        Debug.Log("Gnome value: " + sys.lvl6Value);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", sys.lvl6Value, "");
                                        break;
                                }
                                break;
                            case UpgradeType.ConveyorSpeed:
                                for (int i = 0; i < conveyors.Count; i++)
                                {
                                    switch (conveyors[i].isActiveAndEnabled)
                                    {
                                        case true:
                                            conveyors[i].speed += (conveyors[i].initialSpeed * percentage);
                                            Debug.Log("Conveyor speed: " + conveyors[i].speed);
                                            break;
                                        case false:
                                            break;
                                    }

                                }
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                break;
                            case UpgradeType.ManufactureTime:
                                for (int i = 0; i < dispensers.Count; i++)
                                {
                                    switch (dispensers[i].isActiveAndEnabled)
                                    {
                                        case true:
                                            dispensers[i].manufacturingTime -= (dispensers[i].initialManuTime * percentage);
                                            Debug.Log("Manufacturing time: " + dispensers[i].manufacturingTime);
                                            break;
                                        case false:
                                            break;
                                    }
                                }
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                break;
                            case UpgradeType.ProductionLines:

                                sys.productionLineAmount += (int)percentage;
                                sys.SetProductionLines();
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                break;
                        }
                        currentBuyAmount++;
                        if (currentBuyAmount != upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                        }
                        else if (currentBuyAmount == upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                            upgradeButton.interactable = false;
                            Color sliderColour = slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
                            slider.transform.GetChild(1).gameObject.SetActive(false);
                            slider.transform.GetChild(0).GetComponent<Image>().color = sliderColour;
                        }
                    }
                }
                break;
            case UpgradeCost.GnomeCoins:
                if (gnomeCoinSys.coinCount >= (int)currentPrice)
                {
                    if (currentBuyAmount != upgradeLimit)
                    {
                        gnomeCoinSys.coinCount -= (int)currentPrice;
                        ddolManager.totalUpgradesBought++;
                        sys.UpdatePrice(gnomeCoinSys.gnomeCoinText, true, "c", gnomeCoinSys.coinCount, "");

                        switch (upgradeType)
                        {
                            case UpgradeType.GnomeValue:
                                gnomeCoinSys.permanentValue += percentage;
                                Debug.Log("Permanent gnome value: " + gnomeCoinSys.permanentValue);
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, true, "c", currentPrice, "");
                                break;
                            case UpgradeType.ConveyorSpeed:
                                gnomeCoinSys.permanentSpeed += percentage;
                                Debug.Log("Permanent conveyor speed: " + gnomeCoinSys.permanentSpeed);
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, true, "c", currentPrice, "");
                                break;
                            case UpgradeType.ManufactureTime:
                                gnomeCoinSys.permanentTime -= percentage;
                                Debug.Log("Permanent manufacturing time: " + gnomeCoinSys.permanentTime);
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, true, "c", currentPrice, "");
                                break;
                        }

                        currentBuyAmount++;
                        if (currentBuyAmount != upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                        }
                        else if (currentBuyAmount == upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                            upgradeButton.interactable = false;
                            Color sliderColour = slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
                            slider.transform.GetChild(1).gameObject.SetActive(false);
                            slider.transform.GetChild(0).GetComponent<Image>().color = sliderColour;
                        }
                    }
                }
                break;
        }
    }
    

    public void ResetAndAdjustPrices(float costIncrease)
    {
        currentPrice = initialCost + (initialCost * costIncrease);
        costPercentage = 0;
        sys.UpdatePrice(costText, false, "$", currentPrice, "");
        slider.value = 0;
        currentBuyAmount = 0;
        switch (sys.prestigeLvl)
        {
            case FinalFactorySystem.PrestigeLevel.Prestige1:
                upgradeLimit += (1 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige2:
                upgradeLimit += (2 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige3:
                upgradeLimit += (3 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige4:
                upgradeLimit += (4 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige5:
                upgradeLimit += (5 * upgradeLimitIncrease);
                break;
        }
        slider.transform.GetChild(1).gameObject.SetActive(true);
        slider.transform.GetChild(0).GetComponent<Image>().color = initialSliderColour;
        upgradeButton.interactable = true;
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime,
        ProductionLines
    }

    public enum UpgradeCost
    {
        Dollans,
        GnomeCoins
    }
}

public class GnomeCoinShopSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float spinnerTime = 2f;
    private bool isReadyToDestroy = false;

    [Header("Object References")]
    private GnomeCoinSystem gnomeCoinSys;
    public GameObject promptBackground;
    public GameObject spinnerBackground;
    [SerializeField] private AudioSource UiSfxSource;
    [SerializeField] private AudioClip chaChingSfx;

    public void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }
    public void BuyGnomeCoins(int amount)
    {
        StartCoroutine(GnomeCoinPurchaseProcess(amount));
    }

    public void DisableListing(GameObject objectToDestroy)
    {
        StartCoroutine(DisableListingDelay(objectToDestroy));
    }

    IEnumerator GnomeCoinPurchaseProcess(int amountToBuy)
    {
        spinnerBackground.SetActive(true);
        yield return new WaitForSeconds(spinnerTime);
        gnomeCoinSys.AddCoins(amountToBuy, false);
        spinnerBackground.SetActive(false);
        UiSfxSource.PlayOneShot(chaChingSfx);
        isReadyToDestroy = true;
    }

    IEnumerator DisableListingDelay(GameObject obj)
    {
        while (!isReadyToDestroy)
        {
            yield return null;
        }
        obj.SetActive(false);
        isReadyToDestroy = false;
        gnomeCoinSys.oneTimeObjects.Add(obj);
    }
}

public class GnomeCoinSystem : MonoBehaviour
{
    [Header("Object References")]
    public TextMeshProUGUI gnomeCoinText;
    private FinalFactorySystem sys;
    private MainMenuScript menuSys;
    public List<GameObject> oneTimeObjects = new List<GameObject>();
    public List<string> oneTimeObjectNames = new List<string>();
    private Image vignette;

    [Header("Values")]
    public int coinCount;
    public float permanentValue;
    public float permanentSpeed;
    public float permanentTime;
    public float permanentCooldown;
    [SerializeField] private int passiveIncomeAmount;
    [SerializeField] private float passiveIncomeTime;
    [SerializeField] private int flashAmount;
    [SerializeField] private float flashLength;
    [SerializeField] private int passiveFlashAmount;
    [SerializeField] private float passiveFlashLength;

    public void Initialise()
    {
        if (gnomeCoinText != null)
        {
            gnomeCoinText.text = "Coins: c" + coinCount;
        }
        AddReferences();
        switch (menuSys.isOver13)
        {
            case false:
                StartCoroutine(PassiveIncome());
                break;
            case true:
                break;
        }
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd, bool isPassive)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "Coins: c" + coinCount;
        StartCoroutine(FlashScreenGreen(isPassive));
    }

    private void AddReferences()
    {
        sys = GameObject.Find("gameManager").GetComponent<FinalFactorySystem>();
        menuSys = GameObject.Find("mainMenuSystemHandler").GetComponent<MainMenuScript>();
        vignette = sys.gnomeCoinVignetteReference;
    }

    private IEnumerator PassiveIncome()
    {
        while (!menuSys.isOver13)
        {
            yield return new WaitForSeconds(passiveIncomeTime);
            AddCoins(passiveIncomeAmount, true);
        }
    }

    private IEnumerator FlashScreenGreen(bool isPassive)
    {
        vignette.gameObject.SetActive(true);
        switch (isPassive)
        {
            case true:
                for (int i = 0; i < passiveFlashAmount; i++)
                {
                    vignette.enabled = true;
                    yield return new WaitForSeconds(passiveFlashLength);
                    vignette.enabled = false;
                    yield return new WaitForSeconds(passiveFlashLength);
                }
                vignette.gameObject.SetActive(false);
                break;
            case false:
                for (int i = 0; i < flashAmount; i++)
                {
                    vignette.enabled = true;
                    yield return new WaitForSeconds(flashLength);
                    vignette.enabled = false;
                    yield return new WaitForSeconds(flashLength);
                }
                vignette.gameObject.SetActive(false);
                break;
        }
    }
}

public class IntroSequence : MonoBehaviour
{
    [Header("Values")]
    private bool skipped; // Change this setting in the DDOL Manager
    [SerializeField] private float fadeTime;
    [SerializeField] private float delayTime;
    private int spriteState = 0;
    private float timeRemaining = 0;

    [Header("Object References > General")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject spriteHolder;
    [SerializeField] private Image blockingImage; // This is so that the player can't play the game during the intro
    [SerializeField] private SwitchPanels switchPanels;
    [SerializeField] private TextMeshProUGUI dialogueHeader;
    [SerializeField] private TextMeshProUGUI dialogueBody;
    private DDOLManager ddolManager;

    [Header("Object References > Panel Switch")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject controlPanelDismissalPoint;
    [SerializeField] private GameObject controlPanelActivationPoint;
    [SerializeField] private GameObject upgradesPanel;
    [SerializeField] private GameObject upgradesPanelDismissalPoint;
    [SerializeField] private GameObject upgradesPanelActivationPoint;    
    [SerializeField] private GameObject prestigePanel;
    [SerializeField] private GameObject prestigePanelDismissalPoint;
    [SerializeField] private GameObject prestigePanelActivationPoint;    
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private GameObject notificationPanelDismissalPoint;
    [SerializeField] private GameObject notificationPanelActivationPoint;
    [SerializeField] private GameObject mainScreens;
    [SerializeField] private GameObject mainScreensDismissalPoint;
    [SerializeField] private GameObject mainScreensActivationPoint;
    [SerializeField] private GameObject gnomeShopButton;
    [SerializeField] private GameObject gnomeShopButtonDismissalPoint;
    [SerializeField] private GameObject gnomeShopButtonActivationPoint;

    [Header("Object References > Sprites")]
    [SerializeField] private Image neutralArmUpSprite;
    [SerializeField] private Image neutralArmDownSprite;
    [SerializeField] private Image nervousSprite;
    [SerializeField] private Image unhappySprite;
    [SerializeField] private Image thinkingSprite;

    public void ProgressIntroStates()
    {
        ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
        skipped = ddolManager.introSkipped;

        switch (skipped)
        {
            case false:
                spriteState++;

                // Huge list of states for the intro sequence. This works by just calling this method, which will progress the state (thank God for switch statements).
                switch (spriteState)
                {
                    case 1:
                        // Set everything up the first time
                        dialoguePanel.SetActive(true);
                        spriteHolder.SetActive(true);
                        dialogueHeader.enabled = true;
                        dialogueBody.enabled = true;
                        neutralArmUpSprite.enabled = false;
                        neutralArmDownSprite.enabled = false;
                        nervousSprite.enabled = false;
                        unhappySprite.enabled = false;
                        thinkingSprite.enabled = false;
                        blockingImage.enabled = true;
                        controlPanel.transform.position = controlPanelDismissalPoint.transform.position;
                        mainScreens.transform.position = mainScreensDismissalPoint.transform.position;
                        gnomeShopButton.transform.position = gnomeShopButtonDismissalPoint.transform.position;

                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Welcome! You must be our new manager for the factory.";
                        break;
                    case 2:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Thanks for agreeing to help manage the factory. We really needed an extra set of hands here.";
                        break;
                    case 3:
                        SwitchSprites(neutralArmUpSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Things have been running a bit slow here lately. Not much business has been coming around here for a while.";
                        break;
                    case 4:
                        SwitchSprites(thinkingSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "The big guys moved in not long ago, you see, and they've been taking up all the business.";
                        break;
                    case 5:
                        SwitchSprites(thinkingSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "They've been harassing small honest businesses like ours so they can stomp the competition.";
                        break;
                    case 6:
                        SwitchSprites(null, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Old timers like me and my wife don't stand a chance against them corpo fellas and their deep pockets.";
                        break;
                    case 7:
                        SwitchSprites(nervousSprite, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "But that's why we got you. We reckon you'll do a much better job in keeping the business afloat.";
                        break;
                    case 8:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "So let me take you through how to use all the equipment and we'll have you up and running in no-time.";
                        break;
                    case 9:
                        dialoguePanel.SetActive(false);
                        switchPanels.SetActivationValuesThroughScript(controlPanel, 0.5f, controlPanelActivationPoint);
                        switchPanels.ExecuteSmooth(2);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 10:
                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "The manufacture button dispenses the material onto your production line, allowing it to go through the production line and make a gnome.";
                        break;
                    case 11:
                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your two big side buttons are the upgrade and prestige buttons. They call the upgrade and prestige menus, which I'll show you in a sec.";
                        break;
                    case 12:
                        SwitchSprites(null, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your two small side buttons are the notification and mode buttons. I'll take you through those as well shortly.";
                        break;
                    case 13:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        spriteHolder.transform.localScale = new Vector3(-1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(controlPanel, 0.5f, controlPanelDismissalPoint);
                        switchPanels.SetActivationValuesThroughScript(upgradesPanel, 0.5f, upgradesPanelActivationPoint);
                        switchPanels.ExecuteSmooth(0);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 14:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your standard upgrades cost money, which you earn by producing gnomes, whereas permanent upgrades cost Gnome Coins.";
                        break;
                    case 15:
                        SwitchSprites(neutralArmUpSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Gnome Coins are a special currency that you can't earn by manufacturing gnomes. You'll have to pay with real money for those.";
                        break;
                    case 16:
                        SwitchSprites(nervousSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Some upgrades are also limited, meaning you can only buy a certain amount. These upgrades have bars to show how many are left.";
                        break;
                    case 17:
                        SwitchSprites(thinkingSprite, neutralArmUpSprite, false);
                        spriteHolder.transform.localScale = new Vector3(1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(upgradesPanel, 0.5f, upgradesPanelDismissalPoint);
                        switchPanels.SetActivationValuesThroughScript(prestigePanel, 0.5f, prestigePanelActivationPoint);
                        switchPanels.ExecuteSmooth(0);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 18:
                        SwitchSprites(neutralArmDownSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Keep in mind that while your gnomes become more valuable, your machines also get a bit slower because of the added weight.";
                        break;
                    case 19:
                        SwitchSprites(thinkingSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Your upgrades and profit are also reset when you prestige, so be careful not to rush into prestiging too quickly.";
                        break;
                    case 20:
                        SwitchSprites(nervousSprite, neutralArmUpSprite, false);
                        spriteHolder.transform.localScale = new Vector3(-1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(prestigePanel, 0.5f, prestigePanelDismissalPoint);
                        switchPanels.SetActivationValuesThroughScript(notificationPanel, 0.5f, notificationPanelActivationPoint);
                        switchPanels.ExecuteSmooth(0);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 21:
                        SwitchSprites(null, null, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Whenever you receive a notification, your main screen (the one that tracks your profit) will flash green and turn on a little bell.";
                        break;
                    case 22:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Sometimes we'll get letters in the mail or offers from companies from Gnome Coins, so keep an eye out for those.";
                        break;
                    case 23:
                        SwitchSprites(null, null, false);
                        spriteHolder.transform.localScale = new Vector3(1, spriteHolder.transform.localScale.y, spriteHolder.transform.localScale.z);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(notificationPanel, 0.5f, notificationPanelDismissalPoint);
                        switchPanels.ExecuteSmooth(1);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 24:
                        SwitchSprites(thinkingSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "I've heard it's connected to the prestige panel in some way, but that's all I know. I'm sure you'll figure it out.";
                        break;
                    case 25:
                        SwitchSprites(nervousSprite, thinkingSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Now what else have I missed?";
                        break;
                    case 26:
                        SwitchSprites(thinkingSprite, neutralArmDownSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Ah, of course, the stat tracker screens! I mentioned it before but my rickety old self forgot to show you.";
                        break;
                    case 27:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetActivationValuesThroughScript(mainScreens, 0.5f, mainScreensActivationPoint);
                        switchPanels.ExecuteSmooth(2);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 28:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "The only thing you really need to know is the passive income, which is calculated depending on the amount of production lines you've automated.";
                        break;
                    case 29:
                        SwitchSprites(null, null, false);
                        dialoguePanel.SetActive(false);
                        switchPanels.SetDismissalValuesThroughScript(mainScreens, 0.5f, mainScreensDismissalPoint);
                        switchPanels.ExecuteSmooth(1);
                        StartCoroutine(DelayedAction(delayTime));
                        break;
                    case 30:
                        SwitchSprites(neutralArmDownSprite, neutralArmUpSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "And hey, I'm sure with your help, we'll be able to teach those corpo fellas a thing about picking on hardworking guys like us!";
                        break;
                    case 31:
                        SwitchSprites(neutralArmUpSprite, nervousSprite, false);
                        dialogueHeader.text = "Pop";
                        dialogueBody.text = "Good luck, kiddo. Go show them who's the real boss around here.";
                        break;
                    case 32:
                        // Finish the intro sequence here
                        dialogueHeader.enabled = false;
                        dialogueBody.enabled = false;
                        neutralArmUpSprite.enabled = false;
                        neutralArmDownSprite.enabled = false;
                        nervousSprite.enabled = false;
                        unhappySprite.enabled = false;
                        thinkingSprite.enabled = false;
                        dialoguePanel.SetActive(false);
                        spriteHolder.SetActive(false);

                        mainScreens.GetComponent<SwitchPanels>().SetActivationValuesThroughScript(mainScreens, 0.5f, mainScreensActivationPoint);
                        mainScreens.GetComponent<SwitchPanels>().ExecuteSmooth(2);
                        controlPanel.GetComponent<SwitchPanels>().SetActivationValuesThroughScript(controlPanel, 0.5f, controlPanelActivationPoint);
                        controlPanel.GetComponent<SwitchPanels>().ExecuteSmooth(2);
                        switchPanels.GetComponent<SwitchPanels>().SetActivationValuesThroughScript(gnomeShopButton, 0.5f, gnomeShopButtonActivationPoint);
                        switchPanels.GetComponent<SwitchPanels>().ExecuteSmooth(2);

                        ddolManager.introSkipped = true;
                        blockingImage.enabled = false;
                        break;
                }
                break;
            case true:
                break;
        }
    }

    private void SwitchSprites(Image previousSprite, Image nextSprite, bool fadeIn)
    {
        switch (fadeIn)
        {
            // ### DEV NOTE: PLEASE ADD FADE IN FUNCTION TO THIS LATER ###
            case true:
                if (previousSprite != null)
                {
                    previousSprite.color = new Color(previousSprite.color.r, previousSprite.color.g, previousSprite.color.b, 1);
                    previousSprite.canvasRenderer.SetAlpha(224f);
                    previousSprite.CrossFadeAlpha(0f, fadeTime, false);
                    previousSprite.enabled = false;
                }
                if (nextSprite != null)
                {
                    nextSprite.color = new Color(nextSprite.color.r, nextSprite.color.g, nextSprite.color.b, 0);
                    nextSprite.canvasRenderer.SetAlpha(0.01f);
                    nextSprite.CrossFadeAlpha(1f, fadeTime, false);
                    nextSprite.enabled = true;
                }
                break;
            case false:
                if (previousSprite != null)
                {
                    previousSprite.enabled = false;
                }
                if (nextSprite != null)
                {
                    nextSprite.enabled = true;
                }
                break;
        }
    }

    private IEnumerator DelayedAction(float time)
    {
        timeRemaining = time;
        for (int i = 0; i < timeRemaining; timeRemaining -= Time.deltaTime)
        {
            Debug.Log(timeRemaining);
            yield return null;
        }
        switch (spriteState)
        {
            case 9:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This here is your control panel. This is where all of your controls are for controlling the factory.";
                break;
            case 13:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This is your upgrade panel. Here you can buy upgrades to increase the production value, speed, efficiency, et cetera.";
                break;
            case 17:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This is your prestige panel. By prestiging, you're upgrading your factory equipment to allow it to handle more valuable materials.";
                break;
            case 20:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "This is your notification panel. Whenever the system wants to tell you something, it'll be sent here.";
                break;
            case 23:
                SwitchSprites(neutralArmUpSprite, thinkingSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "As for the mode button, well, we could never really figure out how to get that working.";
                break;
            case 27:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "These screens show you your profit, passive income, Gnome Coins and prestige level.";
                break;
            case 29:
                SwitchSprites(neutralArmUpSprite, neutralArmDownSprite, false);
                dialoguePanel.SetActive(true);
                dialogueHeader.text = "Pop";
                dialogueBody.text = "Well, I think that's about everything you need to know to start helping us run this place.";
                break;
        }
    }
}

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeTime = 1f;
    public bool isOver13;
    public string adjective;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        {
            EditorApplication.isPlaying = false;
        }
#else
        {
            Application.Quit();
        }
#endif
    }

    public void AgeCheck(int ageRange)
    {
        switch (ageRange)
        {
            case 0:
                isOver13 = false;
                break;
            case 1:
                isOver13 = true;
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string scene)
    {
        blackScreen.enabled = true;
        Color fixedColor = blackScreen.color;
        fixedColor.a = 0;
        blackScreen.color = fixedColor;
        blackScreen.CrossFadeAlpha(1, fadeTime, false);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
}

public class ModeSwitch : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("0 = disabled, 1 = enabled")][Range(0, 1)] public int mode;
    [SerializeField] private float sinkingPosAdditionY;
    [SerializeField] private float sinkingTime;
    [SerializeField] private float sinkingTimePostDelay;
    [SerializeField] private float risingTime;
    private float risingPosY;

    [Header("Object References")]
    [SerializeField] private Button modeButton;
    [SerializeField] private GameObject manufactureButton;
    [SerializeField] private GameObject detonateButton;
    [SerializeField] private GameObject panelPlate;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip machineWhirr1;
    [SerializeField] private AudioClip machineWhirr2;
    [SerializeField] private AudioClip machineThud;
    
    void OnEnable()
    {
        switch (mode)
        {
            case 0:
                modeButton.interactable = false;
                manufactureButton.SetActive(true);
                detonateButton.SetActive(false);
                break;
            case 1:
                modeButton.interactable = true;
                manufactureButton.SetActive(false);
                detonateButton.SetActive(true);
                break;
        }

        risingPosY = manufactureButton.transform.position.y;
    }
    
    public void EnableModeButton()
    {
        modeButton.interactable = true;
    }

    public void SwitchButtons()
    {
        switch (mode)
        {
            case 0:
                StartCoroutine(DoButtonAnimation(manufactureButton, detonateButton));
                StartCoroutine(DoPanelPlateAnimation());
                mode = 1;
                break;
            case 1:
                StartCoroutine(DoButtonAnimation(detonateButton, manufactureButton));
                StartCoroutine(DoPanelPlateAnimation());
                mode = 0;
                break;
        }
    }

    private IEnumerator DoButtonAnimation(GameObject oldButton, GameObject newButton)
    {
        modeButton.GetComponent<Button>().interactable = false;
        oldButton.GetComponent<Button>().interactable = false;

        Vector3 buttonOldPos = oldButton.transform.position;
        Vector3 buttonNewPos = new Vector3(oldButton.transform.position.x, oldButton.transform.position.y + sinkingPosAdditionY, oldButton.transform.position.z);
        float timeElapsed = 0;

        sfxSource.clip = machineWhirr1;
        sfxSource.Play();

        while (timeElapsed < sinkingTime)
        {
            float t = timeElapsed / sinkingTime;
            Vector3 currentPosition = Vector3.Lerp(buttonOldPos, buttonNewPos, t);
            oldButton.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;

            if (!sfxSource.isPlaying && sfxSource.clip == machineWhirr1)
            {
                sfxSource.clip = machineWhirr2;
                sfxSource.loop = true;
                sfxSource.Play();
            }
        }
        oldButton.transform.position = buttonNewPos;
        sfxSource.loop = false;
        sfxSource.Stop();
        sfxSource.PlayOneShot(machineThud);

        yield return new WaitForSeconds(sinkingTimePostDelay);

        oldButton.SetActive(false);
        newButton.SetActive(true);
        newButton.GetComponent<Button>().interactable = false;

        //buttonOldPos = newButton.transform.position;
        //buttonNewPos = new Vector3(newButton.transform.position.x, newButton.transform.position.y + sinkingPosAdditionY, newButton.transform.position.z);
        timeElapsed = 0;

        sfxSource.clip = machineWhirr1;
        sfxSource.Play();
        
        while (timeElapsed < risingTime)
        {
            float t = timeElapsed / risingTime;
            Vector3 currentPosition = Vector3.Lerp(buttonNewPos, buttonOldPos, t);
            newButton.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;

            if (!sfxSource.isPlaying && sfxSource.clip == machineWhirr1)
            {
                sfxSource.clip = machineWhirr2;
                sfxSource.loop = true;
                sfxSource.Play();
            }
        }
        newButton.transform.position = buttonOldPos;
        sfxSource.loop = false;
        sfxSource.Stop();
        sfxSource.PlayOneShot(machineThud);

        newButton.GetComponent<Button>().interactable = true;
        modeButton.GetComponent<Button>().interactable = true;
    }

    private IEnumerator DoPanelPlateAnimation()
    {
        Vector3 panelPlateOldPos = panelPlate.transform.position;
        Vector3 panelPlateNewPos = new Vector3(panelPlate.transform.position.x, panelPlate.transform.position.y + sinkingPosAdditionY, panelPlate.transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < sinkingTime)
        {
            float t = timeElapsed / sinkingTime;
            Vector3 currentPosition = Vector3.Lerp(panelPlateOldPos, panelPlateNewPos, t);
            panelPlate.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panelPlate.transform.position = panelPlateNewPos;

        yield return new WaitForSeconds(sinkingTimePostDelay);

        //panelPlateOldPos = panelPlate.transform.position;
        //panelPlateNewPos = new Vector3(panelPlate.transform.position.x, panelPlate.transform.position.y + sinkingPosAdditionY, panelPlate.transform.position.z);
        timeElapsed = 0;

        while (timeElapsed < risingTime)
        {
            float t = timeElapsed / risingTime;
            Vector3 currentPosition = Vector3.Lerp(panelPlateNewPos, panelPlateOldPos, t);
            panelPlate.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panelPlate.transform.position = panelPlateOldPos;
    }
}

[RequireComponent(typeof(TextMeshProUGUI))]
public class NewDebugCanvas : MonoBehaviour
{
    private TextMeshProUGUI debugText;
    [SerializeField] private FinalConveyor line01Conveyor;
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private FinalDispenser line01Dispenser;
    [SerializeField] private GnomeCoinSystem gnomeCoinSys;

    // Dictionary to store bools by their names
    private Dictionary<string, bool> boolDictionary = new Dictionary<string, bool>();

    List<string> debugList = new List<string>();

    // Example bools (you can add more)
    [Header("General Metrics")]
    public bool enableFPSCounter;
    public bool enableFrameTiming;
    public bool enableLevelName;
    public bool enableResAndAspect;
    [Header("Gnome Tycoon Specific Metrics")]
    public bool enableGnomeValueMetric;
    public bool enableConveyorSpeedMetric;
    public bool enableManufacturingTimeMetric;

    // String to modify
    private string resultString = "";

    // String dictionary
    private string framerateString = "";
    private string frameTimingString = "";
    private string levelNameString = "";
    private string resAndAspectString = "";
    private string gnomeValueString = "";
    private string conveyorSpeedString = "";
    private string manufacturingTimeString = "";

    // Framerate values
    [Header("Metrics Settings")]
    [HideInInspector] public int fpsCounter_avgFrameRate;
    [Tooltip("Adjust how often the average framerate metre updates")] public float fpsCounter_updateInterval = 0.5F;
    private double fpsCounter_lastInterval;
    private int fpsCounter_frames;
    private float fpsCounter_fps;

    private void Start()
    {
        debugText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }

    private void Update()
    {
        CheckBools();

        // Reset dictionary per draw to allow for adding fresh stats
        boolDictionary.Clear();

        // Add the strings with their respective bools here
        boolDictionary.Add(framerateString, enableFPSCounter);
        boolDictionary.Add(frameTimingString, enableFrameTiming);
        boolDictionary.Add(levelNameString, enableLevelName);
        boolDictionary.Add(resAndAspectString, enableResAndAspect);
        boolDictionary.Add(gnomeValueString, enableGnomeValueMetric);
        boolDictionary.Add(conveyorSpeedString, enableConveyorSpeedMetric);
        boolDictionary.Add(manufacturingTimeString, enableManufacturingTimeMetric);

        // Reset list per draw to get fresh stats
        debugList.Clear();

        foreach (var kvp in boolDictionary)
        {
            if (kvp.Value)
            {
                debugList.Add(kvp.Key);
            }
        }

        // Reset string per draw to fix an overflow issue
        resultString = "";
        resultString += "/// Debug Metrics /// \n \n";

        for (int i = 0; i < debugList.Count; i++)
        {
            if (i != (debugList.Count + 1))
            {
                resultString += debugList[i] + " \n";
            }
            else
            {
                break;
            }
        }

        debugText.text = resultString;
    }

    // This big list of switch statements allows optimisation by only allowing functions that are needed to run.
    private void CheckBools()
    {
        switch (enableFPSCounter)
        {
            case true:
                Framerate();
                break;
        }

        switch (enableFrameTiming)
        {
            case true:
                FrameTiming();
                break;
        }

        switch (enableLevelName)
        {
            case true:
                LevelName();
                break;
        }

        switch (enableResAndAspect)
        {
            case true:
                ResAndAspect();
                break;
        }
        
        switch (enableGnomeValueMetric)
        {
            case true:
                GnomeValueMetric();
                break;
        }
        
        switch (enableConveyorSpeedMetric)
        {
            case true:
                ConveyorSpeedMetric();
                break;
        }
        
        switch (enableManufacturingTimeMetric)
        {
            case true:
                ManufacturingTimeMetric();
                break;
        }
    }

    private void Framerate()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter_avgFrameRate = (int)current;
        framerateString = "FPS: " + fpsCounter_avgFrameRate.ToString() + " (" + fpsCounter_fps.ToString() + " avg)";

        ++fpsCounter_frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > fpsCounter_lastInterval + fpsCounter_updateInterval)
        {
            fpsCounter_fps = Mathf.Ceil((float)(fpsCounter_frames / (timeNow - fpsCounter_lastInterval))); // Mathf.Ceil will round the outputted integer up to the nearest whole number
            fpsCounter_frames = 0;
            fpsCounter_lastInterval = timeNow;
        }
    }

    private void FrameTiming()
    {
        float currentFrameTiming = Mathf.Ceil(Time.deltaTime * 1000);
        frameTimingString = "Timing: " + currentFrameTiming.ToString() + " ms";
    }

    private void LevelName()
    {
        Scene scene = SceneManager.GetActiveScene();
        levelNameString = "Active scene: \"" + scene.name + "\"";
    }

    private void ResAndAspect()
    {
        float screenResW = Screen.width;
        float screenResH = Screen.height;
        float aspect = Camera.main.aspect;
        resAndAspectString = "Screen: " + screenResW.ToString() + "x" + screenResH.ToString() + ", " + aspect; 
    }

    private void GnomeValueMetric()
    {
        gnomeValueString = "";
    }

    private void ConveyorSpeedMetric()
    {
        conveyorSpeedString = "Conveyor speed: " + line01Conveyor.speed + " + " + gnomeCoinSys.permanentSpeed;
    }

    private void ManufacturingTimeMetric()
    {
        manufacturingTimeString = "Manufacturing time: " + line01Dispenser.manufacturingTime + " + " + gnomeCoinSys.permanentTime;
    }
}

public class NotificationSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private bool hasTimedSpawn;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnTimerVariation;
    [SerializeField] private int flashAmount;
    [SerializeField] private float flashLength;
    private bool isActive = false;

    [Header("Object References")] 
    [SerializeField] private AdSystem adSys;
    [SerializeField] private GameObject adNotification;
    [SerializeField] private GameObject mailNotification;
    [SerializeField] private GameObject sabotageNotification;
    [SerializeField] private GameObject listArea;
    [SerializeField] private AudioSource uiSfxSource;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private Image greenVignette;
    [SerializeField] private Image bellNotifIcon;
    private List<GameObject> activeNotifications = new List<GameObject>();

    private void OnEnable()
    {
        StartCoroutine(TimedAdSpawn());
    }

    public void AddNotification(int type)
    {
        switch (type)
        {
            case 0:
                GameObject newAdNotif = Instantiate(adNotification, listArea.transform);
                activeNotifications.Add(newAdNotif);
                StartCoroutine(PushAlert());
                Button button = newAdNotif.transform.GetChild(0).GetComponent<Button>();
                button.onClick.AddListener(() => uiSfxSource.PlayOneShot(buttonSound));
                button.onClick.AddListener(() => adSys.PlayFakeAd());
                button.onClick.AddListener(() => DestroyNotification(newAdNotif));
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    public void DestroyNotification(GameObject obj)
    {
        Destroy(obj);
    }

    public void SetState(bool state)
    {
        switch (state)
        {
            case true:
                isActive = true;
                break;
            case false:
                isActive = false;
                break;
        }
    }

    private IEnumerator TimedAdSpawn()
    {
        while (hasTimedSpawn)
        {
            float random = Random.Range(-spawnTimerVariation, spawnTimerVariation);
            float finalTimedSpawn = spawnTimer + random;
            yield return new WaitForSeconds(finalTimedSpawn);
            AddNotification(0);
        }
    }

    private IEnumerator PushAlert()
    {
        switch (isActive)
        {
            case false:
                bellNotifIcon.enabled = true;
                break;
            case true:
                bellNotifIcon.enabled = false;
                break;
        }
        greenVignette.gameObject.SetActive(true);
        for (int i = 0; i < flashAmount; i++)
        {
            greenVignette.enabled = true;
            yield return new WaitForSeconds(flashLength);
            greenVignette.enabled = false;
            yield return new WaitForSeconds(flashLength);
        }
        greenVignette.gameObject.SetActive(false);
    }
}

public class PrestigeSequenceSystem : MonoBehaviour
{
    [Header("Standard Sequence > Values")]
    [SerializeField] private float standardSequenceDelay;
    [SerializeField] private List<float> switchAwayTimeStandard = new List<float>();
    [SerializeField] private List<float> switchToTimeStandard = new List<float>();
    [SerializeField] private int prestigeFlashAmount;
    [SerializeField] private float prestigeFlashLength;
    private bool isFinishedPrestige;

    [Header("Nuke Sequence > Values")]
    [SerializeField] private float alarmDelayTime;
    [SerializeField] private float delayUntilCountdown;
    [SerializeField] private float flashingInInterval;
    [SerializeField] private float flashingOutInterval;
    [SerializeField] private float countdownTimer;
    [SerializeField] private float delayAfterNuke;
    [SerializeField] private float nukeParticlesDelay;
    [SerializeField] private float delayAfterFadeOut;
    [SerializeField] private List<float> switchAwayTimeNuke = new List<float>();
    private Color emissiveColor = Color.red;
    private float timeElapsed;

    [Header("Standard Sequence > Object References > General")]
    [SerializeField] private TextMeshProUGUI prestigingText;
    [SerializeField] private Image greenVignette;

    [Header("Standard Sequence > Object References > Audio")]
    [SerializeField] private AudioSource musicSource1;

    [Header("Standard Sequence > Object References > Lists")]
    [SerializeField] private List<GameObject> uiToDisableStandard = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToEnableStandard = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToSwitchAwayStandard = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToSwitchAwayLocations = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToSwitchToStandard = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToSwitchToLocations = new List<GameObject>();

    [Header("Nuke Sequence > Object References > General")]
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private PrototypeCameraShake camShake;
    [SerializeField] private Material emerLightMaterial;
    [SerializeField] private Light nukeLight;
    [SerializeField] private Light nukeAmbientLight;
    [SerializeField] private Light globalLight;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AnimationClip emerLightAnim;
    [SerializeField] private GameObject whiteScreen;
    [SerializeField] private AnimationCurve nukeSequenceShake;
    [SerializeField] private DDOLManager ddolSys;
    [SerializeField] private SwitchPanels panelSwitchSys;
    [SerializeField] private Image redVignette;
    [SerializeField] private string sceneToLoad;

    [Header("Nuke Sequence > Object References > Audio")]
    [SerializeField] private AudioSource switchOffSource;
    [SerializeField] private AudioSource generatorOffSource;
    [SerializeField] private AudioSource alarmSource;
    [SerializeField] private AudioSource clockSource;
    [SerializeField] private AudioSource nukeSource;
    [SerializeField] private AudioSource sirenSource;
    [SerializeField] private AudioClip switchOff;
    [SerializeField] private AudioClip generatorOff;
    [SerializeField] private AudioClip alarm;
    [SerializeField] private AudioClip computerAlarm;
    [SerializeField] private AudioClip clock;
    [SerializeField] private AudioClip nuke;
    [SerializeField] private AudioClip siren;
    [SerializeField] private AudioMixerGroup outsideNuke;

    [Header("Nuke Sequence > Object References > Lists")]
    [SerializeField] private List<GameObject> uiToDisableNuke = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToEnableNuke = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToSwitchAwayNuke = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToSwitchAwayLocationsNuke = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToFlashNuke = new List<GameObject>();
    public List<Light> lightsToTurnOff = new List<Light>();
    [SerializeField] private List<Light> emergencyLights = new List<Light>();
    [SerializeField] private List<ParticleSystem> nukeParticles = new List<ParticleSystem>();
    [SerializeField] private List<FinalConveyor> conveyors = new List<FinalConveyor>();

    public void OnEnable()
    {
        ddolSys = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
    }

    public void StartNukeSequence()
    {
        StartCoroutine(NukeSequence());
    }

    private IEnumerator NukeSequence()
    {
        // The beginning of the sequence, power turns off
        musicSource1.Stop();
        for (int i = 0; i < lightsToTurnOff.Count; i++)
        {
            lightsToTurnOff[i].enabled = false;
        }
        globalLight.enabled = false;
        if (!switchOffSource.isPlaying)
        {
            switchOffSource.clip = switchOff;
            switchOffSource.Play();
        }
        if (!generatorOffSource.isPlaying)
        {
            generatorOffSource.clip = generatorOff;
            generatorOffSource.Play();
        }
        for (int i = 0; i < uiToDisableNuke.Count; i++)
        {
            uiToDisableNuke[i].SetActive(false);
        }
        for (int i = 0; i < conveyors.Count; i++)
        {
            conveyors[i].enabled = false;
        }
        for (int i = 0; i < uiToSwitchAwayNuke.Count; i++)
        {
            panelSwitchSys.SetDismissalValuesThroughScript(uiToSwitchAwayNuke[i], switchAwayTimeNuke[i], uiToSwitchAwayLocationsNuke[i]);
            panelSwitchSys.ExecuteSmooth(1);
        }

        yield return new WaitForSeconds(alarmDelayTime);

        // The alarm starts blaring
        if (!alarmSource.isPlaying)
        {
            alarmSource.clip = alarm;
            alarmSource.loop = true;
            alarmSource.Play();
        }
        for (int i = 0; i < emergencyLights.Count; i++)
        {
            emergencyLights[i].enabled = true;
            emergencyLights[i].GetComponent<Animator>().enabled = true;
        }
        emerLightMaterial.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(3f);
        if (!sirenSource.isPlaying)
        {
            sirenSource.clip = siren;
            sirenSource.loop = false;
            sirenSource.volume = 0.2f;
            sirenSource.outputAudioMixerGroup = outsideNuke;
            sirenSource.Play();
        }

        yield return new WaitForSeconds(delayUntilCountdown - 3f);

        // The countdown timer begins
        for (int i = 0; i < uiToEnableNuke.Count; i++)
        {
            uiToEnableNuke[i].SetActive(true);
        }
        alarmSource.PlayOneShot(computerAlarm);
        timerText.text = "WARNING: Nuclear Protocol Activated";
        redVignette.gameObject.SetActive(true);
        yield return new WaitForSeconds(flashingInInterval);
        redVignette.enabled = false;
        yield return new WaitForSeconds(flashingOutInterval);
        redVignette.enabled = true;
        yield return new WaitForSeconds(flashingInInterval);
        redVignette.enabled = false;
        timerText.enabled = false;
        yield return new WaitForSeconds(flashingOutInterval);
        timerText.enabled = true;
        clockSource.PlayOneShot(clock);

        for (float timer = countdownTimer; timer > 0; timer -= Time.deltaTime)
        {
            timerText.text = sys.RoundToNearestHundredth(timer).ToString("F2");
            if (timer <= 0.5f && sirenSource.isPlaying)
            {
                if (timeElapsed < 0.3f)
                {
                    sirenSource.volume = Mathf.Lerp(0.2f, 0, timeElapsed / 0.3f);
                    timeElapsed += Time.deltaTime;
                }
            }
            yield return null;
        }

        // The nuke goes off
        if (!nukeSource.isPlaying)
        {
            nukeSource.clip = nuke;
            nukeSource.loop = false;
            nukeSource.Play();
        }
        timerText.text = "0.00";
        redVignette.enabled = true;
        nukeLight.gameObject.SetActive(true);
        nukeAmbientLight.gameObject.SetActive(true);
        nukeLight.GetComponent<Animator>().enabled = true;
        nukeAmbientLight.GetComponent<Animator>().enabled = true;
        alarmSource.Stop();
        StartCoroutine(ProgrammedNukeShake());
        StartCoroutine(SetDelayedParticles(true));
        sirenSource.Stop();
        sirenSource.outputAudioMixerGroup = null;
        yield return new WaitForSeconds(1);
        // The moment after the nuke where the text disappears
        timerText.enabled = false;
        redVignette.enabled = false;
        for (int i = 0; i < emergencyLights.Count; i++)
        {
            emergencyLights[i].GetComponent<Animator>().enabled = false;
            emergencyLights[i].enabled = false;
        }
        emerLightMaterial.DisableKeyword("_EMISSION");

        yield return new WaitForSeconds(delayAfterNuke - 1);

        // The nuke finally hits and the screen goes white
        StartCoroutine(SetDelayedParticles(false));
        whiteScreen.GetComponent<Image>().enabled = true;
        whiteScreen.GetComponent<Image>().CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(1.5f);
        timeElapsed = 0;
        for (float f = 0f; f < 3f; f += Time.deltaTime)
        {
            if (timeElapsed < 3f)
            {
                whiteScreen.GetComponent<Image>().color = Color.Lerp(Color.white, Color.black, timeElapsed / 3f);
                timeElapsed += Time.deltaTime;
            }
            yield return null;
        }
        camShake.Stop();

        yield return new WaitForSeconds(delayAfterFadeOut);

        // The ending of the nuke sequence, restarting everything.
        ddolSys.resetTimes++;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    private IEnumerator ProgrammedNukeShake()
    {
        float timeElapsed = 0;
        camShake.isShaking = true;
        float value = 0;

        while (camShake.isShaking == true)
        {
            value = nukeSequenceShake.Evaluate(timeElapsed);
            timeElapsed += Time.deltaTime;
            StartCoroutine(camShake.Shake(value, value));
            yield return null;
        }
    }

    private IEnumerator SetDelayedParticles(bool enable)
    {
        switch (enable)
        {
            case true:
                yield return new WaitForSeconds(nukeParticlesDelay);
                foreach (ParticleSystem nukeParticle in nukeParticles)
                {
                    nukeParticle.Play();
                }
                break;
            case false:
                foreach (ParticleSystem nukeParticle in nukeParticles)
                {
                    nukeParticle.Stop();
                }
                break;
        }
    }

    // Prestige sequence for when normal prestige levels are attained
    public IEnumerator DoPrestigePhase()
    {
        // Shut all the factory equipment down 
        foreach (Light lights in lightsToTurnOff)
        {
            lights.enabled = false;
        }
        globalLight.enabled = false;
        if (!switchOffSource.isPlaying)
        {
            switchOffSource.clip = switchOff;
            switchOffSource.Play();
        }
        if (!generatorOffSource.isPlaying)
        {
            generatorOffSource.clip = generatorOff;
            generatorOffSource.Play();
        }
        for (int i = 0; i < uiToSwitchAwayStandard.Count; i++)
        {
            panelSwitchSys.SetDismissalValuesThroughScript(uiToSwitchAwayStandard[i], switchAwayTimeStandard[i], uiToSwitchAwayLocations[i]);
            panelSwitchSys.ExecuteSmooth(1);
        }
        for (int i = 0; i < uiToDisableStandard.Count; i++)
        {
            uiToDisableStandard[i].SetActive(false);
        }
        musicSource1.Pause();

        isFinishedPrestige = false;
        prestigingText.gameObject.SetActive(true);
        StartCoroutine(AutomatePrestigingText());
        yield return new WaitForSeconds(standardSequenceDelay);
        prestigingText.gameObject.SetActive(false);
        isFinishedPrestige = true;

        StartCoroutine(FlashScreen());

        foreach (Light lights in lightsToTurnOff)
        {
            lights.enabled = true;
        }
        globalLight.enabled = true;
        if (!switchOffSource.isPlaying)
        {
            switchOffSource.clip = switchOff;
            switchOffSource.Play();
        }
        musicSource1.Play();

        for (int i = 0; i < uiToSwitchToStandard.Count; i++)
        {
            panelSwitchSys.SetActivationValuesThroughScript(uiToSwitchToStandard[i], switchToTimeStandard[i], uiToSwitchToLocations[i]);
            panelSwitchSys.ExecuteSmooth(2);
        }
        for (int i = 0; i < uiToEnableStandard.Count; i++)
        {
            uiToEnableStandard[i].SetActive(true);
        }
    }

    // This is to automate the prestige in-progress text
    IEnumerator AutomatePrestigingText()
    {
        while (!isFinishedPrestige)
        {
            prestigingText.text = "Prestiging";
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++)
            {
                prestigingText.text += ".";
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }

    private IEnumerator FlashScreen()
    {
        greenVignette.gameObject.SetActive(true);
        for (int i = 0; i < prestigeFlashAmount; i++)
        {
            greenVignette.enabled = true;
            yield return new WaitForSeconds(prestigeFlashLength);
            greenVignette.enabled = false;
            yield return new WaitForSeconds(prestigeFlashLength);
        }
        greenVignette.gameObject.SetActive(false);
    }
}

public class PrestigeSystem : MonoBehaviour
{
    [Header("General Values")]
    public PrestigeType prestigeType;
    public float price;
    [Header("Reward Values")]
    public float unitMultiplication;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float manufacturerDecreasePercent;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float priceIncreasePercent;
    private int displayablePrestigeLevel = 0;

    [Header("Object References")]
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private PrestigeSequenceSystem finalPrestigeSys;
    [SerializeField] private List<FinalUpgrades> upgradeSys = new List<FinalUpgrades>();
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    [SerializeField] private List<FinalConveyor> conveyors = new List<FinalConveyor>();
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentPrestigeText;
    [SerializeField] private TextMeshProUGUI prestigeBodyText;
    [SerializeField] private GameObject promptParent;
    [SerializeField] private List<GameObject> promptUIToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> PromptUIToEnable = new List<GameObject>();

    void OnEnable()
    {
        sys.UpdatePrice(costText, false, "$", price, "");
        currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
        if (prestigeType != PrestigeType.FinalPrestige)
        {
            // Example: 2x gnome value, 95% manufacturing speed & 110% prices.
            prestigeBodyText.text = unitMultiplication + "x gnome value, " + (100 - (manufacturerDecreasePercent * 100)) + "% manufacturing speed & " + (100 + (priceIncreasePercent * 100)) + "% prices.";
        }   
    }

    public void UpdatePrestige()
    {
        if (sys.pointScore >= price)
        {
            switch (prestigeType)
            {
                case PrestigeType.Prestige1:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige0 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige2:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige1 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige3:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige2 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige4:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige3 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige5:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige4 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.FinalPrestige: // The nuke sequence
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige5 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    }
                    else if (sys.debugMode)
                    {
                        Debug.Log("Hi");
                        finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    }
                    else PromptWindow();
                    break;
            }
        }
    }

    private void ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel newPrestLvl)
    {
        // Reset money to 0
        sys.prestigeLvl = newPrestLvl;
        switch (sys.debugMode)
        {
            case true:
                break;
            case false:
                sys.pointScore = 0;
                break;
        }
        sys.moneyText.text = "Profit: $" + sys.pointScore.ToString("F2");

        // Reset gnome values (and update current prestige text)
        switch (newPrestLvl)
        {
            case FinalFactorySystem.PrestigeLevel.Prestige0:
                displayablePrestigeLevel = 0;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige1:
                sys.lvl2InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl2Value = sys.lvl2InitialValue;
                Debug.Log("New gnome value: " + sys.lvl2Value);
                displayablePrestigeLevel = 1;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige2:
                sys.lvl3InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl3Value = sys.lvl3InitialValue;
                Debug.Log("New gnome value: " + sys.lvl3Value);
                displayablePrestigeLevel = 2;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige3:
                sys.lvl4InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl4Value = sys.lvl4InitialValue;
                Debug.Log("New gnome value: " + sys.lvl4Value);
                displayablePrestigeLevel = 3;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige4:
                sys.lvl5InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl5Value = sys.lvl5InitialValue;
                Debug.Log("New gnome value: " + sys.lvl5Value);
                displayablePrestigeLevel = 4;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige5:
                sys.lvl6InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl6Value = sys.lvl6InitialValue;
                Debug.Log("New gnome value: " + sys.lvl6Value);
                displayablePrestigeLevel = 5;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
        }

        // Reset conveyor values
        for (int i = 0; i < conveyors.Count; i++)
        {
            switch (conveyors[i].isActiveAndEnabled)
            {
                case true: 
                    conveyors[i].speed = conveyors[i].initialSpeed;
                    break;
                case false:
                    break;
            }
        }

        // Reset manufacturing values
        for (int i = 0; i < dispensers.Count; i++)
        {
            switch (dispensers[i].isActiveAndEnabled)
            {
                case true:
                    dispensers[i].manufacturingTime = dispensers[i].initialManuTime + (dispensers[i].initialManuTime * manufacturerDecreasePercent);
                    break;
                case false:
                    break;
            }
        }

        // Reset and adjust total costs
        for (int i = 0; i < upgradeSys.Count; i++)
        {
            //upgradeSys[i].gameObject.SendMessage("ResetAndAdjustPrices", priceIncreasePercent);
            upgradeSys[i].ResetAndAdjustPrices(priceIncreasePercent);
        }

        for (int i = 0; i < dispensers.Count; i++)
        {
            switch (dispensers[i].isActiveAndEnabled)
            {
                case true:
                    for (int j = 0; j < dispensers[i].objectsList.Count; j++)
                    {
                        Destroy(dispensers[i].objectsList[j]);
                    }
                    break;
                case false:
                    break;
            }
        }

        sys.ResetProductionLineAmount();

        StartCoroutine(finalPrestigeSys.DoPrestigePhase());

        Debug.Log("Upgraded Prestige to " + sys.prestigeLvl);
    }

    private void PromptWindow()
    {
        promptParent.SetActive(true);
        for (int i = 0; i < promptUIToDisable.Count; i++)
        {
            promptUIToDisable[i].SetActive(false);
        }
        for (int i = 0; i < PromptUIToEnable.Count; i++)
        {
            PromptUIToEnable[i].SetActive(true);
        }
    }

    public enum PrestigeType
    {
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5,
        FinalPrestige
    }
}

public class SoundTriggerScript : MonoBehaviour
{
    [SerializeField] private AudioSource machineSfxSource;
    [SerializeField] private AudioClip machineSfx;

    private void OnTriggerEnter(Collider other)
    {
        machineSfxSource.PlayOneShot(machineSfx);
    }
}

public class SwitchPanels : MonoBehaviour
{
    private GameObject objectToDismiss;
    private GameObject objectToActivate;
    private float dismissalTime;
    private float activationTime;
    private float altDismissalTime;
    private float altActivationTime;
    private Vector3 dismissalLocation;
    private Vector3 altDismissalLocation;
    private Vector3 activationLocation;
    private Vector3 altActivationLocation;
    private int methodRunMode = 0;
    [SerializeField] private WidescreenUIFix uiFixScript;
    public bool useAltPositions;

    private void OnEnable()
    {
        useAltPositions = uiFixScript.isActivated;
    }

    public void SetAsDismissingPanelObject(GameObject panelToDismiss)
    {
        objectToDismiss = panelToDismiss;
    }

    public void SetDismissalTime(float time)
    {
        dismissalTime = time;
    }
    
    public void SetAltDismissalTime(float time)
    {
        altDismissalTime = time;
    }

    public void SetNewDismissalPos(GameObject newDismissalLocation)
    {
        dismissalLocation = newDismissalLocation.transform.position;
    }
    
    public void SetNewAltDismissalPos(GameObject newAltDismissalLocation)
    {
        altDismissalLocation = newAltDismissalLocation.transform.position;
    }

    public void SetAsActivatingPanelObject(GameObject panelToActivate)
    {
        objectToActivate = panelToActivate;
    }

    public void SetActivationTime(float time)
    {
        activationTime = time;
    }
    
    public void SetAltActivationTime(float time)
    {
        altActivationTime = time;
    }

    public void SetNewActivationPos(GameObject newActivationLocation)
    {
        activationLocation = newActivationLocation.transform.position;
    }
    
    public void SetNewAltActivationPos(GameObject newAltActivationLocation)
    {
        altActivationLocation = newAltActivationLocation.transform.position;
    }

    public void SetDismissalValuesThroughScript(GameObject panelToDismiss, float dismissTime, GameObject newDismissalLocation)
    {
        objectToDismiss = panelToDismiss;
        dismissalTime = dismissTime;
        dismissalLocation = newDismissalLocation.transform.position;
    }

    public void SetActivationValuesThroughScript(GameObject panelToActivate, float activeTime, GameObject newActivationLocation)
    {
        objectToActivate = panelToActivate;
        activationTime = activeTime;
        activationLocation = newActivationLocation.transform.position;
    }

    public void ExecuteSmooth(int mode)
    {
        methodRunMode = mode;
        switch (methodRunMode)
        {
            case 0:
                StartCoroutine(SmoothSwitchDismissal());
                StartCoroutine(SmoothSwitchActivation());
                break;
            case 1:
                StartCoroutine(SmoothSwitchDismissal());
                break;
            case 2:
                StartCoroutine(SmoothSwitchActivation());
                break;
        }
        
    }

    IEnumerator SmoothSwitchDismissal()
    {
        switch (useAltPositions)
        {
            case false:
                // Smooth switch operation for the dismissing object
                float dismissalTimeElapsed = 0;
                while (dismissalTimeElapsed < dismissalTime)
                {
                    objectToDismiss.transform.position = Vector3.Lerp(objectToDismiss.transform.position, dismissalLocation, dismissalTimeElapsed / dismissalTime);
                    dismissalTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToDismiss.transform.position = dismissalLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
            case true:
                // Smooth switch operation for the alt dismissing object
                float altDismissalTimeElapsed = 0;
                while (altDismissalTimeElapsed < altDismissalTime)
                {
                    objectToDismiss.transform.position = Vector3.Lerp(objectToDismiss.transform.position, altDismissalLocation, altDismissalTimeElapsed / altDismissalTime);
                    altDismissalTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToDismiss.transform.position = altDismissalLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
        }
    }

    IEnumerator SmoothSwitchActivation()
    {
        switch (useAltPositions)
        {
            case false:
                // Smooth switch operation for the activating object
                float activationTimeElapsed = 0;
                while (activationTimeElapsed < activationTime)
                {
                    objectToActivate.transform.position = Vector3.Lerp(objectToActivate.transform.position,
                        activationLocation, activationTimeElapsed / activationTime);
                    activationTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToActivate.transform.position = activationLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
            case true:
                // Smooth switch operation for the alt activating object
                float altActivationTimeElapsed = 0;
                while (altActivationTimeElapsed < altActivationTime)
                {
                    objectToActivate.transform.position = Vector3.Lerp(objectToActivate.transform.position,
                        altActivationLocation, altActivationTimeElapsed / altActivationTime);
                    altActivationTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToActivate.transform.position = altActivationLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
        }
    }
}

public class WidescreenUIFix : MonoBehaviour
{
    [SerializeField] private bool runOnEnable = true;
    [SerializeField] private ThresholdType thresholdType;
    [Tooltip("Any aspect ratio above this number will be affected.")][SerializeField] private float aspectRatioThreshold;
    [SerializeField] private List<GameObject> uiToMove = new List<GameObject>();
    [SerializeField] private AdjustmentType adjustmentType;
    [Tooltip("This will ADD to the current UI element position.")][SerializeField] private List<Vector3> newValue = new List<Vector3>();
    [Tooltip("Only assign this if you're using the ObjectAlignment option.")] [SerializeField] private List<GameObject> posPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> uiToEnable = new List<GameObject>();
    public bool isActivated = false;

    private void OnEnable()
    {
        switch (runOnEnable)
        {
            case true:
                FixUI();
                break;
        }
    }

    private void FixUI()
    {
        switch (thresholdType)
        {
            case ThresholdType.Below:
                if (Camera.main.aspect > aspectRatioThreshold)
                {
                    Debug.Log("Your aspect ratio is above the threshold. Current aspect ratio: " + Camera.main.aspect + ".");
                }
                else if (Camera.main.aspect <= aspectRatioThreshold)
                {
                    if (uiToMove.Count != newValue.Count || uiToMove.Count != posPoints.Count)
                    {
                        Debug.Log("You have not set up the widescreen fix script correctly. Please ensure the lists are of equal length.");
                    }
                    for (int i = 0; i < uiToMove.Count; i++)
                    {
                        switch (adjustmentType)
                        {
                            case AdjustmentType.Offset:
                                uiToMove[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().position += newValue[i];
                                break;
                            case AdjustmentType.Position:
                                uiToMove[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().position = new Vector3(newValue[i].x, newValue[i].y);
                                break;
                            case AdjustmentType.ObjectAlignment:
                                uiToMove[i].transform.position = posPoints[i].transform.position;
                                
                                break;
                        }
                    }
                    if (uiToDisable.Count != 0)
                    {
                        foreach (GameObject obj in uiToDisable)
                        {
                            obj.SetActive(false);
                        }
                    }
                    if (uiToEnable.Count != 0)
                    {
                        foreach (GameObject obj in uiToEnable)
                        {
                            obj.SetActive(true);
                        }
                    }
                    isActivated = true;
                    Debug.Log("The widescreen fix script has completed.");
                }
                break;
            case ThresholdType.Above:
                if (Camera.main.aspect <= aspectRatioThreshold)
                {
                    Debug.Log("Your aspect ratio is below the threshold. Current aspect ratio: " + Camera.main.aspect + ".");
                }
                else if (Camera.main.aspect > aspectRatioThreshold)
                {
                    if (uiToMove.Count != newValue.Count)
                    {
                        Debug.Log("You have not set up the widescreen fix script correctly. Please ensure the lists are of equal length.");
                    }
                    else
                    {
                        for (int i = 0; i < uiToMove.Count; i++)
                        {
                            uiToMove[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                            uiToMove[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                            Debug.Log(newValue[i]);
                            switch (adjustmentType)
                            {
                                case AdjustmentType.Offset:
                                    uiToMove[i].transform.position += newValue[i];
                                    break;
                                case AdjustmentType.Position:
                                    uiToMove[i].GetComponent<Transform>().position = new Vector3(newValue[i].x, newValue[i].y, newValue[i].z);
                                    break;
                            }
                        }
                        isActivated = true;
                        Debug.Log("The widescreen fix script has completed.");
                    }
                }
                break;
        }
    }

    private enum AdjustmentType
    {
        Offset,
        Position,
        ObjectAlignment
    }

    private enum ThresholdType
    {
        Above,
        Below
    }
}
*/