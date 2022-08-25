using UnityEngine;

public class EnemySoliderV2 : MonoBehaviour
{
    #region FIELDS
    [Header("Configuration: ")]
    [SerializeField] private Vector2 idleMoveTime = new Vector2(0.5f, 1f);
    private float currentIdleMoveTime = 0f;
    private float randomIdleMoveTime = 0;
    private bool idleMoveTimeObtained = false;
    private bool idleMoveLeft = false;
    [SerializeField] private float visionDistance = 5f;
    [SerializeField] private Vector2 idleTime = new Vector2(.5f, 1.5f);
    private float currentIdleTime = 0f;
    private float randomIdleTime = 0f;
    private bool idleTimeObtained = false;

    [Header("References: ")]
    [SerializeField] private PlayerMovement Movement;
    //[SerializeField] private Gun Gun;

    [Header("Body Parts: ")]
    [SerializeField] private Transform Head;
    [SerializeField] private Transform Head_Left;
    [SerializeField] private Transform Hands;
    [SerializeField] private Transform Hands_Left;

    #endregion
    private void Awake() { }
    private void Start()
    {
        ObtainReferences();
        Init();
        ConfigureForAuto();
    }
    private void Update()
    {
        Work();
    }

    private void ObtainReferences()
    {
        Movement = GetComponent<PlayerMovement>();
        //Gun = GetComponent<Gun>();
    }

    private void Init()
    {
        ConfigureForAuto();
    }

    private void Work()
    {
        IdleMoveShoot();
    }

    #region METHODS
    private void ConfigureForAuto()
    {
        //Movement.auto = true;
        //Gun.auto = true;
        GetComponent<Health>().auto = true;
    }

    private void IdleMoveShoot()
    {
        RaycastHit2D hit;
        //hit = Physics2D.Raycast(Movement.isLeft ? Head_Left.transform.position : Head.transform.position,
        //    Movement.isLeft ? Vector2.left : Vector2.right,
        //    visionDistance, LayerMask.NameToLayer(Layers.BulletsLayer));

        //if (hit.transform?.CompareTag("Player") == true)
        //{
        //    //Movement.axis.x = 0f;
        //    //Gun.Shoot();
        //    return;
        //}

        if (!idleMoveTimeObtained)
        {
            randomIdleMoveTime = Random.Range(idleMoveTime.x, idleMoveTime.y);
            idleMoveTimeObtained = true;

            if (Random.Range(1, 10) % 2 == 0) idleMoveLeft = true;
            else idleMoveLeft = false;
        }
        else
        {
            if(currentIdleMoveTime < randomIdleMoveTime)
            {
                currentIdleMoveTime += Time.deltaTime;

                if (Physics2D.Raycast(idleMoveLeft ? Hands_Left.transform.position : Hands.transform.position,
                                     Vector2.down,
                                     10f).transform == null)
                    return;

                //if (idleMoveLeft) Movement.axis.x = -1f;
                //else Movement.axis.x = 1f;
            }
            else
            {
                if(!idleTimeObtained)
                {
                    randomIdleTime = Random.Range(idleTime.x, idleTime.y);
                    idleTimeObtained = true;
                }
                else
                {
                    if(currentIdleTime < randomIdleTime)
                    {
                        //Movement.axis.x = 0f;

                        currentIdleTime += Time.deltaTime;
                    }
                    else
                    {
                        idleTimeObtained = false;
                        currentIdleTime = 0f;

                        idleMoveTimeObtained = false;
                        currentIdleMoveTime = 0f;
                    }
                }
            }
        }
    }
    #endregion
}