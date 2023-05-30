using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Turning, Interacting, Translating, Petting, Fighting, Dead, Locked, AutoMove };

    public playerState state;

    [HideInInspector]
    public bool paused = false;

    //movement stuff
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    Vector2 movement = Vector2.zero;

    //animation stuff
    private Animator anim;

    //audio stuff
    public FootstepManager step;
    public BowSoundManager bowSound;

    //interaction stuff
    [HideInInspector]
    public Interaction interaction;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;
    [HideInInspector]
    public TextScroller scroller;

    //interaction camera stuff
    private CinemachineVirtualCamera vcam;
    public CinemachineTargetGroup targetGroup;
    private CinemachineBasicMultiChannelPerlin shakeNoise;

    //camera transition values
    private float camTightSize = 7f;
    private float camWideSize = 8f;

    //combat stuff
    [HideInInspector]
    public Enemy enemy;
    private LineRenderer line;
    public GameObject bloodSpray;

    //translation stuff
    public Texture2D pointer;
    [HideInInspector]
    public Translation translation;
    public DecodeScript decodeInterface;
    private TextMeshProUGUI decodeText;

    //pause stuff
    public GameObject pauseUI;

    //turnback stuff
    public GameObject turnbackDisplay;

    //storage and retrieval stuff
    private static PlayerController player;
    public static PlayerController Instance { get { return player; } }

    private void Awake()
    {
        //aggressive singleton
        if(player != null && player != this)
        {
            Destroy(player);
        }
        player = this;

        //variable fetching and setting
        rb = GetComponent<Rigidbody2D>();
        vcam = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        shakeNoise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        anim = GetComponent<Animator>();

        scroller = bottomText.gameObject.GetComponent<TextScroller>();

        line = GetComponent<LineRenderer>();

        decodeText = decodeInterface.transform.Find("DecodePanel").Find("DecodeText").GetComponent<TextMeshProUGUI>();

        //set cursor
        Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        //set checkpoint
        PlayerPrefs.SetString("Checkpoint", SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        //animation variables
        anim.SetBool("Moving", false);

        //check for pause state
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Unpause();
            }
            else
            {
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            return;
        }

        if (state == playerState.Normal)
        {
            //movement vector
            if(Input.GetKey(KeyCode.D))
            {
                movement.x = 1;
                anim.SetBool("Moving", true);
            }
            else
            {
                movement.x = 0;
            }

            //interaction trigger
            if(Input.GetKeyDown(KeyCode.W))
            {
                if(interaction != null)
                {
                    interaction.Interact();
                }
                else if(translation != null)
                {
                    translation.StartTranslation();
                }
            }

            //turnback trigger
            if(Input.GetKeyDown(KeyCode.A))
            {
                turnbackDisplay.SetActive(true);
                state = playerState.Turning;
                anim.Play("PlayerLookBack");
            }

            //petting trigger
            if (Input.GetKeyDown(KeyCode.S))
            {
                state = playerState.Petting;
                anim.Play("PlayerPspsps");

                //set variable in cat to move within petting range
                CatController.Instance.SetPettingState(true);
            }
        }
        else if(state == playerState.Interacting)
        {
            if((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) && interaction != null)
            {
                //advance interaction
                scroller.Clicked();
            }
        }
        else if(state == playerState.Translating)
        {
            if(Input.GetKeyDown(KeyCode.Q) && translation != null)
            {
                //return from translation
                translation.EndTranslation();
            }
        }
        else if(state == playerState.Turning)
        {
            if(Input.GetKeyUp(KeyCode.A))
            {
                turnbackDisplay.SetActive(false);
                state = playerState.Normal;
                anim.Play("PlayerIdle");
            }
        }
        else if (state == playerState.AutoMove)
        {
            anim.SetBool("Moving", true);
            return;
        }
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
        paused = true;

        //enable cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        paused = false;

        //disable cursor if not in translation
        if(state != playerState.Translating)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //make sure turnback is disabled
        if (state == playerState.Turning)
        {
            turnbackDisplay.SetActive(false);
            state = playerState.Normal;
            anim.Play("PlayerIdle");
        }

        //release bow if it was drawn
        CombatScript.Unpaused();
    }

    private void FixedUpdate()
    {
        if(state == playerState.Normal)
        {
            //player movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else if (state == playerState.AutoMove)
        {
            //auto movement
            rb.MovePosition(rb.position + new Vector2(1, 0) * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void UpdateText(string line, bool top = false)
    {
        //check for events
        if(line.StartsWith("{"))
        {
            //read key
            int key = int.Parse(line.Substring(1, 1));

            //edit line
            line = line.Substring(3);

            //call event
            if(interaction != null)
            {
                interaction.inLine[key].Invoke();
            }
        }

        //set text
        if(top)
        {
            topText.text = line;
        }
        else
        {
            scroller.NewLine(line);
        }
    }

    public void ClearText()
    {
        topText.text = "";
        bottomText.text = "";
    }

    public void SetupTranslation(string message)
    {
        decodeText.text = message;
        decodeInterface.gameObject.SetActive(true);

        state = playerState.Translating;

        anim.SetBool("Translating", true);
    }

    public void EndTranslation()
    {
        state = PlayerController.playerState.Normal;
        translation = null;
        anim.SetBool("Translating", false);

        //cleanup particles
        foreach(ParticleSystem ps in decodeInterface.transform.Find("DecodePanel").GetComponentsInChildren<ParticleSystem>())
        {
            var coll = ps.collision;
            coll.enabled = false;
        }

        //hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetupCombat()
    {
        CombatScript.instance.CombatEnabled();
        UpdateText("Hold and release the button shown below.", true);
    }

    public void ArrowTrail(float damageMult)
    {
        line.enabled = true;

        //set start and end point
        //start point;
        Vector3 startPos = new Vector3(0, 0, 0);
        startPos.x = transform.position.x - 1f;
        startPos.y = 1.6875f;
        line.SetPosition(0, startPos);

        //end point
        float offsetX = -0.125f;
        Vector3 targetPos = new Vector3(enemy.gameObject.transform.position.x + offsetX, 1.5f, 0);
        line.SetPosition(1, targetPos);

        //spawn blood spray
        GameObject blood = Instantiate(bloodSpray, targetPos, Quaternion.identity);
        blood.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, 24f * damageMult) });

        //shake camera
        float baseShake = 2.5f;
        ShakeCamera(baseShake * damageMult, 0.1f);

        //fade color
        StartCoroutine(FadeArrowTrail());
    }

    IEnumerator FadeArrowTrail()
    {
        float lineInterval = 0.2f;
        float timer = 0f;

        line.endColor = Color.red;
        while(timer < lineInterval)
        {
            line.endColor = Color.Lerp(Color.red, Color.clear, (timer / lineInterval));
            timer += Time.deltaTime;
            yield return null;
        }

        line.endColor = Color.clear;
    }

    public void Die()
    {
        //death animation
        anim.Play("PlayerDie");

        //screenshake
        ShakeCamera(6f, 0.1f);

        //death transition
        StartCoroutine(DeathTransition());
    }

    //called from animation event
    public void EndPetting()
    {
        state = playerState.Normal;
        CatController.Instance.SetPettingState(false);
        //play regular cat idle animation
    }

    //snap to black, hold, and then reload level
    IEnumerator DeathTransition()
    {
        yield return new WaitForSeconds(0.5f);
        Fade.FadeEffect();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(PlayerPrefs.GetString("Checkpoint"));
    }

    public void SetAnimBool(string key, bool value)
    {
        anim.SetBool(key, value);
    }

    public void CamIn()
    {
        ResizeCam(camTightSize);
    }

    public void CamOut()
    {
        ResizeCam(camWideSize);
    }

    public void CamEngage(Transform enemy, float weight = 1)
    {
        //widen camera from combat framing
        CamOut();

        //add enemy to camera
        AddToCamera(enemy, weight, camWideSize);
    }

    public void CamDisengage()
    {
        //remove enemy from target group
        targetGroup.RemoveMember(targetGroup.m_Targets[1].target);
    }

    public void AddToCamera(Transform t, float weight, float radius)
    {
        targetGroup.AddMember(t, weight, radius);
    }

    public void RemoveFromCamera(Transform t)
    {
        targetGroup.RemoveMember(t);
    }

    public void ResizeCam(float targetSize)
    {
        //resize player camera radius
        targetGroup.m_Targets[0].radius = targetSize;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(DoCameraShake(intensity, duration));
    }

    IEnumerator DoCameraShake(float intensity, float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            shakeNoise.m_AmplitudeGain = Mathf.Lerp(intensity, 0, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        shakeNoise.m_AmplitudeGain = 0;
    }

    public void Step()
    {
        step.PlaySound();
    }

    public void SwitchStep(FootstepManager.groundMaterial newMat)
    {
        step.SwitchSound(newMat);
    }

}
