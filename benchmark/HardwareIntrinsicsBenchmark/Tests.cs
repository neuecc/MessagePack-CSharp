﻿// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias oldmsgpack;
extern alias newmsgpack;

using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

#pragma warning disable SA1649 // File name should match first type name

namespace Benchmark
{
    [ShortRunJob]
    public class BooleanArrayBenchmarkMessagePackNoSingleInstructionMultipleDataVsMessagePackSingleInstructionMultipleData
    {
        [Params(64, 1024, 16 * 1024 * 1024)]
        public int Size { get; set; }

        private bool[] input;
        private byte[] inputSerialized;
        private bool[] inputTrue;
        private bool[] inputFalse;
        private newmsgpack::MessagePack.MessagePackSerializerOptions options;

        [GlobalSetup]
        public void SetUp()
        {
            var resolver = newmsgpack::MessagePack.Resolvers.CompositeResolver.Create(MessagePack.Experimental.Resolvers.PrimitiveArrayResolver.Instance, newmsgpack::MessagePack.Resolvers.StandardResolver.Instance);
            options = newmsgpack::MessagePack.MessagePackSerializerOptions.Standard.WithResolver(resolver);

            inputFalse = new bool[Size];
            inputTrue = new bool[Size];
            input = new bool[Size];

            var r = new Random();
            for (var i = 0; i < inputTrue.Length; i++)
            {
                inputTrue[i] = true;
                input[i] = r.Next(0, 2) == 0;
            }

            inputSerialized = newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input);
        }

        [Benchmark]
        public bool[] DeSerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Deserialize<bool[]>(inputSerialized, options);
        }

        [Benchmark]
        public bool[] DeserializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Deserialize<bool[]>(inputSerialized);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataFalse()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(inputFalse, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataFalse()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(inputFalse);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataTrue()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(inputTrue, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataTrue()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(inputTrue);
        }
    }

    [ShortRunJob]
    public class Int8ArrayBenchmarkMessagePackNoSingleInstructionMultipleDataVsMessagePackSingleInstructionMultipleData
    {
        [Params(64, 1024, 16 * 1024 * 1024)]
        public int Size { get; set; }

        private sbyte[] input;
        private sbyte[] inputM32;
        private sbyte[] inputM33;
        private sbyte[] zero;
        private newmsgpack::MessagePack.MessagePackSerializerOptions options;

        [GlobalSetup]
        public void SetUp()
        {
            var resolver = newmsgpack::MessagePack.Resolvers.CompositeResolver.Create(MessagePack.Experimental.Resolvers.PrimitiveArrayResolver.Instance, newmsgpack::MessagePack.Resolvers.StandardResolver.Instance);
            options = newmsgpack::MessagePack.MessagePackSerializerOptions.Standard.WithResolver(resolver);

            zero = new sbyte[Size];
            inputM33 = new sbyte[Size];
            inputM32 = new sbyte[Size];
            input = new sbyte[Size];

            var r = new Random();
            r.NextBytes(MemoryMarshal.AsBytes(input.AsSpan()));
            for (var i = 0; i < inputM32.Length; i++)
            {
                inputM32[i] = -32;
                inputM33[i] = -33;
            }
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataZero()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(zero, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataZero()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(zero);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataM32()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(inputM32, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataM32()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(inputM32);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataM33()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(inputM33, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataM33()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(inputM33);
        }
    }

    [ShortRunJob]
    public class Int16ArrayBenchmarkMessagePackNoSingleInstructionMultipleDataVsMessagePackSingleInstructionMultipleData
    {
        [Params(16, 1024, 16 * 1024 * 1024)]
        public int Size { get; set; }

        private newmsgpack::MessagePack.MessagePackSerializerOptions options;
        private short[] input;
        private short[] zero;

        [GlobalSetup]
        public void SetUp()
        {
            var resolver = newmsgpack::MessagePack.Resolvers.CompositeResolver.Create(MessagePack.Experimental.Resolvers.PrimitiveArrayResolver.Instance, newmsgpack::MessagePack.Resolvers.StandardResolver.Instance);
            options = newmsgpack::MessagePack.MessagePackSerializerOptions.Standard.WithResolver(resolver);

            input = new short[Size];
            zero = new short[Size];
            var r = new Random();
            r.NextBytes(MemoryMarshal.AsBytes(input.AsSpan()));

            Console.WriteLine(newmsgpack::MessagePack.MessagePackSerializer.Serialize(input).Length);
            Console.WriteLine(oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input).Length);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataZero()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(zero, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataZero()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(zero);
        }
    }

    [ShortRunJob]
    public class Int32ArrayBenchmarkMessagePackNoSingleInstructionMultipleDataVsMessagePackSingleInstructionMultipleData
    {
        [Params(8, 1024, 16 * 1024 * 1024)]
        public int Size { get; set; }

        private newmsgpack::MessagePack.MessagePackSerializerOptions options;
        private int[] input;
        private int[] zero;
        private int[] inputShortMin;

        [GlobalSetup]
        public void SetUp()
        {
            var resolver = newmsgpack::MessagePack.Resolvers.CompositeResolver.Create(MessagePack.Experimental.Resolvers.PrimitiveArrayResolver.Instance, newmsgpack::MessagePack.Resolvers.StandardResolver.Instance);
            options = newmsgpack::MessagePack.MessagePackSerializerOptions.Standard.WithResolver(resolver);

            input = new int[Size];
            zero = new int[Size];
            inputShortMin = new int[Size];
            var r = new Random();
            r.NextBytes(MemoryMarshal.AsBytes(input.AsSpan()));
            for (var i = 0; i < inputShortMin.Length; i++)
            {
                inputShortMin[i] = short.MinValue;
            }
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataZero()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(zero, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataZero()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(zero);
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleDataShortMin()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(inputShortMin, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleDataShortMin()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(inputShortMin);
        }
    }

    [ShortRunJob]
    public class SingleArrayBenchmarkMessagePackNoSingleInstructionMultipleDataVsMessagePackSingleInstructionMultipleData
    {
        [Params(64, 1024, 16 * 1024 * 1024)]
        public int Size { get; set; }

        private newmsgpack::MessagePack.MessagePackSerializerOptions options;
        private float[] input;

        [GlobalSetup]
        public void SetUp()
        {
            var resolver = newmsgpack::MessagePack.Resolvers.CompositeResolver.Create(MessagePack.Experimental.Resolvers.PrimitiveArrayResolver.Instance, newmsgpack::MessagePack.Resolvers.StandardResolver.Instance);
            options = newmsgpack::MessagePack.MessagePackSerializerOptions.Standard.WithResolver(resolver);
            input = new float[Size];

            var r = new Random();
            for (var i = 0; i < input.Length; i++)
            {
                input[i] = (float)r.NextDouble();
            }
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input);
        }
    }

    [ShortRunJob]
    public class DoubleArrayBenchmarkMessagePackNoSingleInstructionMultipleDataVsMessagePackSingleInstructionMultipleData
    {
        [Params(64, 1024, 16 * 1024 * 1024)]
        public int Size { get; set; }

        private newmsgpack::MessagePack.MessagePackSerializerOptions options;
        private double[] input;

        [GlobalSetup]
        public void SetUp()
        {
            var resolver = newmsgpack::MessagePack.Resolvers.CompositeResolver.Create(MessagePack.Experimental.Resolvers.PrimitiveArrayResolver.Instance, newmsgpack::MessagePack.Resolvers.StandardResolver.Instance);
            options = newmsgpack::MessagePack.MessagePackSerializerOptions.Standard.WithResolver(resolver);
            input = new double[Size];

            var r = new Random();
            for (var i = 0; i < input.Length; i++)
            {
                input[i] = r.NextDouble();
            }
        }

        [Benchmark]
        public byte[] SerializeSingleInstructionMultipleData()
        {
            return newmsgpack::MessagePack.MessagePackSerializer.Serialize(input, options);
        }

        [Benchmark]
        public byte[] SerializeNoSingleInstructionMultipleData()
        {
            return oldmsgpack::MessagePack.MessagePackSerializer.Serialize(input);
        }
    }
}
