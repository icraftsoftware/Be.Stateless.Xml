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
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Be.Stateless.Xml.Serialization.Extensions
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	public static class XmlSerializerExtensions
	{
		public static void SerializeWithoutDefaultNamespaces(this XmlSerializer xmlSerializer, Stream stream, object @object)
		{
			if (xmlSerializer == null) throw new ArgumentNullException(nameof(xmlSerializer));
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			xmlSerializer.Serialize(stream, @object, _omitXsdAndXsiXmlns);
		}

		public static void SerializeWithoutDefaultNamespaces(this XmlSerializer xmlSerializer, TextWriter textWriter, object @object)
		{
			if (xmlSerializer == null) throw new ArgumentNullException(nameof(xmlSerializer));
			if (textWriter == null) throw new ArgumentNullException(nameof(textWriter));
			xmlSerializer.Serialize(textWriter, @object, _omitXsdAndXsiXmlns);
		}

		public static void SerializeWithoutDefaultNamespaces(this XmlSerializer xmlSerializer, XmlWriter xmlWriter, object @object)
		{
			if (xmlSerializer == null) throw new ArgumentNullException(nameof(xmlSerializer));
			if (xmlWriter == null) throw new ArgumentNullException(nameof(xmlWriter));
			xmlSerializer.Serialize(xmlWriter, @object, _omitXsdAndXsiXmlns);
		}

		// http://stackoverflow.com/questions/625927/omitting-all-xsi-and-xsd-namespaces-when-serializing-an-object-in-net
		private static readonly XmlSerializerNamespaces _omitXsdAndXsiXmlns = new(new[] { new XmlQualifiedName(string.Empty, string.Empty) });
	}
}
