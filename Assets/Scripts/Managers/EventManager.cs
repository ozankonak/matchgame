using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityObjectEvent : UnityEvent<GameObject> { }

public class EventManager : MonoBehaviour
{
    #region Event Names

    //GAME EVENTS
    public const byte gridGeneratedEvent = 1;

    //INPUT EVENTS
    public const byte tileClickedEvent = 10;

    //SOUND EVENTS
    public const byte successSoundEvent = 20;
    public const byte clickSoundEvent = 21;

    #endregion

    #region Core

    private Dictionary<byte, UnityEvent> eventDictionary;

    private Dictionary<byte, UnityObjectEvent> eventDictionaryWithObj;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one EventManager script on a Gameobject in your scene");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<byte, UnityEvent>();
        }

        if (eventDictionaryWithObj == null)
        {
            eventDictionaryWithObj = new Dictionary<byte, UnityObjectEvent>();
        }
    }

    public static void StartListening(byte eventNumber, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventNumber, thisEvent);
        }
    }

    public static void StartListening(byte eventName, UnityAction<GameObject> listener)
    {
        UnityObjectEvent thisEvent = null;

        if (instance.eventDictionaryWithObj.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityObjectEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionaryWithObj.Add(eventName, thisEvent);
        }
    }



    public static void StopListening(byte eventNumber, UnityAction listener)
    {
        if (eventManager == null) return;

        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(byte eventName, UnityAction<GameObject> listener)
    {
        if (eventManager == null) return; 
        UnityObjectEvent thisEvent = null;

        if (instance.eventDictionaryWithObj.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(byte eventNumber)
    {
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void TriggerEvent(byte eventName, GameObject argument)
    {
        UnityObjectEvent thisEvent = null;
        if (instance.eventDictionaryWithObj.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(argument); 
        }
    }

    #endregion
}