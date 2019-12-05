using System;
using System.Collections;
using System.Collections.Generic;

namespace Days.IntCode
{
    public class Vm
    {
        private bool _running;
        private int _ip;
        private List<int> _initialMemory;
        private List<int> _memory;
        public List<int> Output;
        private List<int> _initialInput;
        private int _inputIndex;
        public List<int> Input;

        public Vm(IList<int> initialMemory)
        {
            _initialMemory = new List<int>(initialMemory);
            Reset();
        }

        public Vm(IList<int> initialMemory, IList<int> initialInput)
        {
            _initialMemory = new List<int>(initialMemory);
            _initialInput = new List<int>(initialInput);
            Reset();
        }

        public void Reset()
        {
            _memory = new List<int>(_initialMemory);
            Input = new List<int>(_initialInput);
            _inputIndex = 0;
            _ip = 0;
            Output = new List<int>();
        }

        public void Run()
        {
            _running = true;
            while (_running)
                Step();
        }

        public void Step()
        {
            var next = Fetch();
            var instruction = Decode(next);
            Execute(instruction);
            
            _ip += instruction.OpWidth;
        }

        public int Fetch() => _memory[_ip];
        public IOp Decode(int instruction)
        {
            var opcode = instruction % 100;
            instruction /= 100;
            var pm = new List<int>();
            while (instruction > 0)
            {
                pm.Add(instruction % 10);
                instruction /= 10;
            }

            return opcode switch
            {
                1 => (IOp)new AddOp(pm),
                2 => new MultiplyOp(pm),
                3 => new InputOp(),
                4 => new OutputOp(pm),
                5 => new JumpIfTrueOp(pm),
                6 => new JumpIfFalseOp(pm),
                7 => new LessThanOp(pm),
                8 => new EqualsOp(pm),
                99 => new HaltOp(),
                _ => throw new ArgumentException($"Unrecognized OpCode: {opcode}"),
            };
        }
        
        public void Execute(IOp op)
        {
            var parameters = FetchParams(op.OpWidth);
            switch (op.Op)
            {
                case OpCode.Add:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamValue(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamValue(parameters[2], op.ParameterMode[2]);
                    MemSet(arg3, arg1 + arg2);
                    Console.WriteLine($"add: {arg3} : {arg1} + {arg2}");
                    break;
                }
                case OpCode.Multiply:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamValue(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamValue(parameters[2], op.ParameterMode[2]);
                    MemSet(arg3, arg1 * arg2);
                    Console.WriteLine($"mult: {arg3} : {arg1} * {arg2}");
                    break;
                }
                case OpCode.Input:
                {
                    var input = GetInput();
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    MemSet(arg1, input);
                    Console.WriteLine($"input: {arg1} : {input}");
                    break;
                }
                case OpCode.Output:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    SendOutput(arg1);
                    Console.WriteLine($"output: {arg1}");
                    break;
                }
                case OpCode.JumpIfTrue:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamValue(parameters[1], op.ParameterMode[1]);
                    if (arg1 != 0)
                    {
                        Console.WriteLine($"JmpIfT: ip = {arg2}");
                        _ip = arg2 - op.OpWidth;
                    }
                    else 
                        Console.WriteLine("JmpIfT: No jump");
                    break;
                }
                case OpCode.JumpIfFalse:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamValue(parameters[1], op.ParameterMode[1]);
                    if (arg1 == 0)
                    {
                        Console.WriteLine($"JmpIfF: ip = {arg2}");
                        _ip = arg2 - op.OpWidth;
                    }
                    else
                        Console.WriteLine("JmpIfF: No jump");
                    break;
                }
                case OpCode.LessThan:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamValue(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamValue(parameters[2], op.ParameterMode[2]);
                    if (arg1 < arg2)
                    {
                        Console.WriteLine($"LT: {arg1} < {arg2} -> {arg3} = 1");
                        MemSet(arg3, 1);
                    }
                    else
                    {
                        Console.WriteLine($"LT: {arg1} < {arg2} -> {arg3} = 0");
                        MemSet(arg3, 0);
                    }
                    break;
                }
                case OpCode.Equals:
                {
                    var arg1 = ParamValue(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamValue(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamValue(parameters[2], op.ParameterMode[2]);
                    if (arg1 == arg2)
                    {
                        Console.WriteLine($"EQ: {arg1} == {arg2} -> {arg3} = 1");
                        MemSet(arg3, 1);
                    }
                    else
                    {
                        Console.WriteLine($"EQ: {arg1} == {arg2} -> {arg3} = 0");
                        MemSet(arg3, 0);
                    }
                    break;
                }
                case OpCode.Halt:
                {
                    _running = false;
                    break;
                }
                default:
                    throw new ArgumentException($"Unrecognized OpCode: {op.Op}");
            }
        }

        private List<int> FetchParams(int opWidth) => _memory.GetRange(_ip + 1, opWidth - 1);
        
        private int ParamValue(int address, int mode) => mode == 0 ? MemAccess(address) : address;
        public int MemAccess(int address) => _memory[address];
        public void MemSet(int address, int value) => _memory[address] = value;

        private int GetInput() => Input[_inputIndex++];
        
        private void SendOutput(int output) => Output.Add(output);
    }
}