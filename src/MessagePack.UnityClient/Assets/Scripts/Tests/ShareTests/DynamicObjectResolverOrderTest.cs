﻿// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MessagePack.Tests
{
    public class DynamicObjectResolverOrderTest
    {
        private readonly ITestOutputHelper logger;

        public DynamicObjectResolverOrderTest(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        private IEnumerable<string> IteratePropertyNames(ReadOnlyMemory<byte> bytes)
        {
            var reader = new MessagePackReader(bytes);
            var mapCount = reader.ReadMapHeader();
            var result = new string[mapCount];
            for (int i = 0; i < mapCount; i++)
            {
                result[i] = reader.ReadString();
                reader.Skip(); // skip the value
            }

            return result;
        }

#if !ENABLE_IL2CPP

        [Fact]
        public void OrderTest()
        {
            var msgRawData = MessagePackSerializer.Serialize(new OrderOrder());
            this.IteratePropertyNames(msgRawData).Is("Bar", "Moge", "Foo", "FooBar", "NoBar");
        }

        [Fact]
        public void InheritIterateOrder()
        {
            RealClass realClass = new RealClass { Str = "X" };
            var msgRawData = MessagePackSerializer.Serialize(realClass);

            this.IteratePropertyNames(msgRawData).Is("Id", "Str");
        }

        [Fact]
        public void NonSequentialKeys_AllowPrivate()
        {
            var options = Resolvers.StandardResolverAllowPrivate.Options;
            var c = new ClassWithMissingKeyPositions
            {
                Id = 2,
                Year = 2017,
                Memo = "some memo",
            };

            byte[] s = MessagePackSerializer.Serialize(c, options);
            this.logger.WriteLine(MessagePackSerializer.ConvertToJson(s, options));

            ClassWithMissingKeyPositions c2 = MessagePackSerializer.Deserialize<ClassWithMissingKeyPositions>(s, options);
            Assert.Equal(c.Id, c2.Id);
            Assert.Equal(c.Year, c2.Year);
            Assert.Equal(c.Memo, c2.Memo);
        }

        [Fact]
        public void ClassWithMissingKeyTest()
        {
            var c = new ClassWithMissingKey
            {
                Id = 2,
                Year = 2017,
            };
            var c2 = new ClassWithMissingKey2
            {
                Id = 3,
                Memo = "some memo",
            };

            var options = MessagePackSerializerOptions.Standard.WithAllowMissingKey(true);

            byte[] s = MessagePackSerializer.Serialize(c);
            this.logger.WriteLine(MessagePackSerializer.ConvertToJson(s));
            ClassWithMissingKey2 d = MessagePackSerializer.Deserialize<ClassWithMissingKey2>(s);
            Assert.Equal(c.Id, d.Id);

            byte[] s2 = MessagePackSerializer.Serialize(c2);
            this.logger.WriteLine(MessagePackSerializer.ConvertToJson(s2));
            Assert.Throws<MessagePackSerializationException>(() =>
            {
                ClassWithMissingKey d2 = MessagePackSerializer.Deserialize<ClassWithMissingKey>(s2);
            });

            ClassWithMissingKey d3 = MessagePackSerializer.Deserialize<ClassWithMissingKey>(s2, options);
            Assert.Equal(c2.Id, d3.Id);
        }
#endif

        [MessagePack.MessagePackObject(keyAsPropertyName: true)]
        [Union(0, typeof(RealClass))]
        public abstract class AbstractBase
        {
            [DataMember(Order = 0)]
            public UInt32 Id = 0xaa00aa00;
        }

        public sealed class RealClass : AbstractBase
        {
            public String Str;
        }

        [MessagePack.MessagePackObject(keyAsPropertyName: true)]
        public class OrderOrder
        {
            [DataMember(Order = 5)]
            public int Foo { get; set; }

            [DataMember(Order = 2)]
            public int Moge { get; set; }

            [DataMember(Order = 10)]
            public int FooBar;

            public string NoBar;

            [DataMember(Order = 0)]
            public string Bar;
        }

        [MessagePackObject]
        internal class ClassWithMissingKeyPositions
        {
            public ClassWithMissingKeyPositions()
            {
            }

            [Key(0)]
            internal int Id { get; set; }

            // This position intentionally omitted for the test.
            ////[Key(1)]
            ////public string Name { get; set; }

            [Key(2)]
            internal int Year { get; set; } = 2019;

            [Key(3)]
            internal string Memo { get; set; }
        }

        [MessagePackObject]
        public class ClassWithMissingKey
        {
            public ClassWithMissingKey()
            {
            }

            [Key(0)]
            public int Id { get; set; }

            [Key(1)]
            public int Year { get; set; }
        }

        [MessagePackObject]
        public class ClassWithMissingKey2
        {
            public ClassWithMissingKey2()
            {
            }

            [Key(0)]
            public int Id { get; set; }

            [Key(2)]
            public string Memo { get; set; }
        }
    }
}
