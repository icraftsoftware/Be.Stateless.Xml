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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Be.Stateless.Extensions;

namespace Be.Stateless.Xml
{
	/// <summary>
	/// XML serializer surrogate that supports the serialization of <see cref="TimeSpan"/>.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required by XML serialization")]
	public class TimeSpanXmlSerializer : IXmlSerializable
	{
		#region Operators

		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
		public static implicit operator TimeSpan(TimeSpanXmlSerializer serializer)
		{
			return serializer == null || serializer._serializedTimeSpan.IsNullOrEmpty()
				? TimeSpan.Zero
				: TimeSpan.Parse(serializer._serializedTimeSpan, CultureInfo.InvariantCulture);
		}

		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
		public static implicit operator TimeSpanXmlSerializer(TimeSpan timeSpan)
		{
			return new TimeSpanXmlSerializer(timeSpan.ToString());
		}

		#endregion

		public TimeSpanXmlSerializer() { }

		public TimeSpanXmlSerializer(string serializedTimeSpan)
		{
			_serializedTimeSpan = serializedTimeSpan;
		}

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			_serializedTimeSpan = reader.ReadElementContentAsString();
		}

		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			writer.WriteString(_serializedTimeSpan);
		}

		#endregion

		private string _serializedTimeSpan;
	}
}
