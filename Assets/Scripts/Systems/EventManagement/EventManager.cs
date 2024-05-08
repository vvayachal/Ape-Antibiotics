using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class will handle all of the events needed to be accessed
 * by multiple different classes. [Tegomlee]
 */
public static class EventManager
{
    /// <summary>
    /// Event that manages the players charge up state.
    /// </summary>
    private static event Action<bool> ChargeUpEvent;

    /// <summary>
    /// Subscribe to the charge up event.
    /// </summary>
    /// <param name="action">The method that will subscribe to the event. The method must have only one parameter and it must be a bool.</param>
    public static void SubscribeToChargeUpEvent(Action<bool> action)
    {
        ChargeUpEvent += action;
    }

    /// <summary>
    /// Unsubscribe to the charge up event
    /// </summary>
    /// <param name="action">The method that will unsubscribe to the event. The method must have only one parameter and it must be a bool.</param>

    public static void UnsubscribeToChargeUpEvent(Action<bool> action)
    {
        ChargeUpEvent -= action;
    }

    /// <summary>
    /// Invoke the ChargeUpEvent.
    /// </summary>
    /// <param name="isCharging">The bool that determines whether the event is true or false.</param>
    public static void InvokeChargeUpEvent(bool isCharging)
    {
        ChargeUpEvent?.Invoke(isCharging);
    }
}
