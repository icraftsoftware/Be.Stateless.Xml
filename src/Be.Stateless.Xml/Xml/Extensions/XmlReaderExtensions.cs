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
using System.Xml;

namespace Be.Stateless.Xml.Extensions
{
	public static class XmlReaderExtensions
	{
		/// <summary>
		/// Calls <see cref="XmlReader.MoveToContent"/> and tests if the current content node is a start tag or empty element tag
		/// whose <see cref="XmlReader.Name"/> matches the given <paramref name="name"/> argument; throws otherwise.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The string to match against the <see cref="XmlReader.Name"/> property of the element found.
		/// </param>
		public static void AssertStartElement(this XmlReader reader, string name)
		{
			if (reader.IsStartElement(name)) return;
			var info = (IXmlLineInfo) reader;
			throw new XmlException($"Element '{name}' was not found. Line {info.LineNumber}, position {info.LinePosition}.");
		}

		/// <summary>
		/// Calls <see cref="XmlReader.MoveToContent"/> and tests if the current content node is a start tag or empty element tag
		/// whose <see cref="XmlReader.LocalName"/> and <see cref="XmlReader.NamespaceURI"/> properties match the given <paramref
		/// name="name"/> and <paramref name="ns"/> arguments; throws otherwise.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The string to match against the <see cref="XmlReader.LocalName"/> property of the element found.
		/// </param>
		/// <param name="ns">
		/// The string to match against the <see cref="XmlReader.NamespaceURI"/> property of the element found.
		/// </param>
		public static void AssertStartElement(this XmlReader reader, string name, string ns)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (reader.IsStartElement(name, ns)) return;
			var info = (IXmlLineInfo) reader;
			throw new XmlException($"Element '{name}' with namespace name '{ns}' was not found. Line {info.LineNumber}, position {info.LinePosition}.");
		}

		/// <summary>
		/// Tests if the current content node is an end tag whose <see cref="XmlReader.Name"/> matches the given <paramref
		/// name="name"/> argument; throws otherwise.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The string to match against the <see cref="XmlReader.Name"/> property of the element found.
		/// </param>
		public static void AssertEndElement(this XmlReader reader, string name)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (reader.IsEndElement(name)) return;
			var info = (IXmlLineInfo) reader;
			throw new XmlException($"End element '{name}' was not found. Line {info.LineNumber}, position {info.LinePosition}.");
		}

		/// <summary>
		/// Tests if the current content node is an end tag whose <see cref="XmlReader.LocalName"/> and <see
		/// cref="XmlReader.NamespaceURI"/> match the given <paramref name="name"/> and <paramref name="ns"/> arguments; throws
		/// otherwise.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The string to match against the <see cref="XmlReader.LocalName"/> property of the element found.
		/// </param>
		/// <param name="ns">
		/// The string to match against the <see cref="XmlReader.NamespaceURI"/> property of the element found.
		/// </param>
		public static void AssertEndElement(this XmlReader reader, string name, string ns)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (reader.IsEndElement(name, ns)) return;
			var info = (IXmlLineInfo) reader;
			throw new XmlException($"End element '{name}' with namespace name '{ns}' was not found. Line {info.LineNumber}, position {info.LinePosition}.");
		}

		/// <summary>
		/// Tests if the current content node is an end tag whose <see cref="XmlReader.Name"/> matches the given <paramref
		/// name="name"/> argument.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The string to match against the <see cref="XmlReader.Name"/> property of the element found.
		/// </param>
		public static bool IsEndElement(this XmlReader reader, string name)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			return reader.NodeType == XmlNodeType.EndElement && reader.Name == name;
		}

		/// <summary>
		/// Tests if the current content node is an end tag whose <see cref="XmlReader.LocalName"/> and <see
		/// cref="XmlReader.NamespaceURI"/> properties match the given <paramref name="name"/> and <paramref name="ns"/>
		/// arguments.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The string to match against the <see cref="XmlReader.LocalName"/> property of the element found.
		/// </param>
		/// <param name="ns">
		/// The string to match against the <see cref="XmlReader.NamespaceURI"/> property of the element found.
		/// </param>
		public static bool IsEndElement(this XmlReader reader, string name, string ns)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			return reader.NodeType == XmlNodeType.EndElement && reader.LocalName == name && reader.NamespaceURI == ns;
		}

		/// <summary>
		/// Gets the value of the attribute with the specified <paramref name="name"/>; throws if the value is either not found
		/// or <see cref="string.Empty"/>.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The qualified name of the attribute.
		/// </param>
		/// <returns>
		/// The value of the specified attribute.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="XmlException">
		/// The attribute is not found or the value is <see cref="string.Empty"/>.
		/// </exception>
		public static string GetMandatoryAttribute(this XmlReader reader, string name)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (reader.MoveToAttribute(name)) return reader.Value;
			var info = (IXmlLineInfo) reader;
			throw new XmlException($"Attribute '{name}' was not found. Line {info.LineNumber}, position {info.LinePosition}.");
		}

		public static bool HasAttribute(this XmlReader reader, string name)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			return reader.MoveToAttribute(name);
		}

		/// <summary>
		/// Checks that the current content node is an element with the given <paramref name="name"/> name="name"/> and advances
		/// the reader to the next node.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The qualified name of the element.
		/// </param>
		public static void ReadEndElement(this XmlReader reader, string name)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			reader.AssertEndElement(name);
			reader.ReadEndElement();
		}

		/// <summary>
		/// Checks that the current content node is an element with the given <see cref="XmlReader.LocalName"/> and <see
		/// cref="XmlReader.NamespaceURI"/> and advances the reader to the next node.
		/// </summary>
		/// <param name="reader">
		/// An <see cref="XmlReader"/> object.
		/// </param>
		/// <param name="name">
		/// The local name of the element.
		/// </param>
		/// <param name="ns">
		/// The namespace URI of the element.
		/// </param>
		public static void ReadEndElement(this XmlReader reader, string name, string ns)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			reader.AssertEndElement(name, ns);
			reader.ReadEndElement();
		}
	}
}
