#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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
	public class TimeSpanXmlSerializerFixture
	{
		[Fact]
		public void CastNullTimeSpanXmlSerializer()
		{
			var timeSpan = (TimeSpan) (TimeSpanXmlSerializer) null;
			timeSpan.Should().Be(TimeSpan.Zero);
		}

		[Fact]
		public void DeserializeTimeSpan()
		{
			using (var stream = new StringStream("<TimeSpanXmlSerializer>00:02:00</TimeSpanXmlSerializer>"))
			{
				var serializer = new XmlSerializer(typeof(TimeSpanXmlSerializer));
				var timeSpanXmlSerializer = serializer.Deserialize(stream);
				timeSpanXmlSerializer.Should().NotBeNull().And.BeOfType<TimeSpanXmlSerializer>();
				var timeSpan = (TimeSpan) (TimeSpanXmlSerializer) timeSpanXmlSerializer;
				timeSpan.Should().Be(TimeSpan.FromMinutes(2));
			}
		}

		[Fact]
		public void SerializeTimeSpan()
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new() { OmitXmlDeclaration = true }))
			{
				var sut = (TimeSpanXmlSerializer) TimeSpan.FromMinutes(2);
				var serializer = new XmlSerializer(typeof(TimeSpanXmlSerializer));
				serializer.Serialize(writer, sut);
			}
			builder.ToString().Should().Be("<TimeSpanXmlSerializer>00:02:00</TimeSpanXmlSerializer>");
		}

		[Fact]
		public void SerializeZeroTimeSpan()
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new() { OmitXmlDeclaration = true }))
			{
				var sut = (TimeSpanXmlSerializer) TimeSpan.Zero;
				var serializer = new XmlSerializer(typeof(TimeSpanXmlSerializer));
				serializer.Serialize(writer, sut);
			}
			builder.ToString().Should().Be("<TimeSpanXmlSerializer>00:00:00</TimeSpanXmlSerializer>");
		}
	}
}
