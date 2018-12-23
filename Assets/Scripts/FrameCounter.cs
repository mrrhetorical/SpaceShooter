using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounter : MonoBehaviour
{
    private GameObject _fpsContainer;
    
    [SerializeField] private Text _averageFpsText;
    [SerializeField] private Text _minFpsText;
    [SerializeField] private Text _maxFpsText;
    [SerializeField] private Text _minRtText;
    [SerializeField] private Text _maxRtText;
    [SerializeField] private Text _averageRtText;
    [SerializeField] private Color _textColor;
    [SerializeField] private Color _backgroundColor;

    [Tooltip("The amount of FixedUpdates before the Frame counter is updated again")]
    [SerializeField] private const int FixedTimestamp = 25;


    private List<float> _previousFrames = new List<float>();
    private List<float> _previousRT = new List<float>();
    
    //local

    //Ticks since last update (tslu)
    private int _tslu = 0;

    private void FixedUpdate()
    {
        if (_tslu >= FixedTimestamp)
        {
            {
                var lowest = float.MaxValue;
                var highest = float.MinValue;

                var total = 0f;
                foreach (var value in _previousFrames)
                {
                    if (value < lowest)
                    {
                        lowest = value;
                    }

                    if (value > highest)
                    {
                        highest = value;
                    }

                    total += value;
                }

                var average = total / _previousFrames.Count;
             
                UpdateFps(lowest, highest, average);
            }

            {
                var lowest = float.MaxValue;
                var highest = float.MinValue;

                var total = 0f;

                foreach (var value in _previousRT)
                {
                    if (value < lowest)
                    {
                        lowest = value;
                    }

                    if (value > highest)
                    {
                        highest = value;
                    }

                    total += value;
                }

                var average = total / _previousRT.Count;

                UpdateRt(lowest, highest, average);
            }


            _tslu = 0;
            _previousFrames.Clear();
            _previousRT.Clear();
            return;
        }
        _tslu++;
    }

    private void Update()
    {
        _previousFrames.Add(1f / Time.deltaTime);
        _previousRT.Add(Time.deltaTime * 1000f);
    }


    private void UpdateFps(float minFps, float maxFps, float averageFps)
    {
        _minFpsText.text = "Min: " + minFps;
        _maxFpsText.text = "Max: " + maxFps;
        _averageFpsText.text = "Avg: " + averageFps;
    }

    private void UpdateRt(float minRt, float maxRt, float averageRt)
    {
        _minRtText.text = "Min: " + minRt;
        _maxRtText.text = "Max: " + maxRt;
        _averageRtText.text = "Avg: " + averageRt;
    }

}