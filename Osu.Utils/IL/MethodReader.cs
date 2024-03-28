using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Osu.Utils.Extensions;

namespace Osu.Utils.IL;

public static class MethodReader
{
    private static readonly MethodBase MethodGetInstructions =
        AccessTools.Method("HarmonyLib.MethodBodyReader:GetInstructions");

    private static readonly FieldInfo FieldOpcode =
        AccessTools.Field("HarmonyLib.ILInstruction:opcode");

    private static readonly FieldInfo FieldOperand =
        AccessTools.Field("HarmonyLib.ILInstruction:operand");

    public static IEnumerable<IlInstruction> GetInstructions(MethodBase method)
    {
        // List<HarmonyLib.ILInstruction>, HarmonyLib.ILInstruction is internal
        var instructions = MethodGetInstructions.Invoke<IEnumerable<object>>(null, new object?[]
        {
            /* ILGenerator generator = */ null,
            /* MethodBase method = */ method,
        });

        return instructions.Select(inst => new IlInstruction
        {
            Opcode = FieldOpcode.GetValue<OpCode>(inst),
            Operand = FieldOperand.GetValue(inst),
        });
    }
}

public struct IlInstruction
{
    public OpCode Opcode;
    public object Operand;
}