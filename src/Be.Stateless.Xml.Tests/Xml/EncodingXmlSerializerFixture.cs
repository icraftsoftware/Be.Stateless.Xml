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

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Be.Stateless.IO;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Xml
{
	public class EncodingXmlSerializerFixture
	{
		[Fact]
		public void CastNullEncodingXmlSerializer()
		{
			var encoding = (Encoding) (EncodingXmlSerializer) null;
			encoding.Should().BeNull();
		}

		[Fact]
		public void DeserializeEncoding()
		{
			using (var stream = new StringStream("<EncodingXmlSerializer>utf-8 with signature</EncodingXmlSerializer>"))
			{
				var serializer = new XmlSerializer(typeof(EncodingXmlSerializer));
				var encodingXmlSerializer = serializer.Deserialize(stream);
				encodingXmlSerializer.Should().NotBeNull().And.BeOfType<EncodingXmlSerializer>();
				var encoding = (Encoding) (EncodingXmlSerializer) encodingXmlSerializer;
				encoding.Should().Be(new UTF8Encoding(true));
			}
		}

		[Fact]
		public void SerializeEncoding()
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new XmlWriterSettings { OmitXmlDeclaration = true }))
			{
				var sut = (EncodingXmlSerializer) new UTF8Encoding(true);
				var serializer = new XmlSerializer(typeof(EncodingXmlSerializer));
				serializer.Serialize(writer, sut);
			}
			builder.ToString().Should().Be("<EncodingXmlSerializer>utf-8 with signature</EncodingXmlSerializer>");
		}

		[Fact]
		public void SerializeNullEncoding()
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new XmlWriterSettings { OmitXmlDeclaration = true }))
			{
				var sut = (EncodingXmlSerializer) (Encoding) null;
				var serializer = new XmlSerializer(typeof(EncodingXmlSerializer));
				serializer.Serialize(writer, sut);
			}
			builder.ToString().Should().Be("<EncodingXmlSerializer />");
		}
	}
}
