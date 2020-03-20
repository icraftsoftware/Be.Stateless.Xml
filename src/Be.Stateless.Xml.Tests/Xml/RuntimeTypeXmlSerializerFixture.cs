#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Be.Stateless.IO;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Xml
{
	public class RuntimeTypeXmlSerializerFixture
	{
		[Fact]
		public void CastNullRuntimeTypeXmlSerializer()
		{
			var type = (Type) (RuntimeTypeXmlSerializer) null;
			type.Should().BeNull();
		}

		[Fact]
		public void DeserializeType()
		{
			using (var stream = new StringStream($"<RuntimeTypeXmlSerializer>{typeof(int).AssemblyQualifiedName}</RuntimeTypeXmlSerializer>"))
			{
				var serializer = new XmlSerializer(typeof(RuntimeTypeXmlSerializer));
				var runtimeTypeXmlSerializer = serializer.Deserialize(stream);
				runtimeTypeXmlSerializer.Should().NotBeNull().And.BeOfType<RuntimeTypeXmlSerializer>();
				var type = (Type) (RuntimeTypeXmlSerializer) runtimeTypeXmlSerializer;
				type.Should().Be(typeof(int));
			}
		}

		[Fact]
		public void SerializeNullType()
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new XmlWriterSettings { OmitXmlDeclaration = true }))
			{
				var sut = (RuntimeTypeXmlSerializer) (Type) null;
				var serializer = new XmlSerializer(typeof(RuntimeTypeXmlSerializer));
				serializer.Serialize(writer, sut);
			}
			builder.ToString().Should().Be("<RuntimeTypeXmlSerializer />");
		}

		[Fact]
		public void SerializeType()
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new XmlWriterSettings { OmitXmlDeclaration = true }))
			{
				var sut = (RuntimeTypeXmlSerializer) typeof(int);
				var serializer = new XmlSerializer(typeof(RuntimeTypeXmlSerializer));
				serializer.Serialize(writer, sut);
			}
			builder.ToString().Should().Be($"<RuntimeTypeXmlSerializer>{typeof(int).AssemblyQualifiedName}</RuntimeTypeXmlSerializer>");
		}
	}
}
