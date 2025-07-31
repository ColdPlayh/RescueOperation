using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;
public class Fpstool : MonoBehaviour

{

    public Text FPSTex;

    public float offset=10f;

    private float time, frameCount;

    private float fps = 0;

    void Update()

    {

        if (time >= 1 && frameCount >= 1)

        {

            fps = frameCount / time+offset;

            time = 0;

            frameCount = 0;

        }

        FPSTex.color = fps >= 20 ? Color.white : (fps > 15 ? Color.yellow : Color.red);

        FPSTex.text = "FPS为:" + fps.ToString("f2");

        time += Time.unscaledDeltaTime;

        frameCount++;

    }

} 