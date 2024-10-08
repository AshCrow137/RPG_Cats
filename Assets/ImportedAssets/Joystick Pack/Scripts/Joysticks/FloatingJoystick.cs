using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public static FloatingJoystick Instance { get; private set; }
    private bool bIsTurnBased = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
        GlobalEventManager.TurnManagerEvents.Event_StartCombat.AddListener(OnStartTurnBased);
        GlobalEventManager.TurnManagerEvents.Event_StopCombat.AddListener(OnEndTurnBased);

    }
    private void OnStartTurnBased()
    {
        bIsTurnBased = true;
        background.gameObject.SetActive(false);
    }
    private void OnEndTurnBased()
    {
        bIsTurnBased = false;
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if(!bIsTurnBased)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!bIsTurnBased)
        {
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if(!bIsTurnBased)
        {
            base.OnDrag(eventData);
        }
        
    }
}