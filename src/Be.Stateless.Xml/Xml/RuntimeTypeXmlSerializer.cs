﻿#region Copyright & License

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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Be.Stateless.Extensions;

namespace Be.Stateless.Xml
{
	/// <summary>
	/// XML serializer surrogate that supports the serialization of <see cref="Type"/>.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required by XML serialization")]
	public class RuntimeTypeXmlSerializer : IXmlSerializable
	{
		#region Operators

		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
		public static implicit operator Type(RuntimeTypeXmlSerializer serializer)
		{
			return serializer is null || serializer._assemblyQualifiedName.IsNullOrEmpty()
				? null
				: Type.GetType(serializer._assemblyQualifiedName, true);
		}

		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
		public static implicit operator RuntimeTypeXmlSerializer(Type type)
		{
			return new RuntimeTypeXmlSerializer(type.IfNotNull(t => t.AssemblyQualifiedName));
		}

		#endregion

		public RuntimeTypeXmlSerializer() { }

		private RuntimeTypeXmlSerializer(string assemblyQualifiedName)
		{
			_assemblyQualifiedName = assemblyQualifiedName;
		}

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			_assemblyQualifiedName = reader.ReadElementContentAsString();
		}

		public void WriteXml(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			writer.WriteString(_assemblyQualifiedName);
		}

		#endregion

		private string _assemblyQualifiedName;
	}
}
