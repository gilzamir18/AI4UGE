using Godot;
using System;
using ai4u;
using System.Collections.Generic;
using System.Text;

namespace ai4u;

public class ScreenSensor : Sensor
{
    	
    [Export]
    public int width;
    [Export]
    public int height;

    [Export]
    public bool grayScale = true;

    private Queue<string> values;


    void CreateData()
    {
        values = new Queue<string>(stackedObservations);
        var img = GetASCIIFrame();
        for (int i = 0; i < stackedObservations; i++)
                values.Enqueue(img);
    }

    public override void OnSetup(Agent agent) 
    {
        type = SensorType.sstring;
        shape = new int[2]{width,  height};
        rangeMin = 0;
        rangeMax = 255;
        CreateData();
        agent.AddResetListener(this);
        this.agent = (BasicAgent) agent;
    }

    public override string GetStringValue()
    {
        values.Enqueue(GetASCIIFrame());        
        if (values.Count > stackedObservations)
        {
            values.Dequeue();
        }
        StringBuilder sb = new StringBuilder();
        foreach (var f in values)
        {
            sb.Append(f);
        }
        return sb.ToString();
    }

    public override void OnReset(Agent agent)
    {
        CreateData();
    }

    private string GetASCIIFrame()
    {
        var oldClearMode = GetViewport().RenderTargetClearMode;
        GetViewport().RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame;
        var data = GetViewport().GetTexture().GetData();
        GetViewport().RenderTargetClearMode = oldClearMode;
        data.Resize(width, height);
        data.FlipY();
        if (grayScale)
        {
            data.Convert(Image.Format.L8);
        }
        return data.GetData().GetStringFromASCII();
    }
}
