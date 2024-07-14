using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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