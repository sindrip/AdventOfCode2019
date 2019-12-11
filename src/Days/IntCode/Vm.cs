using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Days.IntCode
{
    public class Vm
    {
        public bool Running { get; private set; }
        public bool WaitForInput { get; private set; }
        private BigInteger _ip;
        private List<BigInteger> _initialMemory;
        private List<BigInteger> _memory;
        public List<BigInteger> Output;
        private List<BigInteger> _initialInput;
        private int _inputIndex;
        public Queue<BigInteger> Input;
        private BigInteger _relativeBase;
        private bool _debug = false;

        public Vm(IList<BigInteger> initialMemory)
        {
            _initialMemory = new List<BigInteger>(initialMemory);
            _initialInput = new List<BigInteger>();
            Reset();
        }

        public Vm(IList<BigInteger> initialMemory, IList<BigInteger> initialInput)
        {
            _initialMemory = new List<BigInteger>(initialMemory);
            _initialInput = new List<BigInteger>(initialInput);
            Reset();
        }

        public void Reset()
        {
            _memory = new List<BigInteger>(_initialMemory);
            Input = new Queue<BigInteger>(_initialInput);
            _inputIndex = 0;
            _ip = 0;
            Output = new List<BigInteger>();
        }

        public void Run()
        {
            Running = true;
            while (Running && !WaitForInput)
                Step();
        }

        public void Step()
        {
            var next = Fetch();
            var instruction = Decode(int.Parse(next.ToString()));
            Execute(instruction);
            
            _ip += instruction.OpWidth;
        }

        public BigInteger Fetch() => _memory[int.Parse(_ip.ToString())];
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
                3 => new InputOp(pm),
                4 => new OutputOp(pm),
                5 => new JumpIfTrueOp(pm),
                6 => new JumpIfFalseOp(pm),
                7 => new LessThanOp(pm),
                8 => new EqualsOp(pm),
                9 => new RelativeBaseOp(pm),
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
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamRead(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamWrite(parameters[2], op.ParameterMode[2]);
                    MemSet(arg3, arg1 + arg2);
                    if (_debug)
                        Console.WriteLine($"Add; val: {arg1+arg2} addr: {arg3}");
                    break;
                }
                case OpCode.Multiply:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamRead(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamWrite(parameters[2], op.ParameterMode[2]);
                    MemSet(arg3, arg1 * arg2);
                    if (_debug)
                        Console.WriteLine($"Mult; val: {arg1+arg2} addr: {arg3}");
                    break;
                }
                case OpCode.Input:
                {
                    var input = GetInput();
                    var arg1 = ParamWrite(parameters[0], op.ParameterMode[0]);
                    if (_debug)
                        Console.WriteLine($"In; val: {input} addr: {arg1}");
                    if (WaitForInput)
                    {
                        _ip -= op.OpWidth;
                    }
                    else
                    {
                        MemSet(arg1, input);
                    }
                    break;
                }
                case OpCode.Output:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    SendOutput(arg1);
                    if (_debug)
                        Console.WriteLine($"Out; val: {arg1}");
                    break;
            }
                case OpCode.JumpIfTrue:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamRead(parameters[1], op.ParameterMode[1]);
                    if (arg1 != 0)
                    {
                        _ip = arg2 - op.OpWidth;
                        if (_debug)
                            Console.WriteLine($"JmpIfT; ip: {_ip}");
                    }
                    else
                    {
                        if (_debug)
                            Console.WriteLine($"JmpIfT; NoJmp");
                    }
                    break;
                }
                case OpCode.JumpIfFalse:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamRead(parameters[1], op.ParameterMode[1]);
                    if (arg1 == 0)
                    {
                        _ip = arg2 - op.OpWidth;
                        if (_debug)
                            Console.WriteLine($"JmpIfF; ip: {_ip}");
                    }
                    else
                    {
                        if (_debug)
                            Console.WriteLine($"JmpIfF; NoJmp");
                    }
                    
                    break;
                }
                case OpCode.LessThan:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamRead(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamWrite(parameters[2], op.ParameterMode[2]);
                    if (arg1 < arg2)
                    {
                        if (_debug)
                            Console.WriteLine($"LT; val: 1 addr: {arg3}");
                        MemSet(arg3, 1);
                    }
                    else
                    {
                        if (_debug)
                            Console.WriteLine($"NotLT; val: 0 addr: {arg3}");
                        MemSet(arg3, 0);
                    }
                    break;
                }
                case OpCode.Equals:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    var arg2 = ParamRead(parameters[1], op.ParameterMode[1]);
                    var arg3 = ParamWrite(parameters[2], op.ParameterMode[2]);
                    if (arg1 == arg2)
                    {
                        if (_debug)
                            Console.WriteLine($"Eq; {arg1}; {arg2}; val: 1 addr: {arg3}");
                        MemSet(arg3, 1);
                    }
                    else
                    {
                        if (_debug)
                            Console.WriteLine($"NotEq; {arg1}; {arg2}; val: 0 addr: {arg3}");
                        MemSet(arg3, 0);
                    }
                    break;
                }
                case OpCode.RelativeBase:
                {
                    var arg1 = ParamRead(parameters[0], op.ParameterMode[0]);
                    if (_debug)
                        Console.WriteLine($"RelaBase; {_relativeBase} + {arg1} = {_relativeBase + arg1}");
                    _relativeBase += arg1;
                    break;
                }
                case OpCode.Halt:
                {
                    Running = false;
                    break;
                }
                default:
                    throw new ArgumentException($"Unrecognized OpCode: {op.Op}");
            }
        }

        private List<BigInteger> FetchParams(BigInteger opWidth) => _memory.GetRange(int.Parse(_ip.ToString()) + 1, int.Parse(opWidth.ToString()) - 1);

        private BigInteger ParamRead(BigInteger address, int mode) => mode switch
        {
            0 => MemAccess(address),
            1 => address,
            2 => MemAccess(_relativeBase + address),
            _ => throw new ArgumentException($"Invalid mode {mode}"),
        };

        private BigInteger ParamWrite(BigInteger address, int mode) => mode switch
        {
            0 => MemAccess(address),
            1 => address,
            2 => _relativeBase + address,
            _ => throw new ArgumentException($"Invalid mode {mode}"),

        };
            
        //private int ParamValue(int address, int mode) => mode == 0 ? MemAccess(address) : address;

        public BigInteger MemAccess(BigInteger address)
        {
            MemCheckCapacity(address);
            return _memory[int.Parse(address.ToString())];
        }

        public void MemSet(BigInteger address, BigInteger value)
        {
            MemCheckCapacity(address);
            _memory[int.Parse(address.ToString())] = value;
        }

        private void MemCheckCapacity(BigInteger address)
        {
            if (address >= _memory.Count)
            {
                var newMemory = new BigInteger[int.Parse(address.ToString()) + 100];
                _memory.CopyTo(newMemory);
                _memory = new List<BigInteger>(newMemory);
            }
        }

        private BigInteger GetInput()
        {
            if (Input.Count == 0)
            {
                WaitForInput = true;
                return -1;
            }

            return Input.Dequeue();
        }

        public void SetInitialInput(List<BigInteger> l)
        {
            _initialInput = new List<BigInteger>(l);   
            Reset();
        }
        public void AddInput(BigInteger n)
        {
            WaitForInput = false;
            Input.Enqueue(n);
        }

        private void SendOutput(BigInteger output) => Output.Add(output);
        public BigInteger LastOutput() => Output.Last();
    }
}