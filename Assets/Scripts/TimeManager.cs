using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Skybox textures for different times of day
    [SerializeField] private Texture2D night;
    [SerializeField] private Texture2D sunrise;
    [SerializeField] private Texture2D day;
    [SerializeField] private Texture2D sunset;

    // Gradients for smooth transitions between different times of day
    [SerializeField] private Gradient night_to_sunrise;
    [SerializeField] private Gradient sunrise_to_day;
    [SerializeField] private Gradient day_to_sunset;
    [SerializeField] private Gradient sunset_to_night;

    // The global directional light used to simulate the sun
    [SerializeField] private Light globalLight;

    // Time variables
    [SerializeField] private int minutes;
    public int Minutes
    {
        get { return minutes; }
        set
        {
            minutes = value;
            OnMinutesChange(value); // Trigger method when minutes are updated
        }
    }

    [SerializeField] private int hours;
    public int Hours
    {
        get { return hours; }
        set
        {
            hours = value;
            OnHoursChange(value); // Trigger method when hours are updated
        }
    }

    [SerializeField] private int days;
    public int Days
    {
        get { return days; }
        set { days = value; }
    }

    // Seconds counter
    private float tempSec;

    public void Awake()
    {
        // Initialize environment settings when the game starts
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.005f;
        RenderSettings.skybox.SetTexture("_Texture1", night);
        RenderSettings.skybox.SetTexture("_Texture2", sunrise);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        globalLight.intensity = 0.1f;
    }

    public void Update()
    {
        // Increment in-game minutes every real second
        tempSec += Time.deltaTime;
        if (tempSec >= 1)
        {
            Minutes++;
            tempSec = 0;   // Reset the seconds counter to 0
        }
    }

    private void OnMinutesChange(int value)
    {
        // Rotate the global light to simulate the sun's movement
        globalLight.transform.Rotate(Vector3.up, (1f / 1440f) * 360f, Space.World);

        // Increment in-game hours every real minute
        if (value >= 60)
        {
            Hours++;
            minutes = 0;  // Reset minutes to 0
        }

        // Increment in-game days every real 24 minutes
        if (Hours >= 24)
        {
            Days++;
            hours = 0;    // Reset hours to 0
        }
    }

    private void OnHoursChange(int value)
    {
        // Change skybox and lighting based on the current hour
        if (value == 6)
        {
            StartCoroutine(Skybox(night, sunrise, 10f));
            StartCoroutine(Light(night_to_sunrise, 10f, 0.5f));
        }
        else if (value == 8)
        {
            StartCoroutine(Skybox(sunrise, day, 10f));
            StartCoroutine(Light(sunrise_to_day, 10f, 1f));
        }
        else if (value == 18)
        {
            StartCoroutine(Skybox(day, sunset, 10f));
            StartCoroutine(Light(day_to_sunset, 10f, 0.5f));
        }
        else if (value == 22)
        {
            StartCoroutine(Skybox(sunset, night, 10f));
            StartCoroutine(Light(sunset_to_night, 10f, 0.5f));
        }
    }

    private IEnumerator Skybox(Texture2D a, Texture2D b, float time)
    {
        // Smoothly blend between two skybox textures over a given time
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time); // Update blend factor
            yield return null; // Wait for the next frame
        }

        RenderSettings.skybox.SetTexture("_Texture1", b); // Finalize transition
    }

    private IEnumerator Light(Gradient lightGradient, float time, float light)
    {
        // Smoothly transition global light color and intensity over a given time
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time); // Update light color
            RenderSettings.fogColor = globalLight.color;
            globalLight.intensity = light;
            yield return null; // Wait for the next frame
        }
    }
}
