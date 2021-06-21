﻿// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Benchmark;
using Benchmark.Serializers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Buffers;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if !DEBUG
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            BenchmarkRunner.Run<SimpleSerializerTest>();
#else
            var test = new SimpleSerializerTest();
            test.Setup();
            test.MessagePackV3_Array();
#endif
        }
    }


    [Config(typeof(BenchmarkConfig))]
    public class SimpleSerializerTest
    {
        //[ParamsSource(nameof(Serializers))]
        //public SerializerBase Serializer;
        //public IEnumerable<SerializerBase> Serializers => new SerializerBase[]
        //{
        //    new MessagePack_v2(),
        //    new MessagePack_v3(),
        //};

        ArrayBufferWriter<byte> bufferWriter = default!;

        [GlobalSetup]
        public void Setup()
        {
            bufferWriter = new ArrayBufferWriter<byte>(1024);
        }

        [Benchmark]
        public byte[] MessagePackV2()
        {
            var writer = new MessagePack.MessagePackWriter(bufferWriter);
            writer.WriteArrayHeader(10);
            for (int i = 0; i < 10; i++)
            {
                writer.WriteInt32(1000);
                writer.WriteInt32(2000);
                writer.WriteInt32(3000);
                writer.WriteInt32(4000);
            }
            writer.Flush();
            var xs = bufferWriter.WrittenSpan.ToArray();
            bufferWriter.Clear();
            return xs;
        }

        [Benchmark]
        public byte[] MessagePackV3_Array()
        {
            var writer = new MessagePackv3.MessagePackWriter(bufferWriter, true);
            writer.WriteArrayHeader(10);
            for (int i = 0; i < 10; i++)
            {
                writer.WriteInt32(1000);
                writer.WriteInt32(2000);
                writer.WriteInt32(3000);
                writer.WriteInt32(4000);
            }
            writer.Flush();
            var xs = bufferWriter.WrittenSpan.ToArray();
            bufferWriter.Clear();
            return xs;
        }

        [Benchmark]
        public byte[] MessagePackV3_Span()
        {
            var writer = new MessagePackv3.MessagePackWriter(bufferWriter, false);
            writer.WriteArrayHeader(10);
            for (int i = 0; i < 10; i++)
            {
                writer.WriteInt32(1000);
                writer.WriteInt32(2000);
                writer.WriteInt32(3000);
                writer.WriteInt32(4000);
            }
            writer.Flush();
            var xs = bufferWriter.WrittenSpan.ToArray();
            bufferWriter.Clear();
            return xs;
        }
    }
}
