using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace Osu.Stubs.Opcode;

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
        // List<HarmonyLib.ILInstruction>
        var instructions = (List<object>)MethodGetInstructions.Invoke(null, new object?[] { null, method });

        return instructions.Select(inst => new IlInstruction
        {
            Opcode = (OpCode)FieldOpcode.GetValue(inst),
            Operand = FieldOperand.GetValue(inst),
        });
    }
}

public struct IlInstruction
{
    public OpCode Opcode;
    public object Operand;
}