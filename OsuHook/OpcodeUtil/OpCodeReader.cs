using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace OsuHook.OpcodeUtil
{
    /// <summary>
    ///     Sequentially reads all OpCodes from a method body's IL byte instructions.
    /// </summary>
    internal class OpCodeReader
    {
        private static readonly OpCode[] OneByteOpcodes, TwoByteOpcodes;

        private readonly IReadOnlyList<byte> _ilInstructions;
        private int _position;

        static OpCodeReader()
        {
            OneByteOpcodes = new OpCode[0xe1];
            TwoByteOpcodes = new OpCode[0x1f];

            var fields = typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var opcode = (OpCode)field.GetValue(null);
                if (opcode.OpCodeType == OpCodeType.Nternal)
                    continue;

                if (opcode.Size == 1)
                    OneByteOpcodes[opcode.Value] = opcode;
                else
                    TwoByteOpcodes[opcode.Value & 0xff] = opcode;
            }
        }

        public OpCodeReader(IReadOnlyList<byte> ilInstructions)
        {
            _ilInstructions = ilInstructions;
            _position = 0;
        }

        public IEnumerable<OpCode> GetOpCodes()
        {
            while (_position < _ilInstructions.Count)
            {
                var op = ReadOpCode();
                AdvanceThroughOperand(op);
                yield return op;
            }
        }

        private OpCode ReadOpCode()
        {
            var op = _ilInstructions[_position++];
            return op != 0xfe
                ? OneByteOpcodes[op]
                : TwoByteOpcodes[_ilInstructions[_position++]];
        }

        private void AdvanceThroughOperand(OpCode op)
        {
            switch (op.OperandType)
            {
                case OperandType.InlineSwitch:
                    _position += 1 + ReadInt32() * 4;
                    break;
                case OperandType.InlineI8:
                case OperandType.InlineR:
                    _position += 8;
                    break;
                case OperandType.InlineBrTarget:
                case OperandType.InlineField:
                case OperandType.InlineI:
                case OperandType.InlineMethod:
                case OperandType.InlineString:
                case OperandType.InlineTok:
                case OperandType.InlineType:
                case OperandType.InlineSig:
                case OperandType.ShortInlineR:
                    _position += 4;
                    break;
                case OperandType.InlineVar:
                    _position += 2;
                    break;
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineI:
                case OperandType.ShortInlineVar:
                    _position += 1;
                    break;
                case OperandType.InlineNone:
                    break;
#pragma warning disable CS0618
                case OperandType.InlinePhi:
#pragma warning restore CS0618
                default:
                    throw new ArgumentException("Unsupported operand type " + op.OperandType);
            }
        }

        private int ReadInt32()
        {
            var value = _ilInstructions[_position]
                        | (_ilInstructions[_position + 1] << 8)
                        | (_ilInstructions[_position + 2] << 16)
                        | (_ilInstructions[_position + 3] << 24);
            _position += 4;
            return value;
        }
    }
}