using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShaderMixer : PlayableBehaviour
{
    public string shaderVarName;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Renderer rend = playerData as Renderer;
        if (rend == null)
            return;

        int inputCount = playable.GetInputCount();
        float finalFloat = 0;
        for(int index = 0; index < inputCount; index++)
        {
            float weight = playable.GetInputWeight(index);
            var inputPlayable = (ScriptPlayable<ShaderPlayable>)playable.GetInput(index);
            ShaderPlayable behavior = inputPlayable.GetBehaviour();

            finalFloat += behavior.floatVal * weight;
        }

        Material mat = rend.material;

        mat.SetFloat(shaderVarName, finalFloat);
    }
}
