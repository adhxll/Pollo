using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper function for the MVC Settings 
public class SettingsElement : MonoBehaviour
{
    // declaring this app as the parent function, so the other elements could access each other
    public SettingsApplication app { get { return GameObject.FindObjectOfType<SettingsApplication>();}} 
}
