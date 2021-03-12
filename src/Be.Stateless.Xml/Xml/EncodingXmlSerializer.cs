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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Be.Stateless.Extensions;
using Be.Stateless.Text;

namespace Be.Stateless.Xml
{
	/// <summary>
	/// XML serializer surrogate that supports the serialization of <see cref="Encoding"/>.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required by XML serialization")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	public class EncodingXmlSerializer : IXmlSerializable
	{
		#region Operators

		public static implicit operator Encoding(EncodingXmlSerializer serializer)
		{
			return serializer == null || serializer._serializedEncoding.IsNullOrEmpty()
				? null
				: EncodingConverter.Deserialize(serializer._serializedEncoding);
		}

		public static implicit operator EncodingXmlSerializer(Encoding encoding)
		{
			return new EncodingXmlSerializer(encoding.IfNotNull(EncodingConverter.Serialize));
		}

		#endregion

		public EncodingXmlSerializer() { }

		private EncodingXmlSerializer(string serializedEncoding)
		{
			_serializedEncoding = serializedEncoding;
		}

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			_serializedEncoding = reader.ReadElementContentAsString();
		}

		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			writer.WriteString(_serializedEncoding);
		}

		#endregion

		private string _serializedEncoding;
	}
}
