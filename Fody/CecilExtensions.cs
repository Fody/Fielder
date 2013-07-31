using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public static class CecilExtensions
{

    public static bool ContainsAttribute(this IEnumerable<CustomAttribute> attributes, string attributeName)
    {
        return attributes.Any(attribute => attribute.Constructor.DeclaringType.Name == attributeName);
    }
    public static bool IsRefOrOut(this Instruction next)
    {
        if (next.OpCode == OpCodes.Call || next.OpCode == OpCodes.Calli)
        {
            var methodReference = next.Operand as MethodReference;
            if (methodReference != null)
            {
                return methodReference.Parameters.Any(x => x.IsOut || x.ParameterType.Name.EndsWith("&"));
            }
        }
        return false;
    }
    public static SequencePoint FindSequencePoint(this Instruction instruction)
    {
        while (instruction != null)
        {
            if (instruction.SequencePoint != null)
            {
                return instruction.SequencePoint;
            }
            instruction = instruction.Previous;
        }
        return null;
    }
}