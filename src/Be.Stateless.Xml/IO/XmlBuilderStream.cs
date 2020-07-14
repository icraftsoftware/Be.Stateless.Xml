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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Be.Stateless.Extensions;
using Be.Stateless.Xml.Builder;
using Be.Stateless.Xml.Builder.Extensions;
using ConservativeEnumerator = System.Collections.Generic.IEnumerator<Be.Stateless.Xml.Builder.IXmlInformationItemBuilder>;

namespace Be.Stateless.IO
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	public class XmlBuilderStream : Stream
	{
		public XmlBuilderStream(IXmlElementBuilder node) : this(node, Encoding.UTF8) { }

		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		public XmlBuilderStream(IXmlElementBuilder node, Encoding encoding)
		{
			Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
			_disposable = node as IDisposable;
			var enumerable = node == null
				? Enumerable.Empty<IXmlElementBuilder>()
				: Enumerable.Repeat(node, 1);
			_enumerators = new LinkedList<ConservativeEnumerator>();
			_enumerators.AddLast(enumerable.GetConservativeEnumerator());
		}

		#region Base Class Member Overrides

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override void Close()
		{
			_disposable?.Dispose();
			base.Close();
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override long Length => throw new NotSupportedException();

		public override long Position
		{
			get => _position;
			set => throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var bufferController = new BufferController(buffer, offset, count);
			// try to exhaust backlog if any while keeping any overflowing content
			_backlog = bufferController.Append(_backlog);
			while (bufferController.Availability > 0 && !EOF)
			{
				// append to buffer and keep any overflowing content
				_backlog = bufferController.Append(ReadNextNode(), Encoding);
			}
			_position += bufferController.Count;
			return bufferController.Count;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		#endregion

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public Encoding Encoding { get; }

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public bool EOF => _enumerators.Count == 0;

		private ConservativeEnumerator CurrentEnumerator
		{
			get { return _enumerators.Last.IfNotNull(n => n.Value); }
		}

		private IXmlInformationItemBuilder CurrentNode
		{
			get { return CurrentEnumerator.IfNotNull(e => e.Current); }
		}

		private string ReadNextNode()
		{
			if (CurrentEnumerator.MoveNext())
			{
				switch (CurrentNode)
				{
					case IXmlAttributeBuilder _:
						return CurrentNode.Prefix.IsNullOrEmpty()
							? $" {CurrentNode.LocalName}=\"{CurrentNode.Value}\""
							: $" {CurrentNode.Prefix}:{CurrentNode.LocalName}=\"{CurrentNode.Value}\"";
					case IXmlElementBuilder _ when CurrentNode.HasAttributes():
					{
						var result = CurrentNode.Prefix.IsNullOrEmpty()
							? $"<{CurrentNode.LocalName}"
							: $"<{CurrentNode.Prefix}:{CurrentNode.LocalName}";
						_enumerators.AddLast(CurrentNode.GetAttributes().GetConservativeEnumerator());
						return result;
					}
					case IXmlElementBuilder _ when CurrentNode.HasChildNodes():
					{
						var result = CurrentNode.Prefix.IsNullOrEmpty()
							? $"<{CurrentNode.LocalName}>"
							: $"<{CurrentNode.Prefix}:{CurrentNode.LocalName}>";
						_enumerators.AddLast(CurrentNode.GetChildNodes().GetConservativeEnumerator());
						return result;
					}
					case IXmlElementBuilder _:
						return CurrentNode.Prefix.IsNullOrEmpty()
							? $"<{CurrentNode.LocalName} />"
							: $"<{CurrentNode.Prefix}:{CurrentNode.LocalName} />";
					case IXmlTextBuilder _:
						return CurrentNode.Value;
					default:
						throw new NotImplementedException();
				}
			}

			if (CurrentNode is IXmlAttributeBuilder)
			{
				_enumerators.RemoveLast();
				if (CurrentNode.HasChildNodes())
				{
					_enumerators.AddLast(CurrentNode.GetChildNodes().GetConservativeEnumerator());
					return ">";
				}
				return " />";
			}

			_enumerators.RemoveLast();

			if (CurrentNode is IXmlElementBuilder)
			{
				return CurrentNode.Prefix.IsNullOrEmpty()
					? $"</{CurrentNode.LocalName}>"
					: $"</{CurrentNode.Prefix}:{CurrentNode.LocalName}>";
			}
			return null;
		}

		private readonly IDisposable _disposable;
		private readonly LinkedList<ConservativeEnumerator> _enumerators;
		private byte[] _backlog;
		private int _position;
	}
}
