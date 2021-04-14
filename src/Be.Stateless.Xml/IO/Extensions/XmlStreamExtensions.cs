﻿#region Copyright & License

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

namespace Be.Stateless.IO.Extensions
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	public static class XmlStreamExtensions
	{
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public static XmlDocument AsXmlDocument(this Stream stream)
		{
			var document = new XmlDocument();
			document.Load(stream ?? throw new ArgumentNullException(nameof(stream)));
			return document;
		}

		[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
		public static XmlDocument AsXmlDocument(this Stream stream, bool closeStream)
		{
			try
			{
				return stream.AsXmlDocument();
			}
			finally
			{
				if (closeStream) stream.Close();
			}
		}
	}
}