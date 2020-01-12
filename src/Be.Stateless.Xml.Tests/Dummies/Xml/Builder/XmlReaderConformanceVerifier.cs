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
using System.Xml;
using FluentAssertions;

namespace Be.Stateless.Dummies.Xml.Builder
{
	/// <summary>
	/// Asserts that the actual <see cref="XmlReader"/>'s behavior is conforming to the expected <see cref="XmlReader"/> one.
	/// </summary>
	public class XmlReaderConformanceVerifier : XmlReader
	{
		public XmlReaderConformanceVerifier(XmlReader actual, XmlReader expected)
		{
			_actual = actual ?? throw new ArgumentNullException(nameof(actual));
			_expected = expected ?? throw new ArgumentNullException(nameof(expected));
		}

		#region Base Class Member Overrides

		public override int AttributeCount
		{
			get
			{
				_actual.AttributeCount.Should().Be(_expected.AttributeCount);
				return _actual.AttributeCount;
			}
		}

		public override string BaseURI
		{
			get
			{
				_actual.BaseURI.Should().Be(_expected.BaseURI);
				return _actual.BaseURI;
			}
		}

		public override void Close()
		{
			_actual.Close();
			_expected.Close();
			AssertClosedStateConformance();
		}

		public override int Depth
		{
			get
			{
				_actual.Depth.Should().Be(_expected.Depth);
				return _actual.Depth;
			}
		}

		public override bool EOF
		{
			get
			{
				_actual.EOF.Should().Be(_expected.EOF);
				return _actual.EOF;
			}
		}

		public override string GetAttribute(string name)
		{
			_actual.GetAttribute(name).Should().Be(_expected.GetAttribute(name));
			return _actual.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceUri)
		{
			_actual.GetAttribute(name, namespaceUri).Should().Be(_expected.GetAttribute(name, namespaceUri));
			return _actual.GetAttribute(name, namespaceUri);
		}

		public override string GetAttribute(int i)
		{
			_actual.GetAttribute(i).Should().Be(_expected.GetAttribute(i));
			return _actual.GetAttribute(i);
		}

		public override bool IsEmptyElement
		{
			get
			{
				_actual.IsEmptyElement.Should().Be(_expected.IsEmptyElement);
				return _actual.IsEmptyElement;
			}
		}

		public override string LocalName
		{
			get
			{
				_actual.LocalName.Should().Be(_expected.LocalName);
				return _actual.LocalName;
			}
		}

		public override string LookupNamespace(string prefix)
		{
			_actual.LookupNamespace(prefix).Should().Be(_expected.LookupNamespace(prefix));
			return _actual.LookupNamespace(prefix);
		}

		public override bool MoveToAttribute(string name)
		{
			var result = _actual.MoveToAttribute(name);
			result.Should().Be(_expected.MoveToAttribute(name));
			AssertStateConformance();
			return result;
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			var result = _actual.MoveToAttribute(name, ns);
			result.Should().Be(_expected.MoveToAttribute(name, ns));
			AssertStateConformance();
			return result;
		}

		public override bool MoveToElement()
		{
			var result = _actual.MoveToElement();
			result.Should().Be(_expected.MoveToElement());
			AssertStateConformance();
			return result;
		}

		public override bool MoveToFirstAttribute()
		{
			var result = _actual.MoveToFirstAttribute();
			result.Should().Be(_expected.MoveToFirstAttribute());
			AssertStateConformance();
			return result;
		}

		public override bool MoveToNextAttribute()
		{
			var result = _actual.MoveToNextAttribute();
			result.Should().Be(_expected.MoveToNextAttribute());
			AssertStateConformance();
			return result;
		}

		public override string NamespaceURI
		{
			get
			{
				_actual.NamespaceURI.Should().Be(_expected.NamespaceURI);
				return _actual.NamespaceURI;
			}
		}

		public override XmlNameTable NameTable => _actual.NameTable;

		public override XmlNodeType NodeType
		{
			get
			{
				_actual.NodeType.Should().Be(_expected.NodeType);
				return _actual.NodeType;
			}
		}

		public override string Prefix
		{
			get
			{
				_actual.Prefix.Should().Be(_expected.Prefix);
				return _actual.Prefix;
			}
		}

		public override bool Read()
		{
			var result = _actual.Read();
			result.Should().Be(_expected.Read());
			AssertStateConformance();
			return result;
		}

		public override bool ReadAttributeValue()
		{
			var result = _actual.ReadAttributeValue();
			result.Should().Be(_expected.ReadAttributeValue());
			AssertStateConformance();
			return result;
		}

		public override ReadState ReadState
		{
			get
			{
				_actual.ReadState.Should().Be(_expected.ReadState);
				return _actual.ReadState;
			}
		}

		public override void ResolveEntity()
		{
			_actual.ResolveEntity();
			_expected.ResolveEntity();
			AssertStateConformance();
		}

		public override string Value
		{
			get
			{
				_actual.Value.Should().Be(_expected.Value);
				return _actual.Value;
			}
		}

		#endregion

		private void AssertClosedStateConformance()
		{
			_actual.BaseURI.Should().Be(_expected.BaseURI);
			_actual.Depth.Should().Be(_expected.Depth);
			_actual.EOF.Should().Be(_expected.EOF);
			_actual.NodeType.Should().Be(_expected.NodeType);
			_actual.ReadState.Should().Be(_expected.ReadState);
		}

		[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
		private void AssertStateConformance()
		{
			_actual.EOF.Should().Be(_expected.EOF);
			_actual.NodeType.Should().Be(_expected.NodeType);
			_actual.ReadState.Should().Be(_expected.ReadState);

			switch (_actual.NodeType)
			{
				case XmlNodeType.Attribute:
					_actual.NamespaceURI.Should().Be(_expected.NamespaceURI);
					_actual.HasValue.Should().Be(_expected.HasValue);
					_actual.Value.Should().Be(_expected.Value);
					_actual.ValueType.Should().Be(_expected.ValueType);
					break;
				case XmlNodeType.Element:
					_actual.AttributeCount.Should().Be(_expected.AttributeCount);
					_actual.BaseURI.Should().Be(_expected.BaseURI);
					_actual.Depth.Should().Be(_expected.Depth);
					_actual.HasAttributes.Should().Be(_expected.HasAttributes);
					_actual.HasValue.Should().Be(_expected.HasValue);
					_actual.IsDefault.Should().Be(_expected.IsDefault);
					_actual.IsEmptyElement.Should().Be(_expected.IsEmptyElement);
					_actual.LocalName.Should().Be(_expected.LocalName);
					_actual.Name.Should().Be(_expected.Name);
					_actual.NamespaceURI.Should().Be(_expected.NamespaceURI);
					_actual.Prefix.Should().Be(_expected.Prefix);
					_actual.Value.Should().Be(_expected.Value);
					_actual.ValueType.Should().Be(_expected.ValueType);
					break;
				case XmlNodeType.EndElement:
					_actual.Depth.Should().Be(_expected.Depth);
					break;
				case XmlNodeType.Text:
					_actual.HasValue.Should().Be(_expected.HasValue);
					_actual.Value.Should().Be(_expected.Value);
					_actual.ValueType.Should().Be(_expected.ValueType);
					break;
				case XmlNodeType.None:
					break;
				default:
					throw new NotSupportedException();
			}

			// TODO _actual.CanReadBinaryContent.Should().Be(_expected.CanReadBinaryContent);
			// TODO _actual.CanReadValueChunk.Should().Be(_expected.CanReadValueChunk);
			// TODO _actual.CanResolveEntity.Should().Be(_expected.CanResolveEntity);
			_actual.QuoteChar.Should().Be(_expected.QuoteChar);
			_actual.XmlLang.Should().Be(_expected.XmlLang);
			_actual.XmlSpace.Should().Be(_expected.XmlSpace);
		}

		private readonly XmlReader _actual;
		private readonly XmlReader _expected;
	}
}
