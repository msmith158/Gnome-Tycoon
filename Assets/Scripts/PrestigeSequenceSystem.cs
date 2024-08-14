using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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