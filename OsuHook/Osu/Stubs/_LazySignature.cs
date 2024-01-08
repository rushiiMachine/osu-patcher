using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using OsuHook.OpcodeUtil;

namespace OsuHook.Osu.Stubs
{
    internal class LazySignature
    {
        private readonly Lazy<MethodBase> _lazy;
        private readonly string _name;

        /// <summary>
        ///     A lazy method signature matcher
        /// </summary>
        /// <param name="name"><c>Class#Method</c> name of what this signature is matching.</param>
        /// <param name="signature">Sequential opcodes to search the target method with.</param>
        public LazySignature(string name, IReadOnlyList<OpCode> signature)
        {
            _name = name;
            _lazy = new Lazy<MethodBase>(() => OpCodeMatcher.FindMethodBySignature(signature));
        }

        public MethodBase Reference
        {
            get
            {
                var value = _lazy.Value;

                if (_lazy.Value == null)
                    throw new Exception($"Method was not found for signature of {_name}");

                return value;
            }
        }
    }
}